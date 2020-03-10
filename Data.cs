using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.PlateReader.Gen4.JETI
{ 
    class Data
    {

        // External Class
//        public Settings.Info info;

        // Public Variables
        public bool DataAvailable { get; set; }     // Data Availailable to Plot


        public AnalysisParameters analysisParameters { get; set; }
        public class AnalysisParameters
        {
            // Wavelength [nm]
            public double WavelengthA;
            public double BandA;

            public double WavelengthB;
            public double BandB;

            public double MomentA;
            public double MomentB;

            // Pixel [index]
            public int PixelA;
            public int PixelA_Low;
            public int PixelA_High;

            public int PixelB;
            public int PixelB_Low;
            public int PixelB_High;

            public int PixelA_Moment;
            public int PixelB_Moment;
        }


        // Class of Plate Data
        public List<Well> PlateResult { get; set; }
        public class Well
        {
            public int Index;               // Well Index, gets set on creation
            public string Name;             // Name of well (Sample, A1, etc.)
            public double Concentration;    // Denaturant Concentration
            public bool Flag;               // Flag, if value saturates detector
            public string Info;             // Maybe some statistics about waveform (snr, % count)
            
            public double IntensityA;
            public double IntensityB;
            public double Ratio;
            public double Moment;

            public double[] Waveform;
            public double Max; 

        }


        // Block Data for export
        public Block block { get; set; }
        public class Block
        {
            public int Parameters = 4;

            public double[] IntensityA;
            public double[] IntensityB;
            public double[] Ratio;
            public double[] Moment;

            public double[][] Data;   // All the data in 1 jagged array
        }



        // Methods
        public void InitialiseData(int wells, int pixels)
        {
            // Initialize a list of classes
            ResetData();
            PlateResult = new List<Well>();
            block = new Block();

            for(int i = 0; i < wells; i++)
            {
                PlateResult.Add(new Well
                {
                    Index = i,
                    Name = "Protein",
                    Concentration = 0,
                    Flag = false, 
                    Info = "",

                    IntensityA = 0,
                    IntensityB = 0,
                    Ratio = 0,
                    Moment = 0,

                    Waveform = new double[pixels],
                    Max = 0

                });
            }

            // Heat map plotting variables
            block.IntensityA = new double[wells];
            block.IntensityB = new double[wells];
            block.Ratio = new double[wells];
            block.Moment = new double[wells];

            block.Data = new double[block.Parameters][];

            for(int i = 0; i < block.Parameters; i++)
            {
                block.Data[i] = new double[wells];
            }

        }

        public void SetData()
        {
            DataAvailable = true;
        }

        public void ResetData()
        {
            DataAvailable = false;
        }

        public void SetResult(int index, double[] waveform, double[] wavelength)
        {
                                   
            // Compile Result
            double[] result = new double[block.Parameters];
            result = CompileResult(waveform, wavelength);
            
            // Set the Data
            PlateResult[index].Waveform = waveform;

            PlateResult[index].IntensityA = result[0];
            PlateResult[index].IntensityB = result[1];
            PlateResult[index].Ratio = result[2];
            PlateResult[index].Moment = result[3];

            // Heat Map Data
            block.IntensityA[index] = result[0];
            block.IntensityB[index] = result[1];
            block.Ratio[index] = result[2];
            block.Moment[index] = result[3];

            block.Data[0][index] = result[0];
            block.Data[1][index] = result[1];
            block.Data[2][index] = result[2];
            block.Data[3][index] = result[3];

            // Find Max Value for Flag
            double max = 0; 
            PlateResult[index].Flag = CheckMax(waveform, ref max);
            PlateResult[index].Max = max;

        }

        public void SetResult(int index, double[] waveform, double[] wavelength, int PixelStart, int PixelEnd)
        {

            // Compile Result
            double[] result = new double[block.Parameters];
            result = CompileResult(waveform, wavelength);

            // Set the Data
            PlateResult[index].Waveform = waveform;

            PlateResult[index].IntensityA = result[0];
            PlateResult[index].IntensityB = result[1];
            PlateResult[index].Ratio = result[2];
            PlateResult[index].Moment = result[3];

            // Heat Map Data
            block.IntensityA[index] = result[0];
            block.IntensityB[index] = result[1];
            block.Ratio[index] = result[2];
            block.Moment[index] = result[3];

            block.Data[0][index] = result[0];
            block.Data[1][index] = result[1];
            block.Data[2][index] = result[2];
            block.Data[3][index] = result[3];

            // Find Max Value for Flag
            double max = 0;
            PlateResult[index].Flag = CheckMax(waveform, ref max, PixelStart, PixelEnd);
            PlateResult[index].Max = max;

        }

        // Analysis
        public void SetAnalysisParameters(double[] wavelength, double wavelengthA, double wavelengthB, double bandA, double bandB, double wavelengthStart, double wavelengthEnd)
        {
            // New instance of analysis parameters class
            analysisParameters = new AnalysisParameters();

            analysisParameters.MomentA = wavelengthStart;
            analysisParameters.MomentB = wavelengthEnd;

            analysisParameters.WavelengthA = wavelengthA;
            analysisParameters.WavelengthB = wavelengthB;

            analysisParameters.BandA = bandA;
            analysisParameters.BandB = bandB;

            // Pixel [index]
            double aLow = wavelengthA - (bandA / 2);
            double aHigh = wavelengthA + (bandA / 2);

            double bLow = wavelengthB - (bandB / 2);
            double bHigh = wavelengthB + (bandB / 2);


            analysisParameters.PixelA = convert2Pixel(wavelengthA, wavelength);
            analysisParameters.PixelA_Low = convert2Pixel(aLow, wavelength);
            analysisParameters.PixelA_High = convert2Pixel(aHigh, wavelength);

            analysisParameters.PixelB = convert2Pixel(wavelengthB, wavelength);
            analysisParameters.PixelB_Low = convert2Pixel(bLow, wavelength);
            analysisParameters.PixelB_High = convert2Pixel(bHigh, wavelength);

            analysisParameters.PixelA_Moment = convert2Pixel(analysisParameters.MomentA, wavelength);
            analysisParameters.PixelB_Moment = convert2Pixel(analysisParameters.MomentB, wavelength);
        }

        // Method convert wavelength to pixel of camera
        int convert2Pixel(double value, double[] wavelength)
        {
            for (int i = 0; i < wavelength.Length; i++)
            {
                if (value < wavelength[i])
                    return i;
            }
            return 0;
        }

        public double[] CompileResult(double[] data, double[] wavelength)
        {

            // Create temp variable
            double[] result = new double[block.Parameters];            

            int idx1 = analysisParameters.PixelA_Low;
            int idx2 = analysisParameters.PixelA_High;

            int idx3 = analysisParameters.PixelB_Low;
            int idx4 = analysisParameters.PixelB_High;

            int idx5 = analysisParameters.PixelA_Moment;
            int idx6 = analysisParameters.PixelB_Moment;

            // Intensity A
            int count = 0;
            for (int i = idx1; i < idx2; i++)
            {
                result[0] += data[i];
                count++;
            }

            // Intensity B
            int count2 = 0;
            for (int i = idx3; i < idx4; i++)
            {
                result[1] += data[i];
                count2++;
            }

            // Moment 
            double a = 0;
            double b = 0;

            for (int i = idx5; i < idx6; i++)
            {
                a += wavelength[i] * data[i];
                b += data[i];
            }

            // Check Values
            if (!CheckValue(result[0]) || !CheckValue(result[1]) || !CheckValue(a) || !CheckValue(b))
            {
                return new double[4] { 0, 0, 0, 0 };
            }


            // Return the variable
            result[0] = result[0] / count;
            result[1] = result[1] / count2;
            result[2] = result[0] / result[1];
            result[3] = a / b;

            return result;

        }

        public bool CheckMax(double[] data, ref double max)
        {

            // Find Max
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] > max)
                {
                    max = data[i];
                }
            }

            // See if it is out of range of the detector
            if (max > 900)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckMax(double[] data, ref double max, int PixelStart, int PixelEnd)
        {
            
            // Find Max
            for (int i = PixelStart; i < PixelEnd; i++)
            {
                if (data[i] > max)
                {
                    max = data[i];
                }
            }

            // See if it is out of range of the detector
            if (max > 900)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckValue(double value)
        {
            value = Math.Abs(value);

            // Check if Not a real number
            if (double.IsNaN(value))
                return false;

            // Check if the value is infinity
            if (double.IsInfinity(value))
                return false;

            // Check to see if the number is zero
            if (value < 0.0000001)
                return false;

            return true;

        }


                          



    }
}
