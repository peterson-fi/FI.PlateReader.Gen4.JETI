using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Threading;
using System.IO;

namespace FI.PlateReader.Gen4.JETI
{
    public partial class Form1 : Form
    {
        // External Components
        Versa versa = new Versa();
        JETI jeti = new JETI();
        Photodiode tia = new Photodiode();
        Meerstetter ms = new Meerstetter();

        // Internal Components
        Instrument instrument = new Instrument();       // Instrument Values (UI Software)
        Microplate microplate = new Microplate();       // Microplate Dimensions
        ChartSettings charting = new ChartSettings();   // Chart Settings 
        PlateSetup plateSetup = new PlateSetup();       // Well Selection
        Time time = new Time();                         // Clock
        WellImage image = new WellImage();          // Microplate Well Image

        // Data
        Settings settings = new Settings();             // Initial Instrument Settings (Configuration Files)
        Data data = new Data();                         // Data (Plate Scan)
        DataExport dataExport = new DataExport();       // Data Export

        Settings.Info info;
        bool PlotWaveforms = true;

        // Delegates
        delegate void voidDelegate();
        delegate void intDelegate(int value1, int value2, int value3);

        // Cancellation Token Source
        CancellationTokenSource tokenSourceScan;
        CancellationTokenSource tokenSourceLabel;


        // Startup Methods
        public Form1()
        {
            // Initialize the Form
            InitializeComponent();
            tabControl.SelectedTab = tabAssayProtocol;
            StateDisableAll();

            // Start Method
            StartForm();
        }

        public void StartForm()
        {
            // Read Config Files
            bool state = ReadConfigFiles();

            if (!state)
            {
                MessageBox.Show("Failed to read data from instrument!","Error");
            }

            // Create Variables (Microplates, Charts, Form Labels)            
            instrument.InitialValues();

            // Microplate
            microplate.CreatePlates();

            // Create Charts
            charting.CreateChartSettings();
            charting.CreateColors();

            tokenSourceLabel = new CancellationTokenSource();
            CancellationToken token = tokenSourceLabel.Token;
            Task.Factory.StartNew(() => LabelTask(token), token);

            // Connect to Instrument
            if (Connect() && state)
            {
                Task.Factory.StartNew(() => InitialiseStage());
            }
            else
            {
                StateReset();
            }

            // Populate the Form with initial values 
            PopulateForm();
        }

        public bool ReadConfigFiles()
        {
            // Read Config Files
            bool state = settings.ReadData();

            // Pass the information to external classes
            versa.info = settings.info;        
            data.info = settings.info;         
            microplate.info = settings.info;
            image.info = settings.info;
            instrument.info = settings.info;
            info = settings.info;

            return state;
        }

        public void PopulateForm()
        {
            // Plate Format Combo Box
            foreach (var x in microplate.PlateList)
                cboPlateFormat.Items.Add(x.Name);

            cboPlateFormat.SelectedIndex = 0;

            // Scan Types
            foreach (var x in instrument.ScanTypes)
                cboScanType.Items.Add(x);

            cboScanType.SelectedIndex = 0;

            // # of Scans & Delay Time
            nudScans.Enabled = false;
            nudDelay.Enabled = false;

            // Leds
            cboLed.Items.Add(info.LEDWavelength);
            cboLed.SelectedIndex = 0;

            // Spectrometer 
            cboDetector.Items.Add(info.Detector);
            cboDetector.SelectedIndex = 0;

            // LED Current
            nudCurrent.Value = 50;
            nudCurrent.Maximum = info.MaxCurrent;

            // Integration
            nudIntegration.Value = 100;

            // Wavelength
            if (info.Detector == "TIA")
            {
                groupBoxAnalysis.Visible = false;
                nudIntegration.Visible = false;
                label45.Visible = false;
            }

            nudWavelengthA.Minimum = (int)instrument.WavelengthMinium;
            nudWavelengthA.Maximum = (int)instrument.WavelengthMaximun;
            nudWavelengthA.Value = instrument.WavelengthAValue;

            nudWavelengthB.Minimum = (int)instrument.WavelengthMinium;
            nudWavelengthB.Maximum = (int)instrument.WavelengthMaximun;
            nudWavelengthB.Value = instrument.WavelengthBValue;

            // Wavelength Band
            foreach (var x in instrument.WavelengthBand)
            {
                cboBandA.Items.Add(x);
                cboBandB.Items.Add(x);
            }

            cboBandA.SelectedIndex = 1;
            cboBandB.SelectedIndex = 1;

            // Initial State
            DisableButtons();
            ResetDataCharts();
            ResetProtocolCharts();
        }

        public void LabelTask(CancellationToken token)
        {
            try
            {
                while (true)
                {
                    if (token.IsCancellationRequested)
                    {
                        break;
                    }
                    else
                    {
                        time.Delay(10);

                        if ((ms.Connected) && (!instrument.ActiveScan))
                        {
                            ms.GetHeatsinkTemp();
                            time.Delay(10);
                            ms.GetObjectTemp();
                            time.Delay(10);
                        }

                        UpdateUILabels();
                    }
                }
            }
            catch (ObjectDisposedException)
            {
                return;
            }
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Stop Updating UI Labels 
            tokenSourceLabel.Cancel();

            // Stop Scan if Active
            if (instrument.ActiveScan)
            {
                tokenSourceScan.Cancel();
                time.Delay(1000);   // Gives time for the scan to stop
            }

            // Dialog Box to close the Form
            if (MessageBox.Show("Are you sure you want to close?", "Information", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                // Update UI, and disable update
                labelStatus.Text = "Closing Software";
                toolStrip.Update();

                // Disconect from the Instrument
                Disconnect();

                // Close Software
                e.Cancel = false;
            }
            else
            {
                // Keep software open
                e.Cancel = true;

                // Start Updating Labels again
                tokenSourceLabel = new CancellationTokenSource();
                CancellationToken token = tokenSourceLabel.Token;
                Task.Factory.StartNew(() => LabelTask(token), token);
            }

        }


        // Connection Methods
        public bool Connect()
        {
            bool jconnect = true;
            bool tconnect = true;
            bool mconnect = true;

            // Connect to Controller
            bool vconnect = versa.Connect();

            // Connect to Meerstetter
            mconnect = ms.Connect();

            // Connect to JETI
            if (info.Detector == "JETI")
            {
                jconnect = jeti.Connect();
                if (jconnect)
                {
                    // Pass the information to external classes
                    settings.info.Wavelength = jeti.Wavelength;
                    settings.info.NPixel = jeti.Npixels;
                    settings.info.StartPixel = 526;
                    settings.info.PixelLength = 412;
                    versa.info = settings.info;
                    data.info = settings.info;
                    microplate.info = settings.info;
                    image.info = settings.info;
                    instrument.info = settings.info;
                    info = settings.info;
                }
            }

            // Connect to TIA
            if (info.Detector == "TIA")
            {
                tconnect = tia.Connect();
                if (tconnect)
                {
                    settings.info.Wavelength = tia.Time;
                    settings.info.NPixel = tia.SamplePoints;
                    settings.info.StartPixel = 0;
                    settings.info.PixelLength = tia.SamplePoints;
                    settings.info.WavelengthStart = 0;
                    settings.info.WavelengthEnd = 10;
                    versa.info = settings.info;
                    data.info = settings.info;
                    microplate.info = settings.info;
                    image.info = settings.info;
                    instrument.info = settings.info;
                    info = settings.info;
                }
            }

            return vconnect & jconnect & tconnect;
        }

        public void Disconnect()
        {
            // Disconnect from Versa
            versa.Disconnect();
            jeti.Disconnect();
            tia.Disconnect();

        }

        public bool VerifyConnection()
        {
            return versa.VerifyConnection();

        }


        // Acquisition Buttons
        private void btnStart_Click(object sender, EventArgs e)
        {
            // Prepare for Scan
            bool status = VerifyConnection();

            if (status)
            {
                // Update State
                instrument.SetInstrumentStatus(7);
                StateScanningPlate();

                // Launch file dialog to save the data        
                SaveDialog();
            }

            if (status)
            {
                // Launch a new task to run a plate scan
                tokenSourceScan = new CancellationTokenSource();
                CancellationToken token = tokenSourceScan.Token;

                switch (instrument.scanType)
                {
                    case 0:
                        Task.Factory.StartNew(() => PlateScan(token), token);
                        break;
                    case 1:
                        Task.Factory.StartNew(() => KineticScan(token), token);
                        break;
                    case 2:
                        instrument.ActiveScan = false;
                        Task.Factory.StartNew(() => RealTimeData(token), token);
                        break;
                    case 3:
                        if ((info.Detector == "LineScan") & (info.RowScan))
                        {
                            Task.Factory.StartNew(() => WellImage(token), token);
                        }
                        else
                        {
                            Task.Factory.StartNew(() => WellImageStatic(token), token);
                        }                        
                        break;
                    case 4:
                        Task.Factory.StartNew(() => LedTest(token), token);
                        break;
                    default:
                        break; 
                }


            }
            else
            {
                StateReset();
            }

        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            // Activate cancellation token
            btnStop.Enabled = false;
            tokenSourceScan.Cancel();
            instrument.SetInstrumentStatus(6);
        }

        private void btnInsertPlate_Click(object sender, EventArgs e)
        {
            if (VerifyConnection())
            {
                // Home Stages
                StateDisableAll();
                Task.Factory.StartNew(() => InsertPlate());
            }
            else
            {
                StateReset();
            }

        }

        private void btnEjectPlate_Click(object sender, EventArgs e)
        {
            if (VerifyConnection())
            {
                // Eject Microplate
                StateDisableAll();
                Task.Factory.StartNew(() => EjectPlate());
            }
            else
            {
                StateReset();
            }
        }


        // Protocol Buttons
        private void btnApplyProtocol_Click(object sender, EventArgs e)
        {
            // Apply Protocol
            instrument.SetInstrumentStatus(18);
            StateDeviceActive();

            // Read Form, Set Values
            ReadForm();

            // Active Protocol
            instrument.ActiveProtocol = true;


        }

        private void btnResetProtocol_Click(object sender, EventArgs e)
        {
            // Rest Protocol
            StateCreateProtocol();
            ResetDataCharts();

            // Make Protocol Tab Active
            tabControl.SelectedTab = tabAssayProtocol;

            if (plateSetup.WellsSelected)
            {
                EnableApplyProtocol();
            }
        }

        private void EnableApplyProtocol()
        {
            btnApplyProtocol.Enabled = true;
            instrument.SetInstrumentStatus(15);
        }

        private void DisableApplyProtocol()
        {
            btnApplyProtocol.Enabled = false;
        }

        private void ReadForm()
        {

            // Get information from the Form
            int ledIndex = cboLed.SelectedIndex;
            versa.Current = (int)nudCurrent.Value;

            int detectorIndex = cboDetector.SelectedIndex;
            versa.Integration = (double)nudIntegration.Value;

            int wavA = (int)nudWavelengthA.Value;
            int wavB = (int)nudWavelengthB.Value;

            int bandAIndex = cboBandA.SelectedIndex;
            int bandBIndex = cboBandB.SelectedIndex;

            // Kinetic
            instrument.CurrentScan = 0; 
            instrument.NScans = (int)nudScans.Value;
            instrument.Delay = (int)nudDelay.Value;

            // Temperature
            instrument.StartingTemperature = (double)nudStartingTemp.Value;
            instrument.EndingTemperature = (double)nudEndingTemp.Value;
            instrument.RampRate = (double)nudRampRate.Value;

            // Data Class
            data.SetAnalysisParameters(wavA,wavB, instrument.WavelengthBand[bandAIndex], instrument.WavelengthBand[bandBIndex]);

            // Update Form
            cboPlotSelection.Items.Clear();
            cboThermalSelection.Items.Clear();

            List<string> value = new List<string>();

            value.Add("Intensity A [" + data.analysisParameters.WavelengthA.ToString() + "]");
            value.Add("Intensity B [" + data.analysisParameters.WavelengthB.ToString() + "]");
            value.Add("Ratio [" + data.analysisParameters.WavelengthA.ToString() + "/" + data.analysisParameters.WavelengthB.ToString() + "]");
            value.Add("Moment [" + data.analysisParameters.MomentA.ToString() + "-" + data.analysisParameters.MomentB.ToString() + "]");

            // Plot Selection Combo Box
            foreach (var x in value)
            {
                cboPlotSelection.Items.Add(x);
                cboThermalSelection.Items.Add(x);
            }
                

            cboPlotSelection.SelectedIndex = 0;
            cboThermalSelection.SelectedIndex = 0;

            // Clear Well selection labels
            lbRowWellSelection.Text = "";
            lbColumnWellSelection.Text = "";

            // Hard code values for well image
            image.Row = plateSetup.Row;
            image.Column = plateSetup.Column;
            image.RowPixel = 40;
            image.ColumnPixel = 40;
            image.Wells = image.RowPixel * image.ColumnPixel;

            image.CreatePlates();
            image.SetCurrentPlate(instrument.plateType);


        }

        private void cboScanType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get Value of the Scan Type ComboBox
            instrument.scanType = cboScanType.SelectedIndex;
            instrument.plateType = cboPlateFormat.SelectedIndex;


            // UI Settings for different scan types
            switch (instrument.scanType)
            {

                case 0:
                    // Plate Scan
                    nudScans.Value = 1;
                    nudDelay.Value = 0;
                    nudScans.Enabled = false;
                    nudDelay.Enabled = false;
                    break;                   

                case 1:
                    // Kinetic Scan
                    nudScans.Enabled = true;
                    nudDelay.Enabled = true;
                    nudScans.Value = 1;
                    nudDelay.Value = 0;
                    break;                

                case 2:
                    // RTD
                    nudScans.Enabled = true;
                    nudDelay.Enabled = true;
                    nudScans.Value = 100;
                    nudDelay.Value = 0;
                    break;                

                case 3:
                    // Well Image
                    nudScans.Value = 1;
                    nudDelay.Value = 0;
                    nudScans.Enabled = false;
                    nudDelay.Enabled = false;
                    break;

                case 4:
                    // Led Test
                    nudScans.Enabled = true;
                    nudDelay.Enabled = true;
                    nudScans.Value = 100;
                    nudDelay.Value = 0;
                    break;

                default:
                    // Default Values
                    nudScans.Value = 1;
                    nudDelay.Value = 0;
                    nudScans.Enabled = false;
                    nudDelay.Enabled = false;
                    break;
            }



            // Only paste a single well for RTD & Well Image
            if (instrument.scanType == 2 || instrument.scanType == 3)
            {
                if (plateSetup.WellsSelected)
                {
                    // Past Well Selection Area
                    ChartPlate_ActivePaste();
                }
            }

            // Update data chart if well image scan type
            if (instrument.scanType == 3)
            {
                charting.SetCurrentChart(instrument.plateType, 3);
            }
            else
            {
                charting.SetCurrentChart(instrument.plateType, instrument.plateType);
            }

            ResetDataCharts();



        }

        private void nudScans_ValueChanged(object sender, EventArgs e)
        {
            instrument.NScans = (int)nudScans.Value;
        }


        // Motion 
        public void InitialiseStage()
        {
            // Delay to allow time for the UI to open
            time.Delay(500); // 500 ms

            // Home Stages
            instrument.SetInstrumentStatus(2);
            int timeout = 10000;

            var t = Task.Factory.StartNew(() => versa.InsertPlate());
            t.Wait(timeout);

            if (t.Result)
            {
                StateCreateProtocol();
            }
            else
            {
                StateReset();
            }

        }

        public void InsertPlate()
        {
            // Home Stages
            instrument.SetInstrumentStatus(2);
            int timeout = 10000;

            var t = Task.Factory.StartNew(() => versa.InsertPlate());
            t.Wait(timeout);

            if (t.Result)
            {
                if (instrument.ActiveProtocol)
                {
                    instrument.SetInstrumentStatus(18);
                    StateDeviceActive();
                }
                else
                {
                    StateCreateProtocol();
                }
            }
            else
            {
                StateReset();
            }

        }

        public void EjectPlate()
        {
            // Ejecting Microplate
            instrument.SetInstrumentStatus(12);
            int timeout = 10000;

            var t = Task.Factory.StartNew(() => versa.EjectPlate());
            t.Wait(timeout);

            if (t.Result)
            {
                instrument.SetInstrumentStatus(13);
                StatePlateEjected();
            }
            else
            {
                StateReset();
            }
        }


        // Scan Types
        private void PlateScan(CancellationToken token)
        {
            // Initialise Data
            data.InitializeData(1, microplate.plate.Wells);

            // Plate Scan Tasks
            instrument.ActiveScan = true;
            if (info.Detector == "LineScan")
            {
                versa.SetScanHandles();
            }

            // Start stopwatch
            time.StartScanStopwatch();

            // Move to start position
            bool error = versa.MoveReferencePosition(microplate.motor.ColumnReference, microplate.motor.RowReference);

            if (!error)
            {
                MessageBox.Show("Reference Move Error!");
                goto EndPlate;
            }

            // Background measurement
            instrument.SetInstrumentStatus(8);            

            switch (info.Detector)
            {
                case "LineScan":
                    versa.SetIntegrationTime();
                    error = versa.DarkMeasurement();
                    break;
                case "JETI":
                    jeti.Tint = (int)versa.Integration;
                    error = jeti.DarkMeasurement();
                    break;
                case "TIA":
                    error = tia.DarkMeasurement();
                    break;
            }

            if (!error)
            {
                MessageBox.Show("Error acquiring dark measurement!");
                goto EndPlate;
            }

            // Turn on light source
            versa.SetLedCurrent();
            versa.LedOn();

            // Stop Btn
            EnableStopBtn();

            // Update Instrument Status (Scanning Microplate)
            instrument.SetInstrumentStatus(9);

            // Measure microplate (token, scan)
            if ((info.Detector == "LineScan") & (info.RowScan))
            {
                error = MeasurePlate(token, 0);
            }
            else
            {
                error = MeasurePlateStatic(token, 0);
            }
            

            // End Plate Tasks
            EndPlate:

            // Turn off Light Source
            versa.LedOff();

            // End Timer
            time.EndScanStopwatch();
            
            // Save Data
            data.SetData();
            SavePlate(0);

            // End Plate Tasks
            if (error)
            {
                // Move Back to Reference Position
                versa.MoveReferencePosition(microplate.motor.ColumnReference, microplate.motor.RowReference);

                // Instrument State
                StateDeviceActive();
                EnableSaveBtn();
                instrument.ActiveScan = false;

                // Update UI Label
                if (tokenSourceScan.IsCancellationRequested)
                {
                    // Scan Cancelled
                    instrument.SetInstrumentStatus(22);
                }
                else
                {
                    // Scan finishes sucessfully
                    instrument.SetInstrumentStatus(21);
                }

            }
            else
            {
                StateError();
            }

        }

        private void KineticScan(CancellationToken token)
        {
            // Variables
            int scans = instrument.NScans;
            int delay = instrument.Delay;
            bool error;

            // Initialise Data
            data.InitializeData(scans, microplate.plate.Wells);

            // Plate Scan Tasks
            instrument.ActiveScan = true;
            if (info.Detector == "LineScan")
            {
                versa.SetScanHandles();
            }
 
            // Start stopwatch
            time.StartScanStopwatch();

            // Set starting temp... wait for stable
            if (ms.Connected)
            {
                ms.SetTargetTemp((float)instrument.StartingTemperature);
            }

            // Move to start position
            error = versa.MoveReferencePosition(microplate.motor.ColumnReference, microplate.motor.RowReference);

            if (!error)
            {
                MessageBox.Show("Reference Move Error!");
                goto EndPlate;
            }

            // Background measurement
            instrument.SetInstrumentStatus(8);

            switch (info.Detector)
            {
                case "LineScan":
                    versa.SetIntegrationTime();
                    error = versa.DarkMeasurement();
                    break;
                case "JETI":
                    jeti.Tint = (int)versa.Integration;
                    error = jeti.DarkMeasurement();
                    break;
                case "TIA":
                    error = tia.DarkMeasurement();
                    break;
            }

            if (!error)
            {
                MessageBox.Show("Error acquiring dark measurement!");
                goto EndPlate;
            }

            if (!error)
            {
                MessageBox.Show("Error acquiring dark measurement!");
                goto EndPlate;
            }

            // Turn on light source
            versa.SetLedCurrent();
            versa.LedOn();

            // Stop Btn
            EnableStopBtn();

            // Set ramp rate and ending temperature
            if (ms.Connected)
            {
                time.Delay(10);
                ms.SetRampRate((float)instrument.RampRate);
                time.Delay(10);
                ms.SetTargetTemp((float)instrument.EndingTemperature);
            }

            // Kinetic Scan Loop
            for (int i = 0; i < scans; i++)
            {
                // Check Cancel
                if (token.IsCancellationRequested)
                    break;

                // Update Instrument Status (Scanning Microplate)
                instrument.SetInstrumentStatus(9);

                // Set Current Scan
                instrument.CurrentScan = i;

                // Measure microplate (token, scan)
                if ((info.Detector == "LineScan") & (info.RowScan)) { error = MeasurePlate(token, i); }
                else { error = MeasurePlateStatic(token, i); }

                if (!error)
                {
                    MessageBox.Show("Measure Plate Error!");
                    goto EndPlate;
                }

                // Save Data
                SavePlate(i);

                // Move back to reference position
                error = versa.MoveReferencePosition(microplate.motor.ColumnReference, microplate.motor.RowReference);

                if (!error)
                {
                    MessageBox.Show("Reference Move Error!");
                    goto EndPlate;
                }
                
                // Delay between Scans
                if(delay > 0 && i < scans - 1)
                {
                    for(int d = 0; d < delay; d++)
                    {
                        time.Delay(1000);
                        time.GetScanTime();

                        // Update UI Label
                        instrument.InstrumentStatus = "Delaying for " + (delay - d - 1).ToString() + " seconds";

                        // Check Cancel
                        if (token.IsCancellationRequested)
                            break;
                    }
                }
                else
                {
                    time.Delay(300);
                }
            }


            // End Plate Tasks
            EndPlate:

            // Set Temperature back to starting temp
            if (ms.Connected)
            {
                ms.SetRampRate(10);
                ms.SetTargetTemp((float)instrument.StartingTemperature);
            }

            // Turn off Light Source
            versa.LedOff();

            // End Timer
            time.EndScanStopwatch();

            // Save Data
            data.SetData();
            SaveScan();

            // End Plate Tasks
            if (error)
            {
                // Move Back to Reference Position
                versa.MoveReferencePosition(microplate.motor.ColumnReference, microplate.motor.RowReference);

                // Instrument State
                StateDeviceActive();
                EnableSaveBtn();
                instrument.ActiveScan = false;

                // Update UI Label
                if (tokenSourceScan.IsCancellationRequested)
                {
                    // Scan Cancelled
                    instrument.SetInstrumentStatus(22);
                }
                else
                {
                    // Scan finishes sucessfully
                    instrument.SetInstrumentStatus(21);
                }

            }
            else
            {
                StateError();
            }

        }

        private bool MeasurePlate(CancellationToken token, int currentScan)
        {
            // Set the Start Time
            time.StartPlateStopwatch();

            // Variables
            int row = microplate.plate.Row;
            int startColumn = plateSetup.ColumnMin;
            int endColumn = plateSetup.ColumnMax;
            int nColumns = plateSetup.NColumns;

            double startColumnPosition = microplate.motor.ColumnPosition[startColumn];
            double endColumnPosition = microplate.motor.ColumnPosition[endColumn];

            bool error = true;

            // Get Current Temperature
            if (ms.Connected)
            {
                ms.GetHeatsinkTemp();
                time.Delay(10);
                ms.GetObjectTemp();
                time.Delay(10);
            }

            // # of Rows
            for (int i = 0; i < row; i++)
            {
                // Check Cancel
                if (token.IsCancellationRequested)
                    break;

                // Skip inactive rows
                if (!plateSetup.ActiveRow[i])
                    continue;

                // Step Row
                error = versa.StepRowMotor(microplate.motor.RowPosition[i]);

                if (!error)
                {
                    MessageBox.Show("Row Motor Error!");
                    break;
                }

                // Step column to start scan position
                if (i % 2 == 0)
                {
                    // Even Row (A,C,E...)
                    error = versa.StepColumnMotor(startColumnPosition);
                    versa.SetScanParameters(0, info.ColumnDirection * microplate.plate.ColumnSpacing, 0, nColumns);
                }
                else
                {
                    // Odd Row (B,D,F...)
                    error = versa.StepColumnMotor(endColumnPosition);
                    versa.SetScanParameters(0, -info.ColumnDirection * microplate.plate.ColumnSpacing, 0, nColumns);
                }

                // Check Column Motor Error
                if (!error)
                {
                    MessageBox.Show("Column Motor Error!");
                    break;
                }

                // Get Current Temperature
                if (ms.Connected)
                {
                    ms.GetHeatsinkTemp();
                    time.Delay(10);
                    ms.GetObjectTemp();
                    time.Delay(10);
                }

                // Start stop-and-go measurement (Hardware Control)
                error = versa.StartScanMeasurement();

                if (!error)
                {
                    MessageBox.Show("Scan Error!");
                    break;
                }

                // Retrieve the data from the scan
                for (int j = 0; j < nColumns; j++)
                {
                    // Even or Odd Row
                    int columnIndex;

                    if (i % 2 == 0)
                    {
                        // Even Row (A,C,E...)
                        columnIndex = startColumn + j;
                    }
                    else
                    {
                        // Odd Row (B,D,F...)
                        columnIndex = endColumn - j;
                    }

                    // Update Clock
                    time.GetPlateTime();
                    time.GetScanTime();

                    // Well Index
                    int column = microplate.plate.Column;
                    int index = (i * column) + columnIndex;

                    // Get and Set Results
                    versa.GetScanResults(j);

                    //// Get Current Temperature
                    //if (ms.Connected)
                    //{
                    //    //ms.GetHeatsinkTemp();
                    //    //time.Delay(10);
                    //    ms.GetObjectTemp();
                    //    //time.Delay(10);
                    //}

                    data.SetResult(currentScan, index, microplate.plate.Wells, versa.Waveform, ms.ObjectTemp, ms.HeatsinkTemp);

                    // Plot Data  
                    UpdateDataChart(currentScan, i, columnIndex);
                }

                // Wait for scam measurement to finish
                versa.EndScanMeasurement();

                // Get Current Temperature
                if (ms.Connected)
                {
                    ms.GetHeatsinkTemp();
                    time.Delay(10);
                    ms.GetObjectTemp();
                    time.Delay(10);
                }
            }

            // End Time
            time.EndPlateStopwatch();

            return error; 
        
        }

        private bool MeasurePlateStatic(CancellationToken token, int currentScan)
        {
            // Set the Start Time
            time.StartPlateStopwatch();

            // Variables
            int row = microplate.plate.Row;
            int column = microplate.plate.Column;
            int startColumn = plateSetup.ColumnMin;
            int endColumn = plateSetup.ColumnMax;
            int nColumns = plateSetup.NColumns;

            double startColumnPosition = microplate.motor.ColumnPosition[startColumn];
            double endColumnPosition = microplate.motor.ColumnPosition[endColumn];

            bool error = true;

            // # of Rows
            for (int i = 0; i < row; i++)
            {
                // Check Cancel
                if (token.IsCancellationRequested)
                    break;

                // Skip inactive rows
                if (!plateSetup.ActiveRow[i])
                    continue;

                // Step Row
                error = versa.StepRowMotor(microplate.motor.RowPosition[i]);

                if (!error)
                {
                    MessageBox.Show("Row Motor Error!");
                    break;
                }

                //// Step column to start scan position
                //if (i % 2 == 0)
                //{
                //    // Even Row (A,C,E...)
                //    error = versa.StepColumnMotor(startColumnPosition);
                //    versa.SetScanParameters(0, -microplate.plate.ColumnSpacing, 0, nColumns);
                //}
                //else
                //{
                //    // Odd Row (B,D,F...)
                //    error = versa.StepColumnMotor(endColumnPosition);
                //    versa.SetScanParameters(0, microplate.plate.ColumnSpacing, 0, nColumns);
                //}

                //// Check Column Motor Error
                //if (!error)
                //{
                //    MessageBox.Show("Column Motor Error!");
                //    break;
                //}

                // Start stop-and-go measurement (Hardware Control)
                //error = versa.StartScanMeasurement();

                //if (!error)
                //{
                //    MessageBox.Show("Scan Error!");
                //    break;
                //}

                //// Get Current Temperature
                //if (ms.Connected)
                //{
                //    ms.GetHeatsinkTemp();
                //    time.Delay(10);
                //    ms.GetObjectTemp();
                //    time.Delay(10);
                //}

                // Retrieve the data from the scan
                for (int j = 0; j < column; j++)
                {
                    // Check Cancel
                    if (token.IsCancellationRequested)
                        break;

                    // Even or Odd Row
                    int columnIndex;

                    if (i % 2 == 0)
                    {
                        // Even Row (A,C,E...)
                        columnIndex = j;
                    }
                    else
                    {
                        // Odd Row (B,D,F...)
                        columnIndex = (column - 1) - j;
                    }

                    // Skip Inactive Columns
                    if (!plateSetup.ActiveColumn[columnIndex])
                        continue;

                    // Step Row
                    error = versa.StepColumnMotor(microplate.motor.ColumnPosition[columnIndex]);

                    if (!error)
                    {
                        MessageBox.Show("Column Motor Error!");
                        break;
                    }

                    // Update Clock
                    time.GetPlateTime();
                    time.GetScanTime();

                    // Well Index                    
                    int index = (i * column) + columnIndex;

                    // Get Current Temperature
                    if (ms.Connected)
                    {
                        //ms.GetHeatsinkTemp();
                        //time.Delay(10);
                        ms.GetObjectTemp();
                        //time.Delay(10);
                    }

                    // Measure and set results
                    switch (info.Detector)
                    {
                        case "LineScan":
                            versa.LightMeasurement();
                            data.SetResult(currentScan, index, microplate.plate.Wells, versa.Waveform, ms.ObjectTemp, ms.HeatsinkTemp);
                            break;
                        case "JETI":
                            jeti.LightMeasurement();
                            data.SetResult(currentScan, index, microplate.plate.Wells, jeti.Waveform, ms.ObjectTemp, ms.HeatsinkTemp);
                            break;
                        case "TIA":
                            tia.LightMeasurement();
                            data.SetResultTIA(currentScan, index, microplate.plate.Wells, tia.Waveform, tia.Waveform1, tia.Waveform2, ms.ObjectTemp, ms.HeatsinkTemp);
                            break;

                    }

                    // Plot Data  
                    UpdateDataChart(currentScan, i, columnIndex);
                }

                // Wait for scan measurement to finish
                //versa.EndScanMeasurement();

            }

            // End Time
            time.EndPlateStopwatch();

            return error;

        }

        private void RealTimeData(CancellationToken token)
        {

            // Variables
            int row = microplate.plate.Row;
            int column = microplate.plate.Column;
            int wells = microplate.plate.Wells;

            int scans = instrument.NScans;
            int delay = instrument.Delay;

            bool error;

            // Initialise Data
            data.InitializeData(scans, wells);

            // Plate Scan Tasks
            instrument.ActiveScan = false;
            if (info.Detector == "LineScan")
            {
                versa.SetScanHandles();
            }

            // Start stopwatch
            time.StartScanStopwatch();

            // Move to start position
            error = versa.MoveReferencePosition(microplate.motor.ColumnReference, microplate.motor.RowReference);

            if (!error)
            {
                MessageBox.Show("Reference Move Error!");
                goto EndPlate;
            }

            // Background measurement
            instrument.SetInstrumentStatus(8);
            
            switch (info.Detector)
            {
                case "LineScan":
                    versa.SetIntegrationTime();
                    error = versa.DarkMeasurement();
                    break;
                case "JETI":
                    jeti.Tint = (int)versa.Integration;
                    error = jeti.DarkMeasurement();
                    break;
                case "TIA":
                    error = tia.DarkMeasurement();
                    break;
            }

            if (!error)
            {
                MessageBox.Show("Error acquiring dark measurement!");
                goto EndPlate;
            }

            // Turn on light source
            versa.SetLedCurrent();
            versa.LedOn();

            // Stop Btn
            EnableStopBtn();

            // Update Instrument Status (Scanning Microplate)
            instrument.SetInstrumentStatus(26);

            // Monitor Well
            for(int i = 0; i < scans; i++)
            {
                // Check Cancel
                if (token.IsCancellationRequested)
                    break;

                // Get the current well
                int rowMeasurement = plateSetup.Row;
                int columnMeasurement = plateSetup.Column;

                // Set current scan
                instrument.CurrentScan = i;

                // Start plate stopwatch
                time.StartPlateStopwatch();

                // Move Row
                versa.StepRowMotor(microplate.motor.RowPosition[rowMeasurement]);

                // Move Column
                versa.StepColumnMotor(microplate.motor.ColumnPosition[columnMeasurement]);

                // Set Result
                int index = rowMeasurement * column + columnMeasurement;

                // Get Current Temperature
                if (ms.Connected)
                {
                    ms.GetHeatsinkTemp();
                    time.Delay(10);
                    ms.GetObjectTemp();
                    time.Delay(10);
                }

                // Measure and set results
                switch (info.Detector)
                {
                    case "LineScan":
                        versa.LightMeasurement();
                        data.SetResult(i, index, wells, versa.Waveform, ms.ObjectTemp, ms.HeatsinkTemp);
                        break;
                    case "JETI":
                        jeti.LightMeasurement();
                        data.SetResult(i, index, wells, jeti.Waveform, ms.ObjectTemp, ms.HeatsinkTemp);
                        break;
                    case "TIA":
                        tia.LightMeasurement();
                        data.SetResultTIA(i, index, wells, tia.Waveform, tia.Waveform1, tia.Waveform2, ms.ObjectTemp, ms.HeatsinkTemp);
                        break;

                }
                //versa.LightMeasurement();

                // Update Clock
                time.GetPlateTime();
                time.GetScanTime();

                // Plot Data  
                UpdateDataChart(i,rowMeasurement, columnMeasurement);

                // Delay between Scans
                if (delay > 0 && i < scans - 1)
                {
                    for (int d = 0; d < delay; d++)
                    {
                        time.Delay(1000);
                        time.GetScanTime();

                        // Update UI Label
                        instrument.InstrumentStatus = "Delaying for " + (delay - d - 1).ToString() + " seconds";

                        // Check Cancel
                        if (token.IsCancellationRequested)
                            break;
                    }
                }
                else
                {
                    time.Delay(300);
                }

                // End Time
                time.EndPlateStopwatch();

            }

            // End Plate Tasks
            EndPlate:

            // Turn off Light Source
            versa.LedOff();

            // End Timer
            time.EndScanStopwatch();

            // Save Data
            data.SetData();
            SaveScan();

            // End Plate Tasks
            if (error)
            {
                // Move Back to Reference Position
                versa.MoveReferencePosition(microplate.motor.ColumnReference, microplate.motor.RowReference);

                // Instrument State
                StateDeviceActive();
                EnableSaveBtn();
                instrument.ActiveScan = false;

                // Update UI Label
                if (tokenSourceScan.IsCancellationRequested)
                {
                    // Scan Cancelled
                    instrument.SetInstrumentStatus(22);
                }
                else
                {
                    // Scan finishes sucessfully
                    instrument.SetInstrumentStatus(21);
                }

            }
            else
            {
                StateError();
            }




        }

        private void WellImage(CancellationToken token)
        {

            // Variables
            bool error;
            int row = image.RowPixel;
            int column = image.ColumnPixel;
            int wells = image.Wells;

            double startColumnPosition = image.motor.ColumnPosition[0];
            double endColumnPosition = image.motor.ColumnPosition[column - 1];

            // need to fix for pixel round off
            // convert start position and step to integer steps.
            double dColumnStart = startColumnPosition * ((versa.info.ColumnStepsPerRev * versa.info.ColumnMicrostep) / (versa.info.ColumnUnitsPerRev));
            int iColumnStart = (int)dColumnStart;
            double dColumnStep = image.motor.ColumnStepSize * ((versa.info.ColumnStepsPerRev * versa.info.ColumnMicrostep) / (versa.info.ColumnUnitsPerRev));
            int iColumnStep = (int)dColumnStep;
            dColumnStart = iColumnStart * ((versa.info.ColumnUnitsPerRev) / (versa.info.ColumnStepsPerRev * versa.info.ColumnMicrostep));
            dColumnStep = iColumnStep * ((versa.info.ColumnUnitsPerRev) / (versa.info.ColumnStepsPerRev * versa.info.ColumnMicrostep));
            double dColumnEnd = dColumnStart + (dColumnStep * (double)(column - 1));

            // Initialise Data
            data.InitializeData(1, wells);

            // Plate Scan Tasks
            instrument.ActiveScan = true;
            if (info.Detector == "LineScan")
            {
                versa.SetScanHandles();
            }

            // Start stopwatch
            time.StartScanStopwatch();

            // Move to start position
            error = versa.MoveReferencePosition(microplate.motor.ColumnReference, microplate.motor.RowReference);

            if (!error)
            {
                MessageBox.Show("Reference Move Error!");
                goto EndPlate;
            }

            // Background measurement
            instrument.SetInstrumentStatus(8);
            versa.SetIntegrationTime();
            error = versa.DarkMeasurement();

            if (!error)
            {
                MessageBox.Show("Error acquiring dark measurement!");
                goto EndPlate;
            }

            // Turn on light source
            versa.SetLedCurrent();
            versa.LedOn();

            // Stop Btn
            EnableStopBtn();

            // Update Instrument Status (Scanning Microplate)
            instrument.SetInstrumentStatus(27);

            // Start plate time
            time.StartPlateStopwatch();

            // # of Rows
            for (int i = 0; i < row; i++)
            {
                // Check Cancel
                if (token.IsCancellationRequested)
                    break;

                // Step Row
                error = versa.StepRowMotor(image.motor.RowPosition[i]);

                if (!error)
                {
                    MessageBox.Show("Row Motor Error!");
                    break;
                }

                versa.CheckColumnMotor();

                // Step column to start or end of scan area & Set Scanning Parameters
                if (i % 2 == 0)
                {
                    // Even Row (A,C,E...)
                    error = versa.StepColumnMotor(dColumnStart); // (startColumnPosition);
                    versa.SetScanParameters(0, info.ColumnDirection * dColumnStep, 0, column); //image.motor.ColumnStepSize
                }
                else
                {
                    // Odd Row (B,D,F...)
                    error = versa.StepColumnMotor(dColumnEnd);    // (endColumnPosition);
                    versa.SetScanParameters(0, -info.ColumnDirection * dColumnStep, 0, column);
                }

                versa.CheckColumnMotor();
                
                
                // Check Column Motor Error
                if (!error)
                {
                    MessageBox.Show("Column Motor Error!");
                    break;
                }

                // Start stop-and-go measurement (Hardware Control)
                error = versa.StartScanMeasurement();

                if (!error)
                {
                    MessageBox.Show("Scan Error!");
                    break;
                }

                // Retrieve the data from the scan
                for (int j = 0; j < column; j++)
                {
                    // Even or Odd Row
                    int columnIndex;

                    if (i % 2 == 0)
                    {
                        // Even Row (A,C,E...)
                        columnIndex = j;
                    }
                    else
                    {
                        // Odd Row (B,D,F...)
                        columnIndex = (column - 1) - j;
                    }

                    // Update Clock
                    time.GetPlateTime();
                    time.GetScanTime();

                    // Well Index
                    int index = (i * column) + columnIndex;

                    // Get and Set Results
                    versa.GetScanResults(j);

                    data.SetResult(0, index, wells, versa.Waveform, ms.ObjectTemp, ms.HeatsinkTemp);

                    // Plot Data  
                    UpdateDataChart(0, i, columnIndex);

                }

                // Wait for scam measurement to finish
                versa.EndScanMeasurement();

            }

            // End Time
            time.EndPlateStopwatch();

            // End Plate Tasks
            EndPlate:

            // Turn off Light Source
            versa.LedOff();

            // End Timer
            time.EndScanStopwatch();

            // Save Data
            data.SetData();
            SaveImage();

            // End Plate Tasks
            if (error)
            {
                // Move Back to Reference Position
                versa.MoveReferencePosition(microplate.motor.ColumnReference, microplate.motor.RowReference);

                // Instrument State
                StateDeviceActive();
                EnableSaveBtn();
                instrument.ActiveScan = false;

                // Update UI Label
                if (tokenSourceScan.IsCancellationRequested)
                {
                    // Scan Cancelled
                    instrument.SetInstrumentStatus(22);
                }
                else
                {
                    // Scan finishes sucessfully
                    instrument.SetInstrumentStatus(21);
                }

            }
            else
            {
                StateError();
            }


        }

        private void WellImageStatic(CancellationToken token)
        {

            // Variables
            bool error;
            int row = image.RowPixel;
            int column = image.ColumnPixel;
            int wells = image.Wells;

            double startColumnPosition = image.motor.ColumnPosition[0];
            double endColumnPosition = image.motor.ColumnPosition[column - 1];


            // Initialise Data
            data.InitializeData(1, wells);

            // Plate Scan Tasks
            instrument.ActiveScan = true;
            if (info.Detector == "LineScan")
            {
                versa.SetScanHandles();
            }

            // Start stopwatch
            time.StartScanStopwatch();

            // Move to start position
            error = versa.MoveReferencePosition(microplate.motor.ColumnReference, microplate.motor.RowReference);

            if (!error)
            {
                MessageBox.Show("Reference Move Error!");
                goto EndPlate;
            }

            // Background measurement
            instrument.SetInstrumentStatus(8);            

            switch (info.Detector)
            {
                case "LineScan":
                    versa.SetIntegrationTime();
                    error = versa.DarkMeasurement();
                    break;
                case "JETI":
                    jeti.Tint = (int)versa.Integration;
                    error = jeti.DarkMeasurement();
                    break;
                case "TIA":
                    error = tia.DarkMeasurement();
                    break;
            }

            if (!error)
            {
                MessageBox.Show("Error acquiring dark measurement!");
                goto EndPlate;
            }

            // Turn on light source
            versa.SetLedCurrent();
            versa.LedOn();

            // Stop Btn
            EnableStopBtn();

            // Update Instrument Status (Scanning Microplate)
            instrument.SetInstrumentStatus(27);

            // Start plate time
            time.StartPlateStopwatch();

            // # of Rows
            // # of Rows
            for (int i = 0; i < row; i++)
            {
                // Check Cancel
                if (token.IsCancellationRequested)
                    break;

                // Step Row
                error = versa.StepRowMotor(image.motor.RowPosition[i]);

                if (!error)
                {
                    MessageBox.Show("Row Motor Error!");
                    break;
                }

                //// Step column to start scan position
                //if (i % 2 == 0)
                //{
                //    // Even Row (A,C,E...)
                //    error = versa.StepColumnMotor(startColumnPosition);
                //    versa.SetScanParameters(0, -microplate.plate.ColumnSpacing, 0, nColumns);
                //}
                //else
                //{
                //    // Odd Row (B,D,F...)
                //    error = versa.StepColumnMotor(endColumnPosition);
                //    versa.SetScanParameters(0, microplate.plate.ColumnSpacing, 0, nColumns);
                //}

                //// Check Column Motor Error
                //if (!error)
                //{
                //    MessageBox.Show("Column Motor Error!");
                //    break;
                //}

                // Start stop-and-go measurement (Hardware Control)
                //error = versa.StartScanMeasurement();

                //if (!error)
                //{
                //    MessageBox.Show("Scan Error!");
                //    break;
                //}

                // Retrieve the data from the scan
                for (int j = 0; j < column; j++)
                {
                    // Even or Odd Row
                    int columnIndex;

                    if (i % 2 == 0)
                    {
                        // Even Row (A,C,E...)
                        columnIndex = j;
                    }
                    else
                    {
                        // Odd Row (B,D,F...)
                        columnIndex = (column - 1) - j;
                    }

                    // Step Row
                    error = versa.StepColumnMotor(image.motor.ColumnPosition[columnIndex]);

                    if (!error)
                    {
                        MessageBox.Show("Column Motor Error!");
                        break;
                    }

                    // Update Clock
                    time.GetPlateTime();
                    time.GetScanTime();

                    // Well Index                    
                    int index = (i * column) + columnIndex;

                    // Measure and set results
                    switch (info.Detector)
                    {
                        case "LineScan":
                            versa.LightMeasurement();
                            data.SetResult(0, index, wells, versa.Waveform, ms.ObjectTemp, ms.HeatsinkTemp);
                            break;
                        case "JETI":
                            jeti.LightMeasurement();
                            data.SetResult(0, index, wells, jeti.Waveform, ms.ObjectTemp, ms.HeatsinkTemp);
                            break;
                        case "TIA":
                            tia.LightMeasurement();
                            data.SetResultTIA(0, index, wells, tia.Waveform, tia.Waveform1, tia.Waveform2, ms.ObjectTemp, ms.HeatsinkTemp);
                            break;

                    }

                    // Plot Data  
                    UpdateDataChart(0, i, columnIndex);
                }

            }

            // End Time
            time.EndPlateStopwatch();

        // End Plate Tasks
        EndPlate:

            // Turn off Light Source
            versa.LedOff();

            // End Timer
            time.EndScanStopwatch();

            // Save Data
            data.SetData();
            SaveImage();

            // End Plate Tasks
            if (error)
            {
                // Move Back to Reference Position
                versa.MoveReferencePosition(microplate.motor.ColumnReference, microplate.motor.RowReference);

                // Instrument State
                StateDeviceActive();
                EnableSaveBtn();
                instrument.ActiveScan = false;

                // Update UI Label
                if (tokenSourceScan.IsCancellationRequested)
                {
                    // Scan Cancelled
                    instrument.SetInstrumentStatus(22);
                }
                else
                {
                    // Scan finishes sucessfully
                    instrument.SetInstrumentStatus(21);
                }

            }
            else
            {
                StateError();
            }


        }

        private void LedTest(CancellationToken token)
        {


            // Variables
            int scans = instrument.NScans;
            int delay = instrument.Delay;

            bool error = true;

            // Plate Scan Tasks
            instrument.ActiveScan = true;
            time.StartScanStopwatch();

            // Turn on light source
            versa.SetLedCurrent();
            versa.LedOn();

            // Stop Btn
            EnableStopBtn();

            // Update Instrument Status (Scanning Microplate)
            instrument.SetInstrumentStatus(28);

            // Monitor Well
            for (int i = 0; i < scans; i++)
            {
                // Check Cancel
                if (token.IsCancellationRequested)
                    break;

                // Set current scan
                instrument.CurrentScan = i;

                // Start plate stopwatch
                time.StartPlateStopwatch();

                // Update Clock
                time.GetPlateTime();
                time.GetScanTime();

                // Delay between Scans
                if (delay > 0 && i < scans - 1)
                {
                    for (int d = 0; d < delay; d++)
                    {
                        time.Delay(1000);
                        time.GetScanTime();

                        // Update UI Label
                        instrument.InstrumentStatus = "Delaying for " + (delay - d - 1).ToString() + " seconds";

                        // Check Cancel
                        if (token.IsCancellationRequested)
                            break;
                    }
                }
                else
                {
                    time.Delay(300);
                }

                // End Time
                time.EndPlateStopwatch();

            }

            // EndPlate:

            // Turn off Light Source
            versa.LedOff();

            // End Timer
            time.EndScanStopwatch();

            // End Plate Tasks
            if (error)
            {
                // Move Back to Reference Position
                versa.MoveReferencePosition(microplate.motor.ColumnReference, microplate.motor.RowReference);

                // Instrument State
                StateDeviceActive();
                EnableSaveBtn();
                instrument.ActiveScan = false;

                // Update UI Label
                if (tokenSourceScan.IsCancellationRequested)
                {
                    // Scan Cancelled
                    instrument.SetInstrumentStatus(22);
                }
                else
                {
                    // Scan finishes sucessfully
                    instrument.SetInstrumentStatus(21);
                }

            }
            else
            {
                StateError();
            }



        }


        // Save 
        private void btnSaveData_Click(object sender, EventArgs e)
        {

            // Try to save the data
            SaveDialog();


            switch (instrument.scanType)
            {
                case 0:
                    SavePlateMethod();
                    break;
                case 1:
                    SavePlateMethod();
                    SaveScan();
                    break;
                case 2:
                    SaveScan();
                    break;
                case 3:
                    SaveImage();
                    break;
                case 4:
                    return;
                default:
                    break;
            }

            // Save Methods
            void SavePlateMethod()
            {
                for (int i = 0; i < instrument.NScans; i++)
                {
                    SavePlate(i);
                }
            }           
            

            // Give user confirmation of save
            if (dataExport.Save)
            {
                MessageBox.Show("Data Saved!", "Information");
            }

        }

        private void SaveDialog()
        {
            // Set Save to False
            dataExport.Save = false;

            // Open new file dialog
            SaveFileDialog sf = new SaveFileDialog();
            sf.Title = "Save Plate Reader Data";
            sf.Filter = "Plate Reader Data|*.txt";

            if (sf.ShowDialog() == DialogResult.OK)
            {
                dataExport.Filename = Path.GetFileNameWithoutExtension(sf.FileName);
                dataExport.Filepath = Path.GetDirectoryName(sf.FileName);
                dataExport.Save = true;
            }
        }

        private void SavePlate(int scan)
        {
            // Settings
            dataExport.info = info;

            // Microplate
            dataExport.plate = microplate.plate;
            dataExport.motor = microplate.motor;

            // LED
            dataExport.LED = info.LEDWavelength.ToString();
            dataExport.LedCurrent = versa.Current.ToString();

            // Detector
            dataExport.Detector = info.Detector;
            dataExport.Integration = versa.Integration;

            // Data Information
            dataExport.analysisParameters = data.analysisParameters;
            dataExport.PlateResult = data.PlateResult;
            dataExport.block = data.block;

            // Plate Setup
            dataExport.ActiveRow = plateSetup.ActiveRow;
            dataExport.ActiveColumn = plateSetup.ActiveColumn;
            dataExport.Samples = plateSetup.ActiveWells;

            // Time Information
            dataExport.StartDate = time.StartDate;
            dataExport.StartPlateTime = time.StartPlateTime;
            dataExport.EndPlateTime = time.EndPlateTime;

            dataExport.StartScanTime = time.StartScanTime;
            dataExport.EndScanTime = time.EndScanTime;

            // Scan Information
            dataExport.CurrentScan = scan;
            dataExport.NScans = instrument.NScans;
            dataExport.Delay = instrument.Delay;


            // Save Data                
            dataExport.SavePlate(scan);

            

        }

        private void SaveScan()
        {
            // Settings
            dataExport.info = info;

            // Microplate
            dataExport.plate = microplate.plate;
            dataExport.motor = microplate.motor;

            // LED
            dataExport.LED = info.LEDWavelength.ToString();
            dataExport.LedCurrent = versa.Current.ToString();

            // Detector
            dataExport.Detector = info.Detector;
            dataExport.Integration = versa.Integration;

            // Data Information
            dataExport.analysisParameters = data.analysisParameters;
            dataExport.PlateResult = data.PlateResult;
            dataExport.block = data.block;

            // Plate Setup
            dataExport.ActiveRow = plateSetup.ActiveRow;
            dataExport.ActiveColumn = plateSetup.ActiveColumn;
            dataExport.Samples = plateSetup.ActiveWells;

            // Time Information
            dataExport.StartDate = time.StartDate;
            dataExport.StartPlateTime = time.StartPlateTime;
            dataExport.EndPlateTime = time.EndPlateTime;

            dataExport.StartScanTime = time.StartScanTime;
            dataExport.EndScanTime = time.EndScanTime;

            // Scan Information
            dataExport.NScans = instrument.NScans;
            dataExport.Delay = instrument.Delay;


            // Save Data                
            dataExport.SaveScan();

            
        }

        private void SaveImage()
        {
            // Settings
            dataExport.info = info;

            // Microplate
            dataExport.plate = microplate.plate;
            dataExport.motor = microplate.motor;

            // LED
            dataExport.LED = info.LEDWavelength.ToString();
            dataExport.LedCurrent = versa.Current.ToString();

            // Detector
            dataExport.Detector = info.Detector;
            dataExport.Integration = versa.Integration;

            // Data Information
            dataExport.analysisParameters = data.analysisParameters;
            dataExport.PlateResult = data.PlateResult;
            dataExport.block = data.block;

            // Time Information
            dataExport.StartDate = time.StartDate;
            dataExport.StartPlateTime = time.StartPlateTime;
            dataExport.EndPlateTime = time.EndPlateTime;

            dataExport.StartScanTime = time.StartScanTime;
            dataExport.EndScanTime = time.EndScanTime;

            // Well Image
            dataExport.iMotor = image.motor;
            dataExport.iPlate = image.plate;
            dataExport.RowPixel = image.RowPixel;
            dataExport.ColumnPixel = image.ColumnPixel;
            dataExport.Samples = image.Wells;

            // Save Data
            string wellName = dataExport.ConvertRow(plateSetup.RowMin) + (plateSetup.ColumnMin + 1).ToString();
            dataExport.SaveImage(wellName);
        }


        // Thread Safe: Instrument States
        private void StateDisableAll()
        {
            if (toolStrip.InvokeRequired)
            {
                voidDelegate s = new voidDelegate(StateDisableAll);
                Invoke(s, new object[] { });
            }
            else
            {
                ((Control)this.tabAssayProtocol).Enabled = false;
                ((Control)this.tabMeasure).Enabled = false;

                DisableButtons();

            }
        }

        private void DisableButtons()
        {
            // Invoked Required returns true if calling the method from a different thread than it was created on
            if (toolStrip.InvokeRequired)
            {
                voidDelegate s = new voidDelegate(DisableButtons);
                Invoke(s, new object[] { });
            }
            else
            {
                // Disable Buttons
                btnStart.Enabled = false;
                btnStop.Enabled = false;
                btnInsertPlate.Enabled = false;
                btnEjectPlate.Enabled = false;
                btnApplyProtocol.Enabled = false;
                btnResetProtocol.Enabled = false;
                btnSaveData.Enabled = false;

            }
        }

        private void StateError()
        {
            if (toolStrip.InvokeRequired)
            {
                voidDelegate s = new voidDelegate(StateError);
                Invoke(s, new object[] { });
            }
            else
            {
                // Disable All
                instrument.ActiveScan = false;
                StateDisableAll();
                ResetDataCharts();

                // Enable Reset Btn
                tabControl.SelectedTab = tabAssayProtocol;
                instrument.SetInstrumentStatus(20);

                MessageBox.Show("XY Stage Error! Please check stage and restart instrument", "Warning");

            }
        }

        private void StateReset()
        {
            if (toolStrip.InvokeRequired)
            {
                voidDelegate s = new voidDelegate(StateReset);
                Invoke(s, new object[] { });
            }
            else
            {
                // Disable All
                instrument.ActiveScan = false;
                StateDisableAll();
                ResetDataCharts();

                // Enable Reset Btn
                tabControl.SelectedTab = tabAssayProtocol;
                instrument.SetInstrumentStatus(20);

                MessageBox.Show("Instrument Not Found! Please check Power and USB before re-starting software", "Warning");

            }
        }

        private void StateCreateProtocol()
        {
            if (toolStrip.InvokeRequired)
            {
                voidDelegate s = new voidDelegate(StateCreateProtocol);
                Invoke(s, new object[] { });
            }
            else
            {
                // Disable All
                instrument.ActiveScan = false;
                StateDisableAll();

                // Enable Tab Assay
                ((Control)this.tabAssayProtocol).Enabled = true;
                tabControl.SelectedTab = tabAssayProtocol;

                btnEjectPlate.Enabled = true;

                instrument.SetInstrumentStatus(4);

                if (plateSetup.WellsSelected)
                {
                    EnableApplyProtocol();
                }
            }
        }

        private void StateDeviceActive()
        {
            if (toolStrip.InvokeRequired)
            {
                voidDelegate s = new voidDelegate(StateDeviceActive);
                Invoke(s, new object[] { });
            }
            else
            {
                // Disable All
                instrument.ActiveScan = true;
                StateDisableAll();

                // Btns
                btnStart.Enabled = true;
                btnEjectPlate.Enabled = true;
                btnResetProtocol.Enabled = true;

                // Enable Tab Results
                ((Control)this.tabMeasure).Enabled = true;
                tabControl.SelectedTab = tabMeasure;

            }
        }

        private void StatePlateEjected()
        {
            // Invoked Required returns true if calling the method from a different thread than it was created on
            if (toolStrip.InvokeRequired)
            {
                voidDelegate s = new voidDelegate(StatePlateEjected);
                Invoke(s, new object[] { });
            }
            else
            {
                // Disable All
                StateDisableAll();

                // Btns
                btnInsertPlate.Enabled = true;

                instrument.SetInstrumentStatus(19);
            }
        }

        private void StateScanningPlate()
        {
            // Invoked Required returns true if calling the method from a different thread than it was created on
            if (toolStrip.InvokeRequired)
            {
                voidDelegate s = new voidDelegate(StatePlateEjected);
                Invoke(s, new object[] { });
            }
            else
            {
                // Disable Buttons
                instrument.ActiveScan = true;
                DisableButtons();
                tabControl.SelectedTab = tabMeasure;
            }
        }


        // Thread Safe: Charts, Labels
        private void UpdateDataChart(int scan, int row, int columnIndex)
        {
            if (chartResultMap.InvokeRequired)
            {
                intDelegate d = new intDelegate(UpdateDataChart);
                Invoke(d, new object[] { scan, row, columnIndex });
            }
            else
            {
                // Variables
                int value = cboPlotSelection.SelectedIndex;
                int thermal = cboThermalSelection.SelectedIndex;
                string wellName = dataExport.ConvertRow(row) + (columnIndex + 1).ToString();
                int well_index;
                int total_index;
                int wells;

                charting.FindHeatMapColors(value, data.block.Data[scan][value]);

                // Well Image == 3, Other plots 
                if(instrument.scanType == 3)
                {
                    ChartResultMap_ImagePaste();
                    well_index = (row * image.ColumnPixel) + columnIndex;
                    wells = image.Wells;
                }
                else
                {
                    ChartResultMap_ActivePaste();
                    well_index = (row * microplate.plate.Column) + columnIndex;
                    wells = microplate.plate.Wells;
                }

                if (PlotWaveforms)
                {
                    ChartThermalMap_ActivePaste();
                }

                // Waveform Chart
                if (well_index >= 0)
                {
                    total_index = (scan * wells) + well_index;

                    ChartResultMap_MarkerPaste(row, columnIndex);
                    //if (!instrument.ActiveScan)
                    //{
                        WaveformChart_ActivePaste(total_index, wellName);
                    //}              

                    //if (PlotWaveforms)
                    //{
                        ChartThermalMap_MarkerPaste(row, columnIndex);
                        ThermalChart_ActivePaste(well_index, wellName);
                    //}
                }

            }
        }

        private void UpdateUILabels()
        {
            if (toolStrip.InvokeRequired)
            {
                voidDelegate d = new voidDelegate(UpdateUILabels);
                Invoke(d, new object[] { });
            }
            else
            {
                // Status Label
                labelStatus.Text = instrument.InstrumentStatus;

                // Scan Type
                labelCurrentScan.Text = (instrument.CurrentScan + 1).ToString();
                labelTotalScans.Text = instrument.NScans.ToString();                

                // Clock
                labelClock.Text = time.PlateTime;
                labelPlateTime.Text = time.PlateTime;
                labelScanTime.Text = time.ScanTime;

                // Heat Map Chart
                lbLegendMin.Text = charting.MinLabel;
                lbLegendMax.Text = charting.MaxLabel;

                // Row and Column Offset position (Well Image)
                labelRowOffset.Text = instrument.rowOffset;
                labelColumnOffset.Text = instrument.columnOffset;

                // Object and Heatsink temperatures.
                textObjectTemp.Text = ms.ObjectTemp.ToString("F1");
                textHeatsink.Text = ms.HeatsinkTemp.ToString("F1");

                // Update
                toolStrip.Update();
                lbLegendMin.Update();
                lbLegendMax.Update();


            }

        }

        private void EnableStopBtn()
        {
            if (toolStrip.InvokeRequired)
            {
                voidDelegate d = new voidDelegate(EnableStopBtn);
                Invoke(d, new object[] { });
            }
            else
            {
                btnStop.Enabled = true;
            }
        }

        private void EnableSaveBtn()
        {
            if (toolStrip.InvokeRequired)
            {
                voidDelegate d = new voidDelegate(EnableSaveBtn);
                Invoke(d, new object[] { });
            }
            else
            {
                btnSaveData.Enabled = true;
                toolStrip.Update();
            }
        }


        // Reset Charts
        private void ResetDataCharts()
        {

            // Reset Data, buttons, wells selected
            data.ResetData();

            instrument.Autoscale = true;
            btnAutoscale.Text = "AutoScale On";

            // Reset Heat Map combo box
            cboPlotSelection.Items.Clear();
            foreach (var x in instrument.PlotOptions)
                cboPlotSelection.Items.Add(x);

            cboPlotSelection.SelectedIndex = 0;

            // Heat Map Result Chart Reset
            ChartResultMap_Update();
            ChartResultMap_NullPaste();

            ChartThermalMap_Update();
            ChartThermalMap_NullPaste();


            // Legend Chart
            ChartLegend_Update();

            // Waveform Result Chart Reset
            WaveformChart_Update();
            WaveformChart_NullPaste();

            // Thermal Chart Reset
            ThermalChart_Update();
            ThermalChart_NullPaste();

            // Reset Label Variables
            time.PlateTime = "0:00";
            time.ScanTime = "0:00";

            instrument.CurrentScan = 0;
            instrument.NScans = 1;

            charting.MinLabel = "";
            charting.MaxLabel = "";
        }

        private void ResetProtocolCharts()
        {
            instrument.ActiveProtocol = false;
            plateSetup.WellsSelected = false;
            DisableApplyProtocol();
            ChartPlate_Update();
            ChartPlate_NullPaste();

        }


        // Microplate Well Selection Chart
        private void cboPlateFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get Value of the plateformat ComboBox
            instrument.plateType = cboPlateFormat.SelectedIndex;
            instrument.scanType = cboScanType.SelectedIndex;

            // Get value of scan mode
            int scanType = cboScanType.SelectedIndex;
            int plot = instrument.plateType;

            // Well Image has a different plot parameter
            if (scanType == 3)
            {
                plot = 3;
            }

            // Update Current Microplate (Microplate and Charting)
            microplate.SetCurrentPlate(instrument.plateType);
            charting.SetCurrentChart(instrument.plateType, plot);

            // Reset Charts
            ResetDataCharts();
            ResetProtocolCharts();

            instrument.SetInstrumentStatus(4);

        }

        private void ChartPlate_Update()
        {
            // Clear All Series
            chartPlate.Series.Clear();
            chartPlate.Legends.Clear();
            chartPlate.ChartAreas.Clear();

            // Row/Col
            int row = charting.chartParameters.row;
            int col = charting.chartParameters.column;

            // Initialize axis
            ChartArea chartArea = chartPlate.ChartAreas.Add("chartArea");
            CustomLabel customLabel;

            // Enable Secondary Axis
            chartArea.AxisX2.Enabled = AxisEnabled.True;
            chartArea.AxisY2.Enabled = AxisEnabled.True;

            // Axis Line color and width
            chartArea.AxisX.LineColor = Color.Black;
            chartArea.AxisY.LineColor = Color.Black;
            chartArea.AxisX2.LineColor = Color.Black;
            chartArea.AxisY2.LineColor = Color.Black;

            chartArea.AxisX.LineWidth = 1;
            chartArea.AxisY.LineWidth = 1;
            chartArea.AxisX2.LineWidth = 1;
            chartArea.AxisY2.LineWidth = 1;

            // Form and Chart Area Color
            chartArea.BackColor = Color.White;
            chartPlate.BackColor = Color.White;

            // Enable/Disable Tick Marks
            chartArea.AxisX.MajorTickMark.Enabled = false;
            chartArea.AxisY.MajorTickMark.Enabled = false;
            chartArea.AxisX2.MajorTickMark.Enabled = false;
            chartArea.AxisY2.MajorTickMark.Enabled = false;
            chartArea.AxisX2.MinorTickMark.Enabled = false;
            chartArea.AxisY2.MinorTickMark.Enabled = false;

            // Grid Lines False
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisX.MinorGrid.Enabled = false;
            chartArea.AxisY.MajorGrid.Enabled = false;
            chartArea.AxisY.MinorGrid.Enabled = false;
            chartArea.AxisX2.MajorGrid.Enabled = false;
            chartArea.AxisX2.MinorGrid.Enabled = false;
            chartArea.AxisY2.MajorGrid.Enabled = false;
            chartArea.AxisY2.MinorGrid.Enabled = false;

            // Turn off Labels on secondary axis
            chartArea.AxisX2.LabelStyle.Enabled = false;
            chartArea.AxisY2.LabelStyle.Enabled = false;

            // Axis
            chartArea.AxisX.Minimum = 0;
            chartArea.AxisY.Minimum = 0;
            chartArea.AxisY.IntervalOffset = 1;
            chartArea.AxisX.IntervalOffset = 1;
            chartArea.AxisX.Interval = 1;
            chartArea.AxisY.Interval = 1;

            chartArea.AxisY.IsReversed = true;

            chartArea.AxisX.Maximum = col + 1;
            chartArea.AxisY.Maximum = row + 1;

            chartArea.AxisX.LabelAutoFitMinFontSize = charting.chartParameters.xFontSize;
            chartArea.AxisY.LabelAutoFitMinFontSize = charting.chartParameters.yFontSize;

            // Row Labels
            double temp = charting.chartParameters.rowIntervalStart;
            int increment1 = charting.chartParameters.rowIncrement1;
            int increment2 = charting.chartParameters.rowIncrement2;

            foreach (string rowLabel in charting.chartParameters.rowLabels)
            {
                customLabel = new CustomLabel(temp, temp + increment1, rowLabel, 0, LabelMarkStyle.None);
                chartArea.AxisY.CustomLabels.Add(customLabel);
                temp += increment2;
            }

            // Column Labels
            temp = charting.chartParameters.columnIntervalStart;
            increment1 = charting.chartParameters.columnIncrement1;
            increment2 = charting.chartParameters.columnIncrement2;

            foreach (string columnLabel in charting.chartParameters.columnLabels)
            {
                customLabel = new CustomLabel(temp, temp + increment1, columnLabel, 0, LabelMarkStyle.None);
                chartArea.AxisX.CustomLabels.Add(customLabel);
                temp += increment2;
            }

        }

        private void ChartPlate_NullPaste()
        {
            // Clear All Series
            chartPlate.Series.Clear();
            chartPlate.Legends.Clear();

            // Create Series
            Series S1 = chartPlate.Series.Add("S1");
            S1.ChartType = SeriesChartType.Point;
            int pt;

            // Plate Information
            int row = charting.chartParameters.row;
            int column = charting.chartParameters.column;
            int markerSize = charting.chartParameters.wsMarkerSize;

            // Paste Null Values
            for (int i = 0; i < row; i++)
                for (int j = 0; j < column; j++)
                {
                    pt = S1.Points.AddXY(j + 1, i + 1);
                    S1.Points[pt].MarkerStyle = charting.chartParameters.wsMarkerStyle;
                    S1.Points[pt].MarkerColor = charting.chartParameters.wsNullColor;
                    S1.Points[pt].MarkerSize = markerSize;
                }

        }

        private void ChartPlate_ActivePaste()
        {

            // Clear All Series
            chartPlate.Series.Clear();
            chartPlate.Legends.Clear();
            ChartArea chartArea = chartPlate.ChartAreas[0];

            chartArea.CursorX.LineWidth = 0;
            chartArea.CursorY.LineWidth = 0;

            // Create Series
            Series S1 = chartPlate.Series.Add("S1");
            S1.ChartType = SeriesChartType.Point;
            int pt;

            // Variables
            int row = charting.chartParameters.row;
            int column = charting.chartParameters.column;
            int markerSize = charting.chartParameters.wsMarkerSize;

            // Get Scan Type
            int value = cboScanType.SelectedIndex;

            if(value > 1)
            {
                // Set Row Min = Row Max and Column Min = Column Max
                plateSetup.RowSelection1 = plateSetup.RowMin;
                plateSetup.RowSelection2 = plateSetup.RowMin;

                plateSetup.ColumnSelection1 = plateSetup.ColumnMin;
                plateSetup.ColumnSelection2 = plateSetup.ColumnMin;

                // Set new active wells (only 1 well)
                plateSetup.SetActiveWells(microplate.plate.Row, microplate.plate.Column);
            }


            // Paste Values
            for (int i = 0; i < row; i++)
                for (int j = 0; j < column; j++)
                {
                    // Paste Markers
                    pt = S1.Points.AddXY(j + 1, i + 1);
                    S1.Points[pt].MarkerStyle = charting.chartParameters.wsMarkerStyle;
                    S1.Points[pt].MarkerSize = markerSize;

                    if (plateSetup.ActiveRow[i] && plateSetup.ActiveColumn[j])
                    {
                        // Paste Active Color
                        S1.Points[pt].MarkerColor = charting.chartParameters.wsActiveColor;
                    }
                    else
                    {
                        // Paste Null Color
                        S1.Points[pt].MarkerColor = charting.chartParameters.wsNullColor;
                    }
                }
        }

        private void chartPlate_MouseDown(object sender, MouseEventArgs e)
        {
            ChartArea chartArea = chartPlate.ChartAreas[0];

            // Row/Col
            int row = microplate.plate.Row;
            int col = microplate.plate.Column;

            if (!plateSetup.WellsSelected)
            {
                chartArea.CursorX.LineColor = Color.Black;
                chartArea.CursorY.LineColor = Color.Black;

                chartArea.CursorX.LineWidth = 1;
                chartArea.CursorY.LineWidth = 1;

                chartArea.CursorX.SetCursorPixelPosition(new Point(e.X, e.Y), true);
                chartArea.CursorY.SetCursorPixelPosition(new Point(e.X, e.Y), true);

                double pX = chartArea.CursorX.Position; // X Axis Coordinate of your mouse cursor
                double pY = chartArea.CursorY.Position; // Y Axis Coordinate of your mouse cursor

                // Verify the cursor is inside the chart
                if (pX < 1) { chartArea.CursorX.Position = 1; pX = 1; }
                if (pY < 1) { chartArea.CursorY.Position = 1; pY = 1; }
                if (pX > col) { chartArea.CursorX.Position = col; pX = col; }
                if (pY > row) { chartArea.CursorY.Position = row; pY = row; }

                // Selection 1 Values (Subtract 1 to make index start at 0)
                plateSetup.RowSelection1 = (int)pY - 1;
                plateSetup.ColumnSelection1 = (int)pX - 1;

            }
        }

        private void chartPlate_MouseMove(object sender, MouseEventArgs e)
        {
            ChartArea chartArea = chartPlate.ChartAreas[0];
            int size = 0;

            if (!plateSetup.WellsSelected)
            {
                size = 1;
            }

            // Row/Col
            int row = microplate.plate.Row;
            int col = microplate.plate.Column;

            chartArea.CursorX.LineColor = charting.chartParameters.wsActiveColor;
            chartArea.CursorY.LineColor = charting.chartParameters.wsActiveColor;

            chartArea.CursorX.LineWidth = size;
            chartArea.CursorY.LineWidth = size;

            chartArea.CursorX.SetCursorPixelPosition(new Point(e.X, e.Y), true);
            chartArea.CursorY.SetCursorPixelPosition(new Point(e.X, e.Y), true);

            double pX = chartArea.CursorX.Position; // X Axis Coordinate of your mouse cursor
            double pY = chartArea.CursorY.Position; // Y Axis Coordinate of your mouse cursor

            // Verify the cursor is inside the chart
            if (pX < 1) { chartArea.CursorX.Position = 1; pX = 1; }
            if (pY < 1) { chartArea.CursorY.Position = 1; pY = 1; }
            if (pX > col) { chartArea.CursorX.Position = col; pX = col; }
            if (pY > row) { chartArea.CursorY.Position = row; pY = row; }

            // Update Textbox
            lbColumnWellSelection.Text = pX.ToString();
            lbRowWellSelection.Text = dataExport.ConvertRow((int)pY - 1);

            lbColumnWellSelection.Update();
            lbRowWellSelection.Update();
        }

        private void chartPlate_MouseUp(object sender, MouseEventArgs e)
        {
            ChartArea chartArea = chartPlate.ChartAreas[0];

            // Row/Col
            int row = microplate.plate.Row;
            int col = microplate.plate.Column;

            if (!plateSetup.WellsSelected)
            {

                chartArea.CursorX.LineColor = Color.Black;
                chartArea.CursorY.LineColor = Color.Black;

                chartArea.CursorX.LineWidth = 0;
                chartArea.CursorY.LineWidth = 0;

                chartArea.CursorX.SetCursorPixelPosition(new Point(e.X, e.Y), true);
                chartArea.CursorY.SetCursorPixelPosition(new Point(e.X, e.Y), true);

                double pX = chartArea.CursorX.Position; // X Axis Coordinate of your mouse cursor
                double pY = chartArea.CursorY.Position; // Y Axis Coordinate of your mouse cursor

                // Verify the cursor is inside the chart
                if (pX < 1) { chartArea.CursorX.Position = 1; pX = 1; }
                if (pY < 1) { chartArea.CursorY.Position = 1; pY = 1; }
                if (pX > col) { chartArea.CursorX.Position = col; pX = col; }
                if (pY > row) { chartArea.CursorY.Position = row; pY = row; }

                // Selection 2 Values (Subtract 1 to make index start at 0)
                plateSetup.RowSelection2 = (int)pY - 1;
                plateSetup.ColumnSelection2 = (int)pX - 1;

                // Set Active Wells (Wells to scan with the scan loop)
                plateSetup.SetActiveWells(row, col);

                // Past Well Selection Area
                ChartPlate_ActivePaste();

                // Check Apply Btn Protocol
                plateSetup.WellsSelected = true;
                EnableApplyProtocol();
            }
        }

        private void btnWellSelectionAll_Click(object sender, EventArgs e)
        {
            int row = microplate.plate.Row;
            int col = microplate.plate.Column;

            // Set Well Selection Values
            plateSetup.ColumnSelection1 = 0;
            plateSetup.ColumnSelection2 = col - 1;

            plateSetup.RowSelection1 = 0;
            plateSetup.RowSelection2 = row - 1;

            // Paste Active
            plateSetup.SetActiveWells(row, col);
            ChartPlate_ActivePaste();

            // Set Wells Selected Well Click
            plateSetup.WellsSelected = true;
            EnableApplyProtocol();
        }

        private void btnWellSelectionReset_Click(object sender, EventArgs e)
        {

            // Microplate Well Selection Chart Reset
            ResetProtocolCharts();

        }


        // Microplate Heat Map Chart
        private void cboPlotSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (data.DataAvailable)
            {
                // Plot Data
                UpdateDataChart(instrument.CurrentScan, -1, -1);
            }
        }

        private void ChartResultMap_Update()
        {
            // Clear All Series
            chartResultMap.Series.Clear();
            chartResultMap.Legends.Clear();
            chartResultMap.ChartAreas.Clear();
            
            // Size
            if(instrument.scanType == 3)
            {
                chartResultMap.Size = new Size(450, 450);
            }
            else
            {
                chartResultMap.Size = new Size(648, 415);
            }

            // Row/Col
            int row = charting.plotParameters.row;
            int col = charting.plotParameters.column;

            // Initialize axis
            ChartArea chartArea = chartResultMap.ChartAreas.Add("chartArea");
            CustomLabel customLabel;

            // Enable Secondary Axis
            chartArea.AxisX2.Enabled = AxisEnabled.True;
            chartArea.AxisY2.Enabled = AxisEnabled.True;

            // Axis Line color and width
            chartArea.AxisX.LineColor = Color.Black;
            chartArea.AxisY.LineColor = Color.Black;
            chartArea.AxisX2.LineColor = Color.Black;
            chartArea.AxisY2.LineColor = Color.Black;

            chartArea.AxisX.LineWidth = 1;
            chartArea.AxisY.LineWidth = 1;
            chartArea.AxisX2.LineWidth = 1;
            chartArea.AxisY2.LineWidth = 1;

            // Form and Chart Area Color
            chartResultMap.BackColor = Color.White;

            // Enable/Disable Tick Marks
            chartArea.AxisX.MajorTickMark.Enabled = false;
            chartArea.AxisY.MajorTickMark.Enabled = false;
            chartArea.AxisX2.MajorTickMark.Enabled = false;
            chartArea.AxisY2.MajorTickMark.Enabled = false;
            chartArea.AxisX2.MinorTickMark.Enabled = false;
            chartArea.AxisY2.MinorTickMark.Enabled = false;

            // Grid Lines False
            chartArea.AxisX.MajorGrid.Enabled = true;
            chartArea.AxisX.MinorGrid.Enabled = false;
            chartArea.AxisY.MajorGrid.Enabled = true;
            chartArea.AxisY.MinorGrid.Enabled = false;
            chartArea.AxisX2.MajorGrid.Enabled = false;
            chartArea.AxisX2.MinorGrid.Enabled = false;
            chartArea.AxisY2.MajorGrid.Enabled = false;
            chartArea.AxisY2.MinorGrid.Enabled = false;

            // Grid Lines size
            chartArea.AxisX.MajorGrid.LineWidth = 1;
            chartArea.AxisY.MajorGrid.LineWidth = 1;

            // Turn off Labels on secondary axis
            chartArea.AxisX2.LabelStyle.Enabled = false;
            chartArea.AxisY2.LabelStyle.Enabled = false;

            // Axis
            chartArea.AxisX.Minimum = 0.5;
            chartArea.AxisY.Minimum = 0.5;
            chartArea.AxisY.IntervalOffset = 1;
            chartArea.AxisX.IntervalOffset = 1;
            chartArea.AxisX.Interval = 1;
            chartArea.AxisY.Interval = 1;

            chartArea.AxisY.IsReversed = true;

            chartArea.AxisX.Maximum = col + 0.5;
            chartArea.AxisY.Maximum = row + 0.5;

            chartArea.AxisX.LabelAutoFitMinFontSize = charting.plotParameters.xFontSize;
            chartArea.AxisY.LabelAutoFitMinFontSize = charting.plotParameters.yFontSize;

            // Row Labels
            double temp = charting.plotParameters.rowIntervalStart;
            int increment1 = charting.plotParameters.rowIncrement1;
            int increment2 = charting.plotParameters.rowIncrement2;

            foreach (string rowLabel in charting.plotParameters.rowLabels)
            {
                customLabel = new CustomLabel(temp, temp + increment1, rowLabel, 0, LabelMarkStyle.None);
                chartArea.AxisY.CustomLabels.Add(customLabel);
                temp += increment2;
            }

            // Column Labels
            temp = charting.plotParameters.columnIntervalStart;
            increment1 = charting.plotParameters.columnIncrement1;
            increment2 = charting.plotParameters.columnIncrement2;

            foreach (string columnLabel in charting.plotParameters.columnLabels)
            {
                customLabel = new CustomLabel(temp, temp + increment1, columnLabel, 0, LabelMarkStyle.None);
                chartArea.AxisX.CustomLabels.Add(customLabel);
                temp += increment2;
            }

        }

        private void ChartLegend_Update()
        {
            // Clear All Series
            chartLegend.Series.Clear();
            chartLegend.Legends.Clear();
            chartLegend.ChartAreas.Clear();

            // Initialize axis
            ChartArea chartArea = chartLegend.ChartAreas.Add("chartArea");

            // Disable Axis Lines
            chartArea.AxisX.LineWidth = 0;
            chartArea.AxisY.LineWidth = 0;
            chartArea.AxisX2.LineWidth = 0;
            chartArea.AxisY2.LineWidth = 0;

            // Form and Chart Area Color
            chartArea.BackColor = Color.White;

            // Disable Tick Marks
            chartArea.AxisX.MajorTickMark.Enabled = false;
            chartArea.AxisY.MajorTickMark.Enabled = false;
            chartArea.AxisX2.MajorTickMark.Enabled = false;
            chartArea.AxisY2.MajorTickMark.Enabled = false;
            chartArea.AxisX2.MinorTickMark.Enabled = false;
            chartArea.AxisY2.MinorTickMark.Enabled = false;

            // Disable Grid Lines
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisX.MinorGrid.Enabled = false;
            chartArea.AxisY.MajorGrid.Enabled = false;
            chartArea.AxisY.MinorGrid.Enabled = false;
            chartArea.AxisX2.MajorGrid.Enabled = false;
            chartArea.AxisX2.MinorGrid.Enabled = false;
            chartArea.AxisY2.MajorGrid.Enabled = false;
            chartArea.AxisY2.MinorGrid.Enabled = false;

            // Disable Labels
            chartArea.AxisX.LabelStyle.Enabled = false;
            chartArea.AxisY.LabelStyle.Enabled = false;
            chartArea.AxisX2.LabelStyle.Enabled = false;
            chartArea.AxisY2.LabelStyle.Enabled = false;

            // Axis
            chartArea.AxisX.Minimum = -1;
            chartArea.AxisY.Minimum = 0;

            chartArea.AxisX.Maximum = 11;
            chartArea.AxisY.Maximum = 1;

            // Add Points and values
            int pt;
            Series S = chartLegend.Series.Add("S");
            S.ChartType = SeriesChartType.Point;

            int interval = (int)(charting.ColorList.Count / 9);

            for (int i = 0; i < 10; i++)
            {
                pt = S.Points.AddXY(i, 0.5);
                S.Points[pt].MarkerStyle = MarkerStyle.Square;
                S.Points[pt].MarkerColor = charting.ColorList[interval * i];
                S.Points[pt].MarkerSize = 20;
            }

        }

        private void ChartResultMap_NullPaste()
        {
            // Clear All Series
            chartResultMap.Series.Clear();
            chartResultMap.Legends.Clear();

            // Create Series
            Series S1 = chartResultMap.Series.Add("S1");
            S1.ChartType = SeriesChartType.Point;
            int pt;

            // Paste the Null Data
            int row = charting.plotParameters.row;
            int column = charting.plotParameters.column;

            for (int i = 0; i < row; i++)
                for (int j = 0; j < column; j++)
                {
                    pt = S1.Points.AddXY(j + 1, i + 1);
                    S1.Points[pt].MarkerStyle = charting.plotParameters.wsMarkerStyle;
                    S1.Points[pt].MarkerColor = charting.plotParameters.wsNullColor;
                    S1.Points[pt].MarkerSize = 0;
                }
        }

        private void ChartResultMap_ActivePaste()
        {

            // Clear All Series
            chartResultMap.Series.Clear();
            chartResultMap.Legends.Clear();

            // Create Series
            Series S1 = chartResultMap.Series.Add("S1");
            S1.ChartType = SeriesChartType.Point;
            Series S2 = chartResultMap.Series.Add("S2");
            S2.ChartType = SeriesChartType.Point;

            // Variables
            int pt;
            int row = charting.plotParameters.row;
            int column = charting.plotParameters.column;
            int lenPlate = charting.plotParameters.wells;

            // Get the colors
            int[] colors = new int[lenPlate];
            colors = charting.ColorValue;

            // Paste Values
            for (int i = 0; i < row; i++)
                for (int j = 0; j < column; j++)
                {
                    if (plateSetup.ActiveRow[i] && plateSetup.ActiveColumn[j])
                    {
                        // Paste Active Color
                        int index = (i * column) + j;
                        pt = S1.Points.AddXY(j + 1, i + 1);
                        S1.Points[pt].MarkerStyle = charting.plotParameters.dataMarkerStyle;
                        S1.Points[pt].MarkerColor = Color.FromArgb(150, charting.ColorList[colors[index]]);
                        S1.Points[pt].MarkerSize = charting.plotParameters.dataMarkerSize;
                    }
                    else
                    {
                        // Paste Null Color
                        pt = S1.Points.AddXY(j + 1, i + 1);
                        S1.Points[pt].MarkerStyle = charting.plotParameters.dataMarkerStyle;
                        S1.Points[pt].MarkerColor = Color.White;
                        S1.Points[pt].MarkerSize = 0;
                    }
                }



            chartResultMap.Update();


        }

        private void ChartResultMap_ImagePaste()
        {

            // Clear All Series
            chartResultMap.Series.Clear();
            chartResultMap.Legends.Clear();

            // Create Series
            Series S1 = chartResultMap.Series.Add("S1");
            S1.ChartType = SeriesChartType.Point;
            Series S2 = chartResultMap.Series.Add("S2");
            S2.ChartType = SeriesChartType.Point;

            // Variables
            int pt;
            int row = charting.plotParameters.row;
            int column = charting.plotParameters.column;
            int lenPlate = charting.plotParameters.wells;

            // Get the colors
            int[] colors = new int[lenPlate];
            colors = charting.ColorValue;

            // Paste Values
            for (int i = 0; i < row; i++)
                for (int j = 0; j < column; j++)
                {
                    // Paste Active Color
                    int index = (i * column) + j;
                    pt = S1.Points.AddXY(j + 1, i + 1);
                    S1.Points[pt].MarkerStyle = charting.plotParameters.dataMarkerStyle;
                    S1.Points[pt].MarkerColor = Color.FromArgb(150, charting.ColorList[colors[index]]);
                    S1.Points[pt].MarkerSize = charting.plotParameters.dataMarkerSize;                   

                }



            chartResultMap.Update();


        }

        private void ChartResultMap_MarkerPaste(int row, int column)
        {
            // Paint Well Marker
            int markerSize = charting.plotParameters.markerMarkerSize;
            int pt = chartResultMap.Series["S2"].Points.AddXY(column + 1, row + 1);
            chartResultMap.Series["S2"].Points[pt].MarkerStyle = MarkerStyle.Cross;
            chartResultMap.Series["S2"].Points[pt].MarkerColor = Color.Black;
            chartResultMap.Series["S2"].Points[pt].MarkerSize = markerSize;

        }

        private void chartResultMap_MouseMove(object sender, MouseEventArgs e)
        {
            ChartArea chartArea = chartResultMap.ChartAreas[0];

            // Variables
            int row;
            int col;

            if(instrument.scanType == 3)
            {
                row = image.RowPixel;
                col = image.ColumnPixel;
            }
            else
            {
                row = microplate.plate.Row;
                col = microplate.plate.Column;
            }


            chartArea.CursorX.LineWidth = 0;
            chartArea.CursorY.LineWidth = 0;

            chartArea.CursorX.SetCursorPixelPosition(new Point(e.X, e.Y), true);
            chartArea.CursorY.SetCursorPixelPosition(new Point(e.X, e.Y), true);

            double pX = chartArea.CursorX.Position; //X Axis Coordinate of your mouse cursor
            double pY = chartArea.CursorY.Position; //Y Axis Coordinate of your mouse cursor

            // Verify he cursor is inside the chart
            if (pX < 1) { chartArea.CursorX.Position = 1; pX = 1; }
            if (pY < 1) { chartArea.CursorY.Position = 1; pY = 1; }
            if (pX > col) { chartArea.CursorX.Position = col; pX = col; }
            if (pY > row) { chartArea.CursorY.Position = row; pY = row; }

            // Update Textbox
            if(instrument.scanType == 3)
            {
                // Cursor Position
                int x = (int)(pX - 1);
                int y = (int)(pY - 1);

                double cOffset = image.motor.ColumnPosition[x] - (info.ColumnDirection * image.plate.ColumnOffset);
                double rOffset = image.motor.RowPosition[y] - (info.RowDirection * image.plate.RowOffset);

                // Well Selected
                int cSelected = plateSetup.Column;
                int rSelected = plateSetup.Row;

                double cSpacing = microplate.plate.ColumnSpacing;
                double rSpacing = microplate.plate.RowSpacing;


                // Adjust for Well Location
                cOffset += (cSelected * cSpacing);
                rOffset += (rSelected * rSpacing);

                instrument.rowOffset = rOffset.ToString();
                instrument.columnOffset = cOffset.ToString();

                lbColumn.Text = pX.ToString();
                lbRow.Text = pY.ToString();
            }
            else
            {
                lbColumn.Text = pX.ToString();
                lbRow.Text = dataExport.ConvertRow((int)pY - 1);
            }


            lbColumn.Update();
            lbRow.Update();

        }

        private void chartResultMap_Click(object sender, EventArgs e)
        {
            if (data.DataAvailable)
            {
                // Variables
                ChartArea chartArea = chartResultMap.ChartAreas[0];

                int rowMin;
                int rowMax;
                int columnMin;
                int columnMax;

                // Differente min/max for well image
                if (instrument.scanType == 3)
                {
                    rowMin = 1;
                    rowMax = image.RowPixel;

                    columnMin = 1;
                    columnMax = image.ColumnPixel;
                }
                else
                {
                    rowMin = plateSetup.RowMin + 1;
                    rowMax = plateSetup.RowMax + 1;

                    columnMin = plateSetup.ColumnMin + 1;
                    columnMax = plateSetup.ColumnMax + 1;
                }

                // Get Cursor Position
                double pX = chartArea.CursorX.Position; //X Axis Coordinate of your mouse cursor
                double pY = chartArea.CursorY.Position; //Y Axis Coordinate of your mouse cursor

                // Verify the cursor is inside the chart
                if (pX < columnMin) { pX = columnMin; }
                if (pX > columnMax) { pX = columnMax; }
                if (pY < rowMin) { pY = rowMin; }
                if (pY > rowMax) { pY = rowMax; }


                // Update Plot Value Textboxes (if PlotData > 0)
                int sColumn = (int)pX - 1;
                int sRow = (int)pY - 1;

                UpdateDataChart(instrument.CurrentScan, sRow, sColumn);

            }
        }






        private void ChartThermalMap_NullPaste()
        {
            // Clear All Series
            chartThermalMap.Series.Clear();
            chartThermalMap.Legends.Clear();

            // Create Series
            Series S1 = chartThermalMap.Series.Add("S1");
            S1.ChartType = SeriesChartType.Point;
            int pt;

            // Paste the Null Data
            int row = charting.plotParameters.row;
            int column = charting.plotParameters.column;

            for (int i = 0; i < row; i++)
                for (int j = 0; j < column; j++)
                {
                    pt = S1.Points.AddXY(j + 1, i + 1);
                    S1.Points[pt].MarkerStyle = charting.plotParameters.wsMarkerStyle;
                    S1.Points[pt].MarkerColor = charting.plotParameters.wsNullColor;
                    S1.Points[pt].MarkerSize = 0;
                }
        }

        private void ChartThermalMap_ActivePaste()
        {

            // Clear All Series
            chartThermalMap.Series.Clear();
            chartThermalMap.Legends.Clear();

            // Create Series
            Series S1 = chartThermalMap.Series.Add("S1");
            S1.ChartType = SeriesChartType.Point;
            Series S2 = chartThermalMap.Series.Add("S2");
            S2.ChartType = SeriesChartType.Point;

            // Variables
            int pt;
            int row = charting.plotParameters.row;
            int column = charting.plotParameters.column;
            int lenPlate = charting.plotParameters.wells;

            // Get the colors
            int[] colors = new int[lenPlate];
            colors = charting.ColorValue;

            // Paste Values
            for (int i = 0; i < row; i++)
                for (int j = 0; j < column; j++)
                {
                    //if (plateSetup.ActiveRow[i] && plateSetup.ActiveColumn[j])
                    //{
                    //    // Paste Active Color
                    //    int index = (i * column) + j;
                    //    pt = S1.Points.AddXY(j + 1, i + 1);
                    //    S1.Points[pt].MarkerStyle = charting.plotParameters.dataMarkerStyle;
                    //    S1.Points[pt].MarkerColor = Color.FromArgb(150, charting.ColorList[colors[index]]);
                    //    S1.Points[pt].MarkerSize = charting.plotParameters.dataMarkerSize;
                    //}
                    //else
                    //{
                    //    // Paste Null Color
                    //    pt = S1.Points.AddXY(j + 1, i + 1);
                    //    S1.Points[pt].MarkerStyle = charting.plotParameters.dataMarkerStyle;
                    //    S1.Points[pt].MarkerColor = Color.White;
                    //    S1.Points[pt].MarkerSize = 0;
                    //}
                }



            chartThermalMap.Update();


        }


        private void ChartThermalMap_MarkerPaste(int row, int column)
        {
            // Paint Well Marker
            int markerSize = charting.plotParameters.markerMarkerSize;
            int pt = chartThermalMap.Series["S2"].Points.AddXY(column + 1, row + 1);
            chartThermalMap.Series["S2"].Points[pt].MarkerStyle = MarkerStyle.Cross;
            chartThermalMap.Series["S2"].Points[pt].MarkerColor = Color.Black;
            chartThermalMap.Series["S2"].Points[pt].MarkerSize = markerSize;

        }

        private void chartThermalMap_MouseMove(object sender, MouseEventArgs e)
        {
            ChartArea chartArea = chartThermalMap.ChartAreas[0];

            // Variables
            int row;
            int col;

            row = microplate.plate.Row;
            col = microplate.plate.Column;

            chartArea.CursorX.LineWidth = 0;
            chartArea.CursorY.LineWidth = 0;

            chartArea.CursorX.SetCursorPixelPosition(new Point(e.X, e.Y), true);
            chartArea.CursorY.SetCursorPixelPosition(new Point(e.X, e.Y), true);

            double pX = chartArea.CursorX.Position; //X Axis Coordinate of your mouse cursor
            double pY = chartArea.CursorY.Position; //Y Axis Coordinate of your mouse cursor

            // Verify he cursor is inside the chart
            if (pX < 1) { chartArea.CursorX.Position = 1; pX = 1; }
            if (pY < 1) { chartArea.CursorY.Position = 1; pY = 1; }
            if (pX > col) { chartArea.CursorX.Position = col; pX = col; }
            if (pY > row) { chartArea.CursorY.Position = row; pY = row; }

            // Update Textbox
            lbColumn.Text = pX.ToString();
            lbRow.Text = dataExport.ConvertRow((int)pY - 1);

            lbColumn.Update();
            lbRow.Update();

        }

        private void chartThermalMap_Click(object sender, EventArgs e)
        {
            if (data.DataAvailable)
            {
                // Variables
                ChartArea chartArea = chartThermalMap.ChartAreas[0];

                int rowMin;
                int rowMax;
                int columnMin;
                int columnMax;

                // Differente min/max for well image
                rowMin = plateSetup.RowMin + 1;
                rowMax = plateSetup.RowMax + 1;

                columnMin = plateSetup.ColumnMin + 1;
                columnMax = plateSetup.ColumnMax + 1;

                // Get Cursor Position
                double pX = chartArea.CursorX.Position; //X Axis Coordinate of your mouse cursor
                double pY = chartArea.CursorY.Position; //Y Axis Coordinate of your mouse cursor

                // Verify the cursor is inside the chart
                if (pX < columnMin) { pX = columnMin; }
                if (pX > columnMax) { pX = columnMax; }
                if (pY < rowMin) { pY = rowMin; }
                if (pY > rowMax) { pY = rowMax; }


                // Update Plot Value Textboxes (if PlotData > 0)
                int sColumn = (int)pX - 1;
                int sRow = (int)pY - 1;

                UpdateDataChart(instrument.CurrentScan, sRow, sColumn);

            }
        }

        private void ChartThermalMap_Update()
        {
            // Clear All Series
            chartThermalMap.Series.Clear();
            chartThermalMap.Legends.Clear();
            chartThermalMap.ChartAreas.Clear();

            // Size
            if (instrument.scanType == 3)
            {
                chartThermalMap.Size = new Size(450, 450);
            }
            else
            {
                chartThermalMap.Size = new Size(648, 415);
            }

            // Row/Col
            int row = charting.plotParameters.row;
            int col = charting.plotParameters.column;

            // Initialize axis
            ChartArea chartArea = chartThermalMap.ChartAreas.Add("chartArea");
            CustomLabel customLabel;

            // Enable Secondary Axis
            chartArea.AxisX2.Enabled = AxisEnabled.True;
            chartArea.AxisY2.Enabled = AxisEnabled.True;

            // Axis Line color and width
            chartArea.AxisX.LineColor = Color.Black;
            chartArea.AxisY.LineColor = Color.Black;
            chartArea.AxisX2.LineColor = Color.Black;
            chartArea.AxisY2.LineColor = Color.Black;

            chartArea.AxisX.LineWidth = 1;
            chartArea.AxisY.LineWidth = 1;
            chartArea.AxisX2.LineWidth = 1;
            chartArea.AxisY2.LineWidth = 1;

            // Form and Chart Area Color
            chartThermalMap.BackColor = Color.White;

            // Enable/Disable Tick Marks
            chartArea.AxisX.MajorTickMark.Enabled = false;
            chartArea.AxisY.MajorTickMark.Enabled = false;
            chartArea.AxisX2.MajorTickMark.Enabled = false;
            chartArea.AxisY2.MajorTickMark.Enabled = false;
            chartArea.AxisX2.MinorTickMark.Enabled = false;
            chartArea.AxisY2.MinorTickMark.Enabled = false;

            // Grid Lines False
            chartArea.AxisX.MajorGrid.Enabled = true;
            chartArea.AxisX.MinorGrid.Enabled = false;
            chartArea.AxisY.MajorGrid.Enabled = true;
            chartArea.AxisY.MinorGrid.Enabled = false;
            chartArea.AxisX2.MajorGrid.Enabled = false;
            chartArea.AxisX2.MinorGrid.Enabled = false;
            chartArea.AxisY2.MajorGrid.Enabled = false;
            chartArea.AxisY2.MinorGrid.Enabled = false;

            // Grid Lines size
            chartArea.AxisX.MajorGrid.LineWidth = 1;
            chartArea.AxisY.MajorGrid.LineWidth = 1;

            // Turn off Labels on secondary axis
            chartArea.AxisX2.LabelStyle.Enabled = false;
            chartArea.AxisY2.LabelStyle.Enabled = false;

            // Axis
            chartArea.AxisX.Minimum = 0.5;
            chartArea.AxisY.Minimum = 0.5;
            chartArea.AxisY.IntervalOffset = 1;
            chartArea.AxisX.IntervalOffset = 1;
            chartArea.AxisX.Interval = 1;
            chartArea.AxisY.Interval = 1;

            chartArea.AxisY.IsReversed = true;

            chartArea.AxisX.Maximum = col + 0.5;
            chartArea.AxisY.Maximum = row + 0.5;

            chartArea.AxisX.LabelAutoFitMinFontSize = charting.plotParameters.xFontSize;
            chartArea.AxisY.LabelAutoFitMinFontSize = charting.plotParameters.yFontSize;

            // Row Labels
            double temp = charting.plotParameters.rowIntervalStart;
            int increment1 = charting.plotParameters.rowIncrement1;
            int increment2 = charting.plotParameters.rowIncrement2;

            foreach (string rowLabel in charting.plotParameters.rowLabels)
            {
                customLabel = new CustomLabel(temp, temp + increment1, rowLabel, 0, LabelMarkStyle.None);
                chartArea.AxisY.CustomLabels.Add(customLabel);
                temp += increment2;
            }

            // Column Labels
            temp = charting.plotParameters.columnIntervalStart;
            increment1 = charting.plotParameters.columnIncrement1;
            increment2 = charting.plotParameters.columnIncrement2;

            foreach (string columnLabel in charting.plotParameters.columnLabels)
            {
                customLabel = new CustomLabel(temp, temp + increment1, columnLabel, 0, LabelMarkStyle.None);
                chartArea.AxisX.CustomLabels.Add(customLabel);
                temp += increment2;
            }

        }










        // Fluorescence Spectrum Waveform Chart 
        private void WaveformChart_Update()
        {
            // Clear All Series
            chartWaveform.Series.Clear();
            chartWaveform.Legends.Clear();
            chartWaveform.ChartAreas.Clear();

            // Initialize axis
            ChartArea chartArea = chartWaveform.ChartAreas.Add("chartArea");

            // Axis Line color and width
            chartArea.AxisX.LineColor = Color.Black;
            chartArea.AxisY.LineColor = Color.Black;
            chartArea.AxisX2.LineColor = Color.Black;
            chartArea.AxisY2.LineColor = Color.Black;

            chartArea.AxisX.LineWidth = 1;
            chartArea.AxisY.LineWidth = 1;
            chartArea.AxisX2.LineWidth = 1;
            chartArea.AxisY2.LineWidth = 1;

            // Form and Chart Area Color
            chartResultMap.BackColor = Color.White;

            // Enable/Disable Tick Marks
            chartArea.AxisX.MajorTickMark.Enabled = true;
            chartArea.AxisY.MajorTickMark.Enabled = false;
            chartArea.AxisX2.MajorTickMark.Enabled = false;
            chartArea.AxisY2.MajorTickMark.Enabled = false;
            chartArea.AxisX2.MinorTickMark.Enabled = false;
            chartArea.AxisY2.MinorTickMark.Enabled = false;

            if (info.Detector == "TIA")
            {
                chartArea.AxisX.MajorTickMark.Interval = 1;
            }
            else
            {
                chartArea.AxisX.MajorTickMark.Interval = 10;
            }
            

            // Grid Lines X
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisX.MinorGrid.Enabled = false;
            chartArea.AxisX2.MajorGrid.Enabled = false;
            chartArea.AxisX2.MinorGrid.Enabled = false;

            if (info.Detector == "TIA")
            {
                chartArea.AxisX.MajorGrid.Interval = 2;
            }
            else
            {
                chartArea.AxisX.MajorGrid.Interval = 20;
            }
            
            chartArea.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            // Grid Lines Y
            chartArea.AxisY.MajorGrid.Enabled = false;
            chartArea.AxisY.MinorGrid.Enabled = false;
            chartArea.AxisY2.MajorGrid.Enabled = false;
            chartArea.AxisY2.MinorGrid.Enabled = false;

            chartArea.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            chartWaveform.ChartAreas[0].AxisY.Minimum = 0;
            chartWaveform.ChartAreas[0].AxisY.Maximum = 10;

            // FontSizes
            chartArea.IsSameFontSizeForAllAxes = true;
            chartArea.AxisX.LabelAutoFitMinFontSize = 12;
            chartArea.AxisX.TitleFont = new Font("Arial", 12, FontStyle.Bold);

            chartArea.AxisY.LabelAutoFitMinFontSize = 12;
            chartArea.AxisY.TitleFont = new Font("Arial", 12, FontStyle.Bold);

            // Set up the X Axis
            if (info.Detector == "TIA")
            {
                chartArea.AxisX.Title = "Time [ms]";

                chartArea.AxisX.Minimum = info.WavelengthStart;
                chartArea.AxisX.Maximum = info.WavelengthEnd;
                chartArea.AxisX.Interval = 2;

            }
            else
            {
                chartArea.AxisX.Title = "Wavelength [nm]";

                chartArea.AxisX.Minimum = info.WavelengthStart;
                chartArea.AxisX.Maximum = info.WavelengthEnd;
                chartArea.AxisX.Interval = 20;
            }

            // Set up the Y Axis
            chartArea.AxisY.Title = "Fluorescence";
            chartArea.AxisY.LabelStyle.Format = "#,##";

            chartWaveform.Update();
        }

        private void WaveformChart_NullPaste()
        {

            // Clear All Series
            chartWaveform.Series.Clear();
            chartWaveform.Legends.Clear();

            // Create Line Series
            string seriesName1 = "A1";

            Series S1 = chartWaveform.Series.Add(seriesName1);
            S1.ChartType = SeriesChartType.Line;
            S1.Color = Color.Black;
            S1.BorderWidth = 1;

            S1.MarkerColor = Color.Black;
            S1.MarkerSize = 4;
            S1.MarkerStyle = MarkerStyle.Circle;

            // Create Area Series
            string seriesName2 = "Intensity A [ ] = N/A";
            string seriesName3 = "Intensity B [ ] = N/A";
            string seriesName4 = "Ratio [A / B ] = N/A";
            string seriesName5 = "Moment [ ] = N/A";

            // Intensity 1
            Series S2 = chartWaveform.Series.Add(seriesName2);
            S2.ChartType = SeriesChartType.Area;
            S2.Color = Color.FromArgb(150, Color.Blue);

            // Intensity 2
            Series S3 = chartWaveform.Series.Add(seriesName3);
            S3.ChartType = SeriesChartType.Area;
            S3.Color = Color.FromArgb(150, Color.Red);

            // Ratio
            Series S4 = chartWaveform.Series.Add(seriesName4);
            S4.ChartType = SeriesChartType.Point;
            S4.MarkerSize = 0;
            S4.MarkerColor = Color.White;

            // Moment
            Series S5 = chartWaveform.Series.Add(seriesName5);
            S5.ChartType = SeriesChartType.Point;
            S5.MarkerSize = 0;
            S5.MarkerColor = Color.White;

            // Legend 1 (Well Name & Ratio)
            chartWaveform.Legends.Add(new Legend("Legend1"));

            chartWaveform.Legends[0].Docking = Docking.Top;
            chartWaveform.Legends[0].Alignment = StringAlignment.Far;

            chartWaveform.Legends[0].Font = new Font("Arial", 12, FontStyle.Bold);
            chartWaveform.Legends[0].BorderDashStyle = ChartDashStyle.Solid;
            chartWaveform.Legends[0].BorderColor = Color.Black;

            S1.Legend = "Legend1";

            // Legend 2 (Values)
            chartWaveform.Legends.Add(new Legend("Legend2"));

            chartWaveform.Legends[1].Docking = Docking.Bottom;
            chartWaveform.Legends[1].Alignment = StringAlignment.Center;

            chartWaveform.Legends[1].Font = new Font("Arial", 12, FontStyle.Regular);
            chartWaveform.Legends[1].BorderDashStyle = ChartDashStyle.Solid;
            chartWaveform.Legends[1].BorderColor = Color.Black;

            S2.Legend = "Legend2";
            S3.Legend = "Legend2";
            S4.Legend = "Legend2";
            S5.Legend = "Legend2";

            // Waveform Plot
            chartWaveform.ChartAreas[0].AxisY.Minimum = 0;
            chartWaveform.ChartAreas[0].AxisY.Maximum = 10;

            S1.Points.AddXY(0, 0);

            // Area chart
            S2.Points.AddXY(0, 0);
            S3.Points.AddXY(0, 0);

            chartWaveform.Update();
        }

        private void WaveformChart_ActivePaste(int index, string wellName)
        {

            // Point Chart
            int start = info.StartPixel;
            int pixelLength = info.PixelLength;

            // Area Chart
            int x1 = data.analysisParameters.PixelA_Low;
            int x2 = data.analysisParameters.PixelA_High;

            int x3 = data.analysisParameters.PixelB_Low;
            int x4 = data.analysisParameters.PixelB_High;

            // Wavelengths
            double[] wavelength = info.Wavelength;

            // Get the waveform and find max value
            double[] result = data.PlateResult[index].Waveform;
            double[] result1 = data.PlateResult[index].Waveform;
            double[] result2 = data.PlateResult[index].Waveform;
            if (info.Detector == "TIA")
            {
                result1 = data.PlateResult[index].Waveform1;
                result2 = data.PlateResult[index].Waveform2;                
            }
            

            if (instrument.Autoscale)
            {
                double max = charting.FindMax(data.PlateResult[index].Max);

                chartWaveform.ChartAreas[0].AxisY.Minimum = -0.1*max;
                chartWaveform.ChartAreas[0].AxisY.Maximum = max;
            }


            // Clear All Series and Legends
            chartWaveform.Series.Clear();
            chartWaveform.Legends.Clear();

            // Create Line Series
            string seriesName1 = wellName;

            Series S1 = chartWaveform.Series.Add(seriesName1);
            S1.ChartType = SeriesChartType.Line;
            S1.Color = Color.Black;
            S1.BorderWidth = 1;

            S1.MarkerColor = Color.Black;
            S1.MarkerSize = 4;
            S1.MarkerStyle = MarkerStyle.Circle;

            // Create Area Series
            string seriesName2 = "Intensity A [" + data.analysisParameters.WavelengthA.ToString() + "] = " + data.PlateResult[index].IntensityA.ToString("#,##");
            string seriesName3 = "Intensity B [" + data.analysisParameters.WavelengthB.ToString() + "] = " + data.PlateResult[index].IntensityB.ToString("#,##");
            string seriesName4 = "Ratio [" + data.analysisParameters.WavelengthA.ToString() + "/" + data.analysisParameters.WavelengthB.ToString() + "] = " + data.PlateResult[index].Ratio.ToString("F2");
            string seriesName5 = "Moment [" + data.analysisParameters.MomentA.ToString() + "-" + data.analysisParameters.MomentB.ToString() + "] = " + data.PlateResult[index].Moment.ToString("F2");

            // Intensity 1
            Series S2 = chartWaveform.Series.Add(seriesName2);
            S2.ChartType = SeriesChartType.Area;
            S2.Color = Color.FromArgb(150, Color.Blue);

            // Intensity 2
            Series S3 = chartWaveform.Series.Add(seriesName3);
            S3.ChartType = SeriesChartType.Area;
            S3.Color = Color.FromArgb(150, Color.Red);

            // Ratio
            Series S4 = chartWaveform.Series.Add(seriesName4);
            S4.ChartType = SeriesChartType.Point;
            S4.MarkerSize = 0;
            S4.MarkerColor = Color.White;

            // Moment
            Series S5 = chartWaveform.Series.Add(seriesName5);
            S5.ChartType = SeriesChartType.Point;
            S5.MarkerSize = 0;
            S5.MarkerColor = Color.White;

            // Legend 1 (Well Name)
            chartWaveform.Legends.Add(new Legend("Legend1"));

            chartWaveform.Legends[0].Docking = Docking.Top;
            chartWaveform.Legends[0].Alignment = StringAlignment.Far;

            chartWaveform.Legends[0].Font = new Font("Arial", 12, FontStyle.Bold);
            chartWaveform.Legends[0].BorderDashStyle = ChartDashStyle.Solid;
            chartWaveform.Legends[0].BorderColor = Color.Black;

            S1.Legend = "Legend1";

            // Legend 2 (Values)
            chartWaveform.Legends.Add(new Legend("Legend2"));

            chartWaveform.Legends[1].Docking = Docking.Bottom;
            chartWaveform.Legends[1].Alignment = StringAlignment.Center;

            chartWaveform.Legends[1].Font = new Font("Arial", 12, FontStyle.Regular);
            chartWaveform.Legends[1].BorderDashStyle = ChartDashStyle.Solid;
            chartWaveform.Legends[1].BorderColor = Color.Black;
            chartWaveform.Legends[1].LegendStyle = LegendStyle.Table;

            S2.Legend = "Legend2";
            S3.Legend = "Legend2";
            S4.Legend = "Legend2";
            S5.Legend = "Legend2";

            if ((PlotWaveforms) | (!instrument.ActiveScan))
            {
                if (info.Detector == "TIA")
                {
                    // Waveform Plot
                    for (int i = start; i < (start + pixelLength); i++)
                    {
                        S1.Points.AddXY(wavelength[i], result[i]);  // + 100);
                    }

                    S2.ChartType = SeriesChartType.Line;
                    S2.MarkerSize = 4;
                    S2.MarkerStyle = MarkerStyle.Circle;

                    S3.ChartType = SeriesChartType.Line;
                    S3.MarkerSize = 4;
                    S3.MarkerStyle = MarkerStyle.Circle;

                    // Waveform Plot
                    for (int i = start; i < (start + pixelLength); i++)
                    {
                        S2.Points.AddXY(wavelength[i], result1[i]);  // + 100);
                    }

                    // Waveform Plot
                    for (int i = start; i < (start + pixelLength); i++)
                    {
                        S3.Points.AddXY(wavelength[i], result2[i]);  // + 100);
                    }
                }
                else
                {
                    // Waveform Plot
                    for (int i = start; i < (start + pixelLength); i++)
                    {
                        S1.Points.AddXY(wavelength[i], result[i]);  // + 100);
                    }

                    // Intensity A: Area Chart
                    for (int i = x1; i < x2; i++)
                    {
                        S2.Points.AddXY(wavelength[i], result[i]);  // + 100);
                    }

                    // Intensity B: Area Chart
                    for (int i = x3; i < x4; i++)
                    {
                        S3.Points.AddXY(wavelength[i], result[i]);  // + 100);
                    }
                }
            }


            chartWaveform.Update();

        }






        // Thermal Chart 
        private void ThermalChart_Update()
        {
            // Clear All Series
            chartThermal.Series.Clear();
            chartThermal.Legends.Clear();
            chartThermal.ChartAreas.Clear();

            // Initialize axis
            ChartArea chartArea = chartThermal.ChartAreas.Add("chartArea");

            // Axis Line color and width
            chartArea.AxisX.LineColor = Color.Black;
            chartArea.AxisY.LineColor = Color.Black;
            chartArea.AxisX2.LineColor = Color.Black;
            chartArea.AxisY2.LineColor = Color.Black;

            chartArea.AxisX.LineWidth = 1;
            chartArea.AxisY.LineWidth = 1;
            chartArea.AxisX2.LineWidth = 1;
            chartArea.AxisY2.LineWidth = 1;

            // Form and Chart Area Color
            chartThermal.BackColor = Color.White;

            // Enable/Disable Tick Marks
            chartArea.AxisX.MajorTickMark.Enabled = true;
            chartArea.AxisY.MajorTickMark.Enabled = false;
            chartArea.AxisX2.MajorTickMark.Enabled = false;
            chartArea.AxisY2.MajorTickMark.Enabled = false;
            chartArea.AxisX2.MinorTickMark.Enabled = false;
            chartArea.AxisY2.MinorTickMark.Enabled = false;

            chartArea.AxisX.MajorTickMark.Interval = 2;


            // Grid Lines X
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisX.MinorGrid.Enabled = false;
            chartArea.AxisX2.MajorGrid.Enabled = false;
            chartArea.AxisX2.MinorGrid.Enabled = false;

            chartArea.AxisX.MajorGrid.Interval = 10;

            chartArea.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            // Grid Lines Y
            chartArea.AxisY.MajorGrid.Enabled = false;
            chartArea.AxisY.MinorGrid.Enabled = false;
            chartArea.AxisY2.MajorGrid.Enabled = false;
            chartArea.AxisY2.MinorGrid.Enabled = false;

            chartArea.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            chartThermal.ChartAreas[0].AxisY.Minimum = 0;
            chartThermal.ChartAreas[0].AxisY.Maximum = 10;

            // FontSizes
            chartArea.IsSameFontSizeForAllAxes = true;
            chartArea.AxisX.LabelAutoFitMinFontSize = 12;
            chartArea.AxisX.TitleFont = new Font("Arial", 12, FontStyle.Bold);

            chartArea.AxisY.LabelAutoFitMinFontSize = 12;
            chartArea.AxisY.TitleFont = new Font("Arial", 12, FontStyle.Bold);

            // Set up the X Axis
            chartArea.AxisX.Title = "Temperature [C]";
            chartArea.AxisX.Minimum = instrument.StartingTemperature;
            chartArea.AxisX.Maximum = instrument.EndingTemperature;
            chartArea.AxisX.Interval = 2;

            // Set up the Y Axis
            chartArea.AxisY.Title = "Fluorescence";
            chartArea.AxisY.LabelStyle.Format = "#,##";

            chartThermal.Update();
        }

        private void ThermalChart_NullPaste()
        {

            // Clear All Series
            chartThermal.Series.Clear();
            chartThermal.Legends.Clear();

            // Create Line Series
            string seriesName1 = "A1";

            Series S1 = chartThermal.Series.Add(seriesName1);
            S1.ChartType = SeriesChartType.Line;
            S1.Color = Color.Black;
            S1.BorderWidth = 1;

            S1.MarkerColor = Color.Black;
            S1.MarkerSize = 4;
            S1.MarkerStyle = MarkerStyle.Circle;

            // Create Area Series
            string seriesName2 = "Intensity A [ ] = N/A";
            string seriesName3 = "Intensity B [ ] = N/A";
            string seriesName4 = "Ratio [A / B ] = N/A";
            string seriesName5 = "Moment [ ] = N/A";

            // Intensity 1
            Series S2 = chartThermal.Series.Add(seriesName2);
            S2.ChartType = SeriesChartType.Area;
            S2.Color = Color.FromArgb(150, Color.Blue);

            // Intensity 2
            Series S3 = chartThermal.Series.Add(seriesName3);
            S3.ChartType = SeriesChartType.Area;
            S3.Color = Color.FromArgb(150, Color.Red);

            // Ratio
            Series S4 = chartThermal.Series.Add(seriesName4);
            S4.ChartType = SeriesChartType.Point;
            S4.MarkerSize = 0;
            S4.MarkerColor = Color.White;

            // Moment
            Series S5 = chartThermal.Series.Add(seriesName5);
            S5.ChartType = SeriesChartType.Point;
            S5.MarkerSize = 0;
            S5.MarkerColor = Color.White;

            // Legend 1 (Well Name & Ratio)
            chartThermal.Legends.Add(new Legend("Legend1"));

            chartThermal.Legends[0].Docking = Docking.Top;
            chartThermal.Legends[0].Alignment = StringAlignment.Far;

            chartThermal.Legends[0].Font = new Font("Arial", 12, FontStyle.Bold);
            chartThermal.Legends[0].BorderDashStyle = ChartDashStyle.Solid;
            chartThermal.Legends[0].BorderColor = Color.Black;

            S1.Legend = "Legend1";

            // Legend 2 (Values)
            chartThermal.Legends.Add(new Legend("Legend2"));

            chartThermal.Legends[1].Docking = Docking.Bottom;
            chartThermal.Legends[1].Alignment = StringAlignment.Center;

            chartThermal.Legends[1].Font = new Font("Arial", 12, FontStyle.Regular);
            chartThermal.Legends[1].BorderDashStyle = ChartDashStyle.Solid;
            chartThermal.Legends[1].BorderColor = Color.Black;

            S2.Legend = "Legend2";
            S3.Legend = "Legend2";
            S4.Legend = "Legend2";
            S5.Legend = "Legend2";

            // Waveform Plot
            chartThermal.ChartAreas[0].AxisY.Minimum = 0;
            chartThermal.ChartAreas[0].AxisY.Maximum = 10;

            S1.Points.AddXY(0, 0);

            // Area chart
            S2.Points.AddXY(0, 0);
            S3.Points.AddXY(0, 0);

            chartThermal.Update();
        }

        private void ThermalChart_ActivePaste(int index, string wellName)
        {
            int thermal = cboThermalSelection.SelectedIndex;

            // Point Chart
            int start = info.StartPixel;
            int pixelLength = info.PixelLength;

            // Area Chart
            int x1 = data.analysisParameters.PixelA_Low;
            int x2 = data.analysisParameters.PixelA_High;

            int x3 = data.analysisParameters.PixelB_Low;
            int x4 = data.analysisParameters.PixelB_High;

            // Wavelengths
            double[] wavelength = info.Wavelength;

            // Get the waveform and find max value
            int nscan = data.block.IntensityA.Length;
            int nwell = data.block.IntensityA[0].Length;
            double[] result = new double[nscan];
            double[] temp = new double[nscan];
            for (int i = 0; i < nscan; i++)
            {
                switch (thermal)
                {
                    case 0:
                        result[i] = data.block.IntensityA[i][index];
                        break;
                    case 1:
                        result[i] = data.block.IntensityB[i][index];
                        break;
                    case 2:
                        result[i] = data.block.Ratio[i][index];
                        break;
                    case 3:
                        result[i] = data.block.Moment[i][index];
                        break;
                }
                temp[i] = data.block.Temperature[i][index];
            }

            // Find Max
            double max = 0;
            for (int i = 0; i < nscan; i++)
            {
                if (result[i] > max)
                {
                    max = result[i];
                }
            }


            if (instrument.Autoscale)
            {
                max = charting.FindMax(max);

                chartThermal.ChartAreas[0].AxisY.Minimum = -0.1 * max;
                chartThermal.ChartAreas[0].AxisY.Maximum = max;
            }


            // Clear All Series and Legends
            chartThermal.Series.Clear();
            chartThermal.Legends.Clear();

            // Create Line Series
            string seriesName1 = wellName;

            Series S1 = chartThermal.Series.Add(seriesName1);
            S1.ChartType = SeriesChartType.Line;
            S1.Color = Color.Black;
            S1.BorderWidth = 1;

            S1.MarkerColor = Color.Black;
            S1.MarkerSize = 4;
            S1.MarkerStyle = MarkerStyle.Circle;

            // Create Area Series
            string seriesName2 = "Intensity A [" + data.analysisParameters.WavelengthA.ToString() + "] = " + data.PlateResult[index].IntensityA.ToString("#,##");
            string seriesName3 = "Intensity B [" + data.analysisParameters.WavelengthB.ToString() + "] = " + data.PlateResult[index].IntensityB.ToString("#,##");
            string seriesName4 = "Ratio [" + data.analysisParameters.WavelengthA.ToString() + "/" + data.analysisParameters.WavelengthB.ToString() + "] = " + data.PlateResult[index].Ratio.ToString("F2");
            string seriesName5 = "Moment [" + data.analysisParameters.MomentA.ToString() + "-" + data.analysisParameters.MomentB.ToString() + "] = " + data.PlateResult[index].Moment.ToString("F2");

            // Intensity 1
            Series S2 = chartThermal.Series.Add(seriesName2);
            S2.ChartType = SeriesChartType.Area;
            S2.Color = Color.FromArgb(150, Color.Blue);

            // Intensity 2
            Series S3 = chartThermal.Series.Add(seriesName3);
            S3.ChartType = SeriesChartType.Area;
            S3.Color = Color.FromArgb(150, Color.Red);

            // Ratio
            Series S4 = chartThermal.Series.Add(seriesName4);
            S4.ChartType = SeriesChartType.Point;
            S4.MarkerSize = 0;
            S4.MarkerColor = Color.White;

            // Moment
            Series S5 = chartThermal.Series.Add(seriesName5);
            S5.ChartType = SeriesChartType.Point;
            S5.MarkerSize = 0;
            S5.MarkerColor = Color.White;

            // Legend 1 (Well Name)
            chartThermal.Legends.Add(new Legend("Legend1"));

            chartThermal.Legends[0].Docking = Docking.Top;
            chartThermal.Legends[0].Alignment = StringAlignment.Far;

            chartThermal.Legends[0].Font = new Font("Arial", 12, FontStyle.Bold);
            chartThermal.Legends[0].BorderDashStyle = ChartDashStyle.Solid;
            chartThermal.Legends[0].BorderColor = Color.Black;

            S1.Legend = "Legend1";

            // Legend 2 (Values)
            chartThermal.Legends.Add(new Legend("Legend2"));

            chartThermal.Legends[1].Docking = Docking.Bottom;
            chartThermal.Legends[1].Alignment = StringAlignment.Center;

            chartThermal.Legends[1].Font = new Font("Arial", 12, FontStyle.Regular);
            chartThermal.Legends[1].BorderDashStyle = ChartDashStyle.Solid;
            chartThermal.Legends[1].BorderColor = Color.Black;
            chartThermal.Legends[1].LegendStyle = LegendStyle.Table;

            S2.Legend = "Legend2";
            S3.Legend = "Legend2";
            S4.Legend = "Legend2";
            S5.Legend = "Legend2";

            if ((PlotWaveforms) | (!instrument.ActiveScan))
            {
                // Waveform Plot
                for (int i = 0; i < nscan; i++)
                {
                    S1.Points.AddXY(temp[i], result[i]);  // + 100);
                }
            }


            chartThermal.Update();

        }







        private void btnAutoscale_Click(object sender, EventArgs e)
        {
            instrument.Autoscale = !instrument.Autoscale;

            if (instrument.Autoscale)
            {
                btnAutoscale.Text = "AutoScale On";
            }
            else
            {
                btnAutoscale.Text = "AutoScale Off";
            }
        }

        private void btnSetTarget_Click(object sender, EventArgs e)
        {
            float fTarget;
            bool rtrn;

            // Get length of acquisition and Navg.
            rtrn = float.TryParse(nudStartingTemp.Text, out fTarget);
            if (!rtrn)
            {
                return;
            }
            rtrn = ms.SetRampRate(10);
            rtrn = ms.SetTargetTemp(fTarget);
            rtrn = ms.SetOutputStatus(true);
        }

        private void btnLoadConfig_Click(object sender, EventArgs e)
        {
            // Open new file dialog
            string strFilePath;
            string strFileName;

            OpenFileDialog of = new OpenFileDialog();
            of.Title = "Open SUPR Configration File";
            of.Filter = "Configuration Data|*.scfgx";
            strFilePath = Directory.GetCurrentDirectory();
            strFilePath = Path.GetFullPath(Path.Combine(strFilePath, @"../../../"));
            //strFilePath = strFilePath + "\\" + "configurationFiles" + "\\";
            strFilePath = strFilePath + "configurationFiles" + "\\";
            of.InitialDirectory = strFilePath;


            if (of.ShowDialog() == DialogResult.OK)
            {
                strFileName = Path.GetFileName(of.FileName);
                strFilePath = Path.GetDirectoryName(of.FileName);
                strFilePath = strFilePath + "\\";
                settings.ReadConfigFile(strFilePath + strFileName);
            }

            // Pass the information to external classes
            versa.info = settings.info;
            data.info = settings.info;
            microplate.info = settings.info;
            image.info = settings.info;
            instrument.info = settings.info;
            info = settings.info;

            // Write data to memory.
            bool state = settings.WriteData();
        }

        private void chkPlotWaveform_CheckedChanged(object sender, EventArgs e)
        {
            PlotWaveforms = chkPlotWaveform.Checked;
        }
    }
}
