
#ifndef _VERSALIB_H
#define _VERSALIB_H

#define ERROR_NONE							0

#define ERROR_NOT_INITIALISED               1
#define ERROR_ALREADY_INITIALISED           2
#define ERROR_NO_DEVICE_FOUND               3
#define ERROR_CONNECTING_TO_DEVICE	        4
#define ERROR_SCAN_PERIPHERALS              5
#define ERROR_REQUEST_PERIPHERAL_INFO       6
#define ERROR_UPLOADING_USER_DATA           7
#define ERROR_DOWNLOADING_USER_DATA         8
#define ERROR_INVALID_MOTOR_HANDLE          9
#define ERROR_INVALID_LED_HANDLE            10
#define ERROR_INVALID_LINESCAN_HANDLE       11


#define ERROR_MOTOR_CURRENT                 100
#define ERROR_MOTOR_SPEED                   101
#define ERROR_MOTOR_ACCELERATION            102
#define ERROR_MOTOR_INVALID_LIMIT_PULL      103
#define ERROR_MOTOR_INVALID_LIMIT_TRIGGER   104
#define ERROR_MOTOR_COMMIT_SETTINGS         105
#define ERROR_MOTOR_ZERO_COUNTERS           106
#define ERROR_MOTOR_TURN_ON                 107
#define ERROR_MOTOR_TURN_OFF                108
#define ERROR_MOTOR_MOVE_RELATIVE           109
#define ERROR_MOTOR_MOVE_ABSOLUTE           110
#define ERROR_MOTOR_MOVE_HOME               111
#define ERROR_MOTOR_STOP                    112
#define ERROR_MOTOR_TIMEOUT                 113

#define ERROR_LED_TURN_ON                   200
#define ERROR_LED_TURN_OFF                  201
#define ERROR_LED_CURRENT                   202
#define ERROR_LED_CHANNEL                   203
#define ERROR_LED_COMMIT_SETTINGS           204

#define ERROR_LINESCAN_INVALID_INTEGRATION  300
#define ERROR_LINESCAN_UPLOAD_SETTINGS      301
#define ERROR_LINESCAN_START                302
#define ERROR_LINESCAN_STOP                 303
#define ERROR_LINESCAN_NO_RESULT            304
#define ERROR_LINESCAN_TIMEOUT              305
#define ERROR_LINESCAN_OFFSET_CALIBRATION   306

#define ERROR_APP_INVALID_DELTA_STEPS       400
#define ERROR_APP_INVALID_STOPS             401
#define ERROR_APP_START_STOP_AND_GO         402
#define ERROR_APP_STOP                      403
#define ERROR_APP_TIMEOUT                   404
#define ERROR_APP_NO_LINE_RESULT            405
#define ERROR_APP_RUN_ERROR                 406


#ifdef _WIN32
    #ifdef _BUILD_DLL
        #define VERSADECLSPEC __declspec(dllexport)
        #define VERSACDECL __cdecl
    #else
        #define VERSADECLSPEC __declspec(dllimport)
        #define VERSACDECL __cdecl
    #endif
#else
    #define VERSADECLSPEC
    #define VERSACDECL
#endif

enum Versa_DeviceType         { Versa_UnknownDevice, Versa_InterfaceBoard_35_1 };

enum Versa_MotorState         { Versa_MotorUV, Versa_MotorOff, Versa_MotorHold, Versa_MotorMoving };

enum Versa_PinPull            { Versa_Pin_HiZ, Versa_Pin_PullDown, Versa_Pin_PullUp };

enum Versa_PinTriggerEdge     { Versa_Pin_FallingEdge, Versa_Pin_RisingEdge };

enum Versa_LEDControl         { Versa_LED_HWControl, Versa_LED_SWControl };

enum Versa_MotorMicroStepping { Versa_MotorMicroStep_none, 
                                Versa_MotorMicroStep_x2, 
                                Versa_MotorMicroStep_x4,
                                Versa_MotorMicroStep_x8,
                                Versa_MotorMicroStep_x16,
                                Versa_MotorMicroStep_x32,
                                Versa_MotorMicroStep_x64 };

typedef int Versa_Handle_t;

struct Versa_LineScan_LineData_t
{
    int lineIndex;
    int pixelCount;
    double* lineTrace;
};

struct Versa_LineScan_Result_t
{
    int lineCount;
    Versa_LineScan_LineData_t* data;
};

struct Versa_UserData_t
{
    char data[32];
};

#ifdef __cplusplus
extern "C" {
#endif

VERSADECLSPEC void VERSACDECL Versa_getVersion(int* majorVersion, int* minorVersion);

VERSADECLSPEC int VERSACDECL  Versa_getAvailableDevices(int* deviceCount, Versa_DeviceType* deviceTypes);

VERSADECLSPEC int VERSACDECL  Versa_initialiseSession();
VERSADECLSPEC int VERSACDECL  Versa_closeSession();
VERSADECLSPEC void VERSACDECL Versa_resetAllPorts();

VERSADECLSPEC Versa_Handle_t VERSACDECL Versa_getSBusHandle(int sbusNumber);
VERSADECLSPEC Versa_Handle_t VERSACDECL Versa_getPBusHandle();

VERSADECLSPEC int VERSACDECL  Versa_uploadUserData(const Versa_UserData_t* pData);
VERSADECLSPEC int VERSACDECL  Versa_downloadUserData(Versa_UserData_t* pData);

VERSADECLSPEC int VERSACDECL  Versa_Motor_setCurrent(Versa_Handle_t handle, int mA);
VERSADECLSPEC int VERSACDECL  Versa_Motor_setReducedHoldCurrentEnabled(Versa_Handle_t handle, bool enable);
VERSADECLSPEC int VERSACDECL  Versa_Motor_setDirectionReversed(Versa_Handle_t handle, bool reversed);
VERSADECLSPEC int VERSACDECL  Versa_Motor_setEncoderReversed(Versa_Handle_t handle, bool reversed);
VERSADECLSPEC int VERSACDECL  Versa_Motor_setSpeed(Versa_Handle_t handle, int fullStepsPerSecond);
VERSADECLSPEC int VERSACDECL  Versa_Motor_setAcceleration(Versa_Handle_t handle, int fullStepsPerSecond2);
VERSADECLSPEC int VERSACDECL  Versa_Motor_setMicroStepping(Versa_Handle_t handle, Versa_MotorMicroStepping microStepping);
VERSADECLSPEC int VERSACDECL  Versa_Motor_setStartLimit(Versa_Handle_t handle, Versa_PinPull pinPull, Versa_PinTriggerEdge trigger);
VERSADECLSPEC int VERSACDECL  Versa_Motor_commitSettings(Versa_Handle_t handle);
VERSADECLSPEC int VERSACDECL  Versa_Motor_zeroPositionCounters(Versa_Handle_t handle);
VERSADECLSPEC int VERSACDECL  Versa_Motor_turnOn(Versa_Handle_t handle);
VERSADECLSPEC int VERSACDECL  Versa_Motor_turnOff(Versa_Handle_t handle);
VERSADECLSPEC int VERSACDECL  Versa_Motor_moveRelative(Versa_Handle_t handle, int microSteps);
VERSADECLSPEC int VERSACDECL  Versa_Motor_moveAbsolute(Versa_Handle_t handle,int microSteps);
VERSADECLSPEC int VERSACDECL  Versa_Motor_moveHome(Versa_Handle_t handle);
VERSADECLSPEC int VERSACDECL  Versa_Motor_stop(Versa_Handle_t handle);
VERSADECLSPEC int VERSACDECL  Versa_Motor_requestInfo(Versa_Handle_t handle);
VERSADECLSPEC int VERSACDECL  Versa_Motor_getLastSupplyVoltage(Versa_Handle_t handle, double* voltage_V, bool* isSupplyValid);
VERSADECLSPEC int VERSACDECL  Versa_Motor_getLastState(Versa_Handle_t handle, Versa_MotorState* pState);
VERSADECLSPEC int VERSACDECL  Versa_Motor_getLastMotorPosition(Versa_Handle_t handle, double* pFullSteps);
VERSADECLSPEC int VERSACDECL  Versa_Motor_getLastEncoderPosition(Versa_Handle_t handle, int* pCounts);
VERSADECLSPEC bool VERSACDECL Versa_Motor_isBusy(Versa_Handle_t handle);
VERSADECLSPEC int VERSACDECL  Versa_Motor_waitFor(Versa_Handle_t handle, int timeout_ms);
VERSADECLSPEC int VERSACDECL  Versa_Motor_waitForAll(int timeout_ms);

VERSADECLSPEC int VERSACDECL  Versa_LED_setCurrent(Versa_Handle_t handle, int mA);
VERSADECLSPEC int VERSACDECL  Versa_LED_setChannel(Versa_Handle_t handle, int channel);
VERSADECLSPEC int VERSACDECL  Versa_LED_turnOn(Versa_Handle_t handle);
VERSADECLSPEC int VERSACDECL  Versa_LED_turnOff(Versa_Handle_t handle);

VERSADECLSPEC int VERSACDECL  Versa_LineScan_setIntegrationTime(Versa_Handle_t handle, double ms);
VERSADECLSPEC int VERSACDECL  Versa_LineScan_startMeasurement(Versa_Handle_t handle);
VERSADECLSPEC bool VERSACDECL Versa_LineScan_isBusy(Versa_Handle_t handle);
VERSADECLSPEC int VERSACDECL  Versa_LineScan_waitFor(Versa_Handle_t handle, int timeout_ms);
VERSADECLSPEC void VERSACDECL Versa_LineScan_Result_t_init(Versa_LineScan_Result_t* pResult);
VERSADECLSPEC void VERSACDECL Versa_LineScan_Result_t_free(Versa_LineScan_Result_t* pResult);
VERSADECLSPEC int VERSACDECL  Versa_LineScan_getAllResults(Versa_Handle_t handle, Versa_LineScan_Result_t* pResult);
VERSADECLSPEC int VERSACDECL  Versa_LineScan_getSingleResult(Versa_Handle_t handle, int lineIndex, Versa_LineScan_Result_t* pResult);
VERSADECLSPEC int VERSACDECL  Versa_LineScan_calibrateOffset(Versa_Handle_t handle);
VERSADECLSPEC int VERSACDECL  Versa_LineScan_getOffset(Versa_Handle_t handle, double* offset_mv);

VERSADECLSPEC int VERSACDECL  Versa_App_setScanLineScanHandle(Versa_Handle_t handle);
VERSADECLSPEC int VERSACDECL  Versa_App_setScanMotorDriverHandle(Versa_Handle_t handle);
VERSADECLSPEC int VERSACDECL  Versa_App_setScanLEDDriverHandle(Versa_Handle_t handle);
VERSADECLSPEC int VERSACDECL  Versa_App_setScanParameters(int startSteps, int deltaSteps, int endSteps, int stops);
VERSADECLSPEC int VERSACDECL  Versa_App_startStopAndGoMeasurement(Versa_LEDControl ledControl);
VERSADECLSPEC int VERSACDECL  Versa_App_stopMeasurement();
VERSADECLSPEC bool VERSACDECL Versa_App_isBusy();
VERSADECLSPEC int VERSACDECL  Versa_App_waitFor(int timeout_ms);
VERSADECLSPEC int VERSACDECL  Versa_App_waitForLine(int lineIndex, int timeout_ms);

VERSADECLSPEC void VERSACDECL Versa_getLastErrorMessage(char* message);

#ifdef __cplusplus
}
#endif

#endif // _VERSALIB_H
