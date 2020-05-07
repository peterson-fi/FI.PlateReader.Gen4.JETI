using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;



namespace FI.PlateReader.Gen4.JETI
{
    class DataExport
    {

        // Data Export Variables
        public bool Save { get; set; }

        public string Filename { get; set; }
        public string Filepath { get; set; }

        // Config Files
        public Settings.Info info;

        // Microplate
        public Microplate.Plate plate;
        public Microplate.Motor motor;

        // Data from scan
        public Data.AnalysisParameters analysisParameters;
        public List<Data.Well> PlateResult;
        public Data.Block block;

        // LED
        public string LED { get; set; }
        public string LedCurrent { get; set; }

        // Detector
        public string Detector { get; set; }
        public double Integration { get; set; }

        // Active Row/Column
        public bool[] ActiveRow { get; set; }
        public bool[] ActiveColumn { get; set; }
        public int Samples { get; set; }

        // Time Info
        public string StartDate { get; set; }
        public string StartPlateTime { get; set; }
        public string EndPlateTime { get; set; }

        public string StartScanTime { get; set; }
        public string EndScanTime { get; set; }


        // Scan info
        public int CurrentScan { get; set; }
        public int NScans { get; set; }
        public int Delay { get; set; }


        // Well Image
        public WellImage.Motor iMotor;
        public WellImage.Plate iPlate;
        public int RowPixel { get; set; }
        public int ColumnPixel { get; set; }



        // Save Methods
        public void SavePlate(int scan)
        {
            if (Save)
            {   
              SpectrometerBlockExport(scan);
              SpectrometerWaveformExport(scan);

            }
        }
 
        public void SaveScan()
        {
            if (Save)
            {
                SpectrometerScanExport();
            }
        }

        public void SaveImage(string wellName)
        {
            if (Save)
            {
                SpectrometerImageExport(wellName);
            }
        }



        // File Types
        public void SpectrometerBlockExport(int scan)
        {
            // Variables
            string fnameTemp = "";
            string fnameBlock = "";

            // Create Filename
            string dirSub = Path.Combine(Filepath, Filename);

            if (Directory.Exists(dirSub) == false)
                System.IO.Directory.CreateDirectory(dirSub);

            if(NScans > 1)
            {
                fnameTemp = Filename + "_Block" + "_" + (scan + 1).ToString() + ".txt";
                fnameBlock = Path.Combine(dirSub, fnameTemp);
            }
            else
            {
                fnameTemp = Filename + "_Block.txt";
                fnameBlock = Path.Combine(dirSub, fnameTemp);
            }
            

            // Write out Header information
            WritePlateHeaderSpectrometer(fnameBlock);
            string[] param;

            // 1st Block of Data: Intensity A
            param = new string[2];
            param[0] = "Intensity A";
            param[1] = analysisParameters.WavelengthA.ToString();
            WriteBlock(fnameBlock, param, block.IntensityA[scan]);

            // 2nd Block of Data: Intensity B
            param = new string[2];
            param[0] = "Intensity B";
            param[1] = analysisParameters.WavelengthB.ToString();
            WriteBlock(fnameBlock, param, block.IntensityB[scan]);

            // 3rd Block of Data: Ratio
            param = new string[2];
            param[0] = "Ratio";
            param[1] = analysisParameters.WavelengthA.ToString() + "/" + analysisParameters.WavelengthB.ToString();
            WriteBlock(fnameBlock, param, block.Ratio[scan]);

            // 4th Block of Data: Moment
            param = new string[2];
            param[0] = "Moment";
            param[1] = analysisParameters.MomentA.ToString() + "-" + analysisParameters.MomentB.ToString();
            WriteBlock(fnameBlock, param, block.Moment[scan]);


        }

        public void SpectrometerWaveformExport(int scan)
        {
            // Variables
            string fnameTemp = "";
            string fnameWaveform = "";

            // Create Filename
            string dirSub = Path.Combine(Filepath, Filename);

            if (Directory.Exists(dirSub) == false)
                System.IO.Directory.CreateDirectory(dirSub);

            if (NScans > 1)
            {
                fnameTemp = Filename + "_Waveform" + "_" + (scan+1).ToString() + ".txt";
                fnameWaveform = Path.Combine(dirSub, fnameTemp);
            }
            else
            {
                fnameTemp = Filename + "_Waveform.txt";
                fnameWaveform = Path.Combine(dirSub, fnameTemp);
            }

            // Write out Header information
            WritePlateHeaderSpectrometer(fnameWaveform);

            // Write Waveform
            WriteWaveform(scan, fnameWaveform);
        }

        public void SpectrometerScanExport()
        {
            // Variables
            string fnameTemp = "";
            string fnameWaveform = "";

            // Create Filename
            string dirSub = Path.Combine(Filepath, Filename);

            if (Directory.Exists(dirSub) == false)
                System.IO.Directory.CreateDirectory(dirSub);

            fnameTemp = Filename + "_All.txt";
            fnameWaveform = Path.Combine(dirSub, fnameTemp);

            // Write out Header information
            WriteScanHeaderSpectrometer(fnameWaveform);

            // Write Waveform
            WriteScan(fnameWaveform);
        }

        public void SpectrometerImageExport(string wellName)
        {
            // Variables
            string fnameTemp = "";
            string fnameBlock = "";

            // Create Filename
            string dirSub = Path.Combine(Filepath, Filename);

            if (Directory.Exists(dirSub) == false)
                System.IO.Directory.CreateDirectory(dirSub);

            fnameTemp = Filename + "_" + wellName + ".txt";
            fnameBlock = Path.Combine(dirSub, fnameTemp);

            // Write out Header information
            WritePlateHeaderSpectrometer(fnameBlock);
            string[] param;

            // 1st Block of Data: Intensity A
            param = new string[2];
            param[0] = "Intensity A";
            param[1] = analysisParameters.WavelengthA.ToString();
            WriteImage(fnameBlock, param, block.IntensityA[0]);

            // 2nd Block of Data: Intensity B
            param = new string[2];
            param[0] = "Intensity B";
            param[1] = analysisParameters.WavelengthB.ToString();
            WriteImage(fnameBlock, param, block.IntensityB[0]);

            // 3rd Block of Data: Ratio
            param = new string[2];
            param[0] = "Ratio";
            param[1] = analysisParameters.WavelengthA.ToString() + "/" + analysisParameters.WavelengthB.ToString();
            WriteImage(fnameBlock, param, block.Ratio[0]);

            // 4th Block of Data: Moment
            param = new string[2];
            param[0] = "Moment";
            param[1] = analysisParameters.MomentA.ToString() + "-" + analysisParameters.MomentB.ToString();
            WriteImage(fnameBlock, param, block.Moment[0]);
        }



        // Data Write Methods
        private void WritePlateHeaderSpectrometer(string filename)
        {
            string line;

            using (StreamWriter file = new StreamWriter(@filename))
            {
                // Write Information about the scan
                line = string.Format("{0}\t{1}\t", "Instrument", "SUPR");
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t", "Software", "Version 1.0");
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t", "Filename", filename);
                file.WriteLine(line);


                line = string.Format("{0}\t{1}\t", "Date", StartDate);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t", "Start Plate Time", StartPlateTime);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t", "End Plate Time", EndPlateTime);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t{2}\t", "Offset", 29, 9);
                file.WriteLine(line);

                file.WriteLine();
                file.WriteLine();

                line = string.Format("{0}\t{1}\t", "Plate", plate.Name);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t{2}\t", "Instrument Offset [mm]", info.RowOffset, info.ColumnOffset);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t{2}\t", "A1 Offset [mm]", plate.RowOffset, plate.ColumnOffset);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t", "Rows", plate.Row);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t", "Columns", plate.Column);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t", "Samples", Samples);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t", "Wavelengths", info.PixelLength);
                file.WriteLine(line);

                file.WriteLine();
                file.WriteLine();

                line = string.Format("{0}\t{1}\t", "LED", LED);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t", "Detector", Detector);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t", "LED Curent [mA]", LedCurrent);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t", "Integration Time [ms]", Integration);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t{2}\t{3}\t", "Wavelength A [nm]", analysisParameters.WavelengthA,"Wavelength Band A [nm]",analysisParameters.BandA);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t{2}\t{3}\t", "Wavelength B [nm]", analysisParameters.WavelengthB, "Wavelength Band B [nm]", analysisParameters.BandB);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t{2}\t", "Ratio [Wavelength A / Wavelength B]", analysisParameters.WavelengthA, analysisParameters.WavelengthB);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t{2}\t", "Moment [nm]", analysisParameters.MomentA, analysisParameters.MomentB);
                file.WriteLine(line);

                file.WriteLine();
                file.WriteLine();
            }
        }

        private void WriteScanHeaderSpectrometer(string filename)
        {
            string line;

            using (StreamWriter file = new StreamWriter(@filename))
            {
                // Write Information about the scan
                line = string.Format("{0}\t{1}\t", "Instrument", "SUPR");
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t", "Software", "Version 1.0");
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t", "Filename", filename);
                file.WriteLine(line);


                line = string.Format("{0}\t{1}\t", "Date", StartDate);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t", "Start Scan Time", StartScanTime);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t", "End Scan Time", EndScanTime);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t{2}\t", "Offset", 29, 9);
                file.WriteLine(line);

                file.WriteLine();
                file.WriteLine();

                line = string.Format("{0}\t{1}\t", "Plate", plate.Name);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t{2}\t", "Instrument Offset [mm]", info.RowOffset, info.ColumnOffset);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t{2}\t", "A1 Offset [mm]", plate.RowOffset, plate.ColumnOffset);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t", "Rows", plate.Row);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t", "Columns", plate.Column);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t", "Samples", Samples);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t", "Wavelengths", info.PixelLength);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t", "N Scans", NScans);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t", "Delay [s]", Delay);
                file.WriteLine(line);

                file.WriteLine();
                file.WriteLine();

                line = string.Format("{0}\t{1}\t", "LED", LED);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t", "Detector", Detector);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t", "LED Current [mA]", LedCurrent);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t", "Integration Time [ms]", Integration);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t{2}\t{3}\t", "Wavelength A [nm]", analysisParameters.WavelengthA, "Wavelength Band A [nm]", analysisParameters.BandA);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t{2}\t{3}\t", "Wavelength B [nm]", analysisParameters.WavelengthB, "Wavelength Band B [nm]", analysisParameters.BandB);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t{2}\t", "Ratio [Wavelength A / Wavelength B]", analysisParameters.WavelengthA, analysisParameters.WavelengthB);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t{2}\t", "Moment [nm]", analysisParameters.MomentA, analysisParameters.MomentB);
                file.WriteLine(line);

                file.WriteLine();
                file.WriteLine();
            }
        }

        private void WriteBlock(string filename, string[] parameter, double[] data)
        {
            // Variables
            string line;
            int count;

            using (StreamWriter file = File.AppendText(@filename))
            {
                line = string.Format("{0}\t{1}\t{2}\t", "Parameter", parameter[0], parameter[1]);
                file.WriteLine(line);

                line = string.Format("{0}\t", "");
                file.Write(line);

                // Write out the columns
                for (int i = 0; i < plate.Column; i++)
                {
                    line = string.Format("{0}\t", i + 1);
                    file.Write(line);
                }
                file.Write("\n");

                // Loop through the data
                count = 0;

                for (int i = 0; i < plate.Row; i++)
                {
                    // Write out the Row
                    line = string.Format("{0}\t", ConvertRow(i));
                    file.Write(line);

                    for (int j = 0; j < plate.Column; j++)
                    {
                        // Write out the Data
                        line = string.Format("{0:.000000}\t", data[count]);
                        file.Write(line);
                        count++;
                    }
                    file.WriteLine();
                }

                file.WriteLine();

            }
        }

        private void WriteWaveform(int scan, string filename)
        {
            using (StreamWriter file = File.AppendText(@filename))
            {
                // Variables
                string line;
                int start = info.StartPixel;
                int pixelLength = info.PixelLength;

                // Wavelength
                line = string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t", "", "", "", "", "", "","","","Wavelength [nm]");
                file.Write(line);

                for (int i = start; i < (start + pixelLength); i++)
                {
                    // Write out wavelengths
                    file.Write(info.Wavelength[i] + "\t");
                }

                file.WriteLine();

                // Header
                line = string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t", "Index", "Row", "Column","Intensity A", "Intensity B", "Ratio", "Moment", "Temperature", "Heatsink");
                file.WriteLine(line);

                // Loop through Data
                for (int i = 0; i < plate.Row; i++)
                {
                    // Skip inactive Row
                    if (!ActiveRow[i])
                        continue;

                    for (int j = 0; j < plate.Column; j++)
                    {
                        // Skip inactive Column
                        if (!ActiveColumn[j])
                            continue;

                        int index = scan * plate.Wells + (i * plate.Column) + j;
                        string row = ConvertRow(i);
                        int column = j + 1;
                        string sample = PlateResult[index].Name;
                        double concentration = PlateResult[index].Concentration;
                        double value1 = PlateResult[index].IntensityA;
                        double value2 = PlateResult[index].IntensityB;
                        double value3 = PlateResult[index].Ratio;
                        double value4 = PlateResult[index].Moment;
                        double value5 = PlateResult[index].Temperature;
                        double value6 = PlateResult[index].Heatsink;
              
                        line = string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t", index + 1, row, column, value1, value2, value3,value4, value5, value6);
                        file.Write(line);

                        for (int k = start; k < (start + pixelLength); k++)
                        {
                            file.Write(PlateResult[index].Waveform[k] + "\t");
                        }

                        file.WriteLine();
                    }
                }
            }

        }

        private void WriteScan(string filename)
        {
            using (StreamWriter file = File.AppendText(@filename))
            {
                // Variables
                string line;
                int start = info.StartPixel;
                int pixelLength = info.PixelLength;

                // Wavelength
                line = string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t","", "", "", "", "", "", "", "", "", "", "Wavelength [nm]");
                file.Write(line);

                for (int i = start; i < (start + pixelLength); i++)
                {
                    // Write out wavelengths
                    file.Write(info.Wavelength[i] + "\t");
                }

                file.WriteLine();

                // Header
                line = string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t", "Scan", "Scan Index", "Index", "Row", "Column", "Temperature", "Heatsink", "Intensity A", "Intensity B", "Ratio", "Moment");
                file.WriteLine(line);

                // Loop through Data
                for (int i = 0; i < plate.Row; i++)
                {
                    // Skip inactive Row
                    if (!ActiveRow[i])
                        continue;

                    for (int j = 0; j < plate.Column; j++)
                    {
                        // Skip inactive Column
                        if (!ActiveColumn[j])
                            continue;

                        for (int n = 0; n < NScans; n++)
                        {

                            int idx = i * plate.Column + j;
                            int index = n * plate.Wells + (i * plate.Column + j);
                            string row = ConvertRow(i);
                            int column = j + 1;
                            string sample = PlateResult[index].Name;
                            double concentration = PlateResult[index].Concentration;
                            double value1 = PlateResult[index].IntensityA;
                            double value2 = PlateResult[index].IntensityB;
                            double value3 = PlateResult[index].Ratio;
                            double value4 = PlateResult[index].Moment;
                            double value5 = PlateResult[index].Temperature;
                            double value6 = PlateResult[index].Heatsink;

                            line = string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t", n + 1, index + 1, idx + 1, row, column, value5, value6, value1, value2, value3, value4);
                            file.Write(line);

                            for (int k = start; k < (start + pixelLength); k++)
                            {
                                file.Write(PlateResult[index].Waveform[k] + "\t");
                            }

                            file.WriteLine();
                        }
                    }
                }



            }

        }

        private void WriteImage(string filename, string[] parameter, double[] data)
        {
            // Variables
            string line;
            int count;

            using (StreamWriter file = File.AppendText(@filename))
            {
                line = string.Format("{0}\t{1}\t{2}\t", "Parameter", parameter[0], parameter[1]);
                file.WriteLine(line);

                // Write out the columns
                line = string.Format("{0}\t{1}\t{2}\t", "", "", "");
                file.Write(line);

                for (int i = 0; i < ColumnPixel; i++)
                {
                    line = string.Format("{0}\t", i + 1);
                    file.Write(line);
                }
                file.Write("\n");

                // Write out the column motor positions
                line = string.Format("{0}\t{1}\t{2}\t", "", "", "");
                file.Write(line);

                for (int i = 0; i < ColumnPixel; i++)
                {
                    line = string.Format("{0}\t", iMotor.ColumnPosition[i] - (info.ColumnDirection * iPlate.ColumnOffset));
                    file.Write(line);
                }

                file.Write("\n");
                file.Write("\n");

                // Loop through the data
                count = 0;

                for (int i = 0; i < RowPixel; i++)
                {
                    // Write out the Row
                    line = string.Format("{0}\t", ConvertRow(i));
                    file.Write(line);

                    // Write out the Row motor position
                    line = string.Format("{0}\t{1}\t", iMotor.RowPosition[i] - (info.RowDirection * iPlate.RowOffset), "");
                    file.Write(line);

                    for (int j = 0; j < ColumnPixel; j++)
                    {
                        // Write out the Data
                        line = string.Format("{0:.000000}\t", data[count]);
                        file.Write(line);
                        count++;
                    }
                    file.WriteLine();
                }

                file.WriteLine();

            }
        }



        // Misc Methods
        public string ConvertRow(int value)
        {
            // Converts Int Row Value to String Row Value (Used for Well Selection Mouse Operation)

            // Row Values
            string[] rowValue = new string[50];

            if (value > 49) { value = 49; }

            if (value < 0) { value = 0; }

            rowValue[0] = "A"; rowValue[1] = "B"; rowValue[2] = "C";
            rowValue[3] = "D"; rowValue[4] = "E"; rowValue[5] = "F";
            rowValue[6] = "G"; rowValue[7] = "H"; rowValue[8] = "I";
            rowValue[9] = "J"; rowValue[10] = "K"; rowValue[11] = "L";
            rowValue[12] = "M"; rowValue[13] = "N"; rowValue[14] = "O";
            rowValue[15] = "P"; rowValue[16] = "Q"; rowValue[17] = "R";
            rowValue[18] = "S"; rowValue[19] = "T"; rowValue[20] = "U";
            rowValue[21] = "V"; rowValue[22] = "W"; rowValue[23] = "X";
            rowValue[24] = "Y"; rowValue[25] = "Z"; rowValue[26] = "AA";
            rowValue[27] = "AB"; rowValue[28] = "AC"; rowValue[29] = "AD";
            rowValue[30] = "AE"; rowValue[31] = "AF"; rowValue[32] = "AG";
            rowValue[33] = "AH"; rowValue[34] = "AI"; rowValue[35] = "AJ";
            rowValue[36] = "AK"; rowValue[37] = "AL"; rowValue[38] = "AM";
            rowValue[39] = "AN"; rowValue[40] = "AO"; rowValue[41] = "AP";
            rowValue[42] = "AQ"; rowValue[43] = "AR"; rowValue[41] = "AS";
            rowValue[45] = "AT"; rowValue[46] = "AU"; rowValue[41] = "AV";
            rowValue[48] = "AW"; rowValue[49] = "AX";

            // Return Row Value
            return rowValue[value];
        }


    }
}
