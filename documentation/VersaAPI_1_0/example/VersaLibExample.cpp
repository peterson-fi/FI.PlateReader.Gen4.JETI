
#include <list>
#include <string>
#include <vector>
#include <iostream>
#include <stdexcept>
#include <thread>
#include <chrono> 
#include "VersaLib.h"

#define CHECK_VERSALIB_CODE(arg) \
    { \
        int code = arg; \
        if ( code != 0 ) return code; \
    }

// --- Settings (variables) ---

double              linescanIntegrationTime_ms  = 1.0;
std::string         userDataString;

bool                doDeviceCountExample        = false;
bool                doLineScanExample           = false;
bool                doOffsetCalibrationExample  = false;
bool                doHWStopAndGoExample        = false;
bool                doMotorHomeTest             = false;
bool                doResetExample              = false;
bool                doUserDataWrite             = false;
bool                doUserDataRead              = false;

// Print command-line usage
void printUsage()
{
    std::cout << "Usage:\n";
    std::cout << "VersaLibExample <run-action> [arguments]\n";
    std::cout << "\n";
    std::cout << "Configuration:\n";
    std::cout << "   PBus:  LineScan\n";
    std::cout << "   SBus1: Motor\n";
    std::cout << "   SBus2: LED\n";
    std::cout << "\n";
    std::cout << "Run actions:\n";
    std::cout << "--run-device-count-example    Checks the number and type of connected devices.\n";
    std::cout << "--run-linescan-example        Performs a single LineScan measurement.\n";
    std::cout << "--run-offset-cal-example      Performs LineScan offset calibration.\n";
    std::cout << "--run-hw-stop-and-go-example  Runs the motor scan example with stop-and-go scanning in hardware.\n";
    std::cout << "--run-motor-home-test         Runs a test to check reliability of move-to-home.\n";
    std::cout << "--user-data-write             Writes user data text to the target device.\n";
    std::cout << "--user-data-read              Reads the user data text from the device.\n";
    std::cout << "\n";
    std::cout << "Arguments:\n";
    std::cout << "--linescan-int=<us>           LineScan integration time in [ms]. Default = 1.0 ms.\n";
    std::cout << "--user-data=<text>            User data text.\n";
}

// Process the command-line arguments.
bool processArguments(int argc, char** argv)
{
    std::vector<std::string> arguments;
    for ( int i = 1 ; i < argc ; ++i )
    {
        arguments.push_back(std::string(argv[i]));
    }

    try
    {
        for ( unsigned int i = 0 ; i < arguments.size() ; ++i )
        {
            if ( arguments[i].find("--linescan-int=") == 0 )
            {
                linescanIntegrationTime_ms = std::stod(arguments[i].substr(15,arguments[i].length()-15));
            }
            else if ( arguments[i].find("--user-data=") == 0 )
            {
                userDataString = arguments[i].substr(12,arguments[i].length()-12);
                if ( userDataString.length() > 31 )
                {
                    std::cout << "\nUser data exceeds 31 characters!\n\n";
                    return false;
                }
            }
            else if ( arguments[i].find("--run-device-count-example") == 0 )
            {
                doDeviceCountExample = true;
            }
            else if ( arguments[i].find("--run-linescan-example") == 0 )
            {
                doLineScanExample = true;
            }
            else if ( arguments[i].find("--run-offset-cal-example") == 0 )
            {
                doOffsetCalibrationExample = true;
            }
            else if ( arguments[i].find("--run-hw-stop-and-go-example") == 0 )
            {
                doHWStopAndGoExample = true;
            }
            else if ( arguments[i].find("--run-motor-home-test") == 0 )
            {
                doMotorHomeTest = true;
            }
            else if ( arguments[i].find("--run-reset-example") == 0 )
            {
                doResetExample = true;
            }
            else if ( arguments[i].find("--user-data-write") == 0 )
            {
                doUserDataWrite = true;
            }
            else if ( arguments[i].find("--user-data-read") == 0 )
            {
                doUserDataRead = true;
            }
            else
            {
                std::cout << "\nError - Unknown argument: " << arguments[i].data() << " !\n\n";
                printUsage();
                return false;
            }
        }
    }
    catch (const std::invalid_argument&)
    {
        std::cout << "\nError - Invalid numeric argument !!!!!!\n\n";
        printUsage();
        return false;
    }
    catch (const std::out_of_range&)
    {
        std::cout << "\nError - Numeric argument out of range!\n\n";
        printUsage();
        return false;
    }

    int actionCount = 0;
    if ( doDeviceCountExample ) ++actionCount;
    if ( doLineScanExample ) ++actionCount;
    if ( doOffsetCalibrationExample ) ++actionCount;
    if ( doHWStopAndGoExample ) ++actionCount;
    if ( doMotorHomeTest ) ++actionCount;
    if ( doResetExample ) ++actionCount;
    if ( doUserDataWrite ) ++actionCount;
    if ( doUserDataRead ) ++actionCount;

    if ( actionCount == 0 )
    {
        std::cout << "\nError - No action specified!\n\n";
        printUsage();
        return false;
    }

    if ( actionCount > 1 )
    {
        std::cout << "\nError - More than one action specified!\n\n";
        printUsage();
        return false;
    }

    return true;
}

int runDeviceCountExample()
{
    int count;
    Versa_getAvailableDevices(&count,NULL);
    std::cout << "\nNumber of available devices: " << count << "\n";

    if ( count != 0 )
    {
        Versa_DeviceType deviceTypes[100];
        Versa_getAvailableDevices(&count,deviceTypes);

        for ( int i = 0 ; i < count ; ++i )
        {
            switch ( deviceTypes[i] )
            {
            case Versa_InterfaceBoard_35_1:
                std::cout << "Device [" << i << "]: " << "Interface board (hardware version 1)\n";
                break;
            default:
                std::cout << "Device [" << i << "]: " << "Unknown type\n";
                break;
            }
        }
    }

    return 0;
}

int runResetExample()
{
    Versa_resetAllPorts();
    return 0;
}

// Run the LineScan example code
int runLineScanMeasurementExample()
{
    // Initialise the session.
    CHECK_VERSALIB_CODE(Versa_initialiseSession());

    // Get the relavant handles
    Versa_Handle_t linescanHandle = Versa_getPBusHandle();
    Versa_Handle_t motorHandle = Versa_getSBusHandle(1);
    Versa_Handle_t ledHandle = Versa_getSBusHandle(2);

    // Set the integration time.
    CHECK_VERSALIB_CODE(Versa_LineScan_setIntegrationTime(linescanHandle,linescanIntegrationTime_ms));

    // Set the LED current and turn LED on.
    CHECK_VERSALIB_CODE(Versa_LED_setCurrent(ledHandle,100));
    CHECK_VERSALIB_CODE(Versa_LED_setChannel(ledHandle,1));
    CHECK_VERSALIB_CODE(Versa_LED_turnOn(ledHandle));

    // Set the motor settings and turn motor on.
    CHECK_VERSALIB_CODE(Versa_Motor_setCurrent(motorHandle,600));
    CHECK_VERSALIB_CODE(Versa_Motor_setSpeed(motorHandle,2000));
    CHECK_VERSALIB_CODE(Versa_Motor_setAcceleration(motorHandle,2000));
    CHECK_VERSALIB_CODE(Versa_Motor_setMicroStepping(motorHandle,Versa_MotorMicroStep_x32));
    CHECK_VERSALIB_CODE(Versa_Motor_setAcceleration(motorHandle,20000));
    CHECK_VERSALIB_CODE(Versa_Motor_commitSettings(motorHandle));
    CHECK_VERSALIB_CODE(Versa_Motor_turnOn(motorHandle));

    // Move motor relative
    CHECK_VERSALIB_CODE(Versa_Motor_moveRelative(motorHandle,2000));
    CHECK_VERSALIB_CODE(Versa_Motor_waitForAll(20000));

    // Start the measurement. The function returns immediately.
    CHECK_VERSALIB_CODE(Versa_LineScan_startMeasurement(linescanHandle));
    // Wait for the measurement to complete.
    CHECK_VERSALIB_CODE(Versa_LineScan_waitFor(linescanHandle,5000));

    // Move motor relative
    CHECK_VERSALIB_CODE(Versa_Motor_moveRelative(motorHandle,2000));
    CHECK_VERSALIB_CODE(Versa_Motor_waitForAll(20000));

    // Turn LED and mtoro off.
    CHECK_VERSALIB_CODE(Versa_LED_turnOff(ledHandle));
    CHECK_VERSALIB_CODE(Versa_Motor_turnOff(motorHandle));

    // Initialise the result data structure.
    Versa_LineScan_Result_t result;
    Versa_LineScan_Result_t_init(&result);

    CHECK_VERSALIB_CODE(Versa_LineScan_getAllResults(linescanHandle,&result));

    std::cout << "\nResult info:\n";
    std::cout << "    Line count  = " << result.lineCount << "\n";
    std::cout << "    Pixel count = " << result.data[0].pixelCount << "\n";

    // Write out the line-scan trace.
    std::cout << "\nValues:\n";
    for ( int n = 0 ; n < result.data[0].pixelCount ; ++n )
    {
        std::cout << (int)result.data[0].lineTrace[n] << "  ";
    }
    std::cout << "\n\n\n";

    // Free the result data structure.
    Versa_LineScan_Result_t_free(&result);

    // Close the session
    Versa_closeSession();

    return 0;
}

int runOffsetCalibration()
{
    // Initialise the session.
    CHECK_VERSALIB_CODE(Versa_initialiseSession());

    // Get the relavant handles
    Versa_Handle_t linescanHandle = Versa_getPBusHandle();

    // Run sensor offset calibration.
    CHECK_VERSALIB_CODE(Versa_LineScan_calibrateOffset(linescanHandle));

    // Show the offset.
    double offset_mV;
    CHECK_VERSALIB_CODE(Versa_LineScan_getOffset(linescanHandle,&offset_mV));
    std::cout << "Offset = " << offset_mV << " mV\n";

    // Close the session
    Versa_closeSession();

    return 0;
}

int runHWStopAndGoExample()
{
    // Initialise the session.
    CHECK_VERSALIB_CODE(Versa_initialiseSession());

    // Get the relavant handles
    Versa_Handle_t linescanHandle = Versa_getPBusHandle();
    Versa_Handle_t rowMotorHandle = Versa_getSBusHandle(1);
    Versa_Handle_t colMotorHandle = Versa_getSBusHandle(2);
    Versa_Handle_t ledHandle = Versa_getSBusHandle(3);

    // Set the integration time.
    CHECK_VERSALIB_CODE(Versa_LineScan_setIntegrationTime(linescanHandle,linescanIntegrationTime_ms));

    // Set the LED current and channel.
    CHECK_VERSALIB_CODE(Versa_LED_setCurrent(ledHandle,100));
    CHECK_VERSALIB_CODE(Versa_LED_setChannel(ledHandle,1));

    // Set the motor settings and turn motor on.
    CHECK_VERSALIB_CODE(Versa_Motor_setCurrent(rowMotorHandle,800));
    CHECK_VERSALIB_CODE(Versa_Motor_setSpeed(rowMotorHandle,800));
    CHECK_VERSALIB_CODE(Versa_Motor_setAcceleration(rowMotorHandle,8000));
    CHECK_VERSALIB_CODE(Versa_Motor_setMicroStepping(rowMotorHandle,Versa_MotorMicroStep_x32));
    CHECK_VERSALIB_CODE(Versa_Motor_commitSettings(rowMotorHandle));
    CHECK_VERSALIB_CODE(Versa_Motor_turnOn(rowMotorHandle));

    CHECK_VERSALIB_CODE(Versa_Motor_setCurrent(colMotorHandle,800));
    CHECK_VERSALIB_CODE(Versa_Motor_setSpeed(colMotorHandle,400));
    CHECK_VERSALIB_CODE(Versa_Motor_setAcceleration(colMotorHandle,4000));
    CHECK_VERSALIB_CODE(Versa_Motor_setDirectionReversed(colMotorHandle,true));
    CHECK_VERSALIB_CODE(Versa_Motor_setMicroStepping(colMotorHandle,Versa_MotorMicroStep_x32));
    CHECK_VERSALIB_CODE(Versa_Motor_commitSettings(colMotorHandle));
    CHECK_VERSALIB_CODE(Versa_Motor_turnOn(colMotorHandle));

    // Move motors home
    CHECK_VERSALIB_CODE(Versa_Motor_moveHome(colMotorHandle));
    CHECK_VERSALIB_CODE(Versa_Motor_moveHome(rowMotorHandle));
    CHECK_VERSALIB_CODE(Versa_Motor_waitForAll(10000));

    CHECK_VERSALIB_CODE(Versa_Motor_moveRelative(rowMotorHandle,10000));
    CHECK_VERSALIB_CODE(Versa_Motor_moveRelative(colMotorHandle,2000));
    CHECK_VERSALIB_CODE(Versa_Motor_waitForAll(10000));

    // Set scan handles
    CHECK_VERSALIB_CODE(Versa_App_setScanLineScanHandle(linescanHandle));
    CHECK_VERSALIB_CODE(Versa_App_setScanMotorDriverHandle(colMotorHandle));
    CHECK_VERSALIB_CODE(Versa_App_setScanLEDDriverHandle(ledHandle));

    // Tunr LED on by software and use software control during stop-and-go measurement.
    // Alternative comment out the below line and use Versa_LED_HWControl.
    CHECK_VERSALIB_CODE(Versa_LED_turnOn(ledHandle));

    // Wait for the measurement to complete.
    for (int row = 0 ; row < 30 ; ++row)
    {
        // Set scan parameters
        if (row % 2 == 0)
        {
            CHECK_VERSALIB_CODE(Versa_App_setScanParameters(1000,200,1000,30));
        }
        else
        {
            CHECK_VERSALIB_CODE(Versa_App_setScanParameters(-1000,-200,-1000,30));
        }

        // Start stop-and-go measurement
        CHECK_VERSALIB_CODE(Versa_App_startStopAndGoMeasurement(Versa_LED_SWControl));

        // Wait for the measurement to complete.
        for (int col = 0 ; col < 30 ; ++col)
        {
            CHECK_VERSALIB_CODE(Versa_App_waitForLine(col,2000));

            // Initialise the result data structure.
            Versa_LineScan_Result_t result;
            Versa_LineScan_Result_t_init(&result);

            CHECK_VERSALIB_CODE(Versa_LineScan_getSingleResult(linescanHandle,col,&result));

            std::cout << "\nResult info:\n";
            std::cout << "    Line count  = " << result.lineCount << "\n";
            std::cout << "    Pixel count = " << result.data[0].pixelCount << "\n";

            // Write out the line-scan trace.
            std::cout << "\nLine index = " << result.data[0].lineIndex << "\n";

            // Free the result data structure.
            Versa_LineScan_Result_t_free(&result);
        }

        // Wait for tsop-and-go to finish
        CHECK_VERSALIB_CODE(Versa_App_waitFor(2000));

        // Move to next row
        CHECK_VERSALIB_CODE(Versa_Motor_moveRelative(rowMotorHandle,1000));
        CHECK_VERSALIB_CODE(Versa_Motor_waitForAll(10000));
    }

    // Move motors home
    CHECK_VERSALIB_CODE(Versa_Motor_moveHome(colMotorHandle));
    CHECK_VERSALIB_CODE(Versa_Motor_moveHome(rowMotorHandle));
    CHECK_VERSALIB_CODE(Versa_Motor_waitForAll(10000));

    // Turn LED and motors off.
    CHECK_VERSALIB_CODE(Versa_Motor_turnOff(rowMotorHandle));
    CHECK_VERSALIB_CODE(Versa_Motor_turnOff(colMotorHandle));
    CHECK_VERSALIB_CODE(Versa_LED_turnOff(ledHandle));

    // Close the session
    Versa_closeSession();

    return 0;
}

int runMotorHomeTest()
{
    // Initialise the session.
    CHECK_VERSALIB_CODE(Versa_initialiseSession());

    // Get the relavant handles
    Versa_Handle_t linescanHandle = Versa_getPBusHandle();
    Versa_Handle_t rowMotorHandle = Versa_getSBusHandle(1);
    Versa_Handle_t colMotorHandle = Versa_getSBusHandle(2);
    Versa_Handle_t ledHandle = Versa_getSBusHandle(3);

    // Set the motor settings and turn motor on.
    CHECK_VERSALIB_CODE(Versa_Motor_setCurrent(rowMotorHandle,800));
    CHECK_VERSALIB_CODE(Versa_Motor_setSpeed(rowMotorHandle,400));
    CHECK_VERSALIB_CODE(Versa_Motor_setAcceleration(rowMotorHandle,4000));
    CHECK_VERSALIB_CODE(Versa_Motor_setMicroStepping(rowMotorHandle,Versa_MotorMicroStep_x32));
    CHECK_VERSALIB_CODE(Versa_Motor_commitSettings(rowMotorHandle));
    CHECK_VERSALIB_CODE(Versa_Motor_turnOn(rowMotorHandle));

    CHECK_VERSALIB_CODE(Versa_Motor_setCurrent(colMotorHandle,800));
    CHECK_VERSALIB_CODE(Versa_Motor_setSpeed(colMotorHandle,200));
    CHECK_VERSALIB_CODE(Versa_Motor_setAcceleration(colMotorHandle,2000));
    CHECK_VERSALIB_CODE(Versa_Motor_setDirectionReversed(colMotorHandle,true));
    CHECK_VERSALIB_CODE(Versa_Motor_setMicroStepping(colMotorHandle,Versa_MotorMicroStep_x32));
    CHECK_VERSALIB_CODE(Versa_Motor_commitSettings(colMotorHandle));
    CHECK_VERSALIB_CODE(Versa_Motor_turnOn(colMotorHandle));

    for (int i = 0; i < 200 ; ++i)
    {
        CHECK_VERSALIB_CODE(Versa_Motor_moveHome(colMotorHandle));
        CHECK_VERSALIB_CODE(Versa_Motor_moveHome(rowMotorHandle));
        CHECK_VERSALIB_CODE(Versa_Motor_waitForAll(10000));

        CHECK_VERSALIB_CODE(Versa_Motor_moveRelative(rowMotorHandle,10000));
        CHECK_VERSALIB_CODE(Versa_Motor_moveRelative(colMotorHandle,2000));
        CHECK_VERSALIB_CODE(Versa_Motor_waitForAll(10000));
    }
    // Turn motors off.
    CHECK_VERSALIB_CODE(Versa_Motor_turnOff(rowMotorHandle));
    CHECK_VERSALIB_CODE(Versa_Motor_turnOff(colMotorHandle));

    // Close the session
    Versa_closeSession();

    return 0;
}

int userDataWrite()
{
    // Initialise the session.
    CHECK_VERSALIB_CODE(Versa_initialiseSession());

    // Upload user data
    Versa_UserData_t userData;
    std::cout << "\nWriting user data \"" << userDataString.data() << "\" ...\n";
    std::strcpy(userData.data,userDataString.data());
    CHECK_VERSALIB_CODE(Versa_uploadUserData(&userData));

    // Close the session.
    Versa_closeSession();

    return 0;
}

int userDataRead()
{
    // Initialise the session.
    CHECK_VERSALIB_CODE(Versa_initialiseSession());

    // DOwnload user data
    Versa_UserData_t userData;
    std::cout << "\nReading user data ...\n";
    CHECK_VERSALIB_CODE(Versa_downloadUserData(&userData));
    std::cout << "User data: \"" << userData.data << "\"\n";

    // Close the session.
    Versa_closeSession();

    return 0;
}

int main(int argc, char** argv)
{
    // Print the version of DLL.
    int majorVersion = 0;
    int minorVersion = 0;
    Versa_getVersion(&majorVersion,&minorVersion);

#ifdef _WIN64
    std::cout << "VersaLib DLL Version: " << majorVersion << "." << minorVersion << " (Windows 64-bit)\n";
#else
    #ifdef __linux__
        #ifdef __x86_64__
            std::cout << "VersaLib DLL Version: " << majorVersion << "." << minorVersion << " (Linux 64-bit)\n";
        #else
            std::cout << "VersaLib DLL Version: " << majorVersion << "." << minorVersion << " (Linux 32-bit)\n";
        #endif
    #else
        std::cout << "VersaLib DLL Version: " << majorVersion << "." << minorVersion << " (Windows 32-bit)\n";
    #endif
#endif

    if ( (argc == 2) && (std::string(argv[1]) == "-h") )
    {
        printUsage();
        return 0;
    }

    if ( processArguments(argc,argv) == false )
    {
        return -1;
    }

    int returnCode = 0;
    if ( doDeviceCountExample )
    {
        returnCode = runDeviceCountExample();
    }
    if ( doResetExample )
    {
        returnCode = runResetExample();
    }
    else if ( doLineScanExample )
    {
        returnCode = runLineScanMeasurementExample();
    }
    else if ( doOffsetCalibrationExample )
    {
        returnCode = runOffsetCalibration();
    }
    else if ( doHWStopAndGoExample )
    {
        returnCode = runHWStopAndGoExample();
    }
    else if ( doMotorHomeTest )
    {
        returnCode = runMotorHomeTest();
    }
    else if ( doUserDataWrite)
    {
        returnCode = userDataWrite();
    }
    else if ( doUserDataRead )
    {
        returnCode = userDataRead();
    }

    if ( returnCode != 0 )
    {
        char errorString[1000];
        Versa_getLastErrorMessage(errorString);
        std::cout << "Error message: " << errorString << "\n";

        Versa_closeSession();
    }
    else
    {
        std::cout << "Success!\n";
    }

    return returnCode;
}
