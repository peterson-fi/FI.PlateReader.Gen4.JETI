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
        // Internal Components
        Instrument instrument = new Instrument();       // Instrument Values (UI Software)
        Microplate microplate = new Microplate();       // Microplate Dimensions
        ChartSettings charting = new ChartSettings();   // Chart Settings 
        PlateSetup plateSetup = new PlateSetup();       // Well Selection
        Time time = new Time();                         // Clock

        // Data
        Settings settings = new Settings();             // Initial Instrument Settings (Configuration Files)
        Data data = new Data();                         // Data (Plate Scan)
        DataExport dataExport = new DataExport();       // Data Export

        Settings.Info info;

        // Delegates
        delegate void voidDelegate();
        delegate void intDelegate(int value1, int value2);

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
            ReadConfigFiles();

            // Create Variables (Microplates, Charts, Form Labels)            
            instrument.InitialValues(versa.info.WavelengthStart, versa.info.WavelengthEnd);

            // Microplate
            microplate.CreatePlates(versa.info.RowDirection, versa.info.ColumnDirection, versa.info.RowOffset, versa.info.ColumnOffset);

            // Create Charts
            charting.CreateChartSettings();
            charting.CreateColors();

            // Populate the Form with initial values 
            PopulateForm();

            tokenSourceLabel = new CancellationTokenSource();
            CancellationToken token = tokenSourceLabel.Token;
            Task.Factory.StartNew(() => LabelTask(token), token);

            // Connect to Instrument
            if (Connect())
            {
                // Pass the user data information to external classes
                settings.info = versa.info;

                // Find Start Pixel for Wavelength Start
                for (int i = 0; i < jeti.Npixels; i++)
                {
                    if (jeti.wavelength[i] > info.WavelengthStart)
                    {
                        settings.info.PixelStart = i;
                        break;
                    }
                }

                // Find End Pixel for Wavelength End
                for (int i = 0; i < jeti.Npixels; i++)
                {
                    if (jeti.wavelength[i] > info.WavelengthEnd)
                    {
                        settings.info.PixelEnd = i;
                        break;
                    }
                }

                //data.info = settings.info;
                //microplate.info = settings.info;
                //instrument.info = settings.info;
				settings.info.Wavelength = jeti.wavelength;
                info = settings.info;
                jeti.info = info;
                jeti.vs = versa;

                // Microplate
                microplate.CreatePlates(versa.info.RowDirection, versa.info.ColumnDirection, versa.info.RowOffset, versa.info.ColumnOffset);

                Task.Factory.StartNew(() => InitialiseStage());
            }
            else
            {
                StateReset();
            }

        }

        public void ReadConfigFiles()
        {
            // Read Config Files
            settings.ReadConfigFileNew();

            // Pass the informatin to external classes
            versa.info = settings.info;        
            //data.info = settings.info;         
            //microplate.info = settings.info;
            //instrument.info = settings.info;
            info = settings.info;

        }

        public void PopulateForm()
        {
            // Plate Format Combo Box
            foreach (var x in microplate.PlateList)
                cboPlateFormat.Items.Add(x.Name);

            cboPlateFormat.SelectedIndex = 0;

            // Leds
            cboLed.Items.Add(info.LEDWL1.ToString() );
            cboLed.SelectedIndex = 0;

            // Spectrometer 
            cboDetector.Items.Add(info.SpecName);
            cboDetector.SelectedIndex = 0;

            // LED Power
            foreach (var x in instrument.LedPower)
                cboLedPower.Items.Add(x);

            cboLedPower.SelectedIndex = 2;

            // Integration
            foreach (var x in instrument.Integration)
                cboIntegration.Items.Add(x);

            cboIntegration.SelectedIndex = 4;

            // Wavelength
            foreach (var x in instrument.Wavelength)
            {
                cboWavelengthA.Items.Add(x);
                cboWavelengthB.Items.Add(x);
            }

            cboWavelengthA.SelectedIndex = 3;
            cboWavelengthB.SelectedIndex = 6;

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

                        UpdateUILabels();
                    }
                }
            }
            catch (ObjectDisposedException)
            {
                return;
            }
        }


        // Form closing events
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
            // Connect to Controller
            bool vconnect = versa.Connect();

            // Connect to JETI
            bool jconnect = jeti.Connect();

            return vconnect & jconnect;
        }

        public void Disconnect()
        {
            // Disconnect from Versa
            versa.ColumnMotorEnable(false);
            versa.RowMotorEnable(false);

            versa.Disconnect();

        }

        public bool VerifyConnection()
        {
            return versa.VerifyConnection();

        }


        // Acquisition Buttons
        private void btnStart_Click(object sender, EventArgs e)
        {
            // Prepare for Scan
            bool status = ScanPrep();

            if (status)
            {
                // Launch a new task to run a plate scan
                tokenSourceScan = new CancellationTokenSource();
                CancellationToken token = tokenSourceScan.Token;

                if (versa.info.RowScan)
                {
                    Task.Factory.StartNew(() => StaticScanRow(token), token);
                }
                else
                {
                    Task.Factory.StartNew(() => StaticScan(token), token);
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
            int powerIndex = cboLedPower.SelectedIndex;

            int detectorIndex = cboDetector.SelectedIndex;
            int integrationIndex = cboIntegration.SelectedIndex;

            int wavAIndex = cboWavelengthA.SelectedIndex;
            int bandAIndex = cboBandA.SelectedIndex;

            int wavBIndex = cboWavelengthB.SelectedIndex;
            int bandBIndex = cboBandB.SelectedIndex;

            // LED Class
            versa.Power = instrument.LedPower[powerIndex];

            // Spectrometer Class
            versa.Integration = instrument.Integration[integrationIndex];

            // Data Class
            //data.SetAnalysisParameters(versa.info.Wavelength, instrument.Wavelength[wavAIndex], instrument.Wavelength[wavBIndex], instrument.WavelengthBand[bandAIndex], instrument.WavelengthBand[bandBIndex], versa.info.WavelengthStart, versa.info.WavelengthEnd);
            data.SetAnalysisParameters(jeti.wavelength, instrument.Wavelength[wavAIndex], instrument.Wavelength[wavBIndex], instrument.WavelengthBand[bandAIndex], instrument.WavelengthBand[bandBIndex], versa.info.WavelengthStart, versa.info.WavelengthEnd);

            // Update Form
            cboPlotSelection.Items.Clear();

            List<string> value = new List<string>();

            value.Add("Intensity A [" + data.analysisParameters.WavelengthA.ToString() + "]");
            value.Add("Intensity B [" + data.analysisParameters.WavelengthB.ToString() + "]");
            value.Add("Ratio [" + data.analysisParameters.WavelengthA.ToString() + "/" + data.analysisParameters.WavelengthB.ToString() + "]");
            value.Add("Moment [" + data.analysisParameters.MomentA.ToString() + "-" + data.analysisParameters.MomentB.ToString() + "]");

            // Plot Selection Combo Box
            foreach (var x in value)
                cboPlotSelection.Items.Add(x);

            cboPlotSelection.SelectedIndex = 0;

            // Clear Well selection labels
            lbRowWellSelection.Text = "";
            lbColumnWellSelection.Text = "";


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


        // Static Scan 
        private bool ScanPrep()
        {
            // Verify Instrument is connected
            if (!VerifyConnection())
            {
                return false;
            }

            // Update State
            instrument.SetInstrumentStatus(7);
            StateScanningPlate();

            // Launch file dialog to save the data        
            SaveDialog();

            // Initialise Data Array
            data.InitialiseData(microplate.plate.Wells, versa.info.Pixel);

            // LED
            versa.SetLedPower();

            // Spectrometer
            //versa.SetIntegrationTime();
            int integrationIndex = cboIntegration.SelectedIndex;
            jeti.Tint = instrument.Integration[integrationIndex];
            return true;

        }

        private void StartPlate(CancellationToken token)
        {
            instrument.ActiveScan = true;

            // Stopwatch
            time.StartTime();

            // Move to start position
            versa.MoveReferencePosition();

            // Check for stage error.

            // Check Cancel
            if (token.IsCancellationRequested)
                return;

            // Background measurement
            instrument.SetInstrumentStatus(8);
            //versa.DarkMeasurement();
            jeti.DarkMeasurement();

            // Check Cancel
            if (token.IsCancellationRequested)
                return;

            // Stop Btn
            EnableStopBtn();

            instrument.SetInstrumentStatus(9);

        }

        private void StaticScanRow(CancellationToken token)
        {
            bool row_check = true;
            bool col_check = true;
            bool scan_check = true;

            if (versa.RowError || versa.ColumnError)
            {
                MessageBox.Show("XY Stage Error! Restart Instrument and Software");
                return;
            }

            // Plate Start Tasks
            StartPlate(token);

            int row = microplate.plate.Row;
            int column = microplate.plate.Column;

            versa.SetScanHandles();

            // Get number of active columns, start and ending indices. 
            int active = 0;
            int first = 0;
            int last = 0;
            for (int j = 0; j < column; j++)
            {
                if (plateSetup.ActiveColumn[j])
                {
                    if (active == 0) { first = j; }
                    else { last = j; }
                    active++;
                }
            }

            // get starting and ending positions.
            double columnStart;
            double columnEnd;
            columnStart = microplate.motor.ColumnPosition[first];   // - microplate.info.ColumnDirection * microplate.plate.ColumnSpacing;
            columnEnd = microplate.motor.ColumnPosition[last];  // + microplate.info.ColumnDirection * microplate.plate.ColumnSpacing;

            // Turn Led On
            if (!versa.info.LEDControl) { versa.LedOn(); }
            

            // # of Rows
            for (int i = 0; i < row; i++)
            {
                // Check Cancel
                if ((token.IsCancellationRequested) || (!col_check) || (!row_check) || (!scan_check))
                    break;

                // Skip inactive rows
                if (!plateSetup.ActiveRow[i])
                    continue;

                // Step Row
                //versa.StepRowMotor(microplate.motor.RowPosition[i]);
                row_check = versa.StepRowMotor(microplate.motor.RowPosition[i], versa.info.SoftwarePositionCheck);
                if (!row_check)
                {
                    MessageBox.Show("Row Positioning Error: " + versa.RowDeltaU.ToString("F3"));
                    return;
                }

                // Step column to start or end of scan area
                if (i % 2 == 0)
                {
                    // Step column
                    //versa.StepColumnMotor(microplate.motor.ColumnPosition[columnIndex]);
                    col_check = versa.StepColumnMotor(columnStart, versa.info.SoftwarePositionCheck);                    
                    if (!col_check)
                    {
                        MessageBox.Show("Column Positioning Error: " + versa.ColumnDeltaU.ToString("F3"));
                        break;
                    }
                    versa.SetScanParameters(0, -microplate.plate.ColumnSpacing, 0, active);
                }
                else
                {
                    // Step column
                    //versa.StepColumnMotor(microplate.motor.ColumnPosition[columnIndex]);
                    col_check = versa.StepColumnMotor(columnEnd, versa.info.SoftwarePositionCheck);
                    if (!col_check)
                    {
                        MessageBox.Show("Column Positioning Error: " + versa.ColumnDeltaU.ToString("F3"));
                        break;
                    }
                    versa.SetScanParameters(0, microplate.plate.ColumnSpacing, 0, active);
                }

                // Start stop-and-go measurement
                versa.StartScanMeasurement(versa.info.LEDControl);

                // # of Columns
                for (int j = 0; j < active; j++)
                {
                    //// Check Cancel
                    //if (token.IsCancellationRequested)
                    //    break;

                    // Even or Odd Row
                    int columnIndex;

                    if (i % 2 == 0)
                    {
                        // Even Row (A,C,E...)
                        columnIndex = first + j;
                    }
                    else
                    {
                        // Odd Row (B,D,F...)
                        columnIndex = last - j;
                    }

                    // Measurement
                    scan_check = StaticRowMeasurement(i, columnIndex, j);
                    if (!scan_check) { break; }

                }
                versa.EndScanMeasurement();

            }

            EndPlate();

            // Turn Led Off
            if (!versa.info.LEDControl) { versa.LedOff(); }


        }

        private bool StaticRowMeasurement(int row, int columnIndex, int scanIndex)
        {
            // Update Clock
            time.GetTime();

            // Well Index
            int column = microplate.plate.Column;
            int index = (row * column) + columnIndex;

            // Measurement
            bool check = versa.ScanMeasurement(scanIndex);

            if (check)
            {
                data.SetResult(index, versa.Waveform, versa.info.Wavelength);

                // Plot Data  
                UpdateDataChart(row, columnIndex);
            }
            return check;
        }

        private void EndPlate()
        {
            // End Timer
            time.EndTime();

            // Save Data
            data.SetData();
            SavePlate();

            // Get LED hours and update EEPROM
            versa.ReadDataLED();
            versa.WriteLEDhours();

            // Instrument State
            if (versa.RowError || versa.ColumnError)
            {
                versa.RowMotorEnable(false);
                versa.ColumnMotorEnable(false);

                StateError();
            }
            else
            {
                // Move Back to Reference Position
                versa.MoveReferencePosition();

                StateDeviceActive();
            }
            
            EnableSaveBtn();
            instrument.ActiveScan = false;

            // Update UI Label
            if (tokenSourceScan.IsCancellationRequested)
            {
                instrument.SetInstrumentStatus(22);
                return;
            }

            if (versa.RowError || versa.ColumnError)
            {
                instrument.SetInstrumentStatus(20);
            }
            else
            {
                instrument.SetInstrumentStatus(21);
            }
        }


        // JUNK
        private void StaticScan(CancellationToken token)
        {
            bool row_check = true;
            bool col_check = true;

            if (versa.RowError || versa.ColumnError)
            {
                MessageBox.Show("XY Stage Error! Restart Instrument and Software");
                return;
            }

            // Plate Start Tasks
            StartPlate(token);

            int row = microplate.plate.Row;
            int column = microplate.plate.Column;

            if (!versa.info.LEDControl) { versa.LedOn(); }

            // # of Rows
            for (int i = 0; i < row; i++)
            {
                // Check Cancel
                if ((token.IsCancellationRequested) || (!col_check))
                    break;

                // Skip inactive rows
                if (!plateSetup.ActiveRow[i])
                    continue;

                // Step Row
                //versa.StepRowMotor(microplate.motor.RowPosition[i]);
                row_check = versa.StepRowMotor(microplate.motor.RowPosition[i], versa.info.SoftwarePositionCheck);
                if (!row_check)
                {
                    MessageBox.Show("Row Positioning Error: " + versa.RowDeltaU.ToString("F3"));
                    break;
                }

                // # of Columns
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

                    // Step column
                    //versa.StepColumnMotor(microplate.motor.ColumnPosition[columnIndex]);
                    col_check = versa.StepColumnMotor(microplate.motor.ColumnPosition[columnIndex], versa.info.SoftwarePositionCheck);
                    if (!col_check)
                    {
                        MessageBox.Show("Column Positioning Error: " + versa.ColumnDeltaU.ToString("F3"));
                        break;
                    }

                    // Measurement
                    StaticMeasurement(i, columnIndex);

                }
            }

            EndPlate();

            if (!versa.info.LEDControl) { versa.LedOff(); }
        }

        private void StaticMeasurement(int row, int columnIndex)
        {
            // Update Clock
            time.GetTime();

            // Well Index
            int column = microplate.plate.Column;
            int index = (row * column) + columnIndex;

            // Measurement
            //versa.LightMeasurement();
            //data.SetResult(index, versa.Waveform, versa.info.Wavelength);
            jeti.LightMeasurement();
            data.SetResult(index, jeti.data, jeti.wavelength);

            // Plot Data  
            UpdateDataChart(row, columnIndex);

        }


        // Save 
        private void btnSaveData_Click(object sender, EventArgs e)
        {
            // Try to save the data
            SaveDialog();
            SavePlate();

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

        private void SavePlate()
        {
            if (dataExport.Save)
            {
                // Prepare data export for Save
                PrepareSave();

                // Save Data                
                dataExport.SavePlate();
            }

        }

        private void PrepareSave()
        {
            // New instance of the struct
            dataExport.plateInfo = new DataExport.PlateInfo();

            // File Information
            dataExport.plateInfo.Filename = dataExport.Filename;
            dataExport.plateInfo.Filepath = Path.Combine(dataExport.Filepath, dataExport.Filename);

            // Settings
            dataExport.plateInfo.info = info;

            // Microplate
            dataExport.plateInfo.plate = microplate.plate;
            dataExport.plateInfo.motor = microplate.motor;

            // LED
            dataExport.plateInfo.LED = info.LEDWL1.ToString();
            dataExport.plateInfo.LedPower = versa.Power;

            // Detector
            dataExport.plateInfo.Detector = info.SpecName;
            dataExport.plateInfo.Integration = versa.Integration;

            // Data Information
            dataExport.plateInfo.analysisParameters = data.analysisParameters;
            dataExport.plateInfo.PlateResult = data.PlateResult;
            dataExport.plateInfo.block = data.block;

            // Plate Setup
            dataExport.plateInfo.ActiveRow = plateSetup.ActiveRow;
            dataExport.plateInfo.ActiveColumn = plateSetup.ActiveColumn;
            dataExport.plateInfo.Samples = plateSetup.ActiveWells;

            // Time Information
            dataExport.plateInfo.StartDate = time.StartDate;
            dataExport.plateInfo.StartPlateTime = time.StartPlateTime;
            dataExport.plateInfo.EndPlateTime = time.EndPlateTime;

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
                ((Control)this.tabResults).Enabled = false;

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
                StateDisableAll();

                // Btns
                btnStart.Enabled = true;
                btnEjectPlate.Enabled = true;
                btnResetProtocol.Enabled = true;

                // Enable Tab Results
                ((Control)this.tabResults).Enabled = true;
                tabControl.SelectedTab = tabResults;

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
                DisableButtons();
                tabControl.SelectedTab = tabResults;
            }
        }


        // Thread Safe: Charts, Labels
        private void UpdateDataChart(int row, int columnIndex)
        {
            if (chartResultMap.InvokeRequired)
            {
                intDelegate d = new intDelegate(UpdateDataChart);
                Invoke(d, new object[] { row, columnIndex });
            }
            else
            {
                // Microplate Heat Map Chart
                int value = cboPlotSelection.SelectedIndex;

                charting.FindHeatMapColors(value, data.block.Data[value]);
                ChartResultMap_ActivePaste();

                // Waveform Chart
                int column = microplate.plate.Column;
                int index = (row * column) + columnIndex;

                if (index >= 0)
                {
                    ChartResultMap_MarkerPaste(row, columnIndex);
                    WaveformChart_ActivePaste(row, columnIndex);
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

                // Clock
                labelClock.Text = time.PlateTime;

                // LED hours.
                TimeSpan led_sec = TimeSpan.FromSeconds(versa.ledData.hours[0] * 3600);
                labelLEDhours.Text = led_sec.ToString("hh':'mm':'ss");

                // Heat Map Chart
                lbLegendMin.Text = charting.MinLabel;
                lbLegendMax.Text = charting.MaxLabel;

                // Update
                toolStrip.Update();
                statusStrip.Update();

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


        /// <summary>
        /// Charting Methods
        /// </summary>
        /// 

        // Reset Charts
        private void ResetDataCharts()
        {

            // Reset Data, buttons, wells selected
            data.ResetData();


            // Reset Heat Map combo box
            cboPlotSelection.Items.Clear();
            foreach (var x in instrument.PlotOptions)
                cboPlotSelection.Items.Add(x);

            cboPlotSelection.SelectedIndex = 0;

            // Heat Map Result Chart Reset
            ChartResultMap_Update();
            ChartResultMap_NullPaste();

            // Legend Chart
            ChartLegend_Update();

            // Waveform Result Chart Reset
            WaveformChart_Update();
            WaveformChart_NullPaste();

            // Reset Label Variables
            time.PlateTime = "";

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
            int value = cboPlateFormat.SelectedIndex;

            // Update Current Microplate (Microplate and Charting)
            microplate.SetCurrentPlate(value);
            charting.SetCurrentChart(value);

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
                UpdateDataChart(-1, -1);
            }
        }

        private void ChartResultMap_Update()
        {
            // Clear All Series
            chartResultMap.Series.Clear();
            chartResultMap.Legends.Clear();
            chartResultMap.ChartAreas.Clear();

            // Row/Col
            int row = charting.chartParameters.row;
            int col = charting.chartParameters.column;

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
            int row = charting.chartParameters.row;
            int column = charting.chartParameters.column;

            for (int i = 0; i < row; i++)
                for (int j = 0; j < column; j++)
                {
                    pt = S1.Points.AddXY(j + 1, i + 1);
                    S1.Points[pt].MarkerStyle = charting.chartParameters.wsMarkerStyle;
                    S1.Points[pt].MarkerColor = charting.chartParameters.wsNullColor;
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
            int row = charting.chartParameters.row;
            int column = charting.chartParameters.column;
            int lenPlate = charting.chartParameters.wells;

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
                        S1.Points[pt].MarkerStyle = charting.chartParameters.dataMarkerStyle;
                        S1.Points[pt].MarkerColor = Color.FromArgb(150, charting.ColorList[colors[index]]);
                        S1.Points[pt].MarkerSize = charting.chartParameters.dataMarkerSize;
                    }
                    else
                    {
                        // Paste Null Color
                        pt = S1.Points.AddXY(j + 1, i + 1);
                        S1.Points[pt].MarkerStyle = charting.chartParameters.dataMarkerStyle;
                        S1.Points[pt].MarkerColor = Color.White;
                        S1.Points[pt].MarkerSize = 0;
                    }
                }

            chartResultMap.Update();


        }

        private void ChartResultMap_MarkerPaste(int row, int column)
        {
            // Paint Well Marker
            int markerSize = charting.chartParameters.markerMarkerSize;
            int pt = chartResultMap.Series["S2"].Points.AddXY(column + 1, row + 1);
            chartResultMap.Series["S2"].Points[pt].MarkerStyle = MarkerStyle.Cross;
            chartResultMap.Series["S2"].Points[pt].MarkerColor = Color.Black;
            chartResultMap.Series["S2"].Points[pt].MarkerSize = markerSize;

        }

        private void chartResultMap_MouseMove(object sender, MouseEventArgs e)
        {
            ChartArea chartArea = chartResultMap.ChartAreas[0];

            // Row/Col
            int row = microplate.plate.Row;
            int col = microplate.plate.Column;

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

        private void chartResultMap_Click(object sender, EventArgs e)
        {
            if (data.DataAvailable)
            {
                ChartArea chartArea = chartResultMap.ChartAreas[0];

                // Get Cursor Position
                int rowMin = plateSetup.RowMin + 1;
                int rowMax = plateSetup.RowMax + 1;

                int columnMin = plateSetup.ColumnMin + 1;
                int columnMax = plateSetup.ColumnMax + 1;

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

                UpdateDataChart(sRow, sColumn);

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

            chartArea.AxisX.MajorTickMark.Interval = 10;

            // Grid Lines X
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisX.MinorGrid.Enabled = false;
            chartArea.AxisX2.MajorGrid.Enabled = false;
            chartArea.AxisX2.MinorGrid.Enabled = false;

            chartArea.AxisX.MajorGrid.Interval = 20;
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
            chartArea.AxisX.Title = "Wavelength [nm]";

            chartArea.AxisX.Minimum = info.WavelengthStart;
            chartArea.AxisX.Maximum = info.WavelengthEnd;
            chartArea.AxisX.Interval = info.WavelengthSpacing;

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

        private void WaveformChart_ActivePaste(int row, int column)
        {
            // Get the Index of the desired waveform (Data is in spectrometer class)
            int columnPlate = microplate.plate.Column;
            int index = columnPlate * row + column;
            string wellName = dataExport.ConvertRow(row) + (column + 1).ToString();

            // Point Chart
            int start = info.PixelStart;
            int end = info.PixelEnd;

            // Area Chart
            int x1 = data.analysisParameters.PixelA_Low;
            int x2 = data.analysisParameters.PixelA_High;

            int x3 = data.analysisParameters.PixelB_Low;
            int x4 = data.analysisParameters.PixelB_High;

            // Wavelengths
            double[] wavelength = jeti.wavelength;  // info.Wavelength;

            // Get the waveform and find max value
            double[] result = data.PlateResult[index].Waveform;

            double max = charting.FindMax(data.PlateResult[index].Max);

            chartWaveform.ChartAreas[0].AxisY.Minimum = 0;
            chartWaveform.ChartAreas[0].AxisY.Maximum = max;

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

            // Waveform Plot
            for (int i = start; i < end; i++)
            {
                S1.Points.AddXY(wavelength[i], result[i] + 100);
            }

            // Intensity A: Area Chart
            for (int i = x1; i < x2; i++)
            {
                S2.Points.AddXY(wavelength[i], result[i] + 100);
            }

            // Intensity B: Area Chart
            for (int i = x3; i < x4; i++)
            {
                S3.Points.AddXY(wavelength[i], result[i] + 100);
            }

            chartWaveform.Update();

        }


    }
}
