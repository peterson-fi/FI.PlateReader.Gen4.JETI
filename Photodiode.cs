using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FI.PlateReader.Gen4.JETI
{
    unsafe class Photodiode
    {

        // Data Struts for DLL Commands
        public struct TIA_ResultData_t
        {
            public double sensor1ConvFactor;
            public double sensor2ConvFactor;
            public double sensor1Average;
            public double sensor2Average;
            public int traceCount;
            public double traceDeltaT_s;
            public double* sensor1Trace;
            public double* sensor2Trace;
        };

        public struct TIA_Result_t
        {
            public TIA_ResultType resultType;
            public TIA_ResultError error;
            public int dataCount;
            public TIA_ResultData_t* data;
        };

        // Enum Variable Types
        public enum TIA_MeasurementType { TIA_MeasurementTimeResolved, TIA_MeasurementAveraged };
        public enum TIA_ResultType { TIA_ResultTimeResolved, TIA_ResultAveraged };
        public enum TIA_ResultError { TIA_ResultErrorNone, TIA_ResultErrorEmpty, TIA_ResultErrorScanOverlap, TIA_ResultErrorScanAxisLimit };
        public enum TIA_MeasurementGain { TIA_MeasurementGain_10E9, TIA_MeasurementGain_10E10 };

        // General Commands
        [DllImport("TIALib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TIA_getVersion(ref int majorVersion, ref int minorVersion);

        [DllImport("TIALib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TIA_initialiseSession();

        [DllImport("TIALib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TIA_closeSession();

        [DllImport("TIALib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TIA_resetAllPorts();

        [DllImport("TIALib.dll", CallingConvention = CallingConvention.Cdecl)]
        //public static extern void TIA_getLastErrorMessage(ref string message);
        public static extern void TIA_getLastErrorMessage(byte* message);

        // Measurement Commands
        [DllImport("TIALib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TIA_setMeasurementDuration(double ms);

        [DllImport("TIALib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TIA_setMeasurementSampleRate(int samplesPerSecond);

        [DllImport("TIALib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TIA_setMeasurementGain(TIA_MeasurementGain sensor1Gain, TIA_MeasurementGain sensor2Gain);

        [DllImport("TIALib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TIA_resetMeasurementSampleRate();

        [DllImport("TIALib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TIA_startSingleWellMeasurement();

        [DllImport("TIALib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TIA_stopMeasurement();

        [DllImport("TIALib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool TIA_isMeasurementCompleted();

        [DllImport("TIALib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TIA_waitOnMeasurement(int timeout_ms);

        [DllImport("TIALib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int TIA_getResult(TIA_Result_t* pResult);

        [DllImport("TIALib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TIA_Result_t_init(TIA_Result_t* pResult);

        [DllImport("TIALib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TIA_Result_t_free(TIA_Result_t* pResult);


        // Public Variables
        public bool connected { get; set; }
        public int SamplePoints { get; set; }
        public double TimeStep { get; set; }
        public double[] Time { get; set; }

        public double[] Background1 { get; set; }
        public double[] Background2 { get; set; }

        public double[] Waveform1 { get; set; }
        public double[] Waveform2 { get; set; }

        public double[] Waveform { get; set; }

        // Methods
        // General Connect/Disconnect       
        public void getVersion(ref int majorVersion, ref int minorVersion)
        {
            TIA_getVersion(ref majorVersion, ref minorVersion);
        }

        public bool Connect()
        {
            // Try to connect to the board 
            int mjrVersion = 10;
            int mnrVersion = 10;
            getVersion(ref mjrVersion, ref mnrVersion);

            bool rtn = CheckError(TIA_initialiseSession());
            connected = rtn;

            if (connected) { InitialisePhotodiode(); }

            return true;
        }

        public bool Disconnect()
        {
            // Disconnect from the board
            if (connected) { return CheckError(TIA_closeSession()); }
            else { return false; }
        }


        public void InitialisePhotodiode()
        {
            int sampleRate = 50000;
            double integration = 10;

            CheckError(TIA_setMeasurementDuration(integration));
            CheckError(TIA_setMeasurementSampleRate(sampleRate)); // max sample rate is 50 ks/s
            CheckError(TIA_setMeasurementGain(TIA_MeasurementGain.TIA_MeasurementGain_10E9, TIA_MeasurementGain.TIA_MeasurementGain_10E9));

            TestMeasurement();

            // TimeStep = (1 * 1000) / sampleRate; // (1000 ms / sampleRate)
            // SamplePoints = (sampleRate * (Integration / 1000)) + 1; // (50000 * (Integration/1000) ) + 1
        }

        public void TestMeasurement()
        {
            CheckError(TIA_startSingleWellMeasurement());
            CheckError(TIA_waitOnMeasurement(1000));

            // Initialize result and data structures
            TIA_Result_t result;
            TIA_Result_t_init(&result);

            // Get Result
            CheckError(TIA_getResult(&result));

            // Pre-allocate arrays
            int traceCount = result.data[0].traceCount;
            int dataCount = result.dataCount;
            double convFactor1 = result.data[0].sensor1ConvFactor;
            double convFactor2 = result.data[0].sensor2ConvFactor;

            // Find Sample Points & TimeStep
            SamplePoints = traceCount;
            TimeStep = 1000*result.data[0].traceDeltaT_s;

            // Pre allocate arrays
            Time = new double[traceCount];

            for (int j = 0; j < traceCount; j++)
            {
                // Save Trace Data
                Time[j] = (double)j * TimeStep;
            }

            // Free up the variable
            TIA_Result_t_free(&result);
        }

        public bool DarkMeasurement()
        {
            CheckError(TIA_startSingleWellMeasurement());
            CheckError(TIA_waitOnMeasurement(1000));

            // Initialize result and data structures
            TIA_Result_t result;
            TIA_Result_t_init(&result);

            // Get Result
            CheckError(TIA_getResult(&result));

            // Pre-allocate arrays
            int traceCount = result.data[0].traceCount;
            int dataCount = result.dataCount;
            double convFactor1 = result.data[0].sensor1ConvFactor;
            double convFactor2 = result.data[0].sensor2ConvFactor;

            double[] temp = new double[3];

            // Pre allocate arrays
            Time = new double[traceCount];
            Background1 = new double[traceCount];
            Background2 = new double[traceCount];

            for (int j = 0; j < traceCount; j++)
            {
                // Save Trace Data
                Time[j] = (double)j * TimeStep;
                Background1[j] = result.data[0].sensor1Trace[j] * convFactor1;
                Background2[j] = result.data[0].sensor2Trace[j] * convFactor2;
            }

            // Free up the variable
            TIA_Result_t_free(&result);

            return true;
        }

        public void LightMeasurement()
        {
            CheckError(TIA_startSingleWellMeasurement());
            CheckError(TIA_waitOnMeasurement(1000));

            // Initialize result and data structures
            TIA_Result_t result;
            TIA_Result_t_init(&result);

            // Get Result
            CheckError(TIA_getResult(&result));

            // Pre-allocate arrays
            int traceCount = result.data[0].traceCount;
            int dataCount = result.dataCount;
            double convFactor1 = result.data[0].sensor1ConvFactor;
            double convFactor2 = result.data[0].sensor2ConvFactor;

            double[] temp = new double[3];

            // Pre allocate arrays
            Waveform = new double[traceCount];
            Waveform1 = new double[traceCount];
            Waveform2 = new double[traceCount];

            for (int j = 0; j < traceCount; j++)
            {
                // Save Trace Data
                Waveform1[j] = result.data[0].sensor1Trace[j] * convFactor1;
                Waveform2[j] = result.data[0].sensor2Trace[j] * convFactor2;

                // Subtract Background
                // Sensor1Trace[j] = Sensor1Trace[j] - Sensor1Background[j];
                // Sensor2Trace[j] = Sensor2Trace[j] - Sensor2Background[j];

                // Compute ratio trace
                Waveform[j] = Waveform1[j] / Waveform2[j];
            }

            // Free up the variable
            TIA_Result_t_free(&result);
        }

        // Error Checking
        public bool CheckError(int error)
        {
            byte[] byteArray = new byte[1000];

            string[] Error = new string[23];
            Error[0] = "No Error";
            Error[1] = "ERROR_NOT_INITIALISED";
            Error[2] = "ERROR_ALREADY_INITIALISED";
            Error[3] = "ERROR_NO_DEVICE_FOUND";
            Error[4] = "ERROR_CONNECTING_TO_DEVICE";
            Error[5] = "ERROR_INVALID_DURATION";
            Error[6] = "ERROR_INVALID_SAMPLE_RATE";
            Error[7] = "ERROR_SCAN_STEPS";
            Error[8] = "ERROR_SCAN_POINTS";
            Error[9] = "ERROR_SCAN_TIMING";
            Error[10] = "ERROR_MEASUREMENT_TIMEOUT";
            Error[11] = "ERROR_NO_RESULTS";
            Error[12] = "ERROR_INVALID_RESULTS";
            Error[13] = "ERROR_EMPTY_RESULTS";
            Error[14] = "ERROR_UNKNOWN_RESULT";
            Error[15] = "ERROR_SCAN_NOT_COMPLETED";
            Error[16] = "ERROR_INVALID_SENSOR_NUMBER";
            Error[17] = "ERROR_MOTOR_CURRENT";
            Error[18] = "ERROR_MOTOR_SPEED";
            Error[19] = "ERROR_MOTOR_ACCELERATION";
            Error[20] = "ERROR_MOTOR_TIMEOUT";
            Error[21] = "ERROR_GETTING_MOTOR_INFO";
            Error[22] = "ERROR_LED_CURRENT";

            if (error == 0)
            {
                return true;
            }
            else if (error > 22)
            {
                unsafe
                {
                    fixed (byte* erPointer = &byteArray[0])
                    {
                        TIA_getLastErrorMessage(erPointer);
                    };
                }

                // From byte array to string
                string msg = System.Text.Encoding.UTF8.GetString(byteArray, 0, 32);
                msg = msg.Substring(0, msg.IndexOf('\0'));

                MessageBox.Show("Unknown Error," + msg);
                return false;
            }
            else
            {
                unsafe
                {
                    fixed (byte* erPointer = &byteArray[0])
                    {
                        TIA_getLastErrorMessage(erPointer);
                    };
                }

                // From byte array to string
                string msg = System.Text.Encoding.UTF8.GetString(byteArray, 0, 32);
                //msg = msg.Substring(0, msg.IndexOf('\0'));

                MessageBox.Show(Error[error] + "," + msg);
                return false;
            }
        }


    }
}
