using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;

namespace FI.PlateReader.Gen4.JETI
{
    unsafe public class Meerstetter
    {
        #region "Versa API"

        // RS485
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

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_RS485_configureChannel(int channel, Versa_BaudRate baud, Versa_Parity parity, Versa_StopBits stopBits, Versa_DataSize dataSize);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_RS485_enableChannel(int channel);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_RS485_disableChannel(int channel);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_RS485_writeData(int channel, byte* data, int size);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_RS485_readData(int channel, byte* data, int size, int timeout_ms);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_RS485_getReadBufferSize(int channel, int* size);

        [DllImport("VersaLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Versa_RS485_clearReadBuffer(int channel);

        #endregion

        Time time = new Time();                         // Clock

        #region "Registers"
        public const int DeviceType = 100;
        public const int HardwareVersion = 101;
        public const int SerialNumber = 102;
        public const int FirmwareVersion = 103;
        public const int DeviceStatus = 104;
        public const int ErrorNumber = 105;
        public const int ErrorInstance = 106;
        public const int ErrorParameter = 107;
        public const int FlashSaveOff = 108;
        public const int FlashStatus = 109;

        public const int ObjectTemperature = 1000;
        public const int SinkTemperature = 1001;
        public const int TargetTemperature = 1010;
        public const int RampNominalObjectTemperature = 1011;
        public const int ThermalPowerModelCurrent = 1012;

        public const int ActualOutputCurrent = 1020;
        public const int ActualOutputVoltage = 1021;

        public const int FanRelativeCoolingPower = 1100;
        public const int FanNominalFanSpeed = 1101;
        public const int FanActualFanSpeed = 1102;
        public const int FanActualPwmLevel = 1103;

        public const int PIDLowerLimitation = 1030;
        public const int PIDUpperLimitation = 1031;
        public const int PIDControlVariable = 1032;

        public const int ObjectSensorRawADCValue = 1040;
        public const int SinkSensorRawADCValue = 1041;
        public const int ObjectSensorResistance = 1042;
        public const int SinkSensorResitance = 1043;
        public const int SinkSensorTemperature = 1044;
        public const int ObjectSensorTemperature = 1045;
        public const int ObjectSensorType = 1046;

        public const int FirmwareVersionAlt = 1050;
        public const int FirmwareBuildNumber = 1051;
        public const int HardwareVersionAlt = 1052;
        public const int SerialNumberAlt = 1053;

        public const int DriverInputVoltage = 1060;
        public const int MedVInternalSupply = 1061;
        public const int InternalSupply33V = 1062;
        public const int BasePlateTemperature = 1063;

        public const int MaximumDeviceTemperature = 1110;
        public const int MaximumOutputCurrent = 1111;

        public const int ParallelActualOutputCurrent = 1090;

        public const int ErrorNumberAlt = 1070;
        public const int ErrorInstanceAlt = 1071;
        public const int ErrorParameterAlt = 1072;

        public const int DriverStatus = 1080;
        public const int ParameterSystemFlashStatus = 1081;

        public const int TemperatureIsStable = 1200;

        //Tab: Operation Parameters
        public const int OutputStageInputSelection = 2000;
        public const int OutputStageEnable = 2010;
        public const int SetStaticCurrent = 2020;
        public const int SetStaticVoltage = 2021;
        public const int CurrentLimitation = 2030;
        public const int VoltageLimitation = 2031;
        public const int CurrentErrorThreshold = 2032;
        public const int VoltageErrorThreshold = 2033;
        public const int GeneralOperatingMode = 2040;
        public const int DeviceAddress = 2051;
        public const int RS485CH1BaudRate = 2050;
        public const int RS485CH1ResponseDelay = 2052;
        public const int ComWatchDogTimeout = 2060;

        //Tab Temperature Control
        public const int TargetObjectTemp = 3000;
        public const int CoarseTempRamp = 3003;
        public const int ProximityWidth = 3002;
        public const int Kp = 3010;
        public const int Ti = 3011;
        public const int Td = 3012;
        public const int DPartDampPT1 = 3013;
        public const int ModelizationMode = 3020;

        public const int PeltierMaxCurrent = 3030;
        public const int PeltierMaxVoltage = 3031;
        public const int PeltierCoolingCapacity = 3032;
        public const int PeltierDeltaTemperature = 3033;
        public const int PeltierPositiveCurrentIs = 3034;
        public const int ResistorResistance = 3040;
        public const int ResistorMaxCurrent = 3041;

        public const int UpperBoundary = 3051;
        public const int LowerBoundary = 3050;

        //Tab Object Temperature
        public const int TemperatureOffset = 4001;
        public const int TemperatureGain = 4002;
        public const int LowerErrorThreshold = 4010;
        public const int UpperErrorThreshold = 4011;
        public const int MaxTempChange = 4012;
        public const int NTCLowerPointTemperature = 4020;
        public const int NTCLowerPointResistance = 4021;
        public const int NTCMiddlePointTemperature = 4022;
        public const int NTCMiddlePointResistance = 4023;
        public const int NTCUpperPointTemperature = 4024;
        public const int NTCUpperPointResistance = 4025;
        public const int StabilityTemperatureWindow = 4040;
        public const int StabilityMinTimeInWindow = 4041;
        public const int StabilityMaxStabiTime = 4042;
        public const int MeasLowestResistance = 4030;
        public const int MeasHighestResistance = 4031;
        public const int MeasTempAtLowestResistance = 4032;
        public const int MeasTempAtHighestResistance = 4033;
        public const int ObjectSensorTypeAlt = 4034;

        //Tab Sink Temperature
        public const int TemperatureOffsetAlt = 5001;
        public const int TemperatureGainAlt = 5002;
        public const int LowerErrorThresholdAlt = 5010;
        public const int UpperErrorThresholdAlt = 5011;
        public const int MaxTempChangeAlt = 5012;
        public const int NTCLowerPointTemperatureAlt = 5020;
        public const int NTCLowerPointResistanceAlt = 5021;
        public const int NTCMiddlePointTemperatureAlt = 5022;
        public const int NTCMiddlePointResistanceAlt = 5023;
        public const int NTCUpperPointTemperatureAlt = 5024;
        public const int NTCUpperPointResistanceAlt = 5025;
        public const int SinkTemperatureSelection = 5030;
        public const int FixedTemperature = 5031;
        public const int UpperADCLimitError = 5032;
        public const int MeasLowestResistanceAlt = 5040;
        public const int MeasHighestResistanceAlt = 5041;
        public const int MeasTempAtLowestResistanceAlt = 5042;
        public const int MeasTempAtHighestResistanceAlt = 5043;

        //Tab Expert: Sub Tab Temperature Measurement
        public const int ObjMeasPGAGain = 6000;
        public const int ObjMeasCurrentSource = 6001;
        public const int ObjMeasADCRs = 6002;
        public const int ObjMeasADCCalibOffset = 6003;
        public const int ObjMeasADCCalibGain = 6004;
        public const int ObjMeasSensorTypeSelection = 6005;
        public const int SinMeasADCRp = 6006;
        public const int SinMeasADCRv = 6010;
        public const int SinMeasADCVps = 6013;
        public const int SinMeasADCCalibOffset = 6011;
        public const int SinMeasADCCalibGain = 6012;

        //Tab Expert: Sub Tab Display
        public const int DisplayType = 6020;
        public const int DisplayLineDefText = 6021;
        public const int DisplayLineAltText = 6022;
        public const int DisplayLineAltMode = 6023;

        //Tab Expert: Sub Tab PBC
        public const int PbcFunction = 6100;
        public const int ChangeButtonLowTemperature = 6110;
        public const int ChangeButtonHighTemperature = 6111;
        public const int ChangeButtonStepSize = 6112;

        //Tab Expert: Sub Tab FAN
        public const int FanControlEnable = 6200;
        public const int FanActualTempSource = 6210;
        public const int FanTargetTemp = 6211;
        public const int FanTempKp = 6212;
        public const int FanTempTi = 6213;
        public const int FanTempTd = 6214;
        public const int FanSpeedMin = 6220;
        public const int FanSpeedMax = 6221;
        public const int FanSpeedKp = 6222;
        public const int FanSpeedTi = 6223;
        public const int FanSpeedTd = 6224;
        public const int FanSpeedBypass = 6225;
        public const int PwmFrequency = 6230;

        //Tab Expert: Sub Tab Misc
        public const int MiscActObjectTempSource = 6300;
        public const int MiscDelayTillReset = 6310;
        public const int MiscError108Delay = 6320;

        //Other Parameters (Not directly displayed in the Service Software)
        public const int LiveEnable = 50000;
        public const int LiveSetCurrent = 50001;
        public const int LiveSetVoltage = 50002;
        public const int SineRampStartPoint = 50010;
        public const int ObjectTargetTempSourceSelection = 50011;
        public const int ObjectTargetTemperature = 50012;
        public const int AtmAutoTuningStart = 51000;
        public const int AtmAutoTuningCancel = 51001;
        public const int AtmThermalModelSpeed = 51002;
        public const int AtmTuningParameter2A = 51010;
        public const int AtmTuningParameter2D = 51011;
        public const int AtmTuningParameterKu = 51012;
        public const int AtmTuningParameterTu = 51013;
        public const int AtmPIDParameterKp = 51014;
        public const int AtmPIDParameterTi = 51015;
        public const int AtmPIDParameterTd = 51016;
        public const int AtmSlowPIParameterKp = 51022;
        public const int AtmSlowPIParameterTi = 51023;
        public const int AtmPIDDPartDamping = 51024;
        public const int AtmCoarseTempRamp = 51017;
        public const int AtmProximityWidth = 51018;
        public const int AtmTuningStatus = 51020;
        public const int AtmTuningProgress = 51021;
        public const int LutTableStart = 52000;
        public const int LutTableStop = 52001;
        public const int LutTableStatus = 52002;
        public const int LutCurrentTableLine = 52003;
        public const int LutTableIDSelection = 52010;
        public const int LutNrOfRepetitions = 52012;
        public const int PbcEnableFunction = 52100;
        public const int PbcSetOutputToPushPull = 52101;
        public const int PbcSetOutputStates = 52102;
        public const int PbcReadInputStates = 52103;
        public const int ExternalActualObjectTemperature = 52200;

        #endregion

        #region "Variables"
        public double ObjectTemp;
        public double HeatsinkTemp;
        public double TargetTemp;
        public double RampRate;

        public double OutputPower;
        public double OutputCurrent;
        public double OutputVoltage;

        public bool Enabled;

        public static double MinTemp;
        public static double MaxTemp;
        public bool Stable;

        public static int ConnectType = 0;          // ConnectType = 0 (serial), ConnectType = 1 (RS485)
        public static int Comm;                     // Laser control comm port.
        public bool Connected;                      // Flag indicating laser control is connected.
        public static int ComSequence;
        public static int Address;
        public static int Instance;

        // Global Variables
        public static string COMname;
        private double m_timeout;               // Timeout waiting for command. Everything returns something immediately except reset
        private int retry;

        public SerialPort serialPort1;          // Port. Could replace all com names, timeout?
        public string receivetxt;               // Received text. 
        public string sendtxt;                  // Command text.
        public static bool blnSend;             // Send data flag. If false commands don't get sent.
        public static bool blnReceive;          // Receive data flag. Returns true if data was received.

        #endregion

        #region "Basic Communication"

        // Generic connect function
        public bool Connect()
        {
            if (ConnectType == 0)
            {
                return connectSerialPort();
            }
            else
            {
                return connectVersaRS485();
            }
        }
        // Function to connect to the device on Versa RS485 channel 1
        public bool connectVersaRS485()
        {
            bool bConnect = false;
            Connected = false;

            Instance = 1;
            ComSequence = 1;

            try
            {
                Versa_RS485_enableChannel(1);

                // Test port for connection.
                bConnect = SetTargetTemp(20);
                Connected = bConnect;

                Versa_RS485_disableChannel(1);
            }
            catch
            {
                Connected = false;
            }
               
            return bConnect;
        }

        // Function to connect to device over COM port, searches through all available
        public bool connectSerialPort()
        {
            string[] ArrayComPortsNames = null;
            int index = -1;
            string ComPortName = null;
            bool bConnect = false;
            Connected = false;

            Instance = 1;
            ComSequence = 1;
            System.ComponentModel.IContainer components = new System.ComponentModel.Container();
            serialPort1 = new System.IO.Ports.SerialPort(components);

            m_timeout = 0.1;    // 0.1 seconds
            retry = 3;

            ArrayComPortsNames = SerialPort.GetPortNames();
            if (ArrayComPortsNames.Length != 0)
            {
                // what does this do?
                do
                {
                    index += 1;
                }
                while (!((ArrayComPortsNames[index] == ComPortName) ||
                                    (index == ArrayComPortsNames.GetUpperBound(0))));

                serialPort1.BaudRate = 57600;   // 9600;
                serialPort1.Parity = Parity.None; //SetPortParity(_serialPort.Parity);
                serialPort1.DataBits = 8;   // SetPortDataBits(_serialPort.DataBits);
                serialPort1.StopBits = StopBits.One;    // SetPortStopBits(_serialPort.StopBits);
                serialPort1.Handshake = Handshake.None; // SetPortHandshake(_serialPort.Handshake);

                // Set the read/write timeouts
                serialPort1.ReadTimeout = 2000;         // time in milliseconds waiting for read to finish
                serialPort1.WriteTimeout = 2000;        // time in milliseconds waiting for write to finish


                for (int j = 0; j < ArrayComPortsNames.Length; j++)
                {
                    // Attach a method to be called when there
                    // is data waiting in the port's buffer
                    serialPort1.DataReceived += new
                        SerialDataReceivedEventHandler(port_DataReceived);

                    serialPort1.PortName = ArrayComPortsNames[j];

                    try
                    {
                        serialPort1.Open();

                        // Test port for connection.
                        bConnect = SetTargetTemp(20);
                        
                        // If setting temp returns true, connect successful
                        if (bConnect)
                        {
                            Connected = true;
                            COMname = ArrayComPortsNames[j];

                            // exit for loop.
                            break;
                        }

                        serialPort1.Close();
                    }
                    catch
                    {

                    }
                }

                // change baud rate to 
                // Set timeout to 100s.
                retry = 3;
                m_timeout = 0.1;
            }
            return Connected;
        }


        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // Show all the incoming data in the port's buffer
            SerialPort sp = (SerialPort)sender;
            if (sp.IsOpen) { receivetxt += sp.ReadExisting(); }
        }

        // Function to disconnect from the device based on hUSBDevice
        public bool disconnectSerialPort()
        {
            serialPort1.Close();
            return true;
        }

        // Function to talk with Meerstetter over RS485
        public bool SendCommandRS485(string cmdStr, out string replyStr)
        {
            // Define byte array for sending cmd and getting reply response
            bool status;    // was command succesful

            string sTemp;
            string sAddress;
            string sAddressC;
            string sSequence;
            string sSequenceC;

            string sSend;
            int iCheckSum;
            string sCheckSum;
            string sCheckSumC;
            string sCheck;
            int iStart;
            int iEnd;
            string sValue;

            // Convert address to string and pad with '0' if necessary
            sTemp = Convert.ToString(Address, 16);
            if (sTemp.Length < 2) { sAddress = "0" + sTemp; }
            else { sAddress = sTemp; }
            sAddressC = sAddress.ToUpper();

            // Convert sequence to string and pad with '0' if necessary
            sTemp = Convert.ToString(1, 16);
            if (sTemp.Length == 1) { sSequence = "000" + sTemp; }
            else if (sTemp.Length == 2) { sSequence = "00" + sTemp; }
            else if (sTemp.Length == 1) { sSequence = "0" + sTemp; }
            else { sSequence = sTemp.Substring(sTemp.Length - 4, 4); }
            sSequenceC = sSequence.ToUpper();

            // Construct string and compute checksum
            sSend = "#" + sAddressC + sSequenceC + cmdStr;
            iCheckSum = CRC16A(sSend);

            // Convert checksum to string and pad with '0' if necessary
            sTemp = Convert.ToString(iCheckSum, 16);
            if (sTemp.Length == 1) { sCheckSum = "000" + sTemp; }
            else if (sTemp.Length == 2) { sCheckSum = "00" + sTemp; }
            else if (sTemp.Length == 3) { sCheckSum = "0" + sTemp; }
            else { sCheckSum = sTemp.Substring(sTemp.Length - 4, 4); }
            sCheckSumC = sCheckSum.ToUpper();

            // Add checksum to output string
            sSend = sSend + sCheckSumC;
            m_timeout = 5;
            try
            {
                status = false;
                receivetxt = "";    // clear global receive text
                replyStr = "";      // clear global replybtring

                Versa_RS485_enableChannel(1);

                string msg = sSend + "\r";
                byte[] dData = Encoding.ASCII.GetBytes(msg);
                int dLength = dData.Length;

                unsafe
                {
                    fixed (byte* dPointer = &dData[0])
                    {
                        int chk = Versa_RS485_writeData(1, dPointer, dLength);
                    };
                }
                time.Delay(10);

                int oLength = 0;
                unsafe
                {
                   Versa_RS485_getReadBufferSize(1, &oLength);
                }
                time.Delay(10);

                if (oLength > 0)
                {
                    byte[] uData = new byte[oLength];

                    unsafe
                    {
                        fixed (byte* dPointer = &uData[0])
                        {
                            int chk = Versa_RS485_readData(1, dPointer, oLength, 1000);
                        };
                    }
                    string rcv = System.Text.Encoding.UTF8.GetString(uData, 0, oLength);
                    status = true;

                    replyStr = rcv;
                    replyStr = replyStr.Trim('\r', '\n');   // remove carriage return or new line

                    // check for error
                    sCheck = "!" + sAddressC + sSequenceC;

                    bool check = replyStr.StartsWith(sCheck);

                    if (check)
                    {
                        iStart = sCheck.Length;
                        iEnd = replyStr.Length - 4;

                        if (iStart < iEnd)
                        {
                            replyStr = replyStr.Substring(iStart, iEnd - iStart);
                        }
                        else
                        {
                            sValue = replyStr.Substring(iStart, replyStr.Length - iStart);

                            bool chksum = replyStr.Contains(sCheckSumC);

                            if (chksum)
                            {
                                replyStr = "ok";
                            }
                            else
                            {
                                replyStr = "error";
                            }
                        }
                    }
                }

                Versa_RS485_disableChannel(1);
                
                return status;
            }
            catch
            {
                replyStr = "Unhandled Command Error Caught!";
                return false;
            }
        }
        // Function to talk with Meerstetter over Serial Port
        public bool SendCommandSerial(string cmdStr, out string replyStr)
        {
            // Define byte array for sending cmd and getting reply response
            bool status;    // was command succesful
            bool timeOut;   // timeout waiting for reply

            string sTemp;
            string sAddress;
            string sAddressC;
            string sSequence;
            string sSequenceC;

            string sSend;
            int iCheckSum;
            string sCheckSum;
            string sCheckSumC;
            string sCheck;
            int iStart;
            int iEnd;
            string sValue;

            // Convert address to string and pad with '0' if necessary
            sTemp = Convert.ToString(Address, 16);
            if (sTemp.Length < 2) { sAddress = "0" + sTemp; }
            else { sAddress = sTemp; }
            sAddressC = sAddress.ToUpper();

            // Convert sequence to string and pad with '0' if necessary
            sTemp = Convert.ToString(1, 16);
            if (sTemp.Length == 1) { sSequence = "000" + sTemp; }
            else if (sTemp.Length == 2) { sSequence = "00" + sTemp; }
            else if (sTemp.Length == 1) { sSequence = "0" + sTemp; }
            else { sSequence = sTemp.Substring(sTemp.Length - 4, 4); }
            sSequenceC = sSequence.ToUpper();

            // Construct string and compute checksum
            sSend = "#" + sAddressC + sSequenceC + cmdStr;
            iCheckSum = CRC16A(sSend);

            // Convert checksum to string and pad with '0' if necessary
            sTemp = Convert.ToString(iCheckSum, 16);
            if (sTemp.Length == 1) { sCheckSum = "000" + sTemp; }
            else if (sTemp.Length == 2) { sCheckSum = "00" + sTemp; }
            else if (sTemp.Length == 3) { sCheckSum = "0" + sTemp; }
            else { sCheckSum = sTemp.Substring(sTemp.Length - 4, 4); }
            sCheckSumC = sCheckSum.ToUpper();

            // Add checksum to output string
            sSend = sSend + sCheckSumC;
            m_timeout = 5;
            try
            {
                status = false;
                receivetxt = "";    // clear global receive text
                replyStr = "";      // clear global replybtring
                if (serialPort1.IsOpen)
                {
                    sendtxt = cmdStr;   // set global send string to command
                    blnSend = true;     // send flag set
                    timeOut = false;    // timeout flag cleared

                    serialPort1.Write(sSend + "\r");       // send command with carriage return
                    time.Delay(20);
                    //WaitSeconds(0.02);

                    DateTime Tthen = DateTime.Now;

                    // wait for carriage return or timeout
                    while ((receivetxt.EndsWith("\r") == false) & (timeOut == false))
                    {
                        // if no return is received in defined timeout window, exit.
                        if (Tthen.AddSeconds(m_timeout) < DateTime.Now)
                        {
                            timeOut = true;
                        }
                    }
                    if (timeOut)
                    {
                        status = false;
                        blnReceive = false;
                        replyStr = "timeout";
                    }
                    else
                    {
                        status = true;
                        blnReceive = true;
                        // get receive string.
                        replyStr = receivetxt;
                        replyStr = replyStr.Trim('\r', '\n');   // remove carriage return or new line

                        // check for error
                        sCheck = "!" + sAddressC + sSequenceC;                        
                        bool check = replyStr.StartsWith(sCheck);

                        if (check)
                        {
                            iStart = sCheck.Length;
                            iEnd = replyStr.Length - 4;

                            // Look for checksum if value sent, otherwise get ok
                            if (iStart < iEnd)
                            {
                                replyStr = replyStr.Substring(iStart, iEnd - iStart);
                            }
                            else
                            {
                                sValue = replyStr.Substring(iStart, replyStr.Length - iStart);

                                bool chksum = replyStr.Contains(sCheckSumC);

                                if (chksum)
                                {
                                    replyStr = "ok";
                                }
                                else
                                {
                                    replyStr = "error";
                                }
                            }
                        }
                    }
                }
                else
                {
                    status = false;
                    replyStr = "closed";
                }

                return status;
            }
            catch
            {
                replyStr = "Unhandled Command Error Caught!";
                return false;
            }
        }
        // Check for ok in replay string
        public int CheckStringOK(string strReply)
        {
            // Gets an OK
            // return: 0 for OK, error code otherwise
            string strok = "ok";
            int value;
            bool result = strReply.Equals(strok, StringComparison.Ordinal);

            // check value?
            if (result == false)
            {
                value = -1;
            }
            else
            {
                value = 0;
            }
            return value;
        }

        #endregion

        // Set target temperature of main channel: thermal block
        public bool SetTargetTemp(float target)
        {
            bool success = true;
            float check;

            try
            {
                SetFloatValue(TargetObjectTemp, target);

                // check to see that target temp was set.
                check = GetFloatValue(TargetObjectTemp);

                if (check == target)
                {
                    success = true;
                    TargetTemp = target;
                }
                else
                {
                    success = false;
                }
            }
            catch
            {
                success = false;
            }
            return success;
        }

        // Set temperature ramp rate in degrees/min
        public bool SetRampRate(float rate)
        {
            bool success = true;
            float check;
            float setting = rate / 60;
            try
            {
                SetFloatValue(CoarseTempRamp, setting);

                // check to see that target temp was set.
                check = GetFloatValue(CoarseTempRamp);

                if (check == setting)
                {
                    success = true;
                    RampRate = rate;
                }
                else
                {
                    success = false;
                }
            }
            catch
            {
                success = false;
            }
            return success;
        }

        // Get object sensor temperature
        public float GetObjectTemp()
        {
            float check = float.NaN;

            try
            {
                check = GetFloatValue(ObjectTemperature);
                ObjectTemp = check;
            }
            catch
            {
                check = float.NaN;
            }
            return check;
        }

        // Get heatsink sensor temperature
        public float GetHeatsinkTemp()
        {
            float check = float.NaN;

            try
            {
                check = GetFloatValue(SinkTemperature);
                HeatsinkTemp = check;
            }
            catch
            {
                check = float.NaN;
            }
            return check;
        }

        // Get current output
        public float GetCurrentOutput()
        {
            float check = float.NaN;

            try
            {
                check = GetFloatValue(ActualOutputCurrent);
                OutputCurrent = check;
            }
            catch
            {
                check = float.NaN;
            }
            return check;
        }

        // Get voltage output
        public float GetVoltageOutput()
        {
            float check = float.NaN;

            try
            {
                check = GetFloatValue(ActualOutputVoltage);
                OutputVoltage = check;
            }
            catch
            {
                check = float.NaN;
            }
            return check;
        }

        // Set output status
        public bool SetOutputStatus(bool enable)
        {
            bool blnOut = false;

            try
            {
                int value;
                if (enable) { value = 1; }
                else { value = 0; }
                blnOut = SetINT32Value(OutputStageEnable, value);
                if (blnOut) { Enabled = enable; }
            }
            catch
            {
                blnOut = false;
            }
            return blnOut;
        }

        // Get output status
        public bool GetOutputStatus()
        {
            int check = 0;
            bool blnOut = false;


            try
            {
                check = GetINT32Value(OutputStageEnable);
                switch (check)
                {
                    case 0:
                        Enabled = false;
                        break;
                    case 1:
                        Enabled = true;
                        break;
                    default:
                        Enabled = false;
                        break;
                }
                blnOut = Enabled;
            }
            catch
            {
                blnOut = false;
                check = -1;
            }
            return blnOut;
        }

        // Get temperature stable status
        public bool GetStableStatus(int value)
        {
            int check = 0;
            bool blnOut = false;


            try
            {
                check = GetINT32Value(TemperatureIsStable);
                switch (check)
                {
                    case 0:
                        Stable = false;
                        break;
                    case 1:
                        Stable = false;
                        break;
                    case 2:
                        Stable = true;
                        break;
                }
                blnOut = Stable;
            }
            catch
            {
                blnOut = false;
                check = -1;
            }
            return blnOut;
        }

        // Get integer value from register (enable, stable, etc.)
        public int GetINT32Value(int param)
        {
            string cmdStr;
            string sParam;
            string sTemp;
            string sInstance;
            int value = 0;
            string replyStr;

            // Convert parameter to string, pad with '0' 
            sTemp = Convert.ToString(param, 16);
            if (sTemp.Length == 1) { sParam = "000" + sTemp; }
            else if (sTemp.Length == 2) { sParam = "00" + sTemp; }
            else if (sTemp.Length == 3) { sParam = "0" + sTemp; }
            else { sParam = sTemp.Substring(sTemp.Length - 4, 4); }

            // Convert instance to string, pad with '0' 
            Instance = 1;
            sTemp = Convert.ToString(Instance, 16);
            if (sTemp.Length == 1) { sInstance = "0" + sTemp; }
            else { sInstance = sTemp; }
            sInstance = sInstance.ToUpper();

            // Construct command 
            cmdStr = "?VR" + sParam + sInstance;
            cmdStr = cmdStr.ToUpper();

            // Send command either with Serial port or RS485
            if (ConnectType == 0)
            {
                bool snd = SendCommandSerial(cmdStr, out replyStr);
            }
            else
            {
                bool snd = SendCommandRS485(cmdStr, out replyStr);
            }
            
            // Convert to integer value
            byte[] bytes = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                string chr = replyStr.Substring(i * 2, 2);
                bytes[3 - i] = Convert.ToByte(chr, 16);
            }
            value = BitConverter.ToInt32(bytes, 0);
            return value;
        }

        // Set integer values (enable, stable, etc.)
        public bool SetINT32Value(int param, int value)
        {
            string cmdStr;
            string replyStr;
            string sParam;
            string sTemp;
            string sInstance;
            string sTarget;
            bool snd;

            // Convert param to string and pad with '0'
            sTemp = Convert.ToString(param, 16);
            sTemp = sTemp.ToUpper();
            if (sTemp.Length == 1) { sParam = "000" + sTemp; }
            else if (sTemp.Length == 2) { sParam = "00" + sTemp; }
            else if (sTemp.Length == 3) { sParam = "0" + sTemp; }
            else { sParam = sTemp.Substring(sTemp.Length - 4, 4); }
            sParam = sParam.ToUpper();

            // Convert instance to string and pad with '0'
            Instance = 1;
            sTemp = Convert.ToString(Instance, 16);
            if (sTemp.Length == 1) { sInstance = "0" + sTemp; }
            else { sInstance = sTemp; }
            sInstance = sInstance.ToUpper();

            // Convert value to string
            byte[] bArray = BitConverter.GetBytes(value);
            sTarget = "";
            for (int i = 0; i < 4; i++)
            {
                byte[] bTemp = new byte[1] { bArray[3 - i] };
                string sByte = BitConverter.ToString(bTemp);
                if (sByte.Length < 2)
                {
                    sTarget = sTarget + "0" + sByte;
                }
                else
                {
                    sTarget = sTarget + sByte;
                }
            }

            // Construct command string
            cmdStr = "VS" + sParam + sInstance + sTarget;
            cmdStr = cmdStr.ToUpper();

            if (ConnectType == 0)
            {
                snd = SendCommandSerial(cmdStr, out replyStr);
            }
            else
            {
                snd = SendCommandRS485(cmdStr, out replyStr);
            }
            
            return snd;
        }

        // Get float value from register (temps, current, voltage, etc.)
        public float GetFloatValue(int param)
        {
            string cmdStr;
            string sParam;
            string sTemp;
            string sInstance;
            float value = 0;
            string replyStr;
            bool snd;

            // Convert param to string and pad with '0'
            sTemp = Convert.ToString(param, 16);
            if (sTemp.Length == 1) { sParam = "000" + sTemp; }
            else if (sTemp.Length == 2) { sParam = "00" + sTemp; }
            else if (sTemp.Length == 3) { sParam = "0" + sTemp; }
            else { sParam = sTemp.Substring(sTemp.Length - 4, 4); }

            // Convert instance to string and pad with '0'
            Instance = 1;
            sTemp = Convert.ToString(Instance, 16);
            if (sTemp.Length == 1) { sInstance = "0" + sTemp; }
            else { sInstance = sTemp; }
            sInstance = sInstance.ToUpper();

            // Construct command string
            cmdStr = "?VR" + sParam + sInstance;
            cmdStr = cmdStr.ToUpper();

            if (ConnectType == 0)
            {
                snd = SendCommandSerial(cmdStr, out replyStr);
            }
            else
            {
                snd = SendCommandRS485(cmdStr, out replyStr);
            }
            
            // Convert to float value
            byte[] bytes = new byte[4];            
            for (int i=0; i< 4; i++)
            {
                string chr = replyStr.Substring(i*2, 2);
                bytes[3 - i] = Convert.ToByte(chr, 16);
            }
            value = BitConverter.ToSingle(bytes, 0);
            return value;
        }

        // Set float value (temps, current, voltage, etc.)
        public bool SetFloatValue(int param, float value)
        {
            string cmdStr;
            string replyStr;
            string sParam;
            string sTemp;
            string sInstance;
            string sTarget;
            bool snd;

            // Convert param to string and pad with '0'
            sTemp = Convert.ToString(param, 16);
            sTemp = sTemp.ToUpper();
            if (sTemp.Length == 1) { sParam = "000" + sTemp; }
            else if (sTemp.Length == 2) { sParam = "00" + sTemp; }
            else if (sTemp.Length == 3) { sParam = "0" + sTemp; }
            else { sParam = sTemp.Substring(sTemp.Length - 4, 4); }
            sParam = sParam.ToUpper();

            // Convert instance to string and pad with '0'
            Instance = 1;
            sTemp = Convert.ToString(Instance, 16);
            if (sTemp.Length == 1) { sInstance = "0" + sTemp; }
            else { sInstance = sTemp; }
            sInstance = sInstance.ToUpper();

            // Convert float value to string
            byte[] bArray = BitConverter.GetBytes(value);
            sTarget = "";
            for (int i = 0; i < 4; i++)
            {
                byte[] bTemp = new byte[1] { bArray[3 - i] };
                string sByte = BitConverter.ToString(bTemp);
                if (sByte.Length < 2)
                {
                    sTarget = sTarget + "0" + sByte;
                }
                else
                {
                    sTarget = sTarget + sByte;
                }
            }

            // Construct command string
            cmdStr = "VS" + sParam + sInstance + sTarget;
            cmdStr = cmdStr.ToUpper();

            if (ConnectType == 0)
            {
                snd = SendCommandSerial(cmdStr, out replyStr);
            }
            else
            {
                snd = SendCommandRS485(cmdStr, out replyStr);
            }
            
            return snd;
        }

        // Checksum calculator
        public int CRC16A(string sInput)
        {
            int iLoop;
            int iCheckSum;
            int iAsc;
            int iTemp;
            int iLength;
            int j;

            iLength = sInput.Length;
            iCheckSum = 0;

            char[] cArray = sInput.ToCharArray(0, sInput.Length); // Converts string into char array.

            for (iLoop = 0; iLoop < iLength; iLoop++)
            {
                char c = cArray[iLoop];

                iAsc = Convert.ToInt32(c);
                iTemp = iAsc * 256;

                iCheckSum = iCheckSum ^ iTemp;

                for (j = 0; j < 8; j++)
                {
                    if ((iCheckSum & 32768) != 0)
                    {
                        iCheckSum = ((iCheckSum * 2) ^ 4129) & 65535;
                    }
                    else
                    {
                        iCheckSum = (iCheckSum* 2) & 65535;
                    }
                }
            }
            iCheckSum = iCheckSum & 65535;

            return iCheckSum;
        }


    }
}
