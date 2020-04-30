using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FI.PlateReader.Gen4.JETI
{ 
    class Instrument
    {
        public Settings.Info info;

        // Instrument UI Labels
        public string InstrumentStatus { get; set; }    // String of current label
        public string[] InstrumentLabels;               // String of all available labels

        public bool ActiveScan { get; set; }
        public bool ActiveProtocol { get; set; }
        public bool Autoscale { get; set; }

        // Kinetic Scan
        public int CurrentScan { get; set; }
        public int NScans { get; set; }
        public int Delay { get; set; }

        // Plate Type & Scan
        public int scanType { get; set; }
        public int plateType { get; set; }

        // Row & Column Offset Positions (Well Image)
        public string rowOffset { get; set; }
        public string columnOffset { get; set; }


        // UI Variables
        public List<string> ScanTypes = new List<string>();
        public List<string> PlotOptions = new List<string>();
        public List<double> WavelengthBand = new List<double>();

        public double WavelengthMinium { get; set; }
        public double WavelengthMaximun { get; set; }
        public int WavelengthAValue { get; set; }
        public int WavelengthBValue { get; set; }



        // Methods
        public void InitialValues()
        {
            // If microplate is being scanned
            ActiveScan = false;
            Autoscale = true;

            CurrentScan = 0;
            NScans = 1;

            rowOffset = "";
            columnOffset = "";

            // Wavelength
            WavelengthMinium = info.WavelengthStart;
            WavelengthMaximun = info.WavelengthEnd;

            if(info.LEDWavelength < 330)
            {
                WavelengthAValue = 330;
                WavelengthBValue = 360;
            }
            else if(info.LEDWavelength < 510)
            {
                WavelengthAValue = 520;
                WavelengthBValue = 590;
            }
            else
            {
                WavelengthAValue = 590;
                WavelengthBValue = 620;
            }

            // Wavelength Bands
            WavelengthBand.Add(5);
            WavelengthBand.Add(10);
            WavelengthBand.Add(20);
            WavelengthBand.Add(40);

            // Scan Types
            ScanTypes.Add("Plate Scan");
            ScanTypes.Add("Kinetic Scan");
            ScanTypes.Add("Real Time Data");
            ScanTypes.Add("Well Image");
            ScanTypes.Add("LED Check");

            // Plot Options
            PlotOptions.Add("Intenisty A");
            PlotOptions.Add("Intensity B");
            PlotOptions.Add("Ratio");
            PlotOptions.Add("Moment");

            // Create String of phrases to display to the user
            InstrumentLabels = new string[99];

            InstrumentLabels[0] = "Initialising Instrument";
            InstrumentLabels[2] = "Homing Stages";
            InstrumentLabels[4] = "Instrument initialized. Please create a protocol";
            InstrumentLabels[6] = "Stopping Acquisition";
            InstrumentLabels[7] = "Preparing Instrument for Measurement";
            InstrumentLabels[8] = "Acquiring Background Measurement";
            InstrumentLabels[9] = "Scanning Microplate";
            InstrumentLabels[10] = "LED Warming Up";
            InstrumentLabels[12] = "Ejecting Microplate";
            InstrumentLabels[13] = "Microplate Ejected";
            InstrumentLabels[15] = "Click apply protocol to upload protocol";
            InstrumentLabels[18] = "Protocol Uploaded. Start Scan, Eject Microplate, or Reset Protocol";
            InstrumentLabels[19] = "Plate Ejected. Place Microplate in Holder (A1 Top Right). Click Insert Plate.";
            InstrumentLabels[20] = "Lost Connection to the Instrument. Verify the USB is connected and Power is on before Restarting the Software. ";
            InstrumentLabels[21] = "Scan Finished Sucessfully! Start Scan, Eject Microplate, or Reset Protocol.";
            InstrumentLabels[22] = "Scan Canceled! Start Scan, Eject Microplate, or Reset Protocol.";
            InstrumentLabels[23] = "Closing Software";
            InstrumentLabels[24] = "Plate Scan";
            InstrumentLabels[25] = "Kinetic Scan";
            InstrumentLabels[26] = "Real Time Data";
            InstrumentLabels[27] = "Well Image";
            InstrumentLabels[28] = "Led Check";

            SetInstrumentStatus(0);
        }

        public void SetInstrumentStatus(int value)
        {
            InstrumentStatus = InstrumentLabels[value];
        }


    }
}
