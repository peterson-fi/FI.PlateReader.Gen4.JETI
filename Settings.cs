using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Versa_Handle_t = System.Int32;

namespace FI.PlateReader.Gen4.JETI
{
    unsafe class Settings
    {
        public enum Versa_DeviceType { Versa_UnknownDevice, Versa_InterfaceBoard_35_1 };

        // Versa
        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Versa_getVersion(int* majorVersion, int* minorVersion);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_getAvailableDevices(int* deviceCount, Versa_DeviceType* deviceTypes);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_initialiseUSBSession();

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_closeSession();

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Versa_resetAllPorts();

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Versa_getLastErrorMessage(byte* message);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_uploadUserData(byte* pData);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_downloadUserData(byte* pData);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_Peripheral_uploadUserData(Versa_Handle_t handle, byte* pData);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_Peripheral_downloadUserData(Versa_Handle_t handle, byte* pData);


        public Info info { get; set; }
        public class Info
        {

            // Hard code
            public int RowBus = 8;
            public int ColumnBus = 4;
            public int LEDBus = 3;
            public bool RowScan = true;
            public bool LEDControl = true;
            public bool MotorClosedLoop = true;
            public bool SoftwarePositionCheck = true;

            public int RowCurrent = 1200;                       // mA
            public int RowStepsPerRev = 200;                    // steps per revolution
            public int RowEncoderCountsPerRev = 4000;           // counts per revolution
            public double RowUnitsPerRev = 40;                     // mm per revolution
            public double RowSpeed = 100;                          // mm per second
            public double RowAcceleration = 1000;                  // mm per second per second
            public double RowPositionError = 0.5;               // mm
            public bool RowMotorReverse = true;
            public bool RowEncoderReverse = true;
            public int RowMicrostep = 16;
            public bool RowAccurateHome = true;

            public int ColumnCurrent = 600;                     // mA
            public int ColumnStepsPerRev = 200;                 // steps per revolution
            public int ColumnEncoderCountsPerRev = 4000;        // counts per revolution
            public double ColumnUnitsPerRev = 40;                  // mm per revolution
            public double ColumnSpeed = 100;                       // mm per second
            public double ColumnAcceleration = 1000;               // mm per second per second
            public double ColumnPositionError = 0.5;            // mm
            public bool ColumnMotorReverse = true;
            public bool ColumnEncoderReverse = true;
            public int ColumnMicrostep = 16;
            public bool ColumnAccurateHome = true;

            // VERSA Memory
            public byte[] data;
            public string FirmwareVersion;

            // Serial Numbers & Name
            public string InstrumentName;
            public int InstrumentSerialNumber;
            public int LEDSerialNumber;
            public int SpectrometerSerialNumber;

            // Reference Positions
            public double RowOffset;
            public double ColumnOffset;
            public double RowEject;
            public double ColumnEject;

            // Scan Direction
            public int RowDirection;
            public int ColumnDirection;

            // LED
            public int LEDChannel = 1;
            public bool LEDPulse;
            public int LEDWavelength;
            public int MaxCurrent;

            // Spectrometer
            public string Detector = "Spectrometer";
            public int PostIntegrationWaitTime = 88;            // ms
            public int MinimumCycles;
            public bool SpectrometerReverse;
            public double SpectrometerGain;
            public int NPixel;
            public double P0;
            public double P1;
            public double P2;
            public double P3;
            public double P4;
            public int StartPixel;
            public int PixelLength;

            public double WavelengthStart;
            public double WavelengthEnd;

            public double[] Wavelength;
            public double[] WavelengthCorrection;

        }



        // Methods
        public bool ReadData()
        {
            info = new Info();

            // Read default values from configuration file.   
            string fname = "";
            ReadConfigFile(fname);

            // Connect to Versa
            bool state = Connect();

            // Try to download the data
            if (state)
            {
                // Download data from EEPROM 
                //state = DownloadData();
                Disconnect();
            }

            // Check if download was sucessfull
            if (state)
            {
                return true;
            }
            else
            {
                DefaultValues();
                return false;
            }

        }

        public bool WriteData()
        {
            // Connect to Versa
            //bool state = Connect();
            bool state = false;
            // Try to download the data
            //if (state)
            //{
                // Download data from EEPROM 
                state = UploadData();
                //Disconnect();
            //}

            // Check if download was sucessfull
            if (state)
            {
                return true;
            }
            else
            {
                //DefaultValues();
                return false;
            }

        }

        public bool Connect()
        {

            // Get API Firmware Version
            int majorVersion = 0;
            int minorVersion = 0;
            Versa_getVersion(&majorVersion, &minorVersion);

            info.FirmwareVersion = majorVersion.ToString() + "." + minorVersion.ToString();

            // Variables
            bool state = false;

            // Available devices
            int deviceCount;
            Versa_DeviceType versa_DeviceType;

            CheckError(Versa_getAvailableDevices(&deviceCount, &versa_DeviceType));

            if (deviceCount > 0)
            {
                state = CheckError(Versa_initialiseUSBSession());
            }

            return state;
        }

        public bool Disconnect()
        {
            return CheckError(Versa_closeSession());
        }

        public bool DownloadData()
        {
            // Initialise array to Versa Memory size
            byte[] data = new byte[1792];
            bool validData;

            // Use fixed statement to access managed data
            bool state = false;
            unsafe
            {
                fixed (byte* dPointer = &data[0])
                {
                    state = CheckError(Versa_downloadUserData(dPointer));
                    validData = state;
                }
            }


            // Sort Data
            //info = new Info();
            //ReadConfigFile();

            // Name & Serial Number
            info.data = data;
            info.InstrumentName = Encoding.ASCII.GetString(data, 0, 10);
            info.InstrumentSerialNumber = BitConverter.ToInt32(data, 10);
            info.LEDSerialNumber = BitConverter.ToInt32(data, 14);
            info.SpectrometerSerialNumber = BitConverter.ToInt32(data, 18);

            // Stage
            float rowOffset = BitConverter.ToSingle(data, 22);
            // check if offset is a valid number
            if (rowOffset == float.NaN) { validData = false; }
            else
            {
                // should change to max row travel?
                if ((rowOffset >= 0) && (rowOffset < 200))
                {
                    info.RowOffset = rowOffset;
                }
                else
                {
                    info.RowOffset = 45.145;
                    validData = false;
                }
            }

            float columnOffset = BitConverter.ToSingle(data, 26);
            // check if offset is a valid number
            if (columnOffset == float.NaN) { validData = false; }
            else
            {
                // should change to max row travel?
                if ((columnOffset >= 0) && (columnOffset < 200))
                {
                    info.ColumnOffset = columnOffset;
                }
                else
                {
                    info.ColumnOffset = -3.465;
                    validData = false;
                }
            }
            float rowEject = BitConverter.ToSingle(data, 30);
            // check if eject is a valid number
            if (rowEject == float.NaN) { validData = false; }
            else
            {
                // should change to max row travel?
                if ((rowEject >= 0) && (rowEject < 200))
                {
                    info.RowEject = rowEject;
                }
                else
                {
                    info.RowEject = 10;
                    validData = false;
                }
            }
            float columnEject = BitConverter.ToSingle(data, 34);
            // check if eject is a valid number
            if (columnEject == float.NaN) { validData = false; }
            else
            {
                // should change to max row travel?
                if ((columnEject >= 0) && (rowEject < 200))
                {
                    info.ColumnEject = columnEject;
                }
                else
                {
                    info.ColumnEject = 10;
                    validData = false;
                }
            }

            // Round values to two decimal places
            info.RowOffset = Math.Round(info.RowOffset, 2);
            info.ColumnOffset = Math.Round(info.ColumnOffset, 2);
            info.RowEject = Math.Round(info.RowEject, 2);
            info.ColumnEject = Math.Round(info.ColumnEject, 2);

            info.RowDirection = BitConverter.ToInt32(data, 38);
            info.ColumnDirection = BitConverter.ToInt32(data, 42);

            // LED  
            int temp = BitConverter.ToInt32(data, 46);
            if (temp == 0)
            {
                info.LEDPulse = false;
            }
            else
            {
                info.LEDPulse = true;
            }

            info.LEDWavelength = BitConverter.ToInt32(data, 50);
            info.MaxCurrent = BitConverter.ToInt32(data, 54);

            // Spectrometer
            info.SpectrometerGain = BitConverter.ToInt32(data, 58);
            info.NPixel = BitConverter.ToInt32(data, 62);
            info.MinimumCycles = info.NPixel + 102;

            info.P0 = BitConverter.ToSingle(data, 66);
            info.P1 = BitConverter.ToSingle(data, 70);
            info.P2 = BitConverter.ToSingle(data, 74);
            info.P3 = BitConverter.ToSingle(data, 78);
            info.P4 = BitConverter.ToSingle(data, 82);

            info.StartPixel = BitConverter.ToInt32(data, 86);
            info.PixelLength = BitConverter.ToInt32(data, 90);

            // Wavelength Correction
            info.WavelengthCorrection = new double[412];
            int startByte = 94;

            for(int i = 0; i < 412; i++)
            {
                info.WavelengthCorrection[i] = BitConverter.ToSingle(data, i * 4 + startByte);
            }

            //// Read row motor reverse
            //int rowmotorReverse = BitConverter.ToInt32(data, 1742);
            //if (rowmotorReverse == 0) { info.RowMotorReverse = false; }
            //else { info.RowMotorReverse = true; }

            //// Read row motor current
            //int rowmotorCurrent = BitConverter.ToInt32(data, 1746);
            //if (rowmotorCurrent < 50) { rowmotorCurrent = 50; }
            //else if (rowmotorCurrent > 1800) { rowmotorCurrent = 1800; }
            //info.RowCurrent = rowmotorCurrent;

            //// Read row motor microstep
            //int rowmotorMicrostep = BitConverter.ToInt32(data, 1750);
            //if ((rowmotorMicrostep == 1) | (rowmotorMicrostep == 2) | (rowmotorMicrostep == 4) | (rowmotorMicrostep == 8) | (rowmotorMicrostep == 16) | (rowmotorMicrostep == 32) | (rowmotorMicrostep == 64))
            //{
            //}
            //else
            //{
            //    rowmotorMicrostep = 8;
            //}    
            //info.RowMicrostep = rowmotorMicrostep;

            //// Read row motor units
            //float rowmotorUnits = BitConverter.ToSingle(data, 1754);
            //if (rowmotorUnits < 10) { rowmotorUnits = 10; }
            //else if (rowmotorUnits > 1000) { rowmotorUnits = 1000; }
            //info.RowUnitsPerRev = rowmotorUnits;

            //// Read column motor reverse
            //int colmotorReverse = BitConverter.ToInt32(data, 1758);
            //if (colmotorReverse == 0) { info.ColumnMotorReverse = false; }
            //else { info.ColumnMotorReverse = true; }

            //// Read column motor current
            //int colmotorCurrent = BitConverter.ToInt32(data, 1762);
            //if (colmotorCurrent < 50) { colmotorCurrent = 50; }
            //else if (colmotorCurrent > 1800) { colmotorCurrent = 1800; }
            //info.ColumnCurrent = colmotorCurrent;

            //// Read column motor microstep
            //int colmotorMicrostep = BitConverter.ToInt32(data, 1766);
            //if ((colmotorMicrostep == 1) | (colmotorMicrostep == 2) | (colmotorMicrostep == 4) | (colmotorMicrostep == 8) | (colmotorMicrostep == 16) | (colmotorMicrostep == 32) | (colmotorMicrostep == 64))
            //{
            //}
            //else
            //{
            //    colmotorMicrostep = 8;
            //}
            //info.ColumnMicrostep = colmotorMicrostep;

            //// Read column motor units
            //float colmotorUnits = BitConverter.ToSingle(data, 1770);
            //if (colmotorUnits < 10) { colmotorUnits = 10; }
            //else if (colmotorUnits > 1000) { colmotorUnits = 1000; }
            //info.ColumnUnitsPerRev = colmotorUnits;

            // Create Wavelength
            info.Wavelength = new double[info.NPixel];

            for (int i = 0; i < info.NPixel; i++)
            {
                double p0 = info.P0;
                double p1 = Math.Pow(i+1, 1) * info.P1;
                double p2 = Math.Pow(i+1, 2) * info.P2;
                double p3 = Math.Pow(i+1, 3) * info.P3;
                double p4 = Math.Pow(i+1, 4) * info.P4;

                double value = (p0 + p1 + p2 + p3 + p4);
                info.Wavelength[i] = value;
            }

            // If array is from High to Low, then reverse it (It should be low to high)
            if(info.Wavelength[0] > info.Wavelength[info.NPixel - 1])
            {
                info.SpectrometerReverse = true;
                Array.Reverse(info.Wavelength);                
            }
            else
            {
                info.SpectrometerReverse = false;
            }

            info.WavelengthStart = Math.Floor(info.Wavelength[info.StartPixel]);
            info.WavelengthEnd = Math.Floor(info.Wavelength[info.StartPixel + info.PixelLength]);

            // Check Values
            if (state)
            {
                return CheckValues();
            }
            else
            {
                return false;
            }

        }

        public bool UploadData()
        {
            // From string to byte array
            byte[] uData = new byte[1792];       //128

            // if name is longer than 10 characters, truncate to fixed 10 characters
            if (info.InstrumentName.Length > 10) { info.InstrumentName = info.InstrumentName.Substring(0, 10); }
            byte[] name = Encoding.ASCII.GetBytes(info.InstrumentName);
            name.CopyTo(uData, 0);

            byte[] sn = BitConverter.GetBytes(info.InstrumentSerialNumber);
            sn.CopyTo(uData, 10);

            byte[] ledSN = BitConverter.GetBytes(info.LEDSerialNumber);
            ledSN.CopyTo(uData, 14);

            byte[] specSN = BitConverter.GetBytes(info.SpectrometerSerialNumber);
            specSN.CopyTo(uData, 18);

            byte[] roffset = BitConverter.GetBytes((float)info.RowOffset);
            roffset.CopyTo(uData, 22);

            byte[] coffset = BitConverter.GetBytes((float)info.ColumnOffset);
            coffset.CopyTo(uData, 26);

            byte[] reject = BitConverter.GetBytes((float)info.RowEject);
            reject.CopyTo(uData, 30);

            byte[] ceject = BitConverter.GetBytes((float)info.ColumnEject);
            ceject.CopyTo(uData, 34);

            byte[] rdir = BitConverter.GetBytes(info.RowDirection);
            rdir.CopyTo(uData, 38);

            byte[] cdir = BitConverter.GetBytes(info.ColumnDirection);
            cdir.CopyTo(uData, 42);

            byte[] ledPulse = BitConverter.GetBytes(info.LEDPulse);
            ledPulse.CopyTo(uData, 46);

            byte[] ledWL = BitConverter.GetBytes(info.LEDWavelength);
            ledWL.CopyTo(uData, 50);

            byte[] ledMax = BitConverter.GetBytes(info.MaxCurrent);
            ledMax.CopyTo(uData, 54);

            byte[] instGain = BitConverter.GetBytes((float)info.SpectrometerGain);
            instGain.CopyTo(uData, 58);

            byte[] pixels = BitConverter.GetBytes(info.NPixel);
            pixels.CopyTo(uData, 62);

            byte[] p0 = BitConverter.GetBytes((float)info.P0);
            p0.CopyTo(uData, 66);

            byte[] p1 = BitConverter.GetBytes((float)info.P1);
            p1.CopyTo(uData, 70);

            byte[] p2 = BitConverter.GetBytes((float)info.P2);
            p2.CopyTo(uData, 74);

            byte[] p3 = BitConverter.GetBytes((float)info.P3);
            p3.CopyTo(uData, 78);

            byte[] p4 = BitConverter.GetBytes((float)info.P4);
            p4.CopyTo(uData, 82);

            byte[] cStart = BitConverter.GetBytes(info.StartPixel);
            cStart.CopyTo(uData, 86);

            byte[] cLength = BitConverter.GetBytes(info.PixelLength);
            cLength.CopyTo(uData, 90);

            for (int n = 0; n < info.PixelLength; n++)
            {
                byte[] cValue = BitConverter.GetBytes((float)info.WavelengthCorrection[n]);
                cValue.CopyTo(uData, 94 + (4 * n));
            }

            // Write row motor reverse
            byte[] rowReverse = BitConverter.GetBytes(info.RowMotorReverse);
            rowReverse.CopyTo(uData, 1742);

            // Write row motor current
            byte[] rowCurrent = BitConverter.GetBytes(info.RowCurrent);
            rowCurrent.CopyTo(uData, 1746);

            // Write row motor microstep
            byte[] rowMicrostep = BitConverter.GetBytes(info.RowMicrostep);
            rowMicrostep.CopyTo(uData, 1750);

            // Read row motor units
            byte[] rowUnits = BitConverter.GetBytes((float)info.RowUnitsPerRev);
            rowUnits.CopyTo(uData, 1754);

            // Write row motor reverse
            byte[] columnReverse = BitConverter.GetBytes(info.RowMotorReverse);
            columnReverse.CopyTo(uData, 1758);

            // Write row motor current
            byte[] columnCurrent = BitConverter.GetBytes(info.RowCurrent);
            columnCurrent.CopyTo(uData, 1762);

            // Write row motor microstep
            byte[] columnMicrostep = BitConverter.GetBytes(info.RowMicrostep);
            columnMicrostep.CopyTo(uData, 1766);

            // Read row motor units
            byte[] columnUnits = BitConverter.GetBytes((float)info.RowUnitsPerRev);
            columnUnits.CopyTo(uData, 1770);

            bool state = false;
            unsafe
            {
                fixed (byte* dPointer = &uData[0])
                {
                    state = CheckError(Versa_uploadUserData(dPointer));
                };
            }

            // Check Values
            if (state)
            {
                return CheckValues();
            }
            else
            {
                return false;
            }

        }

        public void DefaultValues()
        {

            // Sort Data
            info = new Info();

            // Name & Serial Number
            info.InstrumentName = "SUPR-UV";
            info.InstrumentSerialNumber = 1;
            info.LEDSerialNumber = 1;
            info.SpectrometerSerialNumber = 1;

            // Stage
            info.RowOffset = 96;
            info.ColumnOffset = 127;
            info.RowEject = 230;
            info.ColumnEject = 52;
            info.RowDirection = -1;
            info.ColumnDirection = -1;

            // LED
            info.LEDPulse = false;
            info.LEDWavelength = 275;
            info.MaxCurrent = 200;

            // Spectrometer
            info.SpectrometerGain = 1;
            info.NPixel = 2048;
            info.MinimumCycles = info.NPixel + 102;

            info.P0 = 35.6005;
            info.P1 = 0.211684;
            info.P2 = 0.0000849592;
            info.P3 = -0.0000000370257;
            info.P4 = 0.00000000000232069;

            info.StartPixel = 1045;
            info.PixelLength = 400;

            // Wavelength Correction
            info.WavelengthCorrection = new double[400];


            for (int i = 0; i < 400; i++)
            {
                info.WavelengthCorrection[i] = 1;
            }

            // Create Wavelength
            info.Wavelength = new double[info.NPixel];

            for (int i = 0; i < info.NPixel; i++)
            {
                double p0 = info.P0;
                double p1 = Math.Pow(i + 1, 1) * info.P1;
                double p2 = Math.Pow(i + 1, 2) * info.P2;
                double p3 = Math.Pow(i + 1, 3) * info.P3;
                double p4 = Math.Pow(i + 1, 4) * info.P4;

                double value = (p0 + p1 + p2 + p3 + p4);
                info.Wavelength[i] = value;
            }

            info.WavelengthStart = info.Wavelength[info.StartPixel];
            info.WavelengthEnd = info.Wavelength[info.StartPixel + info.PixelLength];
        }

        public void ReadConfigFile(string configfile)
        {
            info = new Info();

            string filepath;
            string filename;

            // check if configFile is empty string. If it is, load default
            if (String.IsNullOrEmpty(configfile))
            {
                filename = "SUPR-CM_Default.scfgx";
            }
            else
            {
                filename = configfile;
            }

            filepath = Directory.GetCurrentDirectory();
            filepath = Path.GetFullPath(Path.Combine(filepath, @"../../../"));
            filename = Path.Combine(filepath, "configurationFiles", filename);

            // check if file exist
            bool fcheck = File.Exists(filename);
            if (!fcheck)
            {
                MessageBox.Show("Configuration file does not exist!");
                return;
            }
            

            // Read File
            List<string> Values = new List<string>();
            bool status = ReadFile(filename, ref Values);

            if (status)
            {
                // Instrument
                info.InstrumentName = Values[0];
                info.InstrumentSerialNumber = Convert.ToInt32(Values[1]);
                info.LEDSerialNumber = Convert.ToInt32(Values[2]);
                info.SpectrometerSerialNumber = Convert.ToInt32(Values[3]);
                info.LEDBus = Convert.ToInt32(Values[4]);
                info.LEDChannel = Convert.ToInt32(Values[5]);
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

                // LED
                info.LEDPulse = Convert.ToBoolean(Values[18]);
                info.LEDWavelength = Convert.ToInt32(Values[19]);
                info.MaxCurrent = Convert.ToInt32(Values[20]);

                // Row Motor
                info.RowMotorReverse = Convert.ToBoolean(Values[21]);
                info.RowEncoderReverse = Convert.ToBoolean(Values[22]);
                info.RowCurrent = Convert.ToInt32(Values[23]);
                info.RowMicrostep = Convert.ToInt32(Values[24]);
                info.RowStepsPerRev = Convert.ToInt32(Values[25]);
                info.RowEncoderCountsPerRev = Convert.ToInt32(Values[26]);
                info.RowUnitsPerRev = Convert.ToDouble(Values[27]);
                info.RowPositionError = Convert.ToDouble(Values[28]);
                info.RowSpeed = Convert.ToDouble(Values[29]);
                info.RowAcceleration = Convert.ToDouble(Values[30]);
                info.RowAccurateHome = Convert.ToBoolean(Values[31]);

                // Column Motor
                info.ColumnMotorReverse = Convert.ToBoolean(Values[32]);
                info.ColumnEncoderReverse = Convert.ToBoolean(Values[33]);
                info.ColumnCurrent = Convert.ToInt32(Values[34]);
                info.ColumnMicrostep = Convert.ToInt32(Values[35]);
                info.ColumnStepsPerRev = Convert.ToInt32(Values[36]);
                info.ColumnEncoderCountsPerRev = Convert.ToInt32(Values[37]);
                info.ColumnUnitsPerRev = Convert.ToDouble(Values[38]);
                info.ColumnPositionError = Convert.ToDouble(Values[39]);
                info.ColumnSpeed = Convert.ToDouble(Values[40]);
                info.ColumnAcceleration = Convert.ToDouble(Values[41]);
                info.ColumnAccurateHome = Convert.ToBoolean(Values[42]);

                // Spectrometer 
                info.Detector = Values[43];
                info.SpectrometerGain = Convert.ToDouble(Values[44]);

                info.NPixel = Convert.ToInt32(Values[45]);
                info.PostIntegrationWaitTime = Convert.ToInt32(Values[46]);
                info.MinimumCycles = Convert.ToInt32(Values[47]);
                info.P0 = Convert.ToDouble(Values[48]);
                info.P1 = Convert.ToDouble(Values[49]);
                info.P2 = Convert.ToDouble(Values[50]);
                info.P3 = Convert.ToDouble(Values[51]);
                info.P4 = Convert.ToDouble(Values[52]);
           
                // Create Wavelength
                info.Wavelength = new double[info.NPixel];

                for (int i = 0; i < info.NPixel; i++)
                {
                    double p0 = info.P0;
                    double p1 = Math.Pow(i, 1) * info.P1;
                    double p2 = Math.Pow(i, 2) * info.P2;
                    double p3 = Math.Pow(i, 3) * info.P3;
                    double p4 = Math.Pow(i, 4) * info.P4;

                    double value = (p0 + p1 + p2 + p3 + p4);
                    info.Wavelength[i] = value;
                }

                info.StartPixel = Convert.ToInt32(Values[53]);
                info.PixelLength = Convert.ToInt32(Values[54]);

                info.WavelengthStart = info.Wavelength[info.StartPixel];
                info.WavelengthEnd = info.Wavelength[info.StartPixel + info.PixelLength];

                info.WavelengthCorrection = new double[info.PixelLength];

                for (int i = 0; i < info.PixelLength; i++)
                {
                    info.WavelengthCorrection[i] = Convert.ToDouble(Values[55 + i]);
                }

            }
        }

        public bool ReadFile(string filename, ref List<string> values)
        {

            // Step 1: Check if filename exists
            bool fileExist = File.Exists(@filename);

            if (!fileExist)
            {
                MessageBox.Show("Could not find '" + filename + "' Configuration File!", "Information");
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

        // Error Handling
        public bool CheckError(int value)
        {
            // Initialize new byte array to receive the error message
            byte[] data = new byte[1000];

            if (value == 0)
            {
                return true;
            }
            else
            {
                unsafe
                {
                    fixed (byte* erPointer = &data[0])
                    {
                        Versa_getLastErrorMessage(erPointer);
                    }
                }

                // Convert byte array to string.
                string msg = System.Text.Encoding.UTF8.GetString(data, 0, 1000);
                if (msg.Contains('\0')) { msg = msg.Substring(0, msg.IndexOf('\0')); }

                MessageBox.Show(msg);
            }

            return false;
        }

        public bool CheckValues()
        {
            return true;
        }











    }
}
