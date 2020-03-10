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
        //public Settings.Info info;

        // Instrument UI Labels
        public string InstrumentStatus { get; set; }    // String of current label
        public string[] InstrumentLabels;               // String of all available labels

        public bool ActiveScan { get; set; }
        public bool ActiveProtocol { get; set; }

        // UI Variables
        public List<string> PlotOptions = new List<string>();

        public List<int> LedPower = new List<int>();
        public List<int> Integration = new List<int>();

        public List<double> Wavelength = new List<double>();
        public List<double> WavelengthBand = new List<double>();


        // Methods
        public void InitialValues(double WavelengthStart, double WavelengthEnd)
        {
            // If microplate is being scanned
            ActiveScan = false;

            // Led Power
            LedPower.Add(10);
            LedPower.Add(20);
            LedPower.Add(30);
            LedPower.Add(40);
            LedPower.Add(50);
            LedPower.Add(60);
            LedPower.Add(70);
            LedPower.Add(80);
            LedPower.Add(90);
            LedPower.Add(100);

            //Integration 
            Integration.Add(1);
            Integration.Add(10);
            Integration.Add(25);
            Integration.Add(50);
            Integration.Add(100);
            Integration.Add(250);
            Integration.Add(500);
            Integration.Add(1000);
            Integration.Add(2000);

            // Wavelength
            int start = (int)WavelengthStart;
            int end = (int)WavelengthEnd;

            for(int i = 0; i < 20; i++)
            {
                double value = start + (i * 10);

                if (value > end)
                    break;

                Wavelength.Add(value);                
            }

            WavelengthBand.Add(5);
            WavelengthBand.Add(10);
            WavelengthBand.Add(20);
            WavelengthBand.Add(40);

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

            SetInstrumentStatus(0);
        }

        public void SetInstrumentStatus(int value)
        {
            InstrumentStatus = InstrumentLabels[value];
        }


    }
}
