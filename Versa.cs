using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;


using Versa_Handle_t = System.Int32;

namespace FI.PlateReader.Gen4.JETI
{
    unsafe class Versa
    {
        // Classes
        public Settings.Info info;

        public enum Versa_DeviceType { Versa_UnknownDevice, Versa_InterfaceBoard_35_1 };
        public enum Versa_MotorState { Versa_MotorUV, Versa_MotorOff, Versa_MotorHold, Versa_MotorMoving };
        public enum Versa_MotorError { Versa_MotorErrorNone, Versa_MotorErrorUV, Versa_MotorErrorStall, Versa_MotorErrorUnknown };
        public enum Versa_PinPull { Versa_Pin_HiZ, Versa_Pin_PullDown, Versa_Pin_PullUp };
        public enum Versa_PinTriggerEdge { Versa_Pin_FallingEdge, Versa_Pin_RisingEdge, Versa_Pin_NoEdge };
        public enum Versa_LEDControl { Versa_LED_HWControl, Versa_LED_SWControl };

        public enum Versa_MotorMicroStepping
        {
            Versa_MotorMicroStep_none,
            Versa_MotorMicroStep_x2,
            Versa_MotorMicroStep_x4,
            Versa_MotorMicroStep_x8,
            Versa_MotorMicroStep_x16,
            Versa_MotorMicroStep_x32,
            Versa_MotorMicroStep_x64
        };

        public enum Versa_MotorControl { Versa_MotorOpenLoop, Versa_MotorOpenLoopWithCheck };
        public enum Versa_PeripheralType { Versa_NoDevice, Versa_LEDDriver, Versa_MotorDriver, Versa_LineScan };

        public enum Versa_BaudRate
        {
            Versa_Baud_2400,
            Versa_Baud_4800,
            Versa_Baud_9600,
            Versa_Baud_14k4,
            Versa_Baud_19k2,
            Versa_Baud_28k8,
            Versa_Baud_38k4,
            Versa_Baud_57k6
        };

        public enum Versa_Parity
        {
            Versa_Parity_None,
            Versa_Parity_Even,
            Versa_Parity_Odd
        };

        public enum Versa_StopBits
        {
            Versa_StopBits_1,
            Versa_StopBits_2
        };

        public enum Versa_DataSize
        {
            Versa_DataSize_5,
            Versa_DataSize_6,
            Versa_DataSize_7,
            Versa_DataSize_8
        };

        public struct Versa_LineScan_LineData_t
        {
            public int lineIndex;
            public int pixelCount;
            public double* lineTrace;
        };

        public struct Versa_LineScan_Result_t
        {
            public int lineCount;
            public Versa_LineScan_LineData_t* data;
        };

        public Versa_UserData_t userData;
        public struct Versa_UserData_t
        {
            public byte[] data;
            public string name;
            public int instSerial;
            public int ledSerial;
            public int specSerial;
            public float rowOffset;
            public float columnOffset;
            public float rowEject;
            public float columnEject;
            public int rowDirection;
            public int columnDirection;
            public int ledPulse;
            public int ledWavelength;
            public int ledMaxCurrent;
            public int instrumentGain;
            public int Npixels;
            public float[] coef;
            public int correctStart;
            public int correctLength;
            public float[] correctValues;
        };

        public Versa_LineScanData_t linescanData;
        public struct Versa_LineScanData_t
        {
            public byte[] data;
            public Versa_PeripheralInfo_t pinfo;
            public string name;
            public int serial;
            public int Npixels;
            public int postIntegrationWait;
            public int minimumCycles;
            public int degree;
            public float[] coef;
            public double[] wavelength;
        };

        public Versa_MotorData_t RowMotorData;
        public Versa_MotorData_t ColumnMotorData;
        public struct Versa_MotorData_t
        {
            public byte[] data;
            public Versa_PeripheralInfo_t pinfo;
            public string name;
            public bool motorReverse;
            public bool encoderReverse;
            public int current;
            public int microstep;
            public int fullstepsPerRev;
            public int encodercountsPerRev;
            public float unitsPerRev;
            public float errorTolerance;
            public float speed;
            public float accel;
            public bool accurateHome;

            public Versa_MotorState state;
            public Versa_MotorError error;
        };

        public Versa_LEDData_t ledData;
        public struct Versa_LEDData_t
        {
            public byte[] data;
            public Versa_PeripheralInfo_t pinfo;
            public string name;
            public int[] wavelength;
            public int[] limit;
            public float[] hours;
        };

        public struct Versa_PeripheralInfo_t
        {
            public Versa_PeripheralType peripheralType;
            public int hardwareID;
            public int hardwareVersion;
            public int firmwareMajorVersion;
            public int firmwareMinorVersion;
        };

        // Versa
        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Versa_getVersion(int* majorVersion, int* minorVersion);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_getAvailableDevices(int* deviceCount, Versa_DeviceType* deviceTypes);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_configureEthernet(int mac1, int mac2, int mac3, int mac4, int mac5, int mac6, byte* hostName);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_initialiseUSBSession();

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_initialiseEthernetSession(byte* ipAddress);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_closeSession();

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Versa_resetAllPorts();

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_requestStatus();

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern Versa_Handle_t Versa_getSBusHandle(int sbusNumber);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern Versa_Handle_t Versa_getPBusHandle();

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_getSBusInfo(int sbusNumber, Versa_PeripheralInfo_t* pInfo);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_getPBusInfo(Versa_PeripheralInfo_t* pInfo);

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
        
        // Input/Output
        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_IO_setOutputState(int channel, bool on);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_IO_configureInputChannel(int channel, Versa_PinPull pinPull, Versa_PinTriggerEdge trigger);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Versa_IO_clearInputEvent(int channel);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Versa_IO_wasInputEventEncountered(int channel);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_IO_requestInfo();

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_IO_getLastOutputState(int channel, bool* state);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_IO_getLastInputState(int channel, bool* state);


        // RS485
        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_RS485_configureChannel(int channel, Versa_BaudRate baud, Versa_Parity parity, Versa_StopBits stopBits, Versa_DataSize dataSize);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_RS485_enableChannel(int channel);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_RS485_disableChannel(int channel);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_RS485_writeData(int channel, byte* data, int size);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_RS485_readData(int channel, char* data, int size, int timeout_ms);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_RS485_getReadBufferSize(int channel, int* size);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_RS485_clearReadBuffer(int channel);



        // Motor
        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_Motor_setCurrent(Versa_Handle_t handle, int mA);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_Motor_setReducedHoldCurrentEnabled(Versa_Handle_t handle, bool enable);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_Motor_setDirectionReversed(Versa_Handle_t handle, bool reversed);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_Motor_setEncoderReversed(Versa_Handle_t handle, bool reversed);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_Motor_setControlMode(Versa_Handle_t handle, Versa_MotorControl control);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_Motor_setMaxEncoderDiscrepancy(Versa_Handle_t handle, int maxEncoderDiscrepancy);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_Motor_setTranslationParameters(Versa_Handle_t handle, int fullsteps_per_rev, int encoder_counts_per_rev, double units_per_rev);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_Motor_setSpeed(Versa_Handle_t handle, double unitsPerSecond);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_Motor_setAcceleration(Versa_Handle_t handle, double unitsStepsPerSecond2);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_Motor_setMicroStepping(Versa_Handle_t handle, Versa_MotorMicroStepping microStepping);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_Motor_setStartLimit(Versa_Handle_t handle, Versa_PinPull pinPull, Versa_PinTriggerEdge trigger);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_Motor_enableAccurateHomeSearch(Versa_Handle_t handle, bool enable);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_Motor_commitSettings(Versa_Handle_t handle);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_Motor_zeroPositionCounters(Versa_Handle_t handle);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_Motor_turnOn(Versa_Handle_t handle);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_Motor_turnOff(Versa_Handle_t handle);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_Motor_moveRelative(Versa_Handle_t handle, double units);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_Motor_moveAbsolute(Versa_Handle_t handle, double units);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_Motor_moveHome(Versa_Handle_t handle);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_Motor_stop(Versa_Handle_t handle);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_Motor_requestInfo(Versa_Handle_t handle);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_Motor_getLastSupplyVoltage(Versa_Handle_t handle, double* voltage_V, bool* isSupplyValid);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_Motor_getLastState(Versa_Handle_t handle, Versa_MotorState* pState, Versa_MotorError* pError);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_Motor_getLastMotorPosition(Versa_Handle_t handle, double* pUnits, double* pFullSteps);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_Motor_getLastEncoderPosition(Versa_Handle_t handle, double* pUnits, int* pCounts);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Versa_Motor_isBusy(Versa_Handle_t handle);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_Motor_waitFor(Versa_Handle_t handle, int timeout_ms);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_Motor_waitForAll(int timeout_ms);


        // LED
        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_LED_setCurrent(Versa_Handle_t handle, int mA);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_LED_setChannel(Versa_Handle_t handle, int channel);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_LED_turnOn(Versa_Handle_t handle);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_LED_turnOff(Versa_Handle_t handle);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_LED_requestInfo(Versa_Handle_t handle);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_LED_getLastUsage(Versa_Handle_t handle, int channel, int* pSeconds);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_LED_getLastSupplyVoltage(Versa_Handle_t handle, double* voltage_V);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_LED_resetUsage(Versa_Handle_t handle, int channel);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_LED_persistsUsage(Versa_Handle_t handle);



        // Spectrometer
        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_LineScan_setIntegrationTime(Versa_Handle_t handle, double ms);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_LineScan_startMeasurement(Versa_Handle_t handle);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Versa_LineScan_isBusy(Versa_Handle_t handle);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_LineScan_waitFor(Versa_Handle_t handle, int timeout_ms);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Versa_LineScan_Result_t_init(Versa_LineScan_Result_t* pResult);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Versa_LineScan_Result_t_free(Versa_LineScan_Result_t* pResult);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_LineScan_getAllResults(Versa_Handle_t handle, Versa_LineScan_Result_t* pResult);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_LineScan_getSingleResult(Versa_Handle_t handle, int lineIndex, Versa_LineScan_Result_t* pResult);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_LineScan_calibrateOffset(Versa_Handle_t handle);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_LineScan_getOffset(Versa_Handle_t handle, double* offset_mv);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_LineScan_configureSensor(Versa_Handle_t handle, int pixelCount, int postIntegrationWait, int minimumCycles);


        // Scan Parameters
        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_App_setScanLineScanHandle(Versa_Handle_t handle);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_App_setScanMotorDriverHandle(Versa_Handle_t handle);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_App_setScanLEDDriverHandle(Versa_Handle_t handle);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_App_setScanParameters(double startUnits, double deltaUnits, double endUnits, int stops);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_App_startStopAndGoMeasurement(Versa_LEDControl ledControl);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_App_stopMeasurement();

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Versa_App_isBusy();

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_App_waitFor(int timeout_ms);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_App_waitForLine(int lineIndex, int timeout_ms);

        // SBus Configuration
        public int ledBus;
        public int ledChn;
        public int rowBus;
        public int columnBus;

        // Handles
        Versa_Handle_t ledHandle { get; set; }
        Versa_Handle_t rowHandle { get; set; }
        Versa_Handle_t columnHandle { get; set; }
        Versa_Handle_t linescanHandle { get; set; }


        // Variables
        public string SerialNumber { get; set; }
        public bool VersaConnect { get; set; }
        public bool LEDConnect { get; set; }
        public bool SpecConnect { get; set; }
        public bool RowConnect { get; set; }
        public bool ColumnConnect { get; set; }

        public bool VersaError { get; set; }
        public string ErrorMessage { get; set; }

        // LED
        public int Power { get; set; }

        // Motor
        public double RowValue { get; set; }
        public double RowPositionU { get; set; }
        public double RowPosition { get; set; }
        public int RowCounts { get; set; }
        public double RowEncoderU { get; set; }
        public double RowEncoder { get; set; }
        public double RowDelta { get; set; }
        public double RowDeltaU { get; set; }
        public double RowErrorThreshold { get; set; }
        public bool RowError { get; set; }

        public double ColumnValue { get; set; }
        public double ColumnPositionU { get; set; }
        public double ColumnPosition { get; set; }
        public int ColumnCounts { get; set; }
        public double ColumnEncoderU { get; set; }
        public double ColumnEncoder { get; set; }
        public double ColumnDelta { get; set; }
        public double ColumnDeltaU { get; set; }
        public double ColumnErrorThreshold { get; set; }
        public bool ColumnError { get; set; }

  //      public bool RowScan { get; set; }
  //      public bool LEDControl { get; set; }
  //      public bool PositionCheck { get; set; }
  //      public bool SoftwarePositionCheck { get; set; }
  //      public bool ReadUserData { get; set; }
		//public bool LEDPersist { get; set; }
        public bool VersaValid { get; set; }
        public bool LEDValid { get; set; }
        public bool SpecValid { get; set; }
        public bool RowValid { get; set; }
        public bool ColumnValid { get; set; }

        // Spectrometer
        public int Integration { get; set; }

        public double[] Background { get; set; }    // Background Spectrum [Pre Scan, Dark]
        public double[] Waveform { get; set; }      // Waveform Spectrum [Well, Light]       

        public double[] Wavelength { get; set; }
        public string FirmwareVersion;


        // Initialise
        public bool Connect()
        {
            RowErrorThreshold = info.RowPositionError;
            ColumnErrorThreshold = info.ColumnPositionError;
            RowDelta = 0;
            ColumnDelta = 0;

            ledBus = info.LedBus;
            ledChn = info.LedChannel;
            rowBus = info.RowBus;
            columnBus = info.ColumnBus;

            //// Set row scan = true for Versa row scanning
            //RowScan = true;
            //// Set LEDcontrol = true for Versa row scanning LED pulse control
            //LEDControl = true;
            //// Set PositionCheck = true to check positions
            //PositionCheck = true;
            //// Set SoftwarePositionCheck = true to check positions via software, false for firmware checking
            //SoftwarePositionCheck = false;
            //// Set ReadUserData = true to read data from device instead of config files, false when setting up for first time
            //ReadUserData = true;

            VersaValid = false;
            LEDValid = false;
            SpecValid = false;
            RowValid = false;
            ColumnValid = false;

            // Set LEDPersist = true to save led hours after each plate scan.
            //LEDPersist = true;

            GetVersion();
            UserDataDefaults();

            // Variables
            bool state = false;
            VersaConnect = false;
            LEDConnect = false;
            RowConnect = false;
            ColumnConnect = false;
            SpecConnect = false;

            // Available devices
            int deviceCount;
            Versa_DeviceType versa_DeviceType;

            CheckError(Versa_getAvailableDevices(&deviceCount, &versa_DeviceType));

            if (deviceCount > 0)
            {                
                if (CheckError(Versa_initialiseUSBSession()))
                {
                    VersaConnect = true;
                    LEDConnect = true;
                    RowConnect = true;
                    ColumnConnect = true;
                    SpecConnect = true;

                    if (info.ReadUserData)
                    {
                        ReadData();
                    }

                    state = GetHandles();

                    if (state)
                    {
                        CheckError(Versa_IO_setOutputState(1, false));
                        CheckError(Versa_IO_setOutputState(2, false));
                        CheckError(Versa_IO_setOutputState(3, false));
                        CheckError(Versa_IO_setOutputState(4, false));

                        CheckError(Versa_IO_configureInputChannel(1, Versa_PinPull.Versa_Pin_PullDown, Versa_PinTriggerEdge.Versa_Pin_RisingEdge));
                        if (info.ReadUserData)
                        {
                            ReadDataLED();
                            ReadDataRowMotor();
                            ReadDataColumnMotor();
                            ReadDataSpectrometer();

                            // move user data to settings which are used in the initialize functions.
                            UserDataSettings();
                        }

                        state = Initialise();
                    }
                    else
                    {
                        MessageBox.Show("Peripherals not found!");
                    }

                }
            }

            return state;
        }

        public void Disconnect()
        {
            CheckError(Versa_closeSession());
        }

        public void UserDataDefaults()
        {
            userData.name = "SUPR-UV";
            userData.instSerial = 1;
            userData.ledSerial = 1;
            userData.specSerial = 1;
            userData.columnOffset = (float)info.ColumnOffset;
            userData.rowOffset = (float)info.RowOffset;
            userData.columnEject = (float)info.ColumnEject;
            userData.rowEject = (float)info.RowEject;
            userData.columnDirection = info.ColumnDirection;
            userData.rowDirection = info.RowDirection;
            userData.ledPulse = info.LEDPulse;
            userData.ledWavelength = 275;
            userData.ledMaxCurrent = 200;
            userData.instrumentGain = 1;
            userData.Npixels = 2048;
            userData.coef = new float[5];
            userData.coef[0] = (float)info.P0;
            userData.coef[1] = (float)info.P1;
            userData.coef[2] = (float)info.P2;
            userData.coef[3] = (float)info.P3;
            userData.coef[4] = (float)info.P4;
            Wavelength = new double[2048];
            double[] w = new double[2048];
            bool valid_coef = true;
            for (int i = 0; i < userData.Npixels; i++)
            {
                double p0 = userData.coef[0];
                double p1 = Math.Pow(i, 1) * userData.coef[1];
                double p2 = Math.Pow(i, 2) * userData.coef[2];
                double p3 = Math.Pow(i, 3) * userData.coef[3];
                double p4 = Math.Pow(i, 4) * userData.coef[4];

                double value = (p0 + p1 + p2 + p3 + p4);
                w[i] = value;
                // change to min and max for spectrometer?
                if ((w[i] < 0) || (w[i] > 1000))
                {
                    valid_coef = false;
                }
            }
            if (valid_coef) { Wavelength = w; }
            userData.correctStart = 1045;
            userData.correctLength = 400;
            userData.correctValues = new float[400];
            for (int n=0; n < 400; n++)
            {
                userData.correctValues[n] = 1;
            }

            ledData.hours = new float[4];
            ledData.limit = new int[4] { 200, 200, 200, 200 };
            ledData.wavelength = new int[4] { 275, 0, 0, 0 };

            linescanData.name = "F-UV";
            linescanData.serial = 1;
            linescanData.Npixels = 2048;
            linescanData.postIntegrationWait = 88;
            linescanData.minimumCycles = 2150;
            linescanData.degree = 5;
            linescanData.coef = new float[5];
            linescanData.coef[0] = (float)info.P0;
            linescanData.coef[1] = (float)info.P1;
            linescanData.coef[2] = (float)info.P2;
            linescanData.coef[3] = (float)info.P3;
            linescanData.coef[4] = (float)info.P4;

            linescanData.wavelength = new double[2048];
            w = new double[2048];
            valid_coef = true;
            for (int i = 0; i < linescanData.Npixels; i++)
            {
                double p0 = linescanData.coef[0];
                double p1 = Math.Pow(i, 1) * linescanData.coef[1];
                double p2 = Math.Pow(i, 2) * linescanData.coef[2];
                double p3 = Math.Pow(i, 3) * linescanData.coef[3];
                double p4 = Math.Pow(i, 4) * linescanData.coef[4];

                double value = (p0 + p1 + p2 + p3 + p4);
                w[i] = value;
                // change to min and max for spectrometer?
                if ((w[i] < 0) || (w[i] > 1000))
                {
                    valid_coef = false;
                }
            }
            if (valid_coef) { linescanData.wavelength = w; }


            ColumnMotorData.accel = (float)info.ColumnAcceleration;
            ColumnMotorData.current = info.ColumnCurrent;
            //ColumnMotorData.direction = info.ColumnDirection;
            ColumnMotorData.encodercountsPerRev = info.ColumnEncoderCountsPerRev;
            ColumnMotorData.encoderReverse = info.ColumnEncoderReverse;
            ColumnMotorData.errorTolerance = (float)info.ColumnPositionError;
            ColumnMotorData.fullstepsPerRev = info.ColumnFullStepsPerRev;
            ColumnMotorData.microstep = info.ColumnMicrostep;
            ColumnMotorData.motorReverse = info.ColumnMotorReverse;
            ColumnMotorData.name = "C";
            ColumnMotorData.speed = (float)info.ColumnSpeed;
            ColumnMotorData.unitsPerRev = (float)info.ColumnUnitsPerRev;
            ColumnMotorData.accurateHome = info.ColumnAccurateHome;
      
            RowMotorData.accel = (float)info.RowAcceleration;
            RowMotorData.current = info.RowCurrent;
            //RowMotorData.direction = info.RowDirection;
            RowMotorData.encodercountsPerRev = info.RowEncoderCountsPerRev;
            RowMotorData.encoderReverse = info.RowEncoderReverse;
            RowMotorData.errorTolerance = (float)info.RowPositionError;
            RowMotorData.fullstepsPerRev = info.RowFullStepsPerRev;
            RowMotorData.microstep = info.RowMicrostep;
            RowMotorData.motorReverse = info.RowMotorReverse;
            RowMotorData.name = "R";
            RowMotorData.speed = (float)info.RowSpeed;
            RowMotorData.unitsPerRev = (float)info.RowUnitsPerRev;
            RowMotorData.accurateHome = info.ColumnAccurateHome;
        }

        public void UserDataConfigFile()
        {
            userData.name = info.InstrumentName;
            userData.instSerial = info.SerialNumber;
            userData.ledSerial = info.LEDSN;
            userData.specSerial = info.SpecSN;
            userData.columnOffset = (float)info.ColumnOffset;
            userData.rowOffset = (float)info.RowOffset;
            userData.columnEject = (float)info.ColumnEject;
            userData.rowEject = (float)info.RowEject;
            userData.columnDirection = info.ColumnDirection;
            userData.rowDirection = info.RowDirection;

            ledData.hours = new float[4];
            ledData.limit[0] = info.LEDlimit1;
            ledData.limit[1] = info.LEDlimit2;
            ledData.limit[2] = info.LEDlimit3;
            ledData.limit[3] = info.LEDlimit4;
            ledData.wavelength[0] = info.LEDWL1;
            ledData.wavelength[1] = info.LEDWL2;
            ledData.wavelength[2] = info.LEDWL3;
            ledData.wavelength[3] = info.LEDWL4;

            linescanData.name = info.SpecName;
            linescanData.serial = info.SpecSN;
            linescanData.Npixels = info.Pixel;
            linescanData.degree = 5;
            linescanData.coef = new float[5];
            linescanData.coef[0] = (float)info.P0;
            linescanData.coef[1] = (float)info.P1;
            linescanData.coef[2] = (float)info.P2;
            linescanData.coef[3] = (float)info.P3;
            linescanData.coef[4] = (float)info.P4;

            linescanData.wavelength = new double[2048];
            double[] w = new double[2048];
            bool valid_coef = true;
            for (int i = 0; i < linescanData.Npixels; i++)
            {
                double p0 = linescanData.coef[0];
                double p1 = Math.Pow(i, 1) * linescanData.coef[1];
                double p2 = Math.Pow(i, 2) * linescanData.coef[2];
                double p3 = Math.Pow(i, 3) * linescanData.coef[3];
                double p4 = Math.Pow(i, 4) * linescanData.coef[4];

                double value = (p0 + p1 + p2 + p3 + p4);
                w[i] = value;
                // change to min and max for spectrometer?
                if ((w[i] < 0) || (w[i] > 1000))
                {
                    valid_coef = false;
                }
            }
            if (valid_coef) { linescanData.wavelength = w; }


            ColumnMotorData.accel = (float)info.ColumnAcceleration;
            ColumnMotorData.current = info.ColumnCurrent;
            //ColumnMotorData.direction = info.ColumnDirection;
            ColumnMotorData.encodercountsPerRev = info.ColumnEncoderCountsPerRev;
            ColumnMotorData.encoderReverse = info.ColumnEncoderReverse;
            ColumnMotorData.errorTolerance = (float)info.ColumnPositionError;
            ColumnMotorData.fullstepsPerRev = info.ColumnFullStepsPerRev;
            ColumnMotorData.microstep = info.ColumnMicrostep;
            ColumnMotorData.motorReverse = info.ColumnMotorReverse;
            ColumnMotorData.name = info.ColumnName;
            ColumnMotorData.speed = (float)info.ColumnSpeed;
            ColumnMotorData.unitsPerRev = (float)info.ColumnUnitsPerRev;
            ColumnMotorData.accurateHome = info.ColumnAccurateHome;

            RowMotorData.accel = (float)info.RowAcceleration;
            RowMotorData.current = info.RowCurrent;
            //RowMotorData.direction = info.RowDirection;
            RowMotorData.encodercountsPerRev = info.RowEncoderCountsPerRev;
            RowMotorData.encoderReverse = info.RowEncoderReverse;
            RowMotorData.errorTolerance = (float)info.RowPositionError;
            RowMotorData.fullstepsPerRev = info.RowFullStepsPerRev;
            RowMotorData.microstep = info.RowMicrostep;
            RowMotorData.motorReverse = info.RowMotorReverse;
            RowMotorData.name = info.RowName;
            RowMotorData.speed = (float)info.RowSpeed;
            RowMotorData.unitsPerRev = (float)info.RowUnitsPerRev;
            ColumnMotorData.accurateHome = info.RowAccurateHome;
        }

        public void UserDataSettings()
        {
            info.InstrumentName = userData.name;
            info.SerialNumber = userData.instSerial;
            info.LEDSN = userData.ledSerial;
            info.SpecSN = userData.specSerial;
            info.ColumnOffset = userData.columnOffset;
            info.RowOffset = userData.rowOffset;
            info.ColumnEject = userData.columnEject;
            info.RowEject = userData.rowEject;
            info.ColumnDirection = userData.columnDirection;
            info.RowDirection = userData.rowDirection;



            info.LEDlimit1 = ledData.limit[0];
            info.LEDlimit2 = ledData.limit[1];
            info.LEDlimit3 = ledData.limit[2];
            info.LEDlimit4 = ledData.limit[3];
            info.LEDWL1 = ledData.wavelength[0];
            info.LEDWL2 = ledData.wavelength[1];
            info.LEDWL3 = ledData.wavelength[2];
            info.LEDWL4 = ledData.wavelength[3];

            info.SpecName = linescanData.name;
            info.SpecSN = linescanData.serial;
            info.Pixel = linescanData.Npixels;
            info.MinimumCycles = linescanData.minimumCycles;
            info.PostIntegrationWait = linescanData.postIntegrationWait;

            info.P0 = (double)linescanData.coef[0];
            info.P1 = (double)linescanData.coef[1];
            info.P2 = (double)linescanData.coef[2];
            info.P3 = (double)linescanData.coef[3];
            info.P4 = (double)linescanData.coef[4];

            float[] c = new float[5];
            c[0] = (float)info.P0;
            c[1] = (float)info.P1;
            c[2] = (float)info.P2;
            c[3] = (float)info.P3;
            c[4] = (float)info.P4;

            info.Wavelength = linescanData.wavelength;

            info.ColumnAcceleration = ColumnMotorData.accel;
            info.ColumnCurrent = ColumnMotorData.current;
            //info.ColumnDirection = ColumnMotorData.direction;
            info.ColumnEncoderCountsPerRev = ColumnMotorData.encodercountsPerRev;
            info.ColumnEncoderReverse = ColumnMotorData.encoderReverse;
            info.ColumnPositionError = ColumnMotorData.errorTolerance;
            info.ColumnFullStepsPerRev = ColumnMotorData.fullstepsPerRev;
            info.ColumnMicrostep = ColumnMotorData.microstep;
            info.ColumnMotorReverse = ColumnMotorData.motorReverse;
            info.ColumnName = ColumnMotorData.name;
            info.ColumnSpeed = ColumnMotorData.speed;
            info.ColumnUnitsPerRev = ColumnMotorData.unitsPerRev;
            info.ColumnAccurateHome = ColumnMotorData.accurateHome;

            info.RowAcceleration = RowMotorData.accel;
            info.RowCurrent = RowMotorData.current;
            //info.RowDirection = RowMotorData.direction;
            info.RowEncoderCountsPerRev = RowMotorData.encodercountsPerRev;
            info.RowEncoderReverse = RowMotorData.encoderReverse;
            info.RowPositionError = RowMotorData.errorTolerance;
            info.RowFullStepsPerRev = RowMotorData.fullstepsPerRev;
            info.RowMicrostep = RowMotorData.microstep;
            info.RowMotorReverse = RowMotorData.motorReverse;
            info.RowName = RowMotorData.name;
            info.RowSpeed = RowMotorData.speed;
            info.RowUnitsPerRev = RowMotorData.unitsPerRev;
            info.RowAccurateHome = RowMotorData.accurateHome;
        }

        public void WriteData()
        {
            if (!VersaConnect) { return; }

            // From string to byte array
            byte[] uData = new byte[1792];       //128

            // if name is longer than 10 characters, truncate to fixed 10 characters
            if (userData.name.Length > 10) { userData.name = userData.name.Substring(0, 10); }
            byte[] name = Encoding.ASCII.GetBytes(userData.name);
            name.CopyTo(uData, 0);

            byte[] sn = BitConverter.GetBytes(userData.instSerial);
            sn.CopyTo(uData, 10);

            byte[] ledSN = BitConverter.GetBytes(userData.ledSerial);
            ledSN.CopyTo(uData, 14);

            byte[] specSN = BitConverter.GetBytes(userData.specSerial);
            specSN.CopyTo(uData, 18);



            byte[] roffset = BitConverter.GetBytes(userData.rowOffset);
            roffset.CopyTo(uData, 30);

            byte[] coffset = BitConverter.GetBytes(userData.columnOffset);
            coffset.CopyTo(uData, 34);

            byte[] reject = BitConverter.GetBytes(userData.rowEject);
            reject.CopyTo(uData, 38);

            byte[] ceject = BitConverter.GetBytes(userData.columnEject);
            ceject.CopyTo(uData, 42);

            byte[] rdir = BitConverter.GetBytes(userData.rowDirection);
            rdir.CopyTo(uData, 46);

            byte[] cdir = BitConverter.GetBytes(userData.columnDirection);
            cdir.CopyTo(uData, 50);

            byte[] ledPulse = BitConverter.GetBytes(userData.ledPulse);
            ledPulse.CopyTo(uData, 54);

            byte[] ledWL = BitConverter.GetBytes(userData.ledWavelength);
            ledWL.CopyTo(uData, 58);

            byte[] ledMax = BitConverter.GetBytes(userData.ledMaxCurrent);
            ledMax.CopyTo(uData, 62);

            byte[] instGain = BitConverter.GetBytes(userData.instrumentGain);
            instGain.CopyTo(uData, 66);

            byte[] pixels = BitConverter.GetBytes(userData.Npixels);
            pixels.CopyTo(uData, 70);

            for (int n = 0; n < 5; n++)
            {
                byte[] bytes = BitConverter.GetBytes(userData.coef[n]);
                bytes.CopyTo(uData, (n * 4 + 74));
            }

            byte[] cStart = BitConverter.GetBytes(userData.correctStart);
            cStart.CopyTo(uData, 94);

            byte[] cLength = BitConverter.GetBytes(userData.correctLength);
            cLength.CopyTo(uData, 98);

            for (int n = 0; n < userData.correctLength; n++)
            {
                byte[] cValue = BitConverter.GetBytes(userData.correctValues[n]);
                cValue.CopyTo(uData, 102+(4*n));
            }

            userData.data = uData;
            unsafe
            {
                fixed (byte* dPointer = &userData.data[0])
                {
                    if (VersaConnect) { bool ul = CheckError(Versa_uploadUserData(dPointer)); }
                };
            }


        }

        public void WriteDataLED()
        {
            if (!LEDConnect) { return; }

            // From string to byte array
            byte[] uData = new byte[32];

            for (int c = 0; c < 4; c++)
            {
                byte[] wl = BitConverter.GetBytes(ledData.wavelength[c]);
                wl.CopyTo(uData, c * 4);
            }

            for (int c = 0; c < 4; c++)
            {
                byte[] limit = BitConverter.GetBytes(ledData.limit[c]);
                limit.CopyTo(uData, c * 4 + 16);
            }

            ledData.data = uData;
            unsafe
            {
                fixed (byte* dPointer = &ledData.data[0])
                {
                    if (SpecConnect) { bool ul = CheckError(Versa_Peripheral_uploadUserData(ledHandle, dPointer)); }
                };
            }
        }

        public void WriteLEDhours()
        {
            if (!LEDConnect) { return; }

            if (info.LEDPersist)
            {
                if (LEDConnect) { bool check = CheckError(Versa_LED_persistsUsage(ledHandle)); }
            }            
        }

        public void WriteDataSpectrometer()
        {
            if (!SpecConnect) { return; }

            // From string to byte array
            byte[] uData = new byte[32];

            // if name is longer than 10 characters, truncate to fixed 10 characters
            if (userData.name.Length > 4) { linescanData.name = linescanData.name.Substring(0, 4); }
            byte[] name = Encoding.ASCII.GetBytes(linescanData.name);
            name.CopyTo(uData, 0);

            byte[] sn = BitConverter.GetBytes(linescanData.serial);
            sn.CopyTo(uData, 4);

            byte[] pixels = BitConverter.GetBytes(linescanData.Npixels);
            pixels.CopyTo(uData, 8);

            for (int n = 0; n < linescanData.degree; n++)
            {
                byte[] bytes = BitConverter.GetBytes(linescanData.coef[n]);
                bytes.CopyTo(uData, (n * 4 + 12));
            }

            string upload = System.Text.Encoding.UTF8.GetString(uData, 0, 32);

            linescanData.data = uData;
            unsafe
            {
                fixed (byte* dPointer = &linescanData.data[0])
                {
                    if (SpecConnect) { bool ul = CheckError(Versa_Peripheral_uploadUserData(linescanHandle, dPointer)); }
                };
            }
        }

        public void WriteDataRowMotor()
        {
            if (!RowConnect) { return; }

            // From string to byte array
            byte[] uData = new byte[32];

            // if name is longer than 10 characters, truncate to fixed 10 characters
            if (RowMotorData.name.Length > 1) { RowMotorData.name = RowMotorData.name.Substring(0, 1); }
            byte[] name = Encoding.ASCII.GetBytes(RowMotorData.name);
            name.CopyTo(uData, 0);

            byte[] mr = BitConverter.GetBytes(RowMotorData.motorReverse);
            mr.CopyTo(uData, 1);

            byte[] er = BitConverter.GetBytes(RowMotorData.encoderReverse);
            er.CopyTo(uData, 2);

            byte[] ah = BitConverter.GetBytes(RowMotorData.accurateHome);
            ah.CopyTo(uData, 3);

            byte[] current = BitConverter.GetBytes(RowMotorData.current);
            current.CopyTo(uData, 4);

            byte[] ec = BitConverter.GetBytes(RowMotorData.encodercountsPerRev);
            ec.CopyTo(uData, 8);

            byte[] units = BitConverter.GetBytes(RowMotorData.unitsPerRev);
            units.CopyTo(uData, 12);

            byte[] error = BitConverter.GetBytes(RowMotorData.errorTolerance);
            error.CopyTo(uData, 16);

            byte[] speed = BitConverter.GetBytes(RowMotorData.speed);
            speed.CopyTo(uData, 20);

            byte[] accel = BitConverter.GetBytes(RowMotorData.accel);
            accel.CopyTo(uData, 24);

            byte[] microstep = BitConverter.GetBytes(RowMotorData.microstep);
            microstep.CopyTo(uData, 28);

            string upload = System.Text.Encoding.UTF8.GetString(uData, 0, 32);

            RowMotorData.data = uData;
            unsafe
            {
                fixed (byte* dPointer = &RowMotorData.data[0])
                {
                    if (RowConnect) { bool ul = CheckError(Versa_Peripheral_uploadUserData(rowHandle, dPointer)); }
                };
            }
        }

        public void WriteDataColumnMotor()
        {
            if (!ColumnConnect) { return; }

            // From string to byte array
            byte[] uData = new byte[32];

            // if name is longer than 10 characters, truncate to fixed 10 characters
            if (ColumnMotorData.name.Length > 1) { ColumnMotorData.name = ColumnMotorData.name.Substring(0, 1); }
            byte[] name = Encoding.ASCII.GetBytes(ColumnMotorData.name);
            name.CopyTo(uData, 0);

            byte[] mr = BitConverter.GetBytes(ColumnMotorData.motorReverse);
            mr.CopyTo(uData, 1);

            byte[] er = BitConverter.GetBytes(ColumnMotorData.encoderReverse);
            er.CopyTo(uData, 2);

            byte[] ah = BitConverter.GetBytes(ColumnMotorData.accurateHome);
            ah.CopyTo(uData, 3);

            byte[] current = BitConverter.GetBytes(ColumnMotorData.current);
            current.CopyTo(uData, 4);

            byte[] ec = BitConverter.GetBytes(ColumnMotorData.encodercountsPerRev);
            ec.CopyTo(uData, 8);

            byte[] units = BitConverter.GetBytes(ColumnMotorData.unitsPerRev);
            units.CopyTo(uData, 12);

            byte[] error = BitConverter.GetBytes(ColumnMotorData.errorTolerance);
            error.CopyTo(uData, 16);

            byte[] speed = BitConverter.GetBytes(ColumnMotorData.speed);
            speed.CopyTo(uData, 20);

            byte[] accel = BitConverter.GetBytes(ColumnMotorData.accel);
            accel.CopyTo(uData, 24);

            byte[] microstep = BitConverter.GetBytes(ColumnMotorData.microstep);
            microstep.CopyTo(uData, 28);

            string upload = System.Text.Encoding.UTF8.GetString(uData, 0, 32);

            ColumnMotorData.data = uData;
            unsafe
            {
                fixed (byte* dPointer = &ColumnMotorData.data[0])
                {
                    if (ColumnConnect) { bool ul = CheckError(Versa_Peripheral_uploadUserData(columnHandle, dPointer)); }
                };
            }
        }

        public void ReadData()
        {
            if (!VersaConnect) { return; }
            // From string to byte array
            byte[] dData = new byte[1792];      //128

            unsafe
            {
                fixed (byte* dPointer = &dData[0])
                {
                    bool dl = CheckError(Versa_downloadUserData(dPointer));
                    VersaValid = dl;
                    VersaConnect = dl;
                };
            }

            userData.data = dData;

            // From byte array to string
            string name = System.Text.Encoding.UTF8.GetString(dData, 0, 10);
            if (name.Contains('\0')) { name = name.Substring(0, name.IndexOf('\0')); }
            userData.name = name;

            // convert bytes to values.
            int serial_d = BitConverter.ToInt32(dData, 10);
            // check to see if serial number is valid
            if (serial_d < 0) { VersaValid = false; }
            else { userData.instSerial = serial_d; }

            // convert bytes to values.
            int led_sn = BitConverter.ToInt32(dData, 14);
            // check to see if serial number is valid
            if (led_sn < 0) { VersaValid = false; }
            else { userData.ledSerial = led_sn; }

            // convert bytes to values.
            int spec_sn = BitConverter.ToInt32(dData, 18);
            // check to see if serial number is valid
            if (spec_sn < 0) { VersaValid = false; }
            else { userData.specSerial = spec_sn; }

            float rowOffset = BitConverter.ToSingle(dData, 30);
            // check if offset is a valid number
            if (rowOffset == float.NaN) { VersaValid = false; }
            else
            {
                // should change to max row travel?
                if ((rowOffset >= 0) && (rowOffset < 200))
                {
                    userData.rowOffset = rowOffset;
                }
                else
                {
                    VersaValid = false;
                }                
            }

            float colOffset = BitConverter.ToSingle(dData, 34);
            // check if offset is a valid number
            if (colOffset == float.NaN) { VersaValid = false; }
            else
            {
                // should change to max row travel?
                if ((colOffset >= 0) && (colOffset < 250))
                {
                    userData.columnOffset = colOffset;
                }
                else
                {
                    VersaValid = false;
                }
            }

            float rowEject = BitConverter.ToSingle(dData, 38);
            // check if offset is a valid number
            if (rowEject == float.NaN) { VersaValid = false; }
            else
            {
                // should change to max row travel?
                if ((rowEject >= 0) && (rowEject < 250))
                {
                    userData.rowEject = rowEject;
                }
                else
                {
                    VersaValid = false;
                }
            }

            float colEject = BitConverter.ToSingle(dData, 42);
            // check if offset is a valid number
            if (colEject == float.NaN) { VersaValid = false; }
            else
            {
                // should change to max row travel?
                if ((colEject >= 0) && (colEject < 200))
                {
                    userData.columnEject = colEject;
                }
                else
                {
                    VersaValid = false;
                }
            }

            int rowDir = BitConverter.ToInt32(dData, 46);
            if ((rowDir == -1) || (rowDir == 1)) { userData.rowDirection = rowDir; }
            else
            {
                VersaValid = false;
            }

            int colDir = BitConverter.ToInt32(dData, 50);
            if ((colDir == -1) || (colDir == 1)) { userData.columnDirection = colDir; }
            else
            {
                VersaValid = false;
            }

            int ledPulse = BitConverter.ToInt32(dData, 54);
            if ((ledPulse == 0) || (ledPulse == 1)) { userData.ledPulse = ledPulse; }
            else
            {
                VersaValid = false;
            }

            int ledWL = BitConverter.ToInt32(dData, 58);
            if ((ledWL == 0) || (ledWL == 1)) { userData.ledWavelength = ledWL; }
            else
            {
                VersaValid = false;
            }

            int ledMax = BitConverter.ToInt32(dData, 62);
            if ((ledMax == 0) || (ledMax == 1)) { userData.ledMaxCurrent = ledMax; }
            else
            {
                VersaValid = false;
            }


            int pixel_d = BitConverter.ToInt32(dData,70);
            if ((pixel_d == 256) || (pixel_d == 1024) || (pixel_d == 2048))
            {
                userData.Npixels = pixel_d;
            }
            else
            {
                VersaValid = false;
            }


            int degree_d = 5;   // BitConverter.ToInt32(dData, 12);            
            float[] coef_d = new float[degree_d];
            for (int n = 0; n < degree_d; n++)
            {
                coef_d[n] = BitConverter.ToSingle(dData, (n * 4 + 74));
                if (coef_d[n] == float.NaN) { VersaValid = false; }
            }
            // check wavelength values.
            if (VersaValid)
            {
                double[] w = new double[pixel_d];
                bool valid_coef = true;
                for (int i = 0; i < pixel_d; i++)
                {
                    double p0 = coef_d[0];
                    double p1 = Math.Pow(i, 1) * coef_d[1];
                    double p2 = Math.Pow(i, 2) * coef_d[2];
                    double p3 = Math.Pow(i, 3) * coef_d[3];
                    double p4 = Math.Pow(i, 4) * coef_d[4];

                    double value = (p0 + p1 + p2 + p3 + p4);
                    w[i] = value;
                    // change to min and max for spectrometer?
                    if ((w[i] < 0) || (w[i] > 1000))
                    {
                        valid_coef = false;
                    }
                }
                if (valid_coef)
                {
                    userData.coef = coef_d;
                    Wavelength = w;
                }
            }

            int correctStart = BitConverter.ToInt32(dData, 94);
            if (correctStart < (pixel_d - 400))
            {
                userData.correctStart = pixel_d;
            }
            else
            {
                userData.correctStart = 1045;
                VersaValid = false;
            }

            int correctLength = BitConverter.ToInt32(dData, 98);
            if (correctLength <= pixel_d)
            {
                userData.correctLength = correctLength;
            }
            else
            {
                userData.correctLength = 400;
                VersaValid = false;
            }

            float[] correctValues = new float[correctLength];

            for (int i=0; i < correctLength; i++)
            {
                float cValue = BitConverter.ToSingle(dData, 102+(4*i));
                if ((cValue > 0) & (cValue < 5))
                {
                    userData.correctValues[i] = cValue;
                }
                else
                {
                    userData.correctValues[i] = 1;
                    VersaValid = false;
                }
            }
        }

        public void ReadDataLED()
        {
            if (!LEDConnect) { return; }

            // From string to byte array
            bool check;
            int pSeconds;

            byte[] dData = new byte[32];

            unsafe
            {
                fixed (byte* dPointer = &dData[0])
                {
                    bool dl = CheckError(Versa_Peripheral_downloadUserData(ledHandle, dPointer));
                    LEDValid = dl;
                    LEDConnect = dl;
                };
            }

            // From byte array to string
            string download = System.Text.Encoding.UTF8.GetString(dData, 0, 32);
            if (download.Contains('\0')) { download = download.Substring(0, download.IndexOf('\0')); }

            ledData.wavelength = new int[4];
            ledData.limit = new int[4];
            ledData.hours = new float[4];

            // convert bytes to values.
            for (int c = 0; c < 4; c++)
            {
                int wavelength = BitConverter.ToInt32(dData, c * 4);
                if (wavelength < 0) { LEDValid = false; }
                else
                {
                    if ((wavelength >= 0) && (wavelength <= 650))
                    {
                        ledData.wavelength[c] = wavelength;
                    }
                    else
                    {
                        LEDValid = false;
                    }
                }
                
            }

            for (int c = 0; c < 4; c++)
            {
                int limit = BitConverter.ToInt32(dData, c * 4 + 16);
                if (limit < 0) { LEDValid = false; }
                else
                {
                    if ((limit >= 10) && (limit <= 500))
                    {
                        ledData.limit[c] = limit;
                    }
                    else
                    {
                        LEDValid = false;
                    }
                }
            }

            // read hours
            check = CheckError(Versa_LED_requestInfo(ledHandle));

            for (int c = 0; c < 4; c++)
            {
                pSeconds = 0;
                check = CheckError(Versa_LED_getLastUsage(ledHandle, (c + 1), &pSeconds));
                if (check) { ledData.hours[c] = (float)pSeconds / 3600; }                
            }
        }

        public void ResetLED(int chn)
        {
            if (!LEDConnect) { return; }
            bool check = CheckError(Versa_LED_resetUsage(ledHandle, chn));
        }

        public void ResetLEDs()
        {
            if (!LEDConnect) { return; }
            for (int c = 0; c < 4; c++)
            {
                bool check = CheckError(Versa_LED_resetUsage(ledHandle, c+1));
            }
        }

        public void ReadDataSpectrometer()
        {
            if (!SpecConnect) { return; }
            // From string to byte array
            byte[] dData = new byte[32];

            unsafe
            {
                fixed (byte* dPointer = &dData[0])
                {
                    bool dl = CheckError(Versa_Peripheral_downloadUserData(linescanHandle, dPointer));
                    SpecValid = dl;
                    SpecConnect = dl;
                };
            }
            linescanData.data = dData;

            // From byte array to string
            string download = System.Text.Encoding.UTF8.GetString(dData, 0, 32);
            if (download.Contains('\0')) { download = download.Substring(0, download.IndexOf('\0')); }

            // From byte array to string
            string name = System.Text.Encoding.UTF8.GetString(dData, 0, 4);
            if (name.Contains('\0')) { name = name.Substring(0, name.IndexOf('\0')); }
            linescanData.name = name;

            // convert bytes to values.
            int serial_d = BitConverter.ToInt32(dData, 4);
            if (serial_d < 0) { SpecValid = false; }
            else { linescanData.serial = serial_d; }

            int pixel_d = BitConverter.ToInt32(dData, 8);
            if ((pixel_d == 256) || (pixel_d == 1024) || (pixel_d == 2048))
            {
                linescanData.Npixels = pixel_d;
                linescanData.minimumCycles = pixel_d + 102;
            }
            else
            {
                SpecValid = false;
            }

            linescanData.postIntegrationWait = 88;

            int degree_d = 5;   // BitConverter.ToInt32(dData, 12);            
            float[] coef_d = new float[degree_d];
            for (int n = 0; n < degree_d; n++)
            {
                coef_d[n] = BitConverter.ToSingle(dData, (n * 4 + 12));
                if (coef_d[n] == float.NaN) { SpecValid = false; }
            }
            // check wavelength values.
            
            if (SpecValid)
            {
                double[] w = new double[linescanData.Npixels];
                bool valid_coef = true;
                for (int i = 0; i < linescanData.Npixels; i++)
                {
                    double p0 = coef_d[0];
                    double p1 = Math.Pow(i, 1) * coef_d[1];
                    double p2 = Math.Pow(i, 2) * coef_d[2];
                    double p3 = Math.Pow(i, 3) * coef_d[3];
                    double p4 = Math.Pow(i, 4) * coef_d[4];

                    double value = (p0 + p1 + p2 + p3 + p4);
                    w[i] = value;
                    // change to min and max for spectrometer?
                    if ((w[i] < 0) || (w[i] > 1000))
                    {
                        valid_coef = false;
                    }
                }
                if (valid_coef)
                {
                    linescanData.coef = coef_d;
                    linescanData.wavelength = w;
                }                
            }

        }

        public void ReadDataRowMotor()
        {
            if (!RowConnect) { return; }

            // From string to byte array
            byte[] dData = new byte[32];

            unsafe
            {
                fixed (byte* dPointer = &dData[0])
                {
                    bool dl = CheckError(Versa_Peripheral_downloadUserData(rowHandle, dPointer));
                    RowValid = dl;
                    RowConnect = dl;
                };
            }

            RowMotorData.data = dData;

            // From byte array to string
            string download = System.Text.Encoding.UTF8.GetString(dData, 0, 32);
            if (download.Contains('\0')) { download = download.Substring(0, download.IndexOf('\0')); }

            // From byte array to string
            string name = System.Text.Encoding.UTF8.GetString(dData, 0, 1);
            if (name.Contains('\0')) { name = name.Substring(0, name.IndexOf('\0')); }
            RowMotorData.name = name;

            // convert bytes to values.
            //char mreverse = BitConverter.ToChar(dData, 1);
            int mr = (int)dData[1];
            if (mr == 0) { RowMotorData.motorReverse = false; }
            else if (mr == 1) { RowMotorData.motorReverse = true; }
            else { RowValid = false; }

            //char ereverse = BitConverter.ToChar(dData, 2);
            int er = (int)dData[2];
            if (er == 0) { RowMotorData.encoderReverse = false; }
            else if (er == 1) { RowMotorData.encoderReverse = true; }
            else { RowValid = false; }

            int ah = (int)dData[3];
            if (ah == 0) { RowMotorData.accurateHome = false; }
            else if (ah == 1) { RowMotorData.accurateHome = true; }
            else { RowValid = false; }

            int current = BitConverter.ToInt32(dData, 4);
            if ((current > 0) && (current < 1500)) { RowMotorData.current = current; }
            else { RowValid = false; }

            int ec = BitConverter.ToInt32(dData, 8);
            if ((ec > 0) && (ec < 10000)) { RowMotorData.encodercountsPerRev = ec; }
            else { RowValid = false; }

            float units = BitConverter.ToSingle(dData, 12);
            if ((units > 0) && (units < 10000)) { RowMotorData.unitsPerRev = units; }
            else { RowValid = false; }

            float error = BitConverter.ToSingle(dData, 16);
            if ((error > 0) && (error < 1)) { RowMotorData.errorTolerance = error; }
            else { RowValid = false; }

            float speed = BitConverter.ToSingle(dData, 20);
            if ((speed > 0) && (speed < 200)) { RowMotorData.speed = speed; }
            else { RowValid = false; }

            float accel = BitConverter.ToSingle(dData, 24);
            if ((accel > 0) && (accel < 10000)) { RowMotorData.accel = accel; }
            else { RowValid = false; }

            int microstep = BitConverter.ToInt32(dData, 28);
            if ((microstep == 1) || (microstep == 2) || (microstep == 4) || (microstep == 8) || (microstep == 16 || (microstep == 32) || (microstep == 64))) { RowMotorData.microstep = microstep; }
            else { RowValid = false; }

            RowMotorData.fullstepsPerRev = 200;

        }

        public void ReadDataColumnMotor()
        {
            if (!ColumnConnect) { return; }

            // From string to byte array
            byte[] dData = new byte[32];

            unsafe
            {
                fixed (byte* dPointer = &dData[0])
                {
                    bool dl = CheckError(Versa_Peripheral_downloadUserData(columnHandle, dPointer));
                    ColumnValid = dl;
                    ColumnConnect = dl;
                };
            }

            ColumnMotorData.data = dData;

            // From byte array to string
            string download = System.Text.Encoding.UTF8.GetString(dData, 0, 32);
            if (download.Contains('\0')) { download = download.Substring(0, download.IndexOf('\0')); }

            // From byte array to string
            string name = System.Text.Encoding.UTF8.GetString(dData, 0, 1);
            if (name.Contains('\0')) { name = name.Substring(0, name.IndexOf('\0')); }
            ColumnMotorData.name = name;

            // convert bytes to values.
            //char mreverse = BitConverter.ToChar(dData, 1);
            int mr = (int)dData[1];
            if (mr == 0) { ColumnMotorData.motorReverse = false; }
            else if (mr == 1) { ColumnMotorData.motorReverse = true; }
            else { ColumnValid = false; }

            //char ereverse = BitConverter.ToChar(dData, 2);
            int er = (int)dData[2];
            if (er == 0) { ColumnMotorData.encoderReverse = false; }
            else if (mr == 1) { ColumnMotorData.encoderReverse = true; }
            else { ColumnValid = false; }

            int ah = (int)dData[3];
            if (ah == 0) { ColumnMotorData.accurateHome = false; }
            else if (ah == 1) { ColumnMotorData.accurateHome = true; }
            else { ColumnValid = false; }

            int current = BitConverter.ToInt32(dData, 4);
            if ((current > 0) && (current < 1500)) { ColumnMotorData.current = current; }
            else { ColumnValid = false; }

            int ec = BitConverter.ToInt32(dData, 8);
            if ((ec > 0) && (ec < 10000)) { ColumnMotorData.encodercountsPerRev = ec; }
            else { ColumnValid = false; }

            float units = BitConverter.ToSingle(dData, 12);
            if ((units > 0) && (units < 10000)) { ColumnMotorData.unitsPerRev = units; }
            else { ColumnValid = false; }

            float error = BitConverter.ToSingle(dData, 16);
            if ((error > 0) && (error < 1)) { ColumnMotorData.errorTolerance = error; }
            else { ColumnValid = false; }

            float speed = BitConverter.ToSingle(dData, 20);
            if ((speed > 0) && (speed < 200)) { ColumnMotorData.speed = speed; }
            else { ColumnValid = false; }

            float accel = BitConverter.ToSingle(dData, 24);
            if ((accel > 0) && (accel < 10000)) { ColumnMotorData.accel = accel; }
            else { ColumnValid = false; }

            int microstep = BitConverter.ToInt32(dData, 28);
            if ((microstep == 1) || (microstep == 2) || (microstep == 4) || (microstep == 8) || (microstep == 16 || (microstep == 32) || (microstep == 64))) { ColumnMotorData.microstep = microstep; }
            else { ColumnValid = false; }

            ColumnMotorData.fullstepsPerRev = 200;

        }




        public void GetVersion()
        {
            int majorVersion = 0;
            int minorVersion = 0;
            Versa_getVersion(&majorVersion, &minorVersion);

            FirmwareVersion = majorVersion.ToString() + "." + minorVersion.ToString();
        }

        public bool VerifyConnection()
        {
            return true;
        }

        public bool GetHandles()
        {
            // Initialise LED Handle
            ledHandle = Versa_getSBusHandle(info.LedBus);
            Versa_PeripheralInfo_t pinfo;
            bool chk1 = CheckError(Versa_getSBusInfo(info.LedBus, &pinfo));
            ledData.pinfo = pinfo;

            // Set Handle
            rowHandle = Versa_getSBusHandle(info.RowBus);
            bool chk2 = CheckError(Versa_getSBusInfo(info.RowBus, &pinfo));
            RowMotorData.pinfo = pinfo;

            // Set Handle
            columnHandle = Versa_getSBusHandle(info.ColumnBus);
            bool chk3 = CheckError(Versa_getSBusInfo(info.ColumnBus, &pinfo));
            ColumnMotorData.pinfo = pinfo;

            // Get the Handle
            linescanHandle = Versa_getPBusHandle();
            bool chk4 = CheckError(Versa_getPBusInfo(&pinfo));
            linescanData.pinfo = pinfo;

            return true;
        }

        public bool Initialise()
        {
            bool state1 = InitialiseLed();
            bool state2 = InitialiseRowMotor();
            bool state3 = InitialiseColumnMotor();
           //bool state4 = InitialiseSpectrometer();

            if(state1 && state2 && state3)  // && state4)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        // RS485
        //public bool InitialiseRS485(int channel, Versa_BaudRate br, Versa_Parity p, Versa_StopBits sb)
        //{
        //    bool check = CheckError(Versa_RS485_configureChannel(channel, br, p, sb));
        //    return true;
        //}

        // LED
        public bool InitialiseLed()
        {
            if (!LEDConnect) { return false; }

            bool check;
            int pSeconds = 0;
            double voltage_V = 0;
            //// Initialise LED Handle
            //ledHandle = Versa_getSBusHandle(info.LedBus);

            check = CheckError(Versa_LED_setChannel(ledHandle, info.LedChannel));
            if (!check) { return false; }
            check = CheckError(Versa_LED_requestInfo(ledHandle));
            if (!check) { return false; }
            check = CheckError(Versa_LED_getLastUsage(ledHandle, info.LedChannel, &pSeconds));
            if (!check) { return false; }
            check = CheckError(Versa_LED_getLastSupplyVoltage(ledHandle, &voltage_V));
            if (!check) { return false; }

            //if (ReadUserData)
            //{
            //    ReadDataLED();
            //}
            //else
            //{
            //    ledData.name = "default";
            //    ledData.hours = new float[4] { 0, 0, 0, 0 };
            //    ledData.limit = new int[4] { 200, 200, 200, 200 };
            //    ledData.wavelength = new int[4] { 275, 0, 0, 0 };
            //}
            return true;
        }

        public bool SetLedPower()
        {
            if (!LEDConnect) { return false; }

            // Convert Power to Current
            int conversion = 2;
            int current = conversion * Power;

            // Set the Current
            return CheckError(Versa_LED_setCurrent(ledHandle, current));

        }

        public bool LedOn()
        {
            if (!LEDConnect) { return false; }

            return CheckError(Versa_LED_turnOn(ledHandle));
        }

        public bool LedOff()
        {
            if (!LEDConnect) { return false; }

            return CheckError(Versa_LED_turnOff(ledHandle));
        }


        // Motor
        public bool InitialiseRowMotor()
        {
            if (!RowConnect) { return false; }

            bool check;
            //// Set Handle
            //rowHandle = Versa_getSBusHandle(info.RowBus);

            // Initialise Settings
            check = CheckError(Versa_Motor_setTranslationParameters(rowHandle, info.RowFullStepsPerRev, info.RowEncoderCountsPerRev, info.RowUnitsPerRev));
            if (!check) { return false; }
            check = CheckError(Versa_Motor_setCurrent(rowHandle, info.RowCurrent));
            if (!check) { return false; }
            check = CheckError(Versa_Motor_setReducedHoldCurrentEnabled(rowHandle, true));
            if (!check) { return false; }
            check = CheckError(Versa_Motor_setSpeed(rowHandle, info.RowSpeed));
            if (!check) { return false; }
            check = CheckError(Versa_Motor_setAcceleration(rowHandle, info.RowAcceleration));
            if (!check) { return false; }
            check = CheckError(Versa_Motor_setDirectionReversed(rowHandle, info.RowMotorReverse));
            if (!check) { return false; }
            check = CheckError(Versa_Motor_setEncoderReversed(rowHandle, info.RowEncoderReverse));
            if (!check) { return false; }
            check = CheckError(Versa_Motor_setStartLimit(rowHandle, Versa_PinPull.Versa_Pin_PullUp, Versa_PinTriggerEdge.Versa_Pin_FallingEdge));            
            if (!check) { return false; }
            check = CheckError(Versa_Motor_enableAccurateHomeSearch(rowHandle, info.RowAccurateHome));
            if (!check) { return false; }
            if (!info.MotorClosedLoop)
            {
                check = CheckError(Versa_Motor_setControlMode(rowHandle, Versa_MotorControl.Versa_MotorOpenLoop));
            }
            else
            {
                check = CheckError(Versa_Motor_setControlMode(rowHandle, Versa_MotorControl.Versa_MotorOpenLoopWithCheck));
            }
            if (!check) { return false; }

            int maxEncoderDiscrepancy;
            maxEncoderDiscrepancy = (int)(RowErrorThreshold * info.RowEncoderCountsPerRev / info.RowUnitsPerRev);
            check = CheckError(Versa_Motor_setMaxEncoderDiscrepancy(rowHandle, maxEncoderDiscrepancy));
            if (!check) { return false; }

            // Microstepping
            switch (info.RowMicrostep)
            {
                case 1:
                    check = CheckError(Versa_Motor_setMicroStepping(rowHandle, Versa_MotorMicroStepping.Versa_MotorMicroStep_none));
                    break;
                case 2:
                    check = CheckError(Versa_Motor_setMicroStepping(rowHandle, Versa_MotorMicroStepping.Versa_MotorMicroStep_x2));
                    break;
                case 4:
                    check = CheckError(Versa_Motor_setMicroStepping(rowHandle, Versa_MotorMicroStepping.Versa_MotorMicroStep_x4));
                    break;
                case 8:
                    check = CheckError(Versa_Motor_setMicroStepping(rowHandle, Versa_MotorMicroStepping.Versa_MotorMicroStep_x8));
                    break;
                case 16:
                    check = CheckError(Versa_Motor_setMicroStepping(rowHandle, Versa_MotorMicroStepping.Versa_MotorMicroStep_x16));
                    break;
                case 32:
                    check = CheckError(Versa_Motor_setMicroStepping(rowHandle, Versa_MotorMicroStepping.Versa_MotorMicroStep_x32));
                    break;
                case 64:
                    check = CheckError(Versa_Motor_setMicroStepping(rowHandle, Versa_MotorMicroStepping.Versa_MotorMicroStep_x64));
                    break;
                default:
                    check = CheckError(Versa_Motor_setMicroStepping(rowHandle, Versa_MotorMicroStepping.Versa_MotorMicroStep_x8));
                    MessageBox.Show("Invalid Microstepping. Setting it to x8");
                    break;
            }
            if (!check) { return false; }

            // Commit the Settings
            check = CheckError(Versa_Motor_commitSettings(rowHandle));
            if (!check) { return false; }
            check = CheckError(Versa_Motor_turnOn(rowHandle));
            if (!check) { return false; }

            // Set motor and encoder positions to 0.
            check = CheckError(Versa_Motor_zeroPositionCounters(rowHandle));
            if (!check) { return false; }
            check = CheckError(Versa_Motor_commitSettings(rowHandle));
            if (!check) { return false; }


            //if (ReadUserData)
            //{
            //    ReadDataRowMotor();
            //}
            //else
            //{
            //    RowMotorData.name = "R";
            //    RowMotorData.motorReverse = info.RowMotorReverse;
            //    RowMotorData.encoderReverse = info.RowEncoderReverse;
            //    RowMotorData.direction = info.RowDirection;
            //    RowMotorData.current = info.RowCurrent;
            //    RowMotorData.fullstepsPerRev = info.RowFullStepsPerRev;
            //    RowMotorData.encodercountsPerRev = info.RowEncoderCountsPerRev;
            //    RowMotorData.unitsPerRev = (float)info.RowUnitsPerRev;
            //    RowMotorData.errorTolerance = (float)info.RowPositionError;
            //    RowMotorData.speed = (float)info.RowSpeed;
            //    RowMotorData.accel = (float)info.RowAcceleration;
            //    RowMotorData.microstep = info.RowMicrostep;
            //}

            return true;
        }

        public bool InitialiseColumnMotor()
        {
            if (!ColumnConnect) { return false; }

            bool check;
            //// Set Handle
            //columnHandle = Versa_getSBusHandle(info.ColumnBus);

            // Initialise Settings
            check = CheckError(Versa_Motor_setTranslationParameters(columnHandle, info.ColumnFullStepsPerRev, info.ColumnEncoderCountsPerRev, info.ColumnUnitsPerRev));
            if (!check) { return false; }
            check = CheckError(Versa_Motor_setCurrent(columnHandle, info.ColumnCurrent));
            if (!check) { return false; }
            check = CheckError(Versa_Motor_setReducedHoldCurrentEnabled(columnHandle, true));
            if (!check) { return false; }
            check = CheckError(Versa_Motor_setSpeed(columnHandle, info.ColumnSpeed));
            if (!check) { return false; }
            check = CheckError(Versa_Motor_setAcceleration(columnHandle, info.ColumnAcceleration));
            if (!check) { return false; }
            check = CheckError(Versa_Motor_setDirectionReversed(columnHandle, info.ColumnMotorReverse));
            if (!check) { return false; }
            check = CheckError(Versa_Motor_setEncoderReversed(columnHandle, info.ColumnEncoderReverse));
            if (!check) { return false; }
            check = CheckError(Versa_Motor_setStartLimit(columnHandle, Versa_PinPull.Versa_Pin_PullUp, Versa_PinTriggerEdge.Versa_Pin_FallingEdge));
            if (!check) { return false; }
            check = CheckError(Versa_Motor_enableAccurateHomeSearch(columnHandle, info.ColumnAccurateHome));
            if (!check) { return false; }
            if (!info.MotorClosedLoop)
            {
                check = CheckError(Versa_Motor_setControlMode(columnHandle, Versa_MotorControl.Versa_MotorOpenLoop));
            }
            else
            {
                check = CheckError(Versa_Motor_setControlMode(columnHandle, Versa_MotorControl.Versa_MotorOpenLoopWithCheck));
            }
            if (!check) { return false; }

            int maxEncoderDiscrepancy;
            maxEncoderDiscrepancy = (int)(ColumnErrorThreshold * info.ColumnEncoderCountsPerRev / info.ColumnUnitsPerRev);
            check = CheckError(Versa_Motor_setMaxEncoderDiscrepancy(columnHandle, maxEncoderDiscrepancy));
            if (!check) { return false; }

            // Microstepping
            switch (info.ColumnMicrostep)
            {
                case 1:
                    check = CheckError(Versa_Motor_setMicroStepping(columnHandle, Versa_MotorMicroStepping.Versa_MotorMicroStep_none));
                    break;
                case 2:
                    check = CheckError(Versa_Motor_setMicroStepping(columnHandle, Versa_MotorMicroStepping.Versa_MotorMicroStep_x2));
                    break;
                case 4:
                    check = CheckError(Versa_Motor_setMicroStepping(columnHandle, Versa_MotorMicroStepping.Versa_MotorMicroStep_x4));
                    break;
                case 8:
                    check = CheckError(Versa_Motor_setMicroStepping(columnHandle, Versa_MotorMicroStepping.Versa_MotorMicroStep_x8));
                    break;
                case 16:
                    check = CheckError(Versa_Motor_setMicroStepping(columnHandle, Versa_MotorMicroStepping.Versa_MotorMicroStep_x16));
                    break;
                case 32:
                    check = CheckError(Versa_Motor_setMicroStepping(columnHandle, Versa_MotorMicroStepping.Versa_MotorMicroStep_x32));
                    break;
                case 64:
                    check = CheckError(Versa_Motor_setMicroStepping(columnHandle, Versa_MotorMicroStepping.Versa_MotorMicroStep_x64));
                    break;
                default:
                    check = CheckError(Versa_Motor_setMicroStepping(columnHandle, Versa_MotorMicroStepping.Versa_MotorMicroStep_x8));
                    MessageBox.Show("Invalid Microstepping. Setting it to x8");
                    break;
            }
            if (!check) { return false; }

            // Commit the Settings
            check = CheckError(Versa_Motor_commitSettings(columnHandle));
            if (!check) { return false; }
            check = CheckError(Versa_Motor_turnOn(columnHandle));
            if (!check) { return false; }

            // Set motor and encoder positions to 0.
            check = CheckError(Versa_Motor_zeroPositionCounters(columnHandle));
            if (!check) { return false; }
            check = CheckError(Versa_Motor_commitSettings(columnHandle));
            if (!check) { return false; }


            //if (ReadUserData)
            //{
            //    ReadDataColumnMotor();
            //}
            //else
            //{
            //    ColumnMotorData.name = "C";
            //    ColumnMotorData.motorReverse = info.ColumnMotorReverse;
            //    ColumnMotorData.encoderReverse = info.ColumnEncoderReverse;
            //    ColumnMotorData.direction = info.ColumnDirection;
            //    ColumnMotorData.current = info.ColumnCurrent;
            //    ColumnMotorData.fullstepsPerRev = info.ColumnFullStepsPerRev;
            //    ColumnMotorData.encodercountsPerRev = info.ColumnEncoderCountsPerRev;
            //    ColumnMotorData.unitsPerRev = (float)info.ColumnUnitsPerRev;
            //    ColumnMotorData.errorTolerance = (float)info.ColumnPositionError;
            //    ColumnMotorData.speed = (float)info.ColumnSpeed;
            //    ColumnMotorData.accel = (float)info.ColumnAcceleration;
            //    ColumnMotorData.microstep = info.ColumnMicrostep;
            //}
            return true;
        }

        public bool RowMotorEnable(bool value)
        {
            if (!RowConnect) { return false; }

            if (value)
            {
                return CheckError(Versa_Motor_turnOn(rowHandle));
            }
            else
            {
                return CheckError(Versa_Motor_turnOff(rowHandle));
            }
        }

        public bool ColumnMotorEnable(bool value)
        {
            if (!ColumnConnect) { return false; }

            if (value)
            {
                return CheckError(Versa_Motor_turnOn(columnHandle));
            }
            else
            {
                return CheckError(Versa_Motor_turnOff(columnHandle));
            }
        }


        // Motion
        public bool InsertPlate()
        {
            if ((!RowConnect) | (!ColumnConnect)) { return false; }

            bool check;
            bool r_check;
            bool c_check;

            // clear any errors that might be present
            RowError = false;
            ColumnError = false;
            check = CheckError(Versa_Motor_zeroPositionCounters(rowHandle));
            if (!check) { return check; }
            check = CheckError(Versa_Motor_zeroPositionCounters(columnHandle));
            if (!check) { return check; }

            // Reduce Speed
            check = CheckError(Versa_Motor_setSpeed(rowHandle, 50));
            if (!check) { return check; }
            check = CheckError(Versa_Motor_setSpeed(columnHandle, 50));
            if (!check) { return check; }
            check = CheckError(Versa_Motor_setReducedHoldCurrentEnabled(rowHandle, true));
            if (!check) { return check; }
            check = CheckError(Versa_Motor_setReducedHoldCurrentEnabled(columnHandle, true));
            if (!check) { return check; }
            check = CheckError(Versa_Motor_setMaxEncoderDiscrepancy(rowHandle, 100));
            if (!check) { return false; }
            check = CheckError(Versa_Motor_setMaxEncoderDiscrepancy(columnHandle, 100));
            if (!check) { return false; }

            check = CheckError(Versa_Motor_commitSettings(rowHandle));
            if (!check) { return check; }
            check = CheckError(Versa_Motor_commitSettings(columnHandle));
            if (!check) { return check; }

            // Home Row Motor
            check = CheckError(Versa_Motor_moveHome(rowHandle));
            if (!check) { return check; }
            check = CheckError(Versa_Motor_waitFor(rowHandle, 10000));
            if (!check) { return check; }

            // Check row position
            r_check = CheckRowMotor(0);
            // If motor error, try homing again...
            if (!r_check)
            {
                //MessageBox.Show("Row Limit Switch Not Found!!");
                // Clear error by setting counters to zero.
                check = CheckError(Versa_Motor_zeroPositionCounters(rowHandle));
                if (!check) { return check; }

                // Move motor 10mm from limit switch.
                check = StepRowMotor(20, true);
                //if (!check) { return check; }

                // Clear error by setting counters to zero.
                check = CheckError(Versa_Motor_zeroPositionCounters(rowHandle));
                if (!check) { return check; }

                // Home Row Motor
                check = CheckError(Versa_Motor_moveHome(rowHandle));
                if (!check) { return check; }
                check = CheckError(Versa_Motor_waitFor(rowHandle, 10000));
                if (!check) { return check; }

                // Check row position
                r_check = CheckRowMotor(0);

                // If home error occurs 2nd time, show message, disable motors, and exit.
                if (!r_check)
                {
                    MessageBox.Show("Row Limit Switch Not Found!!");

                    RowMotorEnable(false);
                    return false;
                }
            }



            // Home Column Motor            
            check = CheckError(Versa_Motor_moveHome(columnHandle));
            if (!check) { return check; }
            check = CheckError(Versa_Motor_waitFor(columnHandle, 10000));
            if (!check) { return check; }

            // Check column position
            c_check = CheckColumnMotor(0);
            // If motor error, try homing again...
            if (!c_check)
            {
                //MessageBox.Show("Row Limit Switch Not Found!!");
                // Clear error by setting counters to zero.
                check = CheckError(Versa_Motor_zeroPositionCounters(columnHandle));
                if (!check) { return check; }

                // Move motor 10mm from limit switch.
                check = StepColumnMotor(20, true);
                //if (!check) { return check; }

                // Clear error by setting counters to zero.
                check = CheckError(Versa_Motor_zeroPositionCounters(columnHandle));
                if (!check) { return check; }

                // Home Column Motor
                check = CheckError(Versa_Motor_moveHome(columnHandle));
                if (!check) { return check; }
                check = CheckError(Versa_Motor_waitFor(columnHandle, 10000));
                if (!check) { return check; }

                // Check column position
                c_check = CheckColumnMotor(0);

                // If home error occurs 2nd time, show message, disable motors, and exit.
                if (!c_check)
                {
                    MessageBox.Show("Column Limit Switch Not Found!!");

                    ColumnMotorEnable(false);
                    return false;
                }
            }

            // Reset Speeds
            check = CheckError(Versa_Motor_setSpeed(rowHandle, info.RowSpeed));
            if (!check) { return check; }
            check = CheckError(Versa_Motor_setSpeed(columnHandle, info.ColumnSpeed));
            if (!check) { return check; }

            int maxEncoderDiscrepancy;

            maxEncoderDiscrepancy = (int)(RowErrorThreshold * info.RowEncoderCountsPerRev / info.RowUnitsPerRev);
            check = CheckError(Versa_Motor_setMaxEncoderDiscrepancy(rowHandle, maxEncoderDiscrepancy));
            if (!check) { return false; }
            maxEncoderDiscrepancy = (int)(ColumnErrorThreshold * info.ColumnEncoderCountsPerRev / info.ColumnUnitsPerRev);
            check = CheckError(Versa_Motor_setMaxEncoderDiscrepancy(columnHandle, maxEncoderDiscrepancy));
            if (!check) { return false; }


            check = CheckError(Versa_Motor_commitSettings(rowHandle));
            if (!check) { return check; }
            check = CheckError(Versa_Motor_commitSettings(columnHandle));
            if (!check) { return check; }


            return true;


        }

        public bool EjectPlate()
        {
            if ((!RowConnect) | (!ColumnConnect)) { return false; }

            bool check;
            // Reduce Speed to 50mm/s
            check = CheckError(Versa_Motor_setSpeed(rowHandle, 50));
            if (!check) { return check; }
            check = CheckError(Versa_Motor_setSpeed(columnHandle, 50));
            if (!check) { return check; }
            check = CheckError(Versa_Motor_setReducedHoldCurrentEnabled(rowHandle, false));
            if (!check) { return check; }
            check = CheckError(Versa_Motor_setReducedHoldCurrentEnabled(columnHandle, false));
            if (!check) { return check; }
            check = CheckError(Versa_Motor_setControlMode(rowHandle, Versa_MotorControl.Versa_MotorOpenLoop));
            if (!check) { return check; }
            check = CheckError(Versa_Motor_setControlMode(columnHandle, Versa_MotorControl.Versa_MotorOpenLoop));
            if (!check) { return check; }
            check = CheckError(Versa_Motor_setMaxEncoderDiscrepancy(rowHandle, 100));
            if (!check) { return false; }
            check = CheckError(Versa_Motor_setMaxEncoderDiscrepancy(columnHandle, 100));
            if (!check) { return false; }

            check = CheckError(Versa_Motor_commitSettings(rowHandle));
            if (!check) { return check; }
            check = CheckError(Versa_Motor_commitSettings(columnHandle));
            if (!check) { return check; }

            // Move Column Stage
            bool col_check = StepColumnMotor(info.ColumnEject, info.SoftwarePositionCheck);
            //if (!col_check) { MessageBox.Show("Column Positioning Error: " + ColumnDelta.ToString("F3")); }

            // Move Row Stage
            bool row_check = StepRowMotor(info.RowEject, info.SoftwarePositionCheck);
            //if (!row_check) { MessageBox.Show("Row Positioning Error: " + RowDelta.ToString("F3")); }

            // Reset Speeds
            check = CheckError(Versa_Motor_setSpeed(rowHandle, info.RowSpeed));
            if (!check) { return check; }
            check = CheckError(Versa_Motor_setSpeed(columnHandle, info.ColumnSpeed));
            if (!check) { return check; }

            if (!info.MotorClosedLoop)
            {
                check = CheckError(Versa_Motor_setControlMode(rowHandle, Versa_MotorControl.Versa_MotorOpenLoop));
            }
            else
            {
                check = CheckError(Versa_Motor_setControlMode(rowHandle, Versa_MotorControl.Versa_MotorOpenLoopWithCheck));
            }
            if (!check) { return false; }
            if (!info.MotorClosedLoop)
            {
                check = CheckError(Versa_Motor_setControlMode(columnHandle, Versa_MotorControl.Versa_MotorOpenLoop));
            }
            else
            {
                check = CheckError(Versa_Motor_setControlMode(columnHandle, Versa_MotorControl.Versa_MotorOpenLoopWithCheck));
            }
            if (!check) { return false; }

            int maxEncoderDiscrepancy;

            maxEncoderDiscrepancy = (int)(RowErrorThreshold * info.RowEncoderCountsPerRev / info.RowUnitsPerRev);
            check = CheckError(Versa_Motor_setMaxEncoderDiscrepancy(rowHandle, maxEncoderDiscrepancy));
            if (!check) { return false; }
            maxEncoderDiscrepancy = (int)(ColumnErrorThreshold * info.ColumnEncoderCountsPerRev / info.ColumnUnitsPerRev);
            check = CheckError(Versa_Motor_setMaxEncoderDiscrepancy(columnHandle, maxEncoderDiscrepancy));
            if (!check) { return false; }

            check = CheckError(Versa_Motor_commitSettings(rowHandle));
            if (!check) { return check; }
            check = CheckError(Versa_Motor_commitSettings(columnHandle));
            if (!check) { return check; }

            return true;
        }

        public bool MoveReferencePosition()
        {
            if ((!RowConnect) | (!ColumnConnect)) { return false; }

            // Move Column Stage
            bool col_check = StepColumnMotor(info.ColumnOffset, info.SoftwarePositionCheck);
            if (!col_check) { MessageBox.Show("Column Positioning Error: " + ColumnDeltaU.ToString("F3")); }

            // Move Row Stage
            bool row_check = StepRowMotor(info.RowOffset, info.SoftwarePositionCheck);
            if (!row_check) { MessageBox.Show("Row Positioning Error: " + RowDeltaU.ToString("F3")); }

            return true;
        }

        public bool StepRowMotor(double value)
        {
            if (!RowConnect) { return false; }

            // Step Motor
            bool status = CheckError(Versa_Motor_moveAbsolute(rowHandle, value));

            if (!status)
                return false;

            // Wait till move has finished
            return CheckError(Versa_Motor_waitFor(rowHandle, 5000));

        }

        public bool StepRowMotor(double value, bool pos_check)
        {
            if (!RowConnect) { return false; }

            double fvalue = Math.Floor(value);
            // Step Motor
            bool status = CheckError(Versa_Motor_moveAbsolute(rowHandle, value));

            if (!status)
            {
                RowError = true;
                return false;
            }
                

            // Wait till move has finished
            bool rtn = CheckError(Versa_Motor_waitFor(rowHandle, 10000));

            if (pos_check)
            {
                bool mcheck = CheckRowMotor(value);
                return mcheck;
            }
            else
            {
                return true;
            }

        }

        public bool HomeRowMotor()
        {
            if (!RowConnect) { return false; }

            bool check;
            bool r_check;

            // clear any errors that might be present
            RowError = false;
            check = CheckError(Versa_Motor_zeroPositionCounters(rowHandle));
            if (!check) { return check; }

            // Reduce Speed
            check = CheckError(Versa_Motor_setSpeed(rowHandle, 50));
            if (!check) { return check; }
            check = CheckError(Versa_Motor_setReducedHoldCurrentEnabled(rowHandle, true));
            if (!check) { return check; }
            check = CheckError(Versa_Motor_setMaxEncoderDiscrepancy(rowHandle, 100));
            if (!check) { return false; }

            check = CheckError(Versa_Motor_commitSettings(rowHandle));
            if (!check) { return check; }

            // Home Row Motor
            check = CheckError(Versa_Motor_moveHome(rowHandle));
            if (!check) { return check; }
            check = CheckError(Versa_Motor_waitFor(rowHandle, 10000));
            if (!check) { return check; }

            // Check row position
            r_check = CheckRowMotor(0);
            // If motor error, try homing again...
            if (!r_check)
            {
                //MessageBox.Show("Row Limit Switch Not Found!!");
                // Clear error by setting counters to zero.
                check = CheckError(Versa_Motor_zeroPositionCounters(rowHandle));
                if (!check) { return check; }

                // Move motor 10mm from limit switch.
                check = StepRowMotor(20, true);
                //if (!check) { return check; }

                // Clear error by setting counters to zero.
                check = CheckError(Versa_Motor_zeroPositionCounters(rowHandle));
                if (!check) { return check; }

                // Home Row Motor
                check = CheckError(Versa_Motor_moveHome(rowHandle));
                if (!check) { return check; }
                check = CheckError(Versa_Motor_waitFor(rowHandle, 10000));
                if (!check) { return check; }

                // Check row position
                r_check = CheckRowMotor(0);

                // If home error occurs 2nd time, show message, disable motors, and exit.
                if (!r_check)
                {
                    MessageBox.Show("Row Limit Switch Not Found!!");

                    RowMotorEnable(false);
                    return false;
                }
            }

            // Reset Speeds
            check = CheckError(Versa_Motor_setSpeed(rowHandle, info.RowSpeed));
            if (!check) { return check; }

            int maxEncoderDiscrepancy;

            maxEncoderDiscrepancy = (int)(RowErrorThreshold * info.RowEncoderCountsPerRev / info.RowUnitsPerRev);
            check = CheckError(Versa_Motor_setMaxEncoderDiscrepancy(rowHandle, maxEncoderDiscrepancy));
            if (!check) { return false; }

            check = CheckError(Versa_Motor_commitSettings(rowHandle));
            if (!check) { return check; }

            return true;
        }

        public bool HomeColumnMotor()
        {
            if (!ColumnConnect) { return false; }

            bool check;
            bool c_check;

            // clear any errors that might be present
            ColumnError = false;
            check = CheckError(Versa_Motor_zeroPositionCounters(columnHandle));
            if (!check) { return check; }

            // Reduce Speed
            check = CheckError(Versa_Motor_setSpeed(columnHandle, 50));
            if (!check) { return check; }
            check = CheckError(Versa_Motor_setReducedHoldCurrentEnabled(columnHandle, true));
            if (!check) { return check; }
            check = CheckError(Versa_Motor_setMaxEncoderDiscrepancy(columnHandle, 100));
            if (!check) { return false; }

            check = CheckError(Versa_Motor_commitSettings(columnHandle));
            if (!check) { return check; }

            // Home Column Motor            
            check = CheckError(Versa_Motor_moveHome(columnHandle));
            if (!check) { return check; }
            check = CheckError(Versa_Motor_waitFor(columnHandle, 10000));
            if (!check) { return check; }

            // Check column position
            c_check = CheckColumnMotor(0);
            // If motor error, try homing again...
            if (!c_check)
            {
                //MessageBox.Show("Row Limit Switch Not Found!!");
                // Clear error by setting counters to zero.
                check = CheckError(Versa_Motor_zeroPositionCounters(columnHandle));
                if (!check) { return check; }

                // Move motor 10mm from limit switch.
                check = StepColumnMotor(20, true);
                //if (!check) { return check; }

                // Clear error by setting counters to zero.
                check = CheckError(Versa_Motor_zeroPositionCounters(columnHandle));
                if (!check) { return check; }

                // Home Column Motor
                check = CheckError(Versa_Motor_moveHome(columnHandle));
                if (!check) { return check; }
                check = CheckError(Versa_Motor_waitFor(columnHandle, 10000));
                if (!check) { return check; }

                // Check column position
                c_check = CheckColumnMotor(0);

                // If home error occurs 2nd time, show message, disable motors, and exit.
                if (!c_check)
                {
                    MessageBox.Show("Column Limit Switch Not Found!!");

                    ColumnMotorEnable(false);
                    return false;
                }
            }

            // Reset Speeds
            check = CheckError(Versa_Motor_setSpeed(columnHandle, info.ColumnSpeed));
            if (!check) { return check; }

            int maxEncoderDiscrepancy;
            maxEncoderDiscrepancy = (int)(ColumnErrorThreshold * info.ColumnEncoderCountsPerRev / info.ColumnUnitsPerRev);
            check = CheckError(Versa_Motor_setMaxEncoderDiscrepancy(columnHandle, maxEncoderDiscrepancy));
            if (!check) { return false; }

            check = CheckError(Versa_Motor_commitSettings(columnHandle));
            if (!check) { return check; }

            return true;
        }

        public bool StepColumnMotor(double value)
        {
            if (!ColumnConnect) { return false; }

            // Step Motor
            bool status = CheckError(Versa_Motor_moveAbsolute(columnHandle, value));

            if (!status)
                return false;

            // Wait till move has finished
            return CheckError(Versa_Motor_waitFor(columnHandle, 5000));
        }

        public bool StepColumnMotor(double value, bool pos_check)
        {
            if (!ColumnConnect) { return false; }

            // Step Motor
            bool status = CheckError(Versa_Motor_moveAbsolute(columnHandle, value));

            if (!status)
            {
                ColumnError = true;
                return false;
            }
                
            // Wait till move has finished
            bool rtn = CheckError(Versa_Motor_waitFor(columnHandle, 10000));

            if (pos_check)
            {
                bool mcheck = CheckColumnMotor(value);
                return mcheck;
            }
            else
            {
                return true;
            }

        }

        public bool CheckColumnMotor(double value)
        {
            if (!ColumnConnect) { return false; }

            bool check;
            double pFullSteps = 0;
            int pCounts = 0;
            double pMotorUnits = 0;
            double pEncoderUnits = 0;

            Versa_MotorState motorState;
            Versa_MotorError motorError;
            check = CheckError(Versa_Motor_requestInfo(columnHandle));
            if (!check) { return false; }
            check = CheckError(Versa_Motor_getLastMotorPosition(columnHandle, &pMotorUnits, &pFullSteps));
            if (!check) { return false; }
            check = CheckError(Versa_Motor_getLastEncoderPosition(columnHandle, &pEncoderUnits, &pCounts));
            if (!check) { return false; }
            check = CheckError(Versa_Motor_getLastState(columnHandle, &motorState, &motorError));
            if (!check) { return false; }

            ColumnPositionU = pMotorUnits;
            ColumnPosition = pFullSteps * (info.ColumnUnitsPerRev / info.ColumnFullStepsPerRev);
            ColumnCounts = pCounts;
            ColumnEncoderU = pEncoderUnits;
            ColumnEncoder = ColumnCounts * (info.ColumnUnitsPerRev / info.ColumnEncoderCountsPerRev);

            ColumnDelta = ColumnPosition - ColumnEncoder;
            ColumnDeltaU = ColumnPositionU - ColumnEncoderU;

            ColumnMotorData.state = motorState;
            ColumnMotorData.error = motorError;

            if (motorError == Versa_MotorError.Versa_MotorErrorNone)
            {
                if ((ColumnDeltaU < -ColumnErrorThreshold) | (ColumnDeltaU > ColumnErrorThreshold))
                {
                    ColumnError = true;
                }
                else
                {
                    ColumnError = false;
                }
            }
            else
            {
                ColumnError = true;
            }
            return !ColumnError;
        }

        public bool CheckRowMotor(double value)
        {
            if (!RowConnect) { return false; }

            bool check;
            double pFullSteps = 0;
            int pCounts = 0;
            double pMotorUnits = 0;
            double pEncoderUnits = 0;

            Versa_MotorState motorState;
            Versa_MotorError motorError;
            check = CheckError(Versa_Motor_requestInfo(rowHandle));
            if (!check) { return false; }
            check = CheckError(Versa_Motor_getLastMotorPosition(rowHandle, &pMotorUnits, &pFullSteps));
            if (!check) { return false; }
            check = CheckError(Versa_Motor_getLastEncoderPosition(rowHandle, &pEncoderUnits, &pCounts));
            if (!check) { return false; }
            check = CheckError(Versa_Motor_getLastState(rowHandle, &motorState, &motorError));
            if (!check) { return false; }

            RowPositionU = pMotorUnits;
            RowPosition = pFullSteps * (info.RowUnitsPerRev / info.RowFullStepsPerRev);
            RowCounts = pCounts;
            RowEncoderU = pEncoderUnits;
            RowEncoder = RowCounts * (info.RowUnitsPerRev / info.RowEncoderCountsPerRev);

            RowDelta = RowPosition - RowEncoder;
            RowDeltaU = RowPositionU - RowEncoderU;

            RowMotorData.state = motorState;
            RowMotorData.error = motorError;

            if (motorError == Versa_MotorError.Versa_MotorErrorNone)
            {
                if ((RowDeltaU < -RowErrorThreshold) | (RowDeltaU > RowErrorThreshold))
                {
                    RowError = true;
                }
                else
                {
                    RowError = false;
                }
            }
            else
            {
                RowError = true;
            }
            return !RowError;
        }


        // Spectrometer
        public bool InitialiseSpectrometer()
        {
            if (!SpecConnect) { return false; }

            //// Get the Handle
            //linescanHandle = Versa_getPBusHandle();

            bool check = CheckError(Versa_LineScan_configureSensor(linescanHandle, info.Pixel, info.PostIntegrationWait, info.MinimumCycles));

            //if (ReadUserData)
            //{
            //    ReadDataSpectrometer();
            //}
            //else
            //{
            //    linescanData.name = "F-UV";
            //    linescanData.serial = 149806;
            //    linescanData.degree = 5;
            //    linescanData.Npixels = info.Pixel;
            //    linescanData.coef = new float[5];
            //    linescanData.coef[0] = (float)info.P0;
            //    linescanData.coef[1] = (float)info.P1;
            //    linescanData.coef[2] = (float)info.P2;
            //    linescanData.coef[3] = (float)info.P3;
            //    linescanData.coef[4] = (float)info.P4;
            //}
            return true;
        }

        public bool SetIntegrationTime()
        {
            if (!SpecConnect) { return false; }

            return CheckError(Versa_LineScan_setIntegrationTime(linescanHandle, Integration));

        }

        public bool DarkMeasurement()
        {
            if (!SpecConnect) { return false; }

            // Perform Background Measurement
            for (int i = 0; i < 3; i++)
            {
                CheckError(Versa_LineScan_startMeasurement(linescanHandle));
                CheckError(Versa_LineScan_waitFor(linescanHandle, 1000));
            }                      

            // Initialise Data Structure
            Versa_LineScan_Result_t result;
            Versa_LineScan_Result_t_init(&result);

            CheckError(Versa_LineScan_getAllResults(linescanHandle, &result));

            for(int i = 0; i < result.lineCount; i++)
            {
                Background = new double[result.data[i].pixelCount];

                for (int j = 0; j < result.data[i].pixelCount; j++)
                {

                    Background[j] = result.data[i].lineTrace[j];

                }
            }

            Versa_LineScan_Result_t_free(&result);

            return true;

        }

        public bool LightMeasurement()
        {
            if (!SpecConnect) { return false; }

            // Turn Led On
            if (info.LEDControl) { LedOn(); }

            // Perform Measurement
            CheckError(Versa_LineScan_startMeasurement(linescanHandle));
            CheckError(Versa_LineScan_waitFor(linescanHandle, 1000));

            // Turn Led Off
            if (info.LEDControl) { LedOff(); }

            // Initialise Data Structure
            Versa_LineScan_Result_t result;
            Versa_LineScan_Result_t_init(&result);

            CheckError(Versa_LineScan_getAllResults(linescanHandle, &result));

            for (int i = 0; i < result.lineCount; i++)
            {
                Waveform = new double[result.data[i].pixelCount];

                for (int j = 0; j < result.data[i].pixelCount; j++)
                {
                    Waveform[j] = result.data[i].lineTrace[j] - Background[j];

                }
            }

            Versa_LineScan_Result_t_free(&result);

            return true;
        }

        public bool ScanMeasurement(int col)
        {
            if (!SpecConnect) { return false; }

            // Perform Measurement
            bool wCheck = CheckError(Versa_App_waitForLine(col, 1000));
            if (!wCheck)
            {
                ColumnError = true;
                return false;
            }

            // Initialise Data Structure
            Versa_LineScan_Result_t result;
            Versa_LineScan_Result_t_init(&result);

            bool lCheck = CheckError(Versa_LineScan_getSingleResult(linescanHandle, col, &result));
            if (!lCheck)
            {
                ColumnError = true;
                return false;
            }

            for (int i = 0; i < result.lineCount; i++)
            {
                Waveform = new double[result.data[i].pixelCount];

                for (int j = 0; j < result.data[i].pixelCount; j++)
                {
                    Waveform[j] = result.data[i].lineTrace[j] - Background[j];

                }
            }

            Versa_LineScan_Result_t_free(&result);

            return true;
        }

        public bool SetScanHandles()
        {
            if ((!SpecConnect) | (!ColumnConnect) | (!LEDConnect)) { return false; }

            // Set scan handles
            CheckError(Versa_App_setScanLineScanHandle(linescanHandle));
            CheckError(Versa_App_setScanMotorDriverHandle(columnHandle));
            CheckError(Versa_App_setScanLEDDriverHandle(ledHandle));

            return true;
        }

        public bool SetScanParameters(double startSteps, double deltaSteps, double endSteps, int stops)
        {
            if (!VersaConnect) { return false; }

            // Set the Parameters
            return CheckError(Versa_App_setScanParameters(startSteps, deltaSteps, endSteps, stops));

        }

        public bool StartScanMeasurement(bool ledControl)
        {
            if (!VersaConnect) { return false; }

            if (ledControl) { return CheckError(Versa_App_startStopAndGoMeasurement(Versa_LEDControl.Versa_LED_HWControl)); }
            else { return CheckError(Versa_App_startStopAndGoMeasurement(Versa_LEDControl.Versa_LED_SWControl)); }

        }

        public bool EndScanMeasurement()
        {
            if (!VersaConnect) { return false; }

            return CheckError(Versa_App_waitFor(10000));
        }


        // Error Handling
        public bool CheckError(int value)
        {
            byte[] byteArray = new byte[1000];

            if (value == 0) 
            {
                VersaError = false;
            }
            else
            {
                unsafe
                {
                    fixed (byte* erPointer = &byteArray[0])
                    {
                        Versa_getLastErrorMessage(erPointer);
                    };
                }

                // Convert byte array to string.
                string msg = System.Text.Encoding.UTF8.GetString(byteArray, 0, 1000);
                if (msg.Contains('\0')) { msg = msg.Substring(0, msg.IndexOf('\0')); }

                MessageBox.Show(msg);
                ErrorMessage = msg;
                VersaError = true;
            }

            return !VersaError;
        }


    }

    
}
