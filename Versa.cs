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
        public static extern int Versa_requestStatus();

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Versa_resetAllPorts();


        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_IO_setOutputStates(int mask);


        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern Versa_Handle_t Versa_getSBusHandle(int sbusNumber);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern Versa_Handle_t Versa_getPBusHandle();

        //[DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        //public static extern int Versa_getSBusInfo(int sbusNumber, Versa_PeripheralInfo_t* pInfo);

        //[DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        //public static extern int Versa_getPBusInfo(Versa_PeripheralInfo_t* pInfo);

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
        public static extern int Versa_LineScan_configureSensor(Versa_Handle_t handle, int pixelCount, int postIntegrationWait,int minimumCycles);

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

        // Handles
        Versa_Handle_t ledHandle { get; set; }
        Versa_Handle_t rowHandle { get; set; }
        Versa_Handle_t columnHandle { get; set; }
        Versa_Handle_t linescanHandle { get; set; }


        // Variables
        public string SerialNumber { get; set; }
        public bool Connection { get; set; }
        public bool VersaError { get; set; }
        public string ErrorMessage { get; set; }


        // Light Source
        public int Current { get; set; }

        // Spectrometer
        public double Integration { get; set; }

        public double[] Background { get; set; }    // Background Spectrum [Pre Scan, Dark]
        public double[] Waveform { get; set; }      // Waveform Spectrum [Well, Light]       

        public string FirmwareVersion;

        Time time = new Time();

        // Initialise
        public bool Connect()
        {
            // Obtain Firmware Version
            GetVersion();

            // Variables
            bool state = false;
            Connection = false;

            // Available devices
            int deviceCount;
            Versa_DeviceType versa_DeviceType;

            CheckError(Versa_getAvailableDevices(&deviceCount, &versa_DeviceType));

            if (deviceCount > 0)
            {                
                if (CheckError(Versa_initialiseUSBSession()))
                {
                    state = Initialise();
                }
            }

            return state;
        }

        public void Disconnect()
        {
            // Available devices
            int deviceCount;
            Versa_DeviceType versa_DeviceType;

            CheckError(Versa_getAvailableDevices(&deviceCount, &versa_DeviceType));

            if(deviceCount > 0)
            {
                // Disable Motors
                Versa_Motor_turnOff(rowHandle);
                Versa_Motor_turnOff(columnHandle);

                // Disconnect
                CheckError(Versa_closeSession());
            }


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
            // Available devices
            bool error = CheckError(Versa_requestStatus());

            if (error)
            {
                return true;
            }

            return false;
        }

        public bool Initialise()
        {
            bool state1 = InitialiseLed();
            bool state2 = InitialiseRowMotor();
            bool state3 = InitialiseColumnMotor();
            bool state4 = InitialiseSpectrometer();

            if(state1 && state2 && state3 && state4)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
                     

        // LED
        public bool InitialiseLed()
        {
            // Initialise LED Handle
            ledHandle = Versa_getSBusHandle(info.LEDBus);

            int pSeconds = 0;
            double voltage_V = 0;

            CheckError(Versa_LED_setChannel(ledHandle, info.LEDChannel));
            CheckError(Versa_LED_requestInfo(ledHandle));
            CheckError(Versa_LED_getLastUsage(ledHandle, info.LEDChannel, &pSeconds));
            CheckError(Versa_LED_getLastSupplyVoltage(ledHandle, &voltage_V));

            return true;
        }

        public bool SetLedCurrent()
        {
            // Set the Current
            return CheckError(Versa_LED_setCurrent(ledHandle, Current));
        }

        public bool LedOn()
        {
            return CheckError(Versa_LED_turnOn(ledHandle));
        }

        public bool LedOff()
        {
            return CheckError(Versa_LED_turnOff(ledHandle));
        }


        // Motor
        public bool InitialiseRowMotor()
        {
            // Set Handle
            rowHandle = Versa_getSBusHandle(info.RowBus);

            // Translation Properties
            if(!CheckError(Versa_Motor_setTranslationParameters(rowHandle, info.RowStepsPerRev, info.RowEncoderCountsPerRev, info.RowUnitsPerRev)))
            {
                return false;
            }

            // Set Motor Current
            if (!CheckError(Versa_Motor_setCurrent(rowHandle, info.RowCurrent)))
            {
                return false;
            }

            // Set Reduced Hold Current
            if (!CheckError(Versa_Motor_setReducedHoldCurrentEnabled(rowHandle, true)))
            {
                return false;
            }

            // Set Motor Speed
            if (!CheckError(Versa_Motor_setSpeed(rowHandle, info.RowSpeed)))
            {
                return false;
            }

            // Set Motor Acceleration
            if (!CheckError(Versa_Motor_setAcceleration(rowHandle, info.RowAcceleration)))
            {
                return false;
            }

            // Set Motor Direction
            if (!CheckError(Versa_Motor_setDirectionReversed(rowHandle, info.RowMotorReverse)))
            {
                return false;
            }

            // Set Encoder Direction
            if (!CheckError(Versa_Motor_setEncoderReversed(rowHandle, info.RowEncoderReverse)))
            {
                return false;
            }

            // Set Start Limit Properties
            if (!CheckError(Versa_Motor_setStartLimit(rowHandle, Versa_PinPull.Versa_Pin_PullUp, Versa_PinTriggerEdge.Versa_Pin_FallingEdge)))
            {
                return false;
            }

            // Set accurate home 
            if (!CheckError(Versa_Motor_enableAccurateHomeSearch(rowHandle, info.RowAccurateHome)))
            { return false; }

            // Set feedback control
            if (!info.MotorClosedLoop)
            {
                if (!CheckError(Versa_Motor_setControlMode(rowHandle, Versa_MotorControl.Versa_MotorOpenLoop)))
                { return false; }
            }
            else
            {
                if (!CheckError(Versa_Motor_setControlMode(rowHandle, Versa_MotorControl.Versa_MotorOpenLoopWithCheck)))
                { return false; }
            }

            // Set Motor Microstepping
            // Microstepping
            bool check;
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

            // Set Encoder Discrepancy
            int maxEncoderDiscrepancy = (int)(info.RowPositionError * info.RowEncoderCountsPerRev / info.RowUnitsPerRev);

            if (!CheckError(Versa_Motor_setMaxEncoderDiscrepancy(rowHandle, maxEncoderDiscrepancy)))
            {
                return false;
            }

            // Zero Motor and Encoder Positions
            if (!CheckError(Versa_Motor_zeroPositionCounters(rowHandle)))
            {
                return false;
            }

            // Commit Settings
            if (!CheckError(Versa_Motor_commitSettings(rowHandle)))
            {
                return false;
            }

            // Turn on Motor
            if (!CheckError(Versa_Motor_turnOn(rowHandle)))
            {
                return false;
            }

            // Return True
            return true;

        }

        public bool InitialiseColumnMotor()
        {
            // Set Handle
            columnHandle = Versa_getSBusHandle(info.ColumnBus);

            // Set Translation Properties
            if (!CheckError(Versa_Motor_setTranslationParameters(columnHandle, info.ColumnStepsPerRev, info.ColumnEncoderCountsPerRev, info.ColumnUnitsPerRev)))
            {
                return false;
            }

            // Set Motor Current
            if (!CheckError(Versa_Motor_setCurrent(columnHandle, info.ColumnCurrent)))
            {
                return false;
            }

            // Set Reduced Hold Current
            if (!CheckError(Versa_Motor_setReducedHoldCurrentEnabled(columnHandle, true)))
            {
                return false;
            }

            // Set Motor Speed
            if (!CheckError(Versa_Motor_setSpeed(columnHandle, info.ColumnSpeed)))
            {
                return false;
            }

            // Set Motor Acceleration
            if (!CheckError(Versa_Motor_setAcceleration(columnHandle, info.ColumnAcceleration)))
            {
                return false;
            }

            // Set Motor Direction
            if (!CheckError(Versa_Motor_setDirectionReversed(columnHandle, info.ColumnMotorReverse)))
            {
                return false;
            }

            // Set Encoder Direction
            if (!CheckError(Versa_Motor_setEncoderReversed(columnHandle, info.ColumnEncoderReverse)))
            {
                return false;
            }

            // Set Start Limit
            if (!CheckError(Versa_Motor_setStartLimit(columnHandle, Versa_PinPull.Versa_Pin_PullUp, Versa_PinTriggerEdge.Versa_Pin_FallingEdge)))
            {
                return false;
            }

            // Set Accurate Home Search
            if (!CheckError(Versa_Motor_enableAccurateHomeSearch(columnHandle, info.ColumnAccurateHome)))
            { return false; }

            // Set Motor Control Mode
            if (!info.MotorClosedLoop)
            {
                if (!CheckError(Versa_Motor_setControlMode(columnHandle, Versa_MotorControl.Versa_MotorOpenLoop)))
                { return false; }
            }
            else
            {
                if (!CheckError(Versa_Motor_setControlMode(columnHandle, Versa_MotorControl.Versa_MotorOpenLoopWithCheck)))
                { return false; }
            }

            // Set Motor Microstepping
            bool check;
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

            // Set Encoder Discrepancy
            int maxEncoderDiscrepancy = (int)(info.ColumnPositionError * info.ColumnEncoderCountsPerRev / info.ColumnUnitsPerRev);
            if (!CheckError(Versa_Motor_setMaxEncoderDiscrepancy(columnHandle, maxEncoderDiscrepancy)))
            {
                return false;
            }

            // Set Motor & Encoder Position to 0
            if (!CheckError(Versa_Motor_zeroPositionCounters(columnHandle)))
            {
                return false;
            }

            // Commit the Settings
            if (!CheckError(Versa_Motor_commitSettings(columnHandle)))
            {
                return false;
            }

            // Turn on Motor
            if (!CheckError(Versa_Motor_turnOn(columnHandle)))
            {
                return false;
            }
            
            // Return true
            return true;

        }

        public bool InitialiseLoadingMotors()
        {

            // Reduce speed to 50 mm/s
            if (!CheckError(Versa_Motor_setSpeed(rowHandle, 50)))
            {
                return false;
            }

            if (!CheckError(Versa_Motor_setSpeed(columnHandle, 50)))
            {
                return false;
            }

            // Disable Reduce Hold Current
            if (!CheckError(Versa_Motor_setReducedHoldCurrentEnabled(rowHandle, false)))
            {
                return false;
            }

            if (!CheckError(Versa_Motor_setReducedHoldCurrentEnabled(columnHandle, false)))
            {
                return false;
            }

            // Commit Settings
            if (!CheckError(Versa_Motor_commitSettings(rowHandle)))
            {
                return false;
            }

            if (!CheckError(Versa_Motor_commitSettings(columnHandle)))
            {
                return false;
            }


            // Return true
            return true;
            

        }

        public bool RowMotorEnable(bool value)
        {
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
            // Variables
            bool error;

            // Reduce speed to 50 mm/s
            error = InitialiseLoadingMotors();

            if (!error)
            {
                return false;
            }
                       
            // Home Row Motor
            CheckError(Versa_Motor_moveHome(rowHandle));
            CheckError(Versa_Motor_waitFor(rowHandle, 10000));

            // Check row position
            error = CheckRowMotor();

            // If motor error, try homing again...
            if (error == false)
            {
                // Clear Error by setting counters to zero
                CheckError(Versa_Motor_zeroPositionCounters(rowHandle));

                // Step Row Motor 20 mm
                CheckError(Versa_Motor_moveRelative(rowHandle, 20));

                // Clear Error by setting counters to zero
                CheckError(Versa_Motor_zeroPositionCounters(rowHandle));

                // Home Row Motor
                CheckError(Versa_Motor_moveHome(rowHandle));
                CheckError(Versa_Motor_waitFor(rowHandle, 10000));

                // Check row position
                error = CheckRowMotor();

                // If home error occurs 2nd time, show message, disable motors, and exit.
                if (error == false)
                {
                    MessageBox.Show("Row Limit Switch Not Found!!");
                    RowMotorEnable(false);
                    return false;
                }
            }


            // Home Column Motor            
            CheckError(Versa_Motor_moveHome(columnHandle));
            CheckError(Versa_Motor_waitFor(columnHandle, 10000));

            // Check column position
            error = CheckColumnMotor();

            // If motor error, try homing again...
            if (error == false)
            {
                // Clear error by setting counters to zero.
                CheckError(Versa_Motor_zeroPositionCounters(columnHandle));

                // Move motor 20mm from limit switch.
                CheckError(Versa_Motor_moveRelative(columnHandle,20));

                // Clear error by setting counters to zero.
                CheckError(Versa_Motor_zeroPositionCounters(columnHandle));

                // Home Column Motor
                CheckError(Versa_Motor_moveHome(columnHandle));
                CheckError(Versa_Motor_waitFor(columnHandle, 10000));

                // Check column position
                error = CheckColumnMotor();

                // If home error occurs 2nd time, show message, disable motors, and exit.
                if (error == false)
                {
                    MessageBox.Show("Column Limit Switch Not Found!!");

                    ColumnMotorEnable(false);
                    return false;
                }
            }


            // Initialise Motors
            if(InitialiseRowMotor() == false)
            {
                return false;
            }

            if(InitialiseColumnMotor() == false)
            {
                return false;
            }

            return true;

        }

        public bool EjectPlate()
        {
            // Variables
            bool error;

            // Reduce speed to 50 mm/s
            error = InitialiseLoadingMotors();

            if (error == false)
            {
                return false;
            }
           
            // Move Column Stage
            error = StepColumnMotor(info.ColumnEject);

            if(error == false)
            {
                return error;
            }

            // Move Row Stage
            error = StepRowMotor(info.RowEject);

            if (error == false)
            {
                return error;
            }

            return true;
        }

        public bool MoveReferencePosition(double colReference, double rowReference)
        {
            // Variables
            bool error;

            // Move Column Stage
            error = StepColumnMotor(colReference);  // info.ColumnOffset);

            if(error == false)
            {
                return false;
            }

            // Move Row Stage
            error = StepRowMotor(rowReference); // info.RowOffset);

            if(error == false)
            {
                return false;
            }

            // Return true
            return true;
        }

        public bool StepRowMotor(double value)
        {
            // Step Motor
            bool status = CheckError(Versa_Motor_moveAbsolute(rowHandle, value));

            if (!status)
            {
                return false;
            }                

            // Wait till move has finished
            CheckError(Versa_Motor_waitFor(rowHandle, 10000));

            // Check Position
            return CheckRowMotor();

        }

        public bool StepColumnMotor(double value)
        {
            // Step Motor
            bool status = CheckError(Versa_Motor_moveAbsolute(columnHandle, value));

            if (!status)
            {
                return false;
            }
                
            // Wait till move has finished
            CheckError(Versa_Motor_waitFor(columnHandle, 10000));

            // Check Position
            return CheckColumnMotor();

        }

        public bool CheckRowMotor()
        {
            // Variables
            double pFullSteps = 0;
            int pCounts = 0;
            double pMotorUnits = 0;
            double pEncoderUnits = 0;

            Versa_MotorState motorState;
            Versa_MotorError motorError;

            // Request Info from Motor
            if (!CheckError(Versa_Motor_requestInfo(rowHandle)))
            {
                return false;
            }

            // Get Motor Position
            if (!CheckError(Versa_Motor_getLastMotorPosition(rowHandle, &pMotorUnits, &pFullSteps)))
            {
                return false; 
            }
            // Get Encoder Position
            if (!CheckError(Versa_Motor_getLastEncoderPosition(rowHandle, &pEncoderUnits, &pCounts)))
            {
                return false;
            }

            // Get Motor State
            if (!CheckError(Versa_Motor_getLastState(rowHandle, &motorState, &motorError)))
            {
                return false;
            }

            // Check Motor Error
            if(motorError != Versa_MotorError.Versa_MotorErrorNone)
            {
                return false;
            }
                       
            // Compute Delta Positions
            double motorPosition = pFullSteps * (info.RowUnitsPerRev / info.RowStepsPerRev);
            double encoderPosition = pCounts * (info.RowUnitsPerRev / info.RowEncoderCountsPerRev);

            double deltaPosition = motorPosition - encoderPosition; // unused
            double deltaUnits = pMotorUnits - pEncoderUnits;

            if (info.SoftwarePositionCheck)
            {
                if (Math.Abs(deltaUnits) > info.ColumnPositionError)
                {
                    return false;
                }
            }
            else
            {
                return true;
            }

            // Return True
            return true;

        }

        public bool CheckColumnMotor()
        {
            // Variables
            double pFullSteps = 0;
            int pCounts = 0;
            double pMotorUnits = 0;
            double pEncoderUnits = 0;

            Versa_MotorState motorState;
            Versa_MotorError motorError;

            // Request Info from Motor
            if (!CheckError(Versa_Motor_requestInfo(columnHandle)))
            {
                return false;
            }

            // Get Motor Position
            if (!CheckError(Versa_Motor_getLastMotorPosition(columnHandle, &pMotorUnits, &pFullSteps)))
            {
                return false;
            }
            // Get Encoder Position
            if (!CheckError(Versa_Motor_getLastEncoderPosition(columnHandle, &pEncoderUnits, &pCounts)))
            {
                return false;
            }

            // Get Motor State
            if (!CheckError(Versa_Motor_getLastState(columnHandle, &motorState, &motorError)))
            {
                return false;
            }

            // Check Motor Error
            if (motorError != Versa_MotorError.Versa_MotorErrorNone)
            {
                return false;
            }

            // Compute Delta Positions
            double motorPosition = pFullSteps * (info.ColumnUnitsPerRev / info.ColumnStepsPerRev);
            double encoderPosition = pCounts * (info.ColumnUnitsPerRev / info.ColumnEncoderCountsPerRev);

            double deltaPosition = motorPosition - encoderPosition; // unused
            double deltaUnits = pMotorUnits - pEncoderUnits;

            if (info.SoftwarePositionCheck)
            {
                if (Math.Abs(deltaUnits) > info.ColumnPositionError)
                {
                    return false;
                }
            }
            else
            {
                return true;
            }

            // Return true
            return true;
        }
        

        // Spectrometer
        public bool InitialiseSpectrometer()
        {
            // Get the Handle
            linescanHandle = Versa_getPBusHandle();
            int pixel = info.NPixel;
            int postIntegrationWait = 88;
            int minimunCycles = pixel + 102;

            Versa_LineScan_configureSensor(linescanHandle, pixel, postIntegrationWait, minimunCycles);

            return true;
        }

        public bool SetIntegrationTime()
        {
            return CheckError(Versa_LineScan_setIntegrationTime(linescanHandle, Integration));

        }

        public bool DarkMeasurement()
        {
            // Perform Background Measurement
            for (int i = 0; i < 3; i++)
            {
                CheckError(Versa_LineScan_startMeasurement(linescanHandle));
                CheckError(Versa_LineScan_waitFor(linescanHandle, 3000));
            }                      

            // Initialise Data Structure
            Versa_LineScan_Result_t result;
            Versa_LineScan_Result_t_init(&result);

            // Retrieve Data
            int lineIndex = 0;
            CheckError(Versa_LineScan_getSingleResult(linescanHandle, lineIndex, &result));

            // Initialise array
            Background = new double[result.data[lineIndex].pixelCount];

            for (int j = 0; j < result.data[lineIndex].pixelCount; j++)
            {
                Background[j] = result.data[lineIndex].lineTrace[j] - Background[j];
            }

            // Free up variable for future usage
            Versa_LineScan_Result_t_free(&result);

            return true;

        }

        public bool LightMeasurement()
        {

            // Perform Measurement
            CheckError(Versa_LineScan_startMeasurement(linescanHandle));
            CheckError(Versa_LineScan_waitFor(linescanHandle, 3000));

            // Initialise Data Structure
            Versa_LineScan_Result_t result;
            Versa_LineScan_Result_t_init(&result);

            // Retrieve Data
            int lineIndex = 0; 
            CheckError(Versa_LineScan_getSingleResult(linescanHandle, lineIndex, &result));

            // Initialise array
            Waveform = new double[result.data[lineIndex].pixelCount];

            for (int j = 0; j < result.data[lineIndex].pixelCount; j++)
            {
                Waveform[j] = result.data[lineIndex].lineTrace[j] - Background[j];
            }
            
            // If array is backwards, reverse spectrum
            if (info.SpectrometerReverse)
            {
                Array.Reverse(Waveform);
            }

            // Free result for future usage
            Versa_LineScan_Result_t_free(&result);

            return true;
        }

 
        // Hardware Scan
        public bool SetScanHandles()
        {
            // Set scan handles
            CheckError(Versa_App_setScanLineScanHandle(linescanHandle));
            CheckError(Versa_App_setScanMotorDriverHandle(columnHandle));
            CheckError(Versa_App_setScanLEDDriverHandle(ledHandle));

            return true;
        }

        public bool SetScanParameters(double startUnits, double deltaUnits, double endUnits, int stops)
        {
            // Set the Parameters
            return CheckError(Versa_App_setScanParameters(startUnits, deltaUnits, endUnits, stops));

        }

        public bool StartScanMeasurement()
        {
            // Start scan measurement by either pulsing LED or Leaving On
            bool error;

            if (info.LEDPulse)
            {
                error = CheckError(Versa_App_startStopAndGoMeasurement(Versa_LEDControl.Versa_LED_HWControl));
            }
            else
            {
                error = CheckError(Versa_App_startStopAndGoMeasurement(Versa_LEDControl.Versa_LED_SWControl));
            }

            if(error == false)
            {
                return false;
            }

            return true;

        }

        public bool EndScanMeasurement()
        {
            return CheckError(Versa_App_waitFor(30000));
        }

        public bool GetScanResults(int col)
        {
            // Wait for Line (Camera Acquisition) to finish
            if(!CheckError(Versa_App_waitForLine(col, 3000)))
            {
                return false; 
            }
            
            // Initialise Data Structure
            Versa_LineScan_Result_t result;
            Versa_LineScan_Result_t_init(&result);

            // Get specific line of data
            if(!CheckError(Versa_LineScan_getSingleResult(linescanHandle, col, &result)))
            {
                return false;
            }

            // Initialise array to retrieve spectrum
            Waveform = new double[result.data[0].pixelCount];

            for (int j = 0; j < result.data[0].pixelCount; j++)
            {
                // Subtract background from the spectrum
                Waveform[j] = result.data[0].lineTrace[j] - Background[j];
            }

            // Reverse array if backwards
            if (info.SpectrometerReverse)
            {
                Array.Reverse(Waveform);
            }            

            // Free up variable for future usage
            Versa_LineScan_Result_t_free(&result);

            return true;

           
        }



        // Error Handling
        public bool CheckError(int value)
        {

            // True = No Error
            // False = Error

            // Variables
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
