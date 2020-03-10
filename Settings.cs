using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace FI.PlateReader.Gen4.JETI
{
    class Settings
    {
                          
        public Info info { get; set; }
        public class Info
        {
            /// <summary>
            /// File 1
            /// </summary>

            // Reference Positions
            public string InstrumentName;
            public int SerialNumber;
            public int LEDSN;
            public int SpecSN;
            public int LedBus;
            public int LedChannel;
            public int RowBus;
            public int ColumnBus;
            public double RowOffset;
            public double ColumnOffset;
            public double RowEject;
            public double ColumnEject;
            public int RowDirection;
            public int ColumnDirection;
            public bool RowScan;
            public bool LEDControl;
            public bool MotorClosedLoop;
            public bool SoftwarePositionCheck;
            public bool ReadUserData;
            public bool LEDPersist;

            // LED
            public int LEDPulse;
            public int LEDWL1;
            public int LEDWL2;
            public int LEDWL3;
            public int LEDWL4;
            public int LEDlimit1;
            public int LEDlimit2;
            public int LEDlimit3;
            public int LEDlimit4;

            // Row Motor
            public string RowName;
            public bool RowMotorReverse;
            public bool RowEncoderReverse;
            public int RowCurrent;
            public int RowMicrostep;
            public int RowFullStepsPerRev;
            public int RowEncoderCountsPerRev;
            public double RowUnitsPerRev;
            public double RowPositionError;
            public double RowSpeed;
            public double RowAcceleration;
            public bool RowAccurateHome;

            // Column Motor
            public string ColumnName;
            public bool ColumnMotorReverse;
            public bool ColumnEncoderReverse;
            public int ColumnCurrent;
            public int ColumnMicrostep;            
            public int ColumnFullStepsPerRev;
            public int ColumnEncoderCountsPerRev;
            public double ColumnUnitsPerRev;
            public double ColumnPositionError;
            public double ColumnSpeed;
            public double ColumnAcceleration;
            public bool ColumnAccurateHome;

            // Instrument
            public double InstrumentGain;

            // Spectrometer 
            public string SpecName;
            public int Pixel;
            public int PostIntegrationWait;
            public int MinimumCycles;
            public double P0;
            public double P1;
            public double P2;
            public double P3;
            public double P4;
            public double[] Wavelength;
            public double WavelengthStart;
            public double WavelengthEnd;
            public double WavelengthSpacing;
            public int PixelStart;
            public int PixelEnd;
            public int ActiveSize;

            public int CorrectStart;
            public int CorrectLength;
            public double[] CorrectValues;
        }



        // Methods
        public void ReadConfigFile()
        {
            info = new Info();

            // Read Config Files
            // Get plate configuration filename
            string filepath;
            string filename = "SUPR-UV_Default.scfg";

            filepath = Directory.GetCurrentDirectory();
            filepath = Path.GetFullPath(Path.Combine(filepath, @"../../../"));
            filename = Path.Combine(filepath, "configurationFiles", filename);

            // Read File
            List<string> Values = new List<string>();
            bool status = ReadFile(filename, ref Values);

            if (status)
            {
                // Instrument
                info.InstrumentName = Values[0];
                info.SerialNumber = Convert.ToInt32(Values[1]);
                info.LedBus= Convert.ToInt32(Values[2]);
                info.LedChannel = Convert.ToInt32(Values[3]);
                info.RowBus = Convert.ToInt32(Values[4]);
                info.ColumnBus = Convert.ToInt32(Values[5]);
                info.RowOffset = Convert.ToDouble(Values[6]);
                info.ColumnOffset = Convert.ToDouble(Values[7]);
                info.RowEject = Convert.ToDouble(Values[8]);
                info.ColumnEject = Convert.ToDouble(Values[9]);

                // LED
                info.LEDWL1 = Convert.ToInt32(Values[10]);
                info.LEDWL2 = Convert.ToInt32(Values[11]);
                info.LEDWL3 = Convert.ToInt32(Values[12]);
                info.LEDWL4 = Convert.ToInt32(Values[13]);
                info.LEDlimit1 = Convert.ToInt32(Values[14]);
                info.LEDlimit2 = Convert.ToInt32(Values[15]);
                info.LEDlimit3 = Convert.ToInt32(Values[16]);
                info.LEDlimit4 = Convert.ToInt32(Values[17]);

                // Row Motor
                info.RowName = Values[18];
                info.RowMotorReverse = Convert.ToBoolean(Values[19]);
                info.RowEncoderReverse = Convert.ToBoolean(Values[20]);
                info.RowDirection = Convert.ToInt32(Values[21]);
                info.RowCurrent = Convert.ToInt32(Values[22]);
                info.RowMicrostep = Convert.ToInt32(Values[23]);
                info.RowFullStepsPerRev = Convert.ToInt32(Values[24]);
                info.RowEncoderCountsPerRev = Convert.ToInt32(Values[25]);
                info.RowUnitsPerRev = Convert.ToDouble(Values[26]);
                info.RowPositionError = Convert.ToDouble(Values[27]);
                info.RowSpeed = Convert.ToDouble(Values[28]);
                info.RowAcceleration = Convert.ToDouble(Values[29]);
                info.RowAccurateHome = false;

                // Column Motor
                info.ColumnName = Values[30];
                info.ColumnMotorReverse = Convert.ToBoolean(Values[31]);
                info.ColumnEncoderReverse = Convert.ToBoolean(Values[32]);
                info.ColumnDirection = Convert.ToInt32(Values[33]);
                info.ColumnCurrent = Convert.ToInt32(Values[34]);
                info.ColumnMicrostep = Convert.ToInt32(Values[35]);
                info.ColumnFullStepsPerRev = Convert.ToInt32(Values[36]);
                info.ColumnEncoderCountsPerRev = Convert.ToInt32(Values[37]);
                info.ColumnUnitsPerRev = Convert.ToDouble(Values[38]);
                info.ColumnPositionError = Convert.ToDouble(Values[39]);
                info.ColumnSpeed = Convert.ToDouble(Values[40]);
                info.ColumnAcceleration = Convert.ToDouble(Values[41]);
                info.ColumnAccurateHome = false;

                // Spectrometer 
                info.SpecName = Values[42];
                info.SpecSN = Convert.ToInt32(Values[43]);
                info.Pixel = Convert.ToInt32(Values[44]);
                info.PostIntegrationWait = Convert.ToInt32(Values[45]);
                info.MinimumCycles = Convert.ToInt32(Values[46]);
                //info.PostIntegrationWait = 88;
                //info.MinimumCycles = info.Pixel + 102;
                info.P0 = Convert.ToDouble(Values[47]);
                info.P1 = Convert.ToDouble(Values[48]);
                info.P2 = Convert.ToDouble(Values[49]);
                info.P3 = Convert.ToDouble(Values[50]);
                info.P4 = Convert.ToDouble(Values[51]);

                info.WavelengthStart = Convert.ToDouble(Values[52]);
                info.WavelengthEnd = Convert.ToDouble(Values[53]);
                info.WavelengthSpacing = Convert.ToDouble(Values[54]);

                // Program Settings
                info.RowScan = Convert.ToBoolean(Values[55]);
                info.LEDControl = Convert.ToBoolean(Values[56]);
                info.SoftwarePositionCheck = Convert.ToBoolean(Values[57]);
                info.ReadUserData = Convert.ToBoolean(Values[58]);
                info.LEDPersist = Convert.ToBoolean(Values[59]);
                info.MotorClosedLoop = false;

                // Create Wavelength
                info.Wavelength = new double[info.Pixel];

                for (int i = 0; i < info.Pixel; i++)
                {
                    double p0 = info.P0;
                    double p1 = Math.Pow(i, 1) * info.P1;
                    double p2 = Math.Pow(i, 2) * info.P2;
                    double p3 = Math.Pow(i, 3) * info.P3;
                    double p4 = Math.Pow(i, 4) * info.P4;

                    double value = (p0 + p1 + p2 + p3 + p4);
                    info.Wavelength[i] = value;
                }

                // Find Start Pixel for Wavelength Start
                for (int i = 0; i < info.Pixel; i++)
                {
                    if (info.Wavelength[i] > info.WavelengthStart)
                    {
                        info.PixelStart = i;
                        break;
                    }
                }

                // Find End Pixel for Wavelength End
                for (int i = 0; i < info.Pixel; i++)
                {
                    if (info.Wavelength[i] > info.WavelengthEnd)
                    {
                        info.PixelEnd = i;
                        break;
                    }
                }

                int count = 0;
                for (int i = info.PixelStart; i < info.PixelEnd; i++)
                {
                    count++;
                }

                info.ActiveSize = count;

            }
        }

        public void ReadConfigFile(string filename)
        {
            info = new Info();

            // Read Config Files
            // Get plate configuration filename
            string filepath;

            filepath = Directory.GetCurrentDirectory();
            filepath = Path.GetFullPath(Path.Combine(filepath, @"../../../"));
            filename = Path.Combine(filepath, "configurationFiles", filename);

            // Read File
            List<string> Values = new List<string>();
            bool status = ReadFile(filename, ref Values);

            if (status)
            {
                // Instrument
                info.InstrumentName = Values[0];
                info.SerialNumber = Convert.ToInt32(Values[1]);
                info.LedBus = Convert.ToInt32(Values[2]);
                info.LedChannel = Convert.ToInt32(Values[3]);
                info.RowBus = Convert.ToInt32(Values[4]);
                info.ColumnBus = Convert.ToInt32(Values[5]);
                info.RowOffset = Convert.ToDouble(Values[6]);
                info.ColumnOffset = Convert.ToDouble(Values[7]);
                info.RowEject = Convert.ToDouble(Values[8]);
                info.ColumnEject = Convert.ToDouble(Values[9]);

                // LED
                info.LEDWL1 = Convert.ToInt32(Values[10]);
                info.LEDWL2 = Convert.ToInt32(Values[11]);
                info.LEDWL3 = Convert.ToInt32(Values[12]);
                info.LEDWL4 = Convert.ToInt32(Values[13]);
                info.LEDlimit1 = Convert.ToInt32(Values[14]);
                info.LEDlimit2 = Convert.ToInt32(Values[15]);
                info.LEDlimit3 = Convert.ToInt32(Values[16]);
                info.LEDlimit4 = Convert.ToInt32(Values[17]);

                // Row Motor
                info.RowName = Values[18];
                info.RowMotorReverse = Convert.ToBoolean(Values[19]);
                info.RowEncoderReverse = Convert.ToBoolean(Values[20]);
                info.RowDirection = Convert.ToInt32(Values[21]);
                info.RowCurrent = Convert.ToInt32(Values[22]);
                info.RowMicrostep = Convert.ToInt32(Values[23]);
                info.RowFullStepsPerRev = Convert.ToInt32(Values[24]);
                info.RowEncoderCountsPerRev = Convert.ToInt32(Values[25]);
                info.RowUnitsPerRev = Convert.ToDouble(Values[26]);
                info.RowPositionError = Convert.ToDouble(Values[27]);
                info.RowSpeed = Convert.ToDouble(Values[28]);
                info.RowAcceleration = Convert.ToDouble(Values[29]);
                info.RowAccurateHome = false;

                // Column Motor
                info.ColumnName = Values[30];
                info.ColumnMotorReverse = Convert.ToBoolean(Values[31]);
                info.ColumnEncoderReverse = Convert.ToBoolean(Values[32]);
                info.ColumnDirection = Convert.ToInt32(Values[33]);
                info.ColumnCurrent = Convert.ToInt32(Values[34]);
                info.ColumnMicrostep = Convert.ToInt32(Values[35]);
                info.ColumnFullStepsPerRev = Convert.ToInt32(Values[36]);
                info.ColumnEncoderCountsPerRev = Convert.ToInt32(Values[37]);
                info.ColumnUnitsPerRev = Convert.ToDouble(Values[38]);
                info.ColumnPositionError = Convert.ToDouble(Values[39]);
                info.ColumnSpeed = Convert.ToDouble(Values[40]);
                info.ColumnAcceleration = Convert.ToDouble(Values[41]);
                info.ColumnAccurateHome = false;

                // Spectrometer 
                info.SpecName = Values[42];
                info.SpecSN = Convert.ToInt32(Values[43]);
                info.Pixel = Convert.ToInt32(Values[44]);
                info.PostIntegrationWait = Convert.ToInt32(Values[45]);
                info.MinimumCycles = Convert.ToInt32(Values[46]);
                //info.PostIntegrationWait = 88;
                //info.MinimumCycles = info.Pixel + 102;
                info.P0 = Convert.ToDouble(Values[47]);
                info.P1 = Convert.ToDouble(Values[48]);
                info.P2 = Convert.ToDouble(Values[49]);
                info.P3 = Convert.ToDouble(Values[50]);
                info.P4 = Convert.ToDouble(Values[51]);

                info.WavelengthStart = Convert.ToDouble(Values[52]);
                info.WavelengthEnd = Convert.ToDouble(Values[53]);
                info.WavelengthSpacing = Convert.ToDouble(Values[54]);

                // Program Settings
                info.RowScan = Convert.ToBoolean(Values[55]);
                info.LEDControl = Convert.ToBoolean(Values[56]);
                info.SoftwarePositionCheck = Convert.ToBoolean(Values[57]);
                info.ReadUserData = Convert.ToBoolean(Values[58]);
                info.LEDPersist = Convert.ToBoolean(Values[59]);
                info.MotorClosedLoop = false;

                // Create Wavelength
                info.Wavelength = new double[info.Pixel];

                for (int i = 0; i < info.Pixel; i++)
                {
                    double p0 = info.P0;
                    double p1 = Math.Pow(i, 1) * info.P1;
                    double p2 = Math.Pow(i, 2) * info.P2;
                    double p3 = Math.Pow(i, 3) * info.P3;
                    double p4 = Math.Pow(i, 4) * info.P4;

                    double value = (p0 + p1 + p2 + p3 + p4);
                    info.Wavelength[i] = value;
                }

                // Find Start Pixel for Wavelength Start
                for (int i = 0; i < info.Pixel; i++)
                {
                    if (info.Wavelength[i] > info.WavelengthStart)
                    {
                        info.PixelStart = i;
                        break;
                    }
                }

                // Find End Pixel for Wavelength End
                for (int i = 0; i < info.Pixel; i++)
                {
                    if (info.Wavelength[i] > info.WavelengthEnd)
                    {
                        info.PixelEnd = i;
                        break;
                    }
                }

                int count = 0;
                for (int i = info.PixelStart; i < info.PixelEnd; i++)
                {
                    count++;
                }

                info.ActiveSize = count;

            }
        }

        public void ReadConfigFileNew()
        {
            info = new Info();

            // Read Config Files
            // Get plate configuration filename
            string filepath;
            string filename = "SUPR-UV_Default.scfgx";

            filepath = Directory.GetCurrentDirectory();
            filepath = Path.GetFullPath(Path.Combine(filepath, @"../../../"));
            filename = Path.Combine(filepath, "configurationFiles", filename);

            // Read File
            List<string> Values = new List<string>();
            bool status = ReadFile(filename, ref Values);

            if (status)
            {
                // Instrument
                info.InstrumentName = Values[0];
                info.SerialNumber = Convert.ToInt32(Values[1]);
                info.LEDSN = Convert.ToInt32(Values[2]);
                info.SpecSN = Convert.ToInt32(Values[3]);
                info.LedBus = Convert.ToInt32(Values[4]);
                info.LedChannel = Convert.ToInt32(Values[5]);
                info.RowBus = Convert.ToInt32(Values[6]);
                info.ColumnBus = Convert.ToInt32(Values[7]);
                info.RowOffset = Convert.ToDouble(Values[8]);
                info.ColumnOffset = Convert.ToDouble(Values[9]);
                info.RowEject = Convert.ToDouble(Values[10]);
                info.ColumnEject = Convert.ToDouble(Values[11]);
                info.RowDirection = Convert.ToInt32(Values[12]);
                info.ColumnDirection = Convert.ToInt32(Values[13]);

                // Program Settings
                info.RowScan = Convert.ToBoolean(Values[14]);
                info.LEDControl = Convert.ToBoolean(Values[15]);
                info.MotorClosedLoop = Convert.ToBoolean(Values[16]);
                info.SoftwarePositionCheck = Convert.ToBoolean(Values[17]);
                info.ReadUserData = Convert.ToBoolean(Values[18]);
                info.LEDPersist = Convert.ToBoolean(Values[19]);

                // LED
                info.LEDPulse = Convert.ToInt32(Values[20]);
                info.LEDWL1 = Convert.ToInt32(Values[21]);
                info.LEDWL2 = Convert.ToInt32(Values[22]);
                info.LEDWL3 = Convert.ToInt32(Values[23]);
                info.LEDWL4 = Convert.ToInt32(Values[24]);
                info.LEDlimit1 = Convert.ToInt32(Values[25]);
                info.LEDlimit2 = Convert.ToInt32(Values[26]);
                info.LEDlimit3 = Convert.ToInt32(Values[27]);
                info.LEDlimit4 = Convert.ToInt32(Values[28]);

                // Row Motor
                info.RowName = Values[29];
                info.RowMotorReverse = Convert.ToBoolean(Values[30]);
                info.RowEncoderReverse = Convert.ToBoolean(Values[31]);
                info.RowCurrent = Convert.ToInt32(Values[32]);
                info.RowMicrostep = Convert.ToInt32(Values[33]);
                info.RowFullStepsPerRev = Convert.ToInt32(Values[34]);
                info.RowEncoderCountsPerRev = Convert.ToInt32(Values[35]);
                info.RowUnitsPerRev = Convert.ToDouble(Values[36]);
                info.RowPositionError = Convert.ToDouble(Values[37]);
                info.RowSpeed = Convert.ToDouble(Values[38]);
                info.RowAcceleration = Convert.ToDouble(Values[39]);
                info.RowAccurateHome = Convert.ToBoolean(Values[40]);

                // Column Motor
                info.ColumnName = Values[41];
                info.ColumnMotorReverse = Convert.ToBoolean(Values[42]);
                info.ColumnEncoderReverse = Convert.ToBoolean(Values[43]);
                info.ColumnCurrent = Convert.ToInt32(Values[44]);
                info.ColumnMicrostep = Convert.ToInt32(Values[45]);
                info.ColumnFullStepsPerRev = Convert.ToInt32(Values[46]);
                info.ColumnEncoderCountsPerRev = Convert.ToInt32(Values[47]);
                info.ColumnUnitsPerRev = Convert.ToDouble(Values[48]);
                info.ColumnPositionError = Convert.ToDouble(Values[49]);
                info.ColumnSpeed = Convert.ToDouble(Values[50]);
                info.ColumnAcceleration = Convert.ToDouble(Values[51]);
                info.ColumnAccurateHome = Convert.ToBoolean(Values[52]);

                info.InstrumentGain = Convert.ToDouble(Values[53]);

                // Spectrometer 
                info.SpecName = Values[54];
                info.Pixel = Convert.ToInt32(Values[55]);
                info.PostIntegrationWait = Convert.ToInt32(Values[56]);
                info.MinimumCycles = Convert.ToInt32(Values[57]);
                info.P0 = Convert.ToDouble(Values[58]);
                info.P1 = Convert.ToDouble(Values[59]);
                info.P2 = Convert.ToDouble(Values[60]);
                info.P3 = Convert.ToDouble(Values[61]);
                info.P4 = Convert.ToDouble(Values[62]);

                info.WavelengthStart = Convert.ToDouble(Values[63]);
                info.WavelengthEnd = Convert.ToDouble(Values[64]);
                info.WavelengthSpacing = Convert.ToDouble(Values[65]);

                // Create Wavelength
                info.Wavelength = new double[info.Pixel];

                for (int i = 0; i < info.Pixel; i++)
                {
                    double p0 = info.P0;
                    double p1 = Math.Pow(i, 1) * info.P1;
                    double p2 = Math.Pow(i, 2) * info.P2;
                    double p3 = Math.Pow(i, 3) * info.P3;
                    double p4 = Math.Pow(i, 4) * info.P4;

                    double value = (p0 + p1 + p2 + p3 + p4);
                    info.Wavelength[i] = value;
                }

                // Find Start Pixel for Wavelength Start
                for (int i = 0; i < info.Pixel; i++)
                {
                    if (info.Wavelength[i] > info.WavelengthStart)
                    {
                        info.PixelStart = i;
                        break;
                    }
                }

                // Find End Pixel for Wavelength End
                for (int i = 0; i < info.Pixel; i++)
                {
                    if (info.Wavelength[i] > info.WavelengthEnd)
                    {
                        info.PixelEnd = i;
                        break;
                    }
                }

                int count = 0;
                for (int i = info.PixelStart; i < info.PixelEnd; i++)
                {
                    count++;
                }

                info.ActiveSize = count;


                info.CorrectStart = Convert.ToInt32(Values[66]);
                info.CorrectLength = Convert.ToInt32(Values[67]);

                info.CorrectValues = new double[info.CorrectLength];

                for (int i = 0; i < info.CorrectLength; i++)
                {
                    info.CorrectValues[i] = Convert.ToDouble(Values[68 + i]);
                }

            }
        }

        public void ReadConfigFileNew(string filename)
        {
            info = new Info();

            // Read Config Files
            // Get plate configuration filename
            string filepath;

            filepath = Directory.GetCurrentDirectory();
            filepath = Path.GetFullPath(Path.Combine(filepath, @"../../../"));
            filename = Path.Combine(filepath, "configurationFiles", filename);

            // Read File
            List<string> Values = new List<string>();
            bool status = ReadFile(filename, ref Values);

            if (status)
            {
                // Instrument
                info.InstrumentName = Values[0];
                info.SerialNumber = Convert.ToInt32(Values[1]);
                info.LEDSN = Convert.ToInt32(Values[2]);
                info.SpecSN = Convert.ToInt32(Values[3]);
                info.LedBus = Convert.ToInt32(Values[4]);
                info.LedChannel = Convert.ToInt32(Values[5]);
                info.RowBus = Convert.ToInt32(Values[6]);
                info.ColumnBus = Convert.ToInt32(Values[7]);
                info.RowOffset = Convert.ToDouble(Values[8]);
                info.ColumnOffset = Convert.ToDouble(Values[9]);
                info.RowEject = Convert.ToDouble(Values[10]);
                info.ColumnEject = Convert.ToDouble(Values[11]);
                info.RowDirection = Convert.ToInt32(Values[12]);
                info.ColumnDirection = Convert.ToInt32(Values[13]);

                // Program Settings
                info.RowScan = Convert.ToBoolean(Values[14]);
                info.LEDControl = Convert.ToBoolean(Values[15]);
                info.MotorClosedLoop = Convert.ToBoolean(Values[16]);
                info.SoftwarePositionCheck = Convert.ToBoolean(Values[17]);
                info.ReadUserData = Convert.ToBoolean(Values[18]);
                info.LEDPersist = Convert.ToBoolean(Values[19]);

                // LED
                info.LEDPulse = Convert.ToInt32(Values[20]);
                info.LEDWL1 = Convert.ToInt32(Values[21]);
                info.LEDWL2 = Convert.ToInt32(Values[22]);
                info.LEDWL3 = Convert.ToInt32(Values[23]);
                info.LEDWL4 = Convert.ToInt32(Values[24]);
                info.LEDlimit1 = Convert.ToInt32(Values[25]);
                info.LEDlimit2 = Convert.ToInt32(Values[26]);
                info.LEDlimit3 = Convert.ToInt32(Values[27]);
                info.LEDlimit4 = Convert.ToInt32(Values[28]);

                // Row Motor
                info.RowName = Values[29];
                info.RowMotorReverse = Convert.ToBoolean(Values[30]);
                info.RowEncoderReverse = Convert.ToBoolean(Values[31]);
                info.RowCurrent = Convert.ToInt32(Values[32]);
                info.RowMicrostep = Convert.ToInt32(Values[33]);
                info.RowFullStepsPerRev = Convert.ToInt32(Values[34]);
                info.RowEncoderCountsPerRev = Convert.ToInt32(Values[35]);
                info.RowUnitsPerRev = Convert.ToDouble(Values[36]);
                info.RowPositionError = Convert.ToDouble(Values[37]);
                info.RowSpeed = Convert.ToDouble(Values[38]);
                info.RowAcceleration = Convert.ToDouble(Values[39]);
                info.RowAccurateHome = Convert.ToBoolean(Values[40]);

                // Column Motor
                info.ColumnName = Values[41];
                info.ColumnMotorReverse = Convert.ToBoolean(Values[42]);
                info.ColumnEncoderReverse = Convert.ToBoolean(Values[43]);                
                info.ColumnCurrent = Convert.ToInt32(Values[44]);
                info.ColumnMicrostep = Convert.ToInt32(Values[45]);
                info.ColumnFullStepsPerRev = Convert.ToInt32(Values[46]);
                info.ColumnEncoderCountsPerRev = Convert.ToInt32(Values[47]);
                info.ColumnUnitsPerRev = Convert.ToDouble(Values[48]);
                info.ColumnPositionError = Convert.ToDouble(Values[49]);
                info.ColumnSpeed = Convert.ToDouble(Values[50]);
                info.ColumnAcceleration = Convert.ToDouble(Values[51]);
                info.ColumnAccurateHome = Convert.ToBoolean(Values[52]);

                // Spectrometer 
                info.SpecName = Values[54];
                
                info.Pixel = Convert.ToInt32(Values[55]);
                info.PostIntegrationWait = Convert.ToInt32(Values[56]);
                info.MinimumCycles = Convert.ToInt32(Values[57]);
                info.P0 = Convert.ToDouble(Values[58]);
                info.P1 = Convert.ToDouble(Values[59]);
                info.P2 = Convert.ToDouble(Values[60]);
                info.P3 = Convert.ToDouble(Values[61]);
                info.P4 = Convert.ToDouble(Values[62]);

                info.WavelengthStart = Convert.ToDouble(Values[63]);
                info.WavelengthEnd = Convert.ToDouble(Values[64]);
                info.WavelengthSpacing = Convert.ToDouble(Values[65]);

                // Create Wavelength
                info.Wavelength = new double[info.Pixel];

                for (int i = 0; i < info.Pixel; i++)
                {
                    double p0 = info.P0;
                    double p1 = Math.Pow(i, 1) * info.P1;
                    double p2 = Math.Pow(i, 2) * info.P2;
                    double p3 = Math.Pow(i, 3) * info.P3;
                    double p4 = Math.Pow(i, 4) * info.P4;

                    double value = (p0 + p1 + p2 + p3 + p4);
                    info.Wavelength[i] = value;
                }

                // Find Start Pixel for Wavelength Start
                for (int i = 0; i < info.Pixel; i++)
                {
                    if (info.Wavelength[i] > info.WavelengthStart)
                    {
                        info.PixelStart = i;
                        break;
                    }
                }

                // Find End Pixel for Wavelength End
                for (int i = 0; i < info.Pixel; i++)
                {
                    if (info.Wavelength[i] > info.WavelengthEnd)
                    {
                        info.PixelEnd = i;
                        break;
                    }
                }

                int count = 0;
                for (int i = info.PixelStart; i < info.PixelEnd; i++)
                {
                    count++;
                }

                info.ActiveSize = count;


                info.CorrectStart = Convert.ToInt32(Values[66]);
                info.CorrectLength = Convert.ToInt32(Values[67]);

                info.CorrectValues = new double[info.CorrectLength];

                for (int i=0; i < info.CorrectLength; i++)
                {
                    info.CorrectValues[i] = Convert.ToDouble(Values[68 + i]);
                }

            }
        }

        public bool ReadFile(string filename, ref List<string> values)
        {

            // Step 1: Check if filename exists
            bool fileExist = File.Exists(@filename);

            if (!fileExist)
            {
                MessageBox.Show("Could not find '" + filename +"' Configuration File!", "Information");
                return fileExist;
            }

            // Step 2: Load the file
            var fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            using (StreamReader sr = new StreamReader(fileStream))
            {
                // Read text file line by line
                string line;
                int count = 0;

                while ((line = sr.ReadLine()) != null)
                {
                    // Parse the string based on = sign
                    string[] words = line.Split('\t');

                    // Check for termination 
                    if (line == "End") { break; }

                    int tmp = 0;
                    foreach (var word in words)
                    {
                        if (tmp == 0) { tmp++; continue; }
                        values.Add(word);
                        count++;
                    }
                }
            }

            return true;

        }


        public void WriteConfigFile(string filename)
        {
            string line;

            using (StreamWriter file = new StreamWriter(@filename))
            {
                line = "Name" + "\t" + info.InstrumentName;
                file.WriteLine(line);

                line = "SerialNumber" + "\t" + info.SerialNumber.ToString();
                file.WriteLine(line);

                line = "LEDbus" + "\t" + info.LedBus.ToString();
                file.WriteLine(line);

                line = "LEDchn" + "\t" + info.LedChannel.ToString();
                file.WriteLine(line);

                line = "RowBus" + "\t" + info.RowBus.ToString();
                file.WriteLine(line);

                line = "ColumnBus" + "\t" + info.ColumnBus.ToString();
                file.WriteLine(line);

                line = "RowOffset" + "\t" + info.RowOffset.ToString("F3");
                file.WriteLine(line);

                line = "ColumnOffset" + "\t" + info.ColumnOffset.ToString("F3");
                file.WriteLine(line);

                line = "RowEject" + "\t" + info.RowEject.ToString("F3");
                file.WriteLine(line);

                line = "ColumnEject" + "\t" + info.ColumnEject.ToString("F3");
                file.WriteLine(line);

                line = "LEDWL1" + "\t" + info.LEDWL1.ToString();
                file.WriteLine(line);

                line = "LEDWL2" + "\t" + info.LEDWL2.ToString();
                file.WriteLine(line);

                line = "LEDWL3" + "\t" + info.LEDWL3.ToString();
                file.WriteLine(line);

                line = "LEDWL4" + "\t" + info.LEDWL4.ToString();
                file.WriteLine(line);

                line = "LEDlimit1" + "\t" + info.LEDlimit1.ToString();
                file.WriteLine(line);

                line = "LEDlimit2" + "\t" + info.LEDlimit2.ToString();
                file.WriteLine(line);

                line = "LEDlimit3" + "\t" + info.LEDlimit3.ToString();
                file.WriteLine(line);

                line = "LEDlimit4" + "\t" + info.LEDlimit4.ToString();
                file.WriteLine(line);

                line = "RowName" + "\t" + info.RowName;
                file.WriteLine(line);

                if (info.RowMotorReverse) { line = "RowMotorReverse" + "\t" + "TRUE"; }
                else { line = "RowMotorReverse" + "\t" + "FALSE"; }
                file.WriteLine(line);

                if (info.RowEncoderReverse) { line = "RowEncoderReverse" + "\t" + "TRUE"; }
                else { line = "RowEncoderReverse" + "\t" + "FALSE"; }
                file.WriteLine(line);

                line = "RowDirection" + "\t" + info.RowDirection.ToString();
                file.WriteLine(line);

                line = "RowCurrent" + "\t" + info.RowCurrent.ToString();
                file.WriteLine(line);

                line = "RowMicrostep" + "\t" + info.RowMicrostep.ToString();
                file.WriteLine(line);

                line = "RowFullsteps_per_rev" + "\t" + info.RowFullStepsPerRev.ToString();
                file.WriteLine(line);

                line = "RowEncoderCounts_per_rev" + "\t" + info.RowEncoderCountsPerRev.ToString();
                file.WriteLine(line);

                line = "RowUnits_per_rev" + "\t" + info.RowUnitsPerRev.ToString("F3");
                file.WriteLine(line);

                line = "RowPositionErrorThresh" + "\t" + info.RowPositionError.ToString("F3");
                file.WriteLine(line);

                line = "RowSpeed" + "\t" + info.RowSpeed.ToString("F3");
                file.WriteLine(line);

                line = "RowAcceleration" + "\t" + info.RowAcceleration.ToString("F3");
                file.WriteLine(line);


                line = "ColumnName" + "\t" + info.ColumnName;
                file.WriteLine(line);

                if (info.ColumnMotorReverse) { line = "ColumnMotorReverse =true"; }
                else { line = "ColumnMotorReverse =false"; }
                file.WriteLine(line);

                if (info.ColumnEncoderReverse) { line = "ColumnEncoderReverse =true"; }
                else { line = "ColumnEncoderReverse =false"; }
                file.WriteLine(line);

                line = "ColumnDirection" + "\t" + info.ColumnDirection.ToString();
                file.WriteLine(line);

                line = "ColumnCurrent" + "\t" + info.ColumnCurrent.ToString();
                file.WriteLine(line);

                line = "ColumnMicrostep" + "\t" + info.ColumnMicrostep.ToString();
                file.WriteLine(line);

                line = "ColumnFullsteps_per_rev" + "\t" + info.ColumnFullStepsPerRev.ToString();
                file.WriteLine(line);

                line = "ColumnEncoderCounts_per_rev" + "\t" + info.ColumnEncoderCountsPerRev.ToString();
                file.WriteLine(line);

                line = "ColumnUnits_per_rev" + "\t" + info.ColumnUnitsPerRev.ToString("F3");
                file.WriteLine(line);

                line = "ColumnPositionErrorThresh" + "\t" + info.ColumnPositionError.ToString("F3");
                file.WriteLine(line);

                line = "ColumnSpeed" + "\t" + info.ColumnSpeed.ToString("F3");
                file.WriteLine(line);

                line = "ColumnAcceleration" + "\t" + info.ColumnAcceleration.ToString("F3");
                file.WriteLine(line);

                line = "SpecName" + "\t" + info.SpecName;
                file.WriteLine(line);

                line = "SpecSN" + "\t" + info.SpecSN.ToString();
                file.WriteLine(line);

                line = "Pixels" + "\t" + info.Pixel.ToString();
                file.WriteLine(line);

                line = "PostIntegrationWait" + "\t" + info.PostIntegrationWait.ToString();
                file.WriteLine(line);

                line = "MinimumCycles" + "\t" + info.MinimumCycles.ToString();
                file.WriteLine(line);

                float P0 = (float)info.P0;
                line = "P0" + "\t" + P0.ToString();
                file.WriteLine(line);

                float P1 = (float)info.P1;
                line = "P1" + "\t" + P1.ToString();
                file.WriteLine(line);

                float P2 = (float)info.P2;
                line = "P2" + "\t" + P2.ToString();
                file.WriteLine(line);

                float P3 = (float)info.P3;
                line = "P3" + "\t" + P3.ToString();
                file.WriteLine(line);

                float P4 = (float)info.P4;
                line = "P4" + "\t" + P4.ToString();
                file.WriteLine(line);

                line = "WavelengthStart"+ "\t" + info.WavelengthStart.ToString();
                file.WriteLine(line);

                line = "WavelengthEnd" + "\t" + info.WavelengthEnd.ToString();
                file.WriteLine(line);

                line = "Spacing" + "\t" + info.WavelengthSpacing.ToString();
                file.WriteLine(line);

                line = "RowScan" + "\t" + info.RowScan.ToString();
                file.WriteLine(line);

                line = "LEDControl" + "\t" + info.LEDControl.ToString();
                file.WriteLine(line);

                line = "SoftwarePositionCheck" + "\t" + info.SoftwarePositionCheck.ToString();
                file.WriteLine(line);

                line = "ReadUserData" + "\t" + info.ReadUserData.ToString();
                file.WriteLine(line);

                line = "LEDPersist" + "\t" + info.LEDPersist.ToString();
                file.WriteLine(line);

                line = "End";
                file.WriteLine(line);
            }
        }

        public void WriteConfigFileNew(string filename)
        {
            string line;

            using (StreamWriter file = new StreamWriter(@filename))
            {
                line = "Name" + "\t" + info.InstrumentName;
                file.WriteLine(line);

                line = "SerialNumber" + "\t" + info.SerialNumber.ToString();
                file.WriteLine(line);

                line = "LEDSN" + "\t" + info.LEDSN.ToString();
                file.WriteLine(line);

                line = "SpecSN" + "\t" + info.SpecSN.ToString();
                file.WriteLine(line);

                line = "LEDbus" + "\t" + info.LedBus.ToString();
                file.WriteLine(line);

                line = "LEDchn" + "\t" + info.LedChannel.ToString();
                file.WriteLine(line);

                line = "RowBus" + "\t" + info.RowBus.ToString();
                file.WriteLine(line);

                line = "ColumnBus" + "\t" + info.ColumnBus.ToString();
                file.WriteLine(line);

                line = "RowOffset" + "\t" + info.RowOffset.ToString("F3");
                file.WriteLine(line);

                line = "ColumnOffset" + "\t" + info.ColumnOffset.ToString("F3");
                file.WriteLine(line);

                line = "RowEject" + "\t" + info.RowEject.ToString("F3");
                file.WriteLine(line);

                line = "ColumnEject" + "\t" + info.ColumnEject.ToString("F3");
                file.WriteLine(line);

                line = "RowScanDirection" + "\t" + info.RowDirection.ToString();
                file.WriteLine(line);

                line = "ColumnScanDirection" + "\t" + info.ColumnDirection.ToString();
                file.WriteLine(line);

                line = "RowScan" + "\t" + info.RowScan.ToString();
                file.WriteLine(line);

                line = "LEDControl" + "\t" + info.LEDControl.ToString();
                file.WriteLine(line);

                line = "MotorClosedLoop" + "\t" + info.MotorClosedLoop.ToString();
                file.WriteLine(line);

                line = "SoftwarePositionCheck" + "\t" + info.SoftwarePositionCheck.ToString();
                file.WriteLine(line);

                line = "ReadUserData" + "\t" + info.ReadUserData.ToString();
                file.WriteLine(line);

                line = "LEDPersist" + "\t" + info.LEDPersist.ToString();
                file.WriteLine(line);

                line = "LEDPulse" + "\t" + info.LEDPulse.ToString();
                file.WriteLine(line);

                line = "LEDWL1" + "\t" + info.LEDWL1.ToString();
                file.WriteLine(line);

                line = "LEDWL2" + "\t" + info.LEDWL2.ToString();
                file.WriteLine(line);

                line = "LEDWL3" + "\t" + info.LEDWL3.ToString();
                file.WriteLine(line);

                line = "LEDWL4" + "\t" + info.LEDWL4.ToString();
                file.WriteLine(line);

                line = "LEDlimit1" + "\t" + info.LEDlimit1.ToString();
                file.WriteLine(line);

                line = "LEDlimit2" + "\t" + info.LEDlimit2.ToString();
                file.WriteLine(line);

                line = "LEDlimit3" + "\t" + info.LEDlimit3.ToString();
                file.WriteLine(line);

                line = "LEDlimit4" + "\t" + info.LEDlimit4.ToString();
                file.WriteLine(line);

                line = "RowName" + "\t" + info.RowName;
                file.WriteLine(line);

                line = "RowMotorReverse" + "\t" + info.RowMotorReverse.ToString();
                file.WriteLine(line);

                line = "RowEncoderReverse" + "\t" + info.RowEncoderReverse.ToString();
                file.WriteLine(line);

                //if (info.RowEncoderReverse) { line = "RowEncoderReverse" + "\t" + "TRUE"; }
                //else { line = "RowEncoderReverse" + "\t" + "FALSE"; }
                //file.WriteLine(line);

                line = "RowCurrent" + "\t" + info.RowCurrent.ToString();
                file.WriteLine(line);

                line = "RowMicrostep" + "\t" + info.RowMicrostep.ToString();
                file.WriteLine(line);

                line = "RowFullsteps_per_rev" + "\t" + info.RowFullStepsPerRev.ToString();
                file.WriteLine(line);

                line = "RowEncoderCounts_per_rev" + "\t" + info.RowEncoderCountsPerRev.ToString();
                file.WriteLine(line);

                line = "RowUnits_per_rev" + "\t" + info.RowUnitsPerRev.ToString("F3");
                file.WriteLine(line);

                line = "RowPositionErrorThresh" + "\t" + info.RowPositionError.ToString("F3");
                file.WriteLine(line);

                line = "RowSpeed" + "\t" + info.RowSpeed.ToString("F3");
                file.WriteLine(line);

                line = "RowAcceleration" + "\t" + info.RowAcceleration.ToString("F3");
                file.WriteLine(line);

                line = "RowAccurateHome" + "\t" + info.RowAccurateHome.ToString();
                file.WriteLine(line);


                line = "ColumnName" + "\t" + info.ColumnName;
                file.WriteLine(line);

                line = "ColumnMotorReverse" + "\t" + info.ColumnMotorReverse.ToString();
                file.WriteLine(line);

                line = "ColumnEncoderReverse" + "\t" + info.ColumnEncoderReverse.ToString();
                file.WriteLine(line);


                //if (info.ColumnEncoderReverse) { line = "ColumnEncoderReverse =true"; }
                //else { line = "ColumnEncoderReverse =false"; }
                //file.WriteLine(line);

                line = "ColumnCurrent" + "\t" + info.ColumnCurrent.ToString();
                file.WriteLine(line);

                line = "ColumnMicrostep" + "\t" + info.ColumnMicrostep.ToString();
                file.WriteLine(line);

                line = "ColumnFullsteps_per_rev" + "\t" + info.ColumnFullStepsPerRev.ToString();
                file.WriteLine(line);

                line = "ColumnEncoderCounts_per_rev" + "\t" + info.ColumnEncoderCountsPerRev.ToString();
                file.WriteLine(line);

                line = "ColumnUnits_per_rev" + "\t" + info.ColumnUnitsPerRev.ToString("F3");
                file.WriteLine(line);

                line = "ColumnPositionErrorThresh" + "\t" + info.ColumnPositionError.ToString("F3");
                file.WriteLine(line);

                line = "ColumnSpeed" + "\t" + info.ColumnSpeed.ToString("F3");
                file.WriteLine(line);

                line = "ColumnAcceleration" + "\t" + info.ColumnAcceleration.ToString("F3");
                file.WriteLine(line);

                line = "ColumnAccurateHome" + "\t" + info.ColumnAccurateHome.ToString();
                file.WriteLine(line);

                float g = (float)info.InstrumentGain;
                line = "InstrumentGain" + "\t" + g.ToString();
                file.WriteLine(line);

                line = "SpecName" + "\t" + info.SpecName;
                file.WriteLine(line);

                line = "Pixels" + "\t" + info.Pixel.ToString();
                file.WriteLine(line);

                line = "PostIntegrationWait" + "\t" + info.PostIntegrationWait.ToString();
                file.WriteLine(line);

                line = "MinimumCycles" + "\t" + info.MinimumCycles.ToString();
                file.WriteLine(line);

                float P0 = (float)info.P0;
                line = "P0" + "\t" + P0.ToString();
                file.WriteLine(line);

                float P1 = (float)info.P1;
                line = "P1" + "\t" + P1.ToString();
                file.WriteLine(line);

                float P2 = (float)info.P2;
                line = "P2" + "\t" + P2.ToString();
                file.WriteLine(line);

                float P3 = (float)info.P3;
                line = "P3" + "\t" + P3.ToString();
                file.WriteLine(line);

                float P4 = (float)info.P4;
                line = "P4" + "\t" + P4.ToString();
                file.WriteLine(line);

                line = "WavelengthStart" + "\t" + info.WavelengthStart.ToString();
                file.WriteLine(line);

                line = "WavelengthEnd" + "\t" + info.WavelengthEnd.ToString();
                file.WriteLine(line);

                line = "Spacing" + "\t" + info.WavelengthSpacing.ToString();
                file.WriteLine(line);

                line = "CorrectStart" + "\t" + info.CorrectStart.ToString();
                file.WriteLine(line);

                line = "CorrectLength" + "\t" + info.CorrectLength.ToString();
                file.WriteLine(line);

                for (int n=0; n < info.CorrectLength; n++)
                {
                    float v = (float)info.CorrectValues[n];
                    line = "V" + (n+1).ToString() + "\t" + v.ToString();
                    file.WriteLine(line);
                }

                line = "End";
                file.WriteLine(line);
            }
        }
    }
}
