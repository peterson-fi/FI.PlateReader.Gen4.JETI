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
        

        public PlateInfo plateInfo;
        public class PlateInfo
        {
            // File Location
            public string Filename;
            public string Filepath;

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
            public string LED;
            public int LedPower;

            // Detector
            public string Detector; 
            public int Integration;

            // Active Row/Column
            public bool[] ActiveRow;
            public bool[] ActiveColumn;
            public int Samples;            

            // Time Info
            public string StartDate;
            public string StartPlateTime;
            public string EndPlateTime;

        }
                                    

        // Save Methods
        public void SavePlate()
        {
            if (Save)
            {   
              SpectrometerBlockExport();
              SpectrometerWaveformExport();

            }
        }
 
        public void SpectrometerBlockExport()
        {
            // Create Filename
            string dirSub = Path.Combine(plateInfo.Filepath);   //, plateInfo.Filename);

            if (Directory.Exists(dirSub) == false)
                System.IO.Directory.CreateDirectory(dirSub);

            string fnameTemp = plateInfo.Filename + "_Block.txt";
            //string fnameBlock = Path.Combine(plateInfo.Filepath, plateInfo.Filename, fnameTemp);
            string fnameBlock = Path.Combine(plateInfo.Filepath, fnameTemp);


            // Write out Header information
            WritePlateHeaderSpectrometer(fnameBlock);
            string[] param;

            // 1st Block of Data: Intensity A
            param = new string[2];
            param[0] = "Intensity A";
            param[1] = plateInfo.analysisParameters.WavelengthA.ToString();
            WriteBlock(fnameBlock, param, plateInfo.block.IntensityA);

            // 2nd Block of Data: Intensity B
            param = new string[2];
            param[0] = "Intensity B";
            param[1] = plateInfo.analysisParameters.WavelengthB.ToString();
            WriteBlock(fnameBlock, param, plateInfo.block.IntensityB);

            // 3rd Block of Data: Ratio
            param = new string[2];
            param[0] = "Ratio";
            param[1] = plateInfo.analysisParameters.WavelengthA.ToString() + "/" + plateInfo.analysisParameters.WavelengthB.ToString();
            WriteBlock(fnameBlock, param, plateInfo.block.Ratio);

            // 4th Block of Data: Moment
            param = new string[2];
            param[0] = "Moment";
            param[1] = plateInfo.analysisParameters.MomentA.ToString() + "-" + plateInfo.analysisParameters.MomentB.ToString();
            WriteBlock(fnameBlock, param, plateInfo.block.Moment);


        }

        public void SpectrometerWaveformExport()
        {

            // Create Filename
            string dirSub = Path.Combine(plateInfo.Filepath);   //, plateInfo.Filename);

            if (Directory.Exists(dirSub) == false)
                System.IO.Directory.CreateDirectory(dirSub);

            string fnameTemp = plateInfo.Filename + "_Waveform.txt";
            //string fnameBlock = Path.Combine(plateInfo.Filepath, plateInfo.Filename, fnameTemp);
            string fnameWaveform = Path.Combine(plateInfo.Filepath, fnameTemp);

            // Write out Header information
            WritePlateHeaderSpectrometer(fnameWaveform);

            // Write Sensor 1
            WriteWaveform(fnameWaveform);
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


                line = string.Format("{0}\t{1}\t", "Date", plateInfo.StartDate);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t", "Start Plate Time", plateInfo.StartPlateTime);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t", "End Plate Time", plateInfo.EndPlateTime);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t{2}\t", "Offset", 29, 9);
                file.WriteLine(line);

                file.WriteLine();
                file.WriteLine();

                line = string.Format("{0}\t{1}\t", "Plate", plateInfo.plate.Name);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t{2}\t", "Instrument Offset [mm]", plateInfo.info.RowOffset, plateInfo.info.ColumnOffset);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t{2}\t", "A1 Offset [mm]", plateInfo.plate.RowOffset, plateInfo.plate.ColumnOffset);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t", "Rows", plateInfo.plate.Row);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t", "Columns", plateInfo.plate.Column);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t", "Samples", plateInfo.Samples);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t", "Wavelengths", plateInfo.info.ActiveSize);
                file.WriteLine(line);

                file.WriteLine();
                file.WriteLine();

                line = string.Format("{0}\t{1}\t", "LED", plateInfo.LED);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t", "Detector", plateInfo.Detector);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t", "LED Power [%]", plateInfo.LedPower);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t", "Integration Time [ms]", plateInfo.Integration);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t{2}\t{3}\t", "Wavelength A [nm]", plateInfo.analysisParameters.WavelengthA,"Wavelength Band A [nm]",plateInfo.analysisParameters.BandA);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t{2}\t{3}\t", "Wavelength B [nm]", plateInfo.analysisParameters.WavelengthB, "Wavelength Band B [nm]", plateInfo.analysisParameters.BandB);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t{2}\t", "Ratio [Wavelength A / Wavelength B]", plateInfo.analysisParameters.WavelengthA, plateInfo.analysisParameters.WavelengthB);
                file.WriteLine(line);

                line = string.Format("{0}\t{1}\t{2}\t", "Moment [nm]", plateInfo.analysisParameters.MomentA, plateInfo.analysisParameters.MomentB);
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
                for (int i = 0; i < plateInfo.plate.Column; i++)
                {
                    line = string.Format("{0}\t", i + 1);
                    file.Write(line);
                }
                file.Write("\n");

                // Loop through the data
                count = 0;

                for (int i = 0; i < plateInfo.plate.Row; i++)
                {
                    // Write out the Row
                    line = string.Format("{0}\t", ConvertRow(i));
                    file.Write(line);

                    for (int j = 0; j < plateInfo.plate.Column; j++)
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

        private void WriteWaveform(string filename)
        {
            using (StreamWriter file = File.AppendText(@filename))
            {
                // Variables
                string line;
                int start = plateInfo.info.PixelStart;
                int end = plateInfo.info.PixelEnd;

                // Wavelength
                line = string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t", "", "", "", "", "", "", "","Wavelength [nm]");
                file.Write(line);

                for (int i = start; i < end; i++)
                {
                    // Write out wavelengths
                    file.Write(plateInfo.info.Wavelength[i] + "\t");
                }

                file.WriteLine();

                // Header
                line = string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t", "Index", "Row", "Column","Intensity A", "Intensity B", "Ratio", "Moment");
                file.WriteLine(line);

                // Loop through Data
                for (int i = 0; i < plateInfo.plate.Row; i++)
                {
                    // Skip inactive Row
                    if (!plateInfo.ActiveRow[i])
                        continue;

                    for (int j = 0; j < plateInfo.plate.Column; j++)
                    {
                        // Skip inactive Column
                        if (!plateInfo.ActiveColumn[j])
                            continue;

                        int index = (i * plateInfo.plate.Column) + j;
                        string row = ConvertRow(i);
                        int column = j + 1;
                        string sample = plateInfo.PlateResult[index].Name;
                        double concentration = plateInfo.PlateResult[index].Concentration;
                        double value1 = plateInfo.PlateResult[index].IntensityA;
                        double value2 = plateInfo.PlateResult[index].IntensityB;
                        double value3 = plateInfo.PlateResult[index].Ratio;
                        double value4 = plateInfo.PlateResult[index].Moment;
              
                        line = string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t", index + 1, row, column, value1, value2, value3,value4,"");
                        file.Write(line);

                        for (int k = start; k < end; k++)
                        {
                            file.Write(plateInfo.PlateResult[index].Waveform[k] + "\t");
                        }

                        file.WriteLine();
                    }
                }

                file.WriteLine();
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
