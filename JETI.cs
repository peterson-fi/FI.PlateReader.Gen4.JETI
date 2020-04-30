using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace FI.PlateReader.Gen4.JETI
{
    class JETI
    {
        // Classes
        public Settings.Info info;
        private int jetiHandle;
        public string apiVersion;
        public int Npixels;
        public double[] Wavelength;
        public double[] Background { get; set; }    // Background Spectrum [Pre Scan, Dark]
        public double[] Waveform { get; set; }      // Waveform Spectrum [Well, Light]  
        public int Tint;

        [DllImport("jeti_core.dll")]
        public static extern int JETI_GetFit(int dwDevice, ref float fFit);

        [DllImport("jeti_spectro_ex.dll")]
        public static extern int JETI_GetSpectroExDLLVersion(ref int wMajorVersion, ref int wMinorVersion, ref int wBuildNumber);

        [DllImport("jeti_spectro_ex.dll")]
        public static extern int JETI_GetNumSpectroEx(ref int dwNumDevices);

        [DllImport("jeti_spectro_ex.dll")]
        public static extern int JETI_OpenSpectroEx(int dwDeviceNum, ref int dwDevice);

        [DllImport("jeti_spectro_ex.dll")]
        public static extern int JETI_CloseSpectroEx(int dwDevice);

        [DllImport("jeti_spectro_ex.dll")]
        public static extern int JETI_PixelCountEx(int dwDevice, ref int dwPixel);

        [DllImport("jeti_spectro_ex.dll")]
        public static extern int JETI_StartDarkEx(int dwDevice, int dwTint, ushort wAver);

        [DllImport("jeti_spectro_ex.dll")]
        public static extern int JETI_DarkPixEx(int dwDevice, ref int dwDark);

        [DllImport("jeti_spectro_ex.dll")]
        public static extern int JETI_StartLightEx(int dwDevice, int dwTint, ushort wAver);

        [DllImport("jeti_spectro_ex.dll")]
        public static extern int JETI_LightPixEx(int dwDevice, ref int dwLight);

        [DllImport("jeti_spectro_ex.dll")]
        public static extern int JETI_LightWaveEx(int dwDevice, int dwBeg, int dwEnd, float fStep, ref float fLight);

        [DllImport("jeti_spectro_ex.dll")]
        public static extern int JETI_StartReferEx(int dwDevice, int dwTint, ushort wAver);

        [DllImport("jeti_spectro_ex.dll")]
        public static extern int JETI_PrepareReferEx(int dwDevice, int dwTint, ushort wAver);

        [DllImport("jeti_spectro_ex.dll")]
        public static extern int JETI_ReferPixEx(int dwDevice, ref int dwRefer);

        [DllImport("jeti_spectro_ex.dll")]
        public static extern int JETI_SpectroStatusEx(int dwDevice, ref bool boIsBusy);

        // Create Methods
        public void dllVersion()
        {
            // Initialize variables
            int major = 0;
            int minor = 0;
            int build = 0;

            // Get DLL Vesion and Return Error
            int error = JETI_GetSpectroExDLLVersion(ref major, ref minor, ref build);
            processError(error);

            apiVersion = major.ToString() + "." + minor.ToString() + "." + build.ToString();

        }

        public bool Connect()
        {
            dllVersion();

            int nDevices = findDevice();
            int nPixel = 0;

            if (nDevices == 1)
            {
                int test = openDevice();
                if (test == 0)
                {
                    // get number of pixels
                    pixelCount(ref nPixel);
                    Npixels = (int)nPixel;

                    Background = new double[Npixels];
                    Waveform = new double[Npixels];
                    Wavelength = new double[Npixels];

                    // get wavelength conversion values
                    pixelFit(ref Wavelength);

                    return true;
                }
                else { return false; }
            }
            else
            {
                return false;
            }
        }

        public bool Disconnect()
        {
            int test = closeDevice();
            if (test == 0) { return true; }
            else { return false; }
            
        }

        public bool DarkMeasurement()
        {
            // Turn Led Off
            //if (info.LEDControl) { vs.LedOff(); }

            // initialize array
            double[] spec = new double[Npixels];
            Background = new double[Npixels];
            // Perform Measurement
            getSpectrum(Tint, ref spec);

            // Subtract dark measurement.
            for (int i = 0; i < Npixels; i++)
            {
                Background[i] = spec[i];
            }

            return true;

        }

        public bool LightMeasurement()
        {

            // Turn Led On
            //if (info.LEDControl) { vs.LedOn(); }

            // initialize array
            double[] spec = new double[Npixels];
            Waveform = new double[Npixels];

            // Perform Measurement
            getSpectrum(Tint, ref spec);

            // Turn Led Off
            //if (info.LEDControl) { vs.LedOff(); }

            // Subtract dark measurement.
            for (int i = 0; i < Npixels; i++)
            {
                Waveform[i] = spec[i] - Background[i];
            }

            return true;
        }

        public int findDevice()
        {
            // Variables
            int status = 1;
            int dwNumDevices = 0;

            // Find number of spectrometers
            int error = JETI_GetNumSpectroEx(ref dwNumDevices);
            processError(error);
            
            // Process number of devices, Only can handle 1 Device
            if (dwNumDevices < 1)
            {
                MessageBox.Show("No Device connected!");
                return status = 0;
            }

            // Process number of devices, Only can handle 1 Device
            if (dwNumDevices > 1)
            {
                MessageBox.Show("More than 1 Device Connected!");
                return status = 0;
            }

            return status;
        }

        public int openDevice()
        {
            // Variables
            int dwDeviceNum = 0;

            // Connect to Spectrometer
            int error = JETI_OpenSpectroEx(dwDeviceNum, ref jetiHandle);
            processError(error);

            return error; 
        }

        public int closeDevice()
        {
            int error = JETI_CloseSpectroEx(jetiHandle);
            processError(error);

            return error;
        }

        public void getSpectrum(int dwTint, ref double[] spec)
        {

            // Variables
            ushort wAver = 1;
            bool boStatus = true;

            // Initiate Measurement
            int[] sp = new int[Npixels];

            int error = JETI_StartLightEx(jetiHandle, dwTint, wAver);
            processError(error);

            // Check to see it is finished 
            
            while (boStatus == true)
            {
                error = JETI_SpectroStatusEx(jetiHandle, ref boStatus);
            }

            // Retrieve the Data
            error = JETI_LightPixEx(jetiHandle, ref sp[0]);
            processError(error);

            for (int i = 0; i < Npixels; i++)
            {
                spec[i] = sp[i];
            }

        }

        public void pixelCount(ref int dwPixel)
        {
            int error = JETI_PixelCountEx(jetiHandle, ref dwPixel);
            processError(error);
        }

        public void pixelFit(ref double[] wavelength)
        {
            // Get number of pixels
            int dwPixel = 0;
            pixelCount(ref dwPixel);

            // Get the Coefficients
            float[] coeff = new float[5];
            JETI_GetFit(jetiHandle, ref coeff[0]);

            // Convert to wavelengths
            
            double value1;
            double value2;
            double value3;
            double value4;
            double value5;

            for(int i = 0; i < dwPixel; i++)
            {
                value1 = coeff[0];
                value2 = coeff[1] * Math.Pow(i, 1);
                value3 = coeff[2] * Math.Pow(i, 2);
                value4 = coeff[3] * Math.Pow(i, 3);
                value5 = coeff[4] * Math.Pow(i, 4);

                wavelength[i] = value1 + value2 + value3 + value4 + value5;
            }

        }
        
        
        
        // Methods for Error Handling
        public void processError(int error)
        {
            // Process Error
            if (error != 0)
            {
                // Find out the error and show it to the user
                string errorOut = getError(error);
                MessageBox.Show(errorOut);
            }
        }

        public string getError(int errorIn)
        {

            string errorOut = "";

            switch (errorIn)
            {
                case 0:
                    errorOut = "No Error";
                    break;
                case 2:
                    errorOut = "Could not open COM-port";
                    break;
                case 3:
                    errorOut = "Could not set COM-port settings";
                    break;
                case 4:
                    errorOut = "Could not set buffer size of COM-port";
                    break;
                case 5:
                    errorOut = "Could not purge buffer of COM-port";
                    break;
                case 6:
                    errorOut = "Could not set COM-port timeout";
                    break;
                case 7:
                    errorOut = "Could not send to device";
                    break;
                case 8:
                    errorOut = "Communication timeout errror";
                    break;
                case 10:
                    errorOut = "Could not receive from device";
                    break;
                case 11:
                    errorOut = "Command not supported";
                    break;
                case 12:
                    errorOut = "Could not convert received data";
                    break;
                case 13:
                    errorOut = "Invalid arguement";
                    break;
                case 14:
                    errorOut = "device Busy";
                    break;
                case 17:
                    errorOut = "Invalid checksum of received data";
                    break;
                case 18:
                    errorOut = "Invalid step width";
                    break;
                case 19:
                    errorOut = "Invalid device number";
                    break;
                case 20:
                    errorOut = "Device not connected";
                    break;
                case 21:
                    errorOut = "Invalid device handle";
                    break;
                case 22:
                    errorOut = "Invalid calibration file number";
                    break;
                case 23:
                    errorOut = "Calibration data not read";
                    break;
                case 32:
                    errorOut = "Measurement failed due to overexposure";
                    break;
                case 34:
                    errorOut = "Measurement failed due to other reasons";
                    break;
                case 35:
                    errorOut = "Adaptatin fail";
                    break;
                case 128:
                    errorOut = "Internal DLL Error";
                    break;
                case 255:
                    errorOut = "Fatal Communication Error";
                    break;
                default:
                    errorOut = "Unknown Error";
                    break;
            }

            return errorOut;
        }
    }
}
