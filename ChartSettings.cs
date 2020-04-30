using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;
using System.Drawing.Drawing2D;


namespace FI.PlateReader.Gen4.JETI
{
    class ChartSettings
    {

        // Color Information for Heat Map
        private int NColors = 1000;


        // Public Variables
        public List<Color> ColorList { get; set; }
        public int[] ColorValue { get; set; }


        // Heat Map Min and Max Values
        public string MinLabel { get; set; }
        public string MaxLabel { get; set; }


        // Current chart selected
        public ChartParameters chartParameters { get; set; }
        public ChartParameters plotParameters { get; set; }

        // List of Chart Parameters used for plotting the different microplate
        public List<ChartParameters> ChartList;
        public class ChartParameters
        {
            public string name;
            public int row;
            public int column;
            public int wells;

            public int xFontSize;
            public int yFontSize;

            public string[] rowLabels;
            public string[] columnLabels;

            // Used in the custom label placement around the chart ("A","B", 1, 3, 5)
            public double rowIntervalStart;
            public int rowIncrement1;
            public int rowIncrement2;

            public double columnIntervalStart;
            public int columnIncrement1;
            public int columnIncrement2;

            public MarkerStyle wsMarkerStyle;
            public MarkerStyle dataMarkerStyle;

            public int wsMarkerSize;
            public int dataMarkerSize;
            public int markerMarkerSize;

            public Color wsNullColor;
            public Color wsActiveColor;

            public Color dataNullColor;
            public Color dataActiveColor;

        }
        

        // Charting
        public void CreateChartSettings()
        {
            // List of Chart Parameters
            ChartList = new List<ChartParameters>();

            // 96 Well Plate
            ChartList.Add(new ChartParameters
            {
                name = "96 Well Plate",
                row = 8,
                column = 12,
                wells = 96,
                wsMarkerSize = 32,
                dataMarkerSize = 48,
                markerMarkerSize = 20,
                xFontSize = 12,
                yFontSize = 12,
                rowIntervalStart = 0,
                rowIncrement1 = 2,
                rowIncrement2 = 1,
                columnIntervalStart = 0,
                columnIncrement1 = 2,
                columnIncrement2 = 1,
                wsMarkerStyle = MarkerStyle.Circle,
                dataMarkerStyle = MarkerStyle.Square,
                wsNullColor = Color.Black,
                wsActiveColor = Color.Red,
                dataNullColor = Color.White,
                dataActiveColor = Color.Red,
                rowLabels = new string[8] { "A", "B", "C", "D", "E", "F", "G", "H" },
                columnLabels = new string[12] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" }
            });


            // 384 Well Plate
            ChartList.Add(new ChartParameters
            {
                name = "384 Well Plate",
                row = 16,
                column = 24,
                wells = 384,
                wsMarkerSize = 18,
                dataMarkerSize = 24,
                markerMarkerSize = 10,
                xFontSize = 7,
                yFontSize = 7,
                rowIntervalStart = 0,
                rowIncrement1 = 2,
                rowIncrement2 = 1,
                columnIntervalStart = 0,
                columnIncrement1 = 2,
                columnIncrement2 = 1,
                wsMarkerStyle = MarkerStyle.Circle,
                dataMarkerStyle = MarkerStyle.Square,
                wsNullColor = Color.Black,
                wsActiveColor = Color.Red,
                dataNullColor = Color.White,
                dataActiveColor = Color.Red,
                rowLabels = new string[16] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P" },
                columnLabels = new string[24] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24" }
            });


            // 1536 Well Plate
            ChartList.Add(new ChartParameters
            {
                name = "1536 Well Plate",
                row = 32,
                column = 48,
                wells = 1536,
                wsMarkerSize = 9,
                dataMarkerSize = 12,
                markerMarkerSize = 5,
                xFontSize = 6,
                yFontSize = 6,
                rowIntervalStart = 0,
                rowIncrement1 = 2,
                rowIncrement2 = 2,
                columnIntervalStart = -0.5,
                columnIncrement1 = 3,
                columnIncrement2 = 2,
                wsMarkerStyle = MarkerStyle.Circle,
                dataMarkerStyle = MarkerStyle.Square,
                wsNullColor = Color.Black,
                wsActiveColor = Color.Red,
                dataNullColor = Color.White,
                dataActiveColor = Color.Red,
                rowLabels = new string[16] { "A", "C", "E", "G", "I", "K", "M", "O", "Q", "S", "U", "W", "Y", "AA", "AC", "AE" },
                columnLabels = new string[24] { "1", "3", "5", "7", "9", "11", "13", "15", "17", "19", "21", "23", "25", "27", "29", "31", "33", "35", "37", "39", "41", "43", "45", "47" }

            });

            // Well Image Plate
            ChartList.Add(new ChartParameters
            {
                name = "Well Image",
                row = 40,
                column = 40,
                wells = 1600,
                wsMarkerSize = 9,
                dataMarkerSize = 12,
                markerMarkerSize = 5,
                xFontSize = 6,
                yFontSize = 6,
                rowIntervalStart = 0,
                rowIncrement1 = 2,
                rowIncrement2 = 2,
                columnIntervalStart = -0.5,
                columnIncrement1 = 3,
                columnIncrement2 = 2,
                wsMarkerStyle = MarkerStyle.Circle,
                dataMarkerStyle = MarkerStyle.Square,
                wsNullColor = Color.Black,
                wsActiveColor = Color.Red,
                dataNullColor = Color.White,
                dataActiveColor = Color.Red,
                rowLabels = new string[20] { "1", "3", "5", "7", "9", "11", "13", "15", "17", "19", "21", "23", "25", "27", "29", "31", "33", "35", "37", "39" },
                columnLabels = new string[20] { "1", "3", "5", "7", "9", "11", "13", "15", "17", "19", "21", "23", "25", "27", "29", "31", "33", "35", "37", "39"}

            });


        }

        public void SetCurrentChart(int value, int value2)
        {
            // Initialize new current Chart Settings class
            chartParameters = new ChartParameters();
            plotParameters = new ChartParameters();

            // Set the class
            chartParameters = ChartList[value];
            plotParameters = ChartList[value2];

        }


        // Colors for Heat Map
        public void CreateColors()
        {

            List<Color> stopColors = new List<Color>();

            stopColors.Add(Color.LightGray);
            stopColors.Add(Color.RoyalBlue);
            stopColors.Add(Color.LightSkyBlue);
            stopColors.Add(Color.LightGreen);
            stopColors.Add(Color.Yellow);
            stopColors.Add(Color.Orange);
            stopColors.Add(Color.Red);

            List<Color> createColors;
            createColors = InterpolateColors(stopColors, NColors + 1);

            ColorList = createColors;

        }

        List<Color> InterpolateColors(List<Color> stopColors, int count)
        {
            SortedDictionary<float, Color> gradient = new SortedDictionary<float, Color>();
            for (int i = 0; i < stopColors.Count; i++)
                gradient.Add(1f * i / (stopColors.Count - 1), stopColors[i]);
            List<Color> ColorList = new List<Color>();

            using (Bitmap bmp = new Bitmap(count, 1))
            using (Graphics G = Graphics.FromImage(bmp))
            {
                Rectangle bmpCRect = new Rectangle(Point.Empty, bmp.Size);
                LinearGradientBrush br = new LinearGradientBrush
                                        (bmpCRect, Color.Empty, Color.Empty, 0, false);
                ColorBlend cb = new ColorBlend();
                cb.Positions = new float[gradient.Count];
                for (int i = 0; i < gradient.Count; i++)
                    cb.Positions[i] = gradient.ElementAt(i).Key;
                cb.Colors = gradient.Values.ToArray();
                br.InterpolationColors = cb;
                G.FillRectangle(br, bmpCRect);
                for (int i = 0; i < count; i++) ColorList.Add(bmp.GetPixel(i, 0));
                br.Dispose();
            }
            return ColorList;
        }

        public void FindHeatMapColors(int value, double[] data)
        {
            // Number of wells
            int Wells = plotParameters.wells;

            // Find Max
            double Max = data.Max();

            // Find Min 
            double Min = Max;

            for(int i = 0; i < Wells; i++)
            {
                // Skip 0 data wells
                if (data[i] < 0.0000001)
                {
                    continue;
                }
                
                // Check to see if there is a new min
                if(data[i] < Min)
                {
                    Min = data[i];
                }
            }

            // Set the Min/Max Labels
            if (value < 2)
            {
                MinLabel = Min.ToString("#,##");
                MaxLabel = Max.ToString("#,##");
            }
            else
            {
                MinLabel = Min.ToString("F2");
                MaxLabel = Max.ToString("F2");
            }


            // Find Colors
            ColorValue = new int[Wells];
            int[] temp = new int[Wells];

            for (int i = 0; i < Wells; i++)
            {
                temp[i] = 0;
                ColorValue[i] = 0;
            }

            // If Min and Max are very small, set all values to 0. 
            double interval = Max - Min;
            if (interval < 0.0001)
            {
                return;
            }

            // Loop through data to set color value for each data point
            for (int i = 0; i < Wells; i++)
            {
                double difference = Max - data[i];
                double position = NColors - (difference / interval) * NColors;

                temp[i] = (int)position;

                if (temp[i] < 0)
                    temp[i] = 0;

                if (temp[i] > NColors)
                    temp[i] = NColors;
            }

            // Set ColorValue variable
            ColorValue = temp;

        }


        // Find Max for waveform chart (Y axis scale)
        public double FindMax(double max)
        {
            if (max < 1000)
            {
                for (int i = 0; i < 100; i++)
                {
                    int value = 10 * (i + 1);

                    if (max < value)
                    {
                        return value;
                    }
                }
            }
            else
            {
                for (int i = 0; i < 65; i++)
                {
                    int value = 1000 * (i + 1);

                    if (max < value)
                    {
                        return value;
                    }
                }
            }

            return 65000;



        }


    }
}
