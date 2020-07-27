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
        public Settings.Info info;

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
            public double Temperature;
            public double Heatsink;

            public double[] Waveform;
            //public double[] Waveform1;
            //public double[] Waveform2;
            public double Max; 

        }


        // Block Data for export
        public Block block { get; set; }
        public class Block
        {
            public int Parameters = 4;

            public double[][] IntensityA; // [Scan][Well]
            public double[][] IntensityB;
            public double[][] Ratio;
            public double[][] Moment;
            public double[][] Temperature;

            public double[][][] Data;   // [Scan] [Parameter] [Well]
        }



        // Methods
        public void InitializeData(int nScans, int wells)
        {
            // Initialize a list of classes
            ResetData();
            PlateResult = new List<Well>();

            int totalWells = nScans * wells;
            int chnLength = 1;
            if (info.Detector == "TIA")
            {
                chnLength = info.NPixel;
            }
            else
            {
                chnLength = 1;
            }

            for (int i = 0; i < totalWells; i++)
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
                    Temperature = 20,
                    Heatsink = 20,

   
                    Waveform = new double[info.NPixel],
                    //Waveform1 = new double[chnLength],
                    //Waveform2 = new double[chnLength],
                    Max = 0

                });
            }



            // Initialise Block Data
            block = new Block();

            // Heat map plotting variables
            block.IntensityA = new double[nScans][];
            block.IntensityB = new double[nScans][];
            block.Ratio = new double[nScans][];
            block.Moment = new double[nScans][];
            block.Temperature = new double[nScans][];           

            for(int i = 0; i < nScans; i++)
            {
                block.IntensityA[i] = new double[wells];
                block.IntensityB[i] = new double[wells];
                block.Ratio[i] = new double[wells];
                block.Moment[i] = new double[wells];
                block.Temperature[i] = new double[wells];
            }


            block.Data = new double[nScans][][];

            for (int i = 0; i < nScans; i++)
            {
                block.Data[i] = new double[block.Parameters][];

                for(int j = 0; j < block.Parameters; j++)
                {
                    block.Data[i][j] = new double[wells];
                }
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

        public void SetResult(int scan, int index, int wells, double[] waveform, double temp, double heatsink)
        {

            // Variables
            int indexScan = (scan * wells) + index;

            // Compile Result
            double[] result = new double[block.Parameters];
            result = CompileResult(waveform);

            // Set the Data
            PlateResult[indexScan].Waveform = waveform;

            PlateResult[indexScan].IntensityA = result[0];
            PlateResult[indexScan].IntensityB = result[1];
            PlateResult[indexScan].Ratio = result[2];
            PlateResult[indexScan].Moment = result[3];
            PlateResult[indexScan].Temperature = temp;
            PlateResult[indexScan].Heatsink = heatsink;

            // Heat Map Data
            block.IntensityA[scan][index] = result[0];
            block.IntensityB[scan][index] = result[1];
            block.Ratio[scan][index] = result[2];
            block.Moment[scan][index] = result[3];
            block.Temperature[scan][index] = temp;

            block.Data[scan][0][index] = result[0];
            block.Data[scan][1][index] = result[1];
            block.Data[scan][2][index] = result[2];
            block.Data[scan][3][index] = result[3];

            // Find Max Value for Flag
            double max = 0;
            PlateResult[indexScan].Flag = CheckMax(waveform, ref max);
            PlateResult[indexScan].Max = max;

        }

        public void SetResultTIA(int scan, int index, int wells, double[] waveform, double[] waveform1, double[] waveform2, double temp, double heatsink)
        {

            // Variables
            int indexScan = (scan * wells) + index;

            // Compile Result
            double[] result = new double[block.Parameters];
            result = CompileResultTIA(waveform1, waveform2);

            // Set the Data
            PlateResult[indexScan].Waveform = waveform;
            //PlateResult[indexScan].Waveform1 = waveform1;
            //PlateResult[indexScan].Waveform2 = waveform2;

            PlateResult[indexScan].IntensityA = result[0];
            PlateResult[indexScan].IntensityB = result[1];
            PlateResult[indexScan].Ratio = result[2];
            PlateResult[indexScan].Moment = result[3];
            PlateResult[indexScan].Temperature = temp;
            PlateResult[indexScan].Heatsink = heatsink;

            // Heat Map Data
            block.IntensityA[scan][index] = result[0];
            block.IntensityB[scan][index] = result[1];
            block.Ratio[scan][index] = result[2];
            block.Moment[scan][index] = result[3];
            block.Temperature[scan][index] = temp;

            block.Data[scan][0][index] = result[0];
            block.Data[scan][1][index] = result[1];
            block.Data[scan][2][index] = result[2];
            block.Data[scan][3][index] = result[3];

            // Find Max Value for Flag
            double max = 0;
            PlateResult[indexScan].Flag = CheckMax(waveform1, ref max);
            PlateResult[indexScan].Max = max;

        }

        // Analysis
        public void SetAnalysisParameters(double wavelengthA, double wavelengthB, double bandA, double bandB)
        {
            // New instance of analysis parameters class
            analysisParameters = new AnalysisParameters();

            if (info.Detector == "TIA")
            {
                analysisParameters.MomentA = 0;
                analysisParameters.MomentB = 10;

                analysisParameters.WavelengthA = 5;
                analysisParameters.WavelengthB = 5;

                analysisParameters.BandA = 10;
                analysisParameters.BandB = 10;

                // Pixel [index]
                double aLow = wavelengthA - (bandA / 2);
                double aHigh = wavelengthA + (bandA / 2);

                double bLow = wavelengthB - (bandB / 2);
                double bHigh = wavelengthB + (bandB / 2);


                analysisParameters.PixelA = convert2Pixel(wavelengthA);
                analysisParameters.PixelA_Low = 0;
                analysisParameters.PixelA_High = info.NPixel;

                analysisParameters.PixelB = convert2Pixel(wavelengthB);
                analysisParameters.PixelB_Low = 0;
                analysisParameters.PixelB_High = info.NPixel;

                analysisParameters.PixelA_Moment = 0;
                analysisParameters.PixelB_Moment = info.NPixel;
            }
            else
            {
                analysisParameters.MomentA = info.WavelengthStart;
                analysisParameters.MomentB = info.WavelengthEnd;

                analysisParameters.WavelengthA = wavelengthA;
                analysisParameters.WavelengthB = wavelengthB;

                analysisParameters.BandA = bandA;
                analysisParameters.BandB = bandB;

                // Pixel [index]
                double aLow = wavelengthA - (bandA / 2);
                double aHigh = wavelengthA + (bandA / 2);

                double bLow = wavelengthB - (bandB / 2);
                double bHigh = wavelengthB + (bandB / 2);


                analysisParameters.PixelA = convert2Pixel(wavelengthA);
                analysisParameters.PixelA_Low = convert2Pixel(aLow);
                analysisParameters.PixelA_High = convert2Pixel(aHigh);

                analysisParameters.PixelB = convert2Pixel(wavelengthB);
                analysisParameters.PixelB_Low = convert2Pixel(bLow);
                analysisParameters.PixelB_High = convert2Pixel(bHigh);

                analysisParameters.PixelA_Moment = convert2Pixel(analysisParameters.MomentA);
                analysisParameters.PixelB_Moment = convert2Pixel(analysisParameters.MomentB);

            }

            // Method convert wavelength to pixel of camera
            int convert2Pixel(double value)
            {
                for (int i = 0; i < info.NPixel; i++)
                {
                    if (value < info.Wavelength[i])
                        return i+1;
                }
                return 0;
            }



        }
                                    
        public double[] CompileResult(double[] data)
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
                a += info.Wavelength[i] * data[i];
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

        public double[] CompileResultTIA(double[] data1, double[] data2)
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
                result[0] += data1[i];
                count++;
            }

            // Intensity B
            int count2 = 0;
            for (int i = idx3; i < idx4; i++)
            {
                result[1] += data2[i];
                count2++;
            }

            // Moment 
            double a = 0;
            double b = 0;

            for (int i = idx5; i < idx6; i++)
            {
                a += info.Wavelength[i] * data1[i];
                b += data1[i];
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


            for (int i = info.StartPixel; i < (info.StartPixel + info.PixelLength); i++)
            {
                if (data[i] > max)
                {
                    max = data[i];
                }
            }

            // See if it is out of range of the detector
            if (max > 65000)
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
