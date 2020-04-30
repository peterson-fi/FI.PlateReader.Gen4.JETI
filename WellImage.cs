using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.PlateReader.Gen4.JETI
{
    class WellImage
    {
        // External Information
        public Settings.Info info;

        // The current microplate selected
        public Plate plate { get; set; }
        public Motor motor { get; set; }

        // Well Image
        public int PlateFormat { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public int RowPixel { get; set; }
        public int ColumnPixel { get; set; }
        public int Wells { get; set; }

        // List of Microplates
        public List<Plate> PlateList;
        public class Plate
        {
            // Name of Plate
            public string Name;

            //  Plate Information
            public int Wells;
            public int Row;
            public double RowSpacing;
            public int Column;
            public double ColumnSpacing;

            // Plate Location
            public double RowOffset;
            public double ColumnOffset;
            public double ZOffset;


        }


        // List of Motor positions for the different plates created
        public List<Motor> MotorList;
        public class Motor
        {
            // Reference Position (Bevel)
            public double RowReference;
            public double ColumnReference;

            // Motor Positions
            public double[] RowPosition;
            public double[] ColumnPosition;

            // Step Size
            public double RowStepSize;
            public double ColumnStepSize;

        }


        // Methods
        public void CreatePlates()
        {
            // Create new list of plates
            PlateList = new List<Plate>();

            PlateList.Add(new Plate
            {
                Name = "96 Well Plate",
                Wells = 96,
                Row = 8,
                Column = 12,
                ColumnOffset = 9.88,    //14.38,
                ColumnSpacing = 9,
                RowOffset = 11.24,
                RowSpacing = 9,
                ZOffset = 0,

            });

            PlateList.Add(new Plate
            {
                Name = "384 Well Plate",
                Wells = 384,
                Row = 16,
                Column = 24,
                ColumnOffset = 7.63,    //12.13,
                ColumnSpacing = 4.5,
                RowOffset = 8.99,
                RowSpacing = 4.5,
                ZOffset = 0,

            });

            PlateList.Add(new Plate
            {
                Name = "1536 Well Plate",
                Wells = 1536,
                Row = 32,
                Column = 48,
                ColumnOffset = 6.5005,  //11.005,
                ColumnSpacing = 2.25,
                RowOffset = 7.865,
                RowSpacing = 2.25,
                ZOffset = 0,

            });


            // Create the motor positions for the plates
            CreateMotorPositions();


        }

        public void CreateMotorPositions()
        {
            // Create new list of Motor Positions
            MotorList = new List<Motor>();

            // Get number of plates created
            int nPlates = PlateList.Count;

            for (int i = 0; i < nPlates; i++)
            {
                // Create new Motor class and add it to the list
                MotorList.Add(new Motor());

                // Get the row and column positions for each individual well
                double[] rowPositon = new double[RowPixel];
                double[] columnPosition = new double[ColumnPixel];

                GetPosition(i, ref rowPositon, ref columnPosition);

                // Add info to Class
                MotorList[i].RowReference = info.RowOffset;
                MotorList[i].ColumnReference = info.ColumnOffset;

                MotorList[i].RowPosition = rowPositon;
                MotorList[i].ColumnPosition = columnPosition;

                MotorList[i].RowStepSize = PlateList[i].RowSpacing / RowPixel;
                MotorList[i].ColumnStepSize = PlateList[i].ColumnSpacing / ColumnPixel;

            }

        }

        public void GetPosition(int count, ref double[] rowPosition, ref double[] columnPosition)
        {
            // Row,Column Stage Direction (Determines if you are stepping forward or background, instrument/stage design dependent)
            int rDir = info.RowDirection;
            int cDir = info.ColumnDirection;

            // Get info from Plate Class
            int row = RowPixel;
            int column = ColumnPixel;

            double RowStep = PlateList[count].RowSpacing / row;
            double ColumnStep = PlateList[count].ColumnSpacing / column;

            double plateRowOffset = PlateList[count].RowOffset;
            double plateColumnOffset = PlateList[count].ColumnOffset;

            // Step Half Well to Offset the image
            plateRowOffset = plateRowOffset - rDir * PlateList[count].RowSpacing / 2;
            plateColumnOffset = plateColumnOffset - cDir * PlateList[count].ColumnSpacing / 2;

            // Offset for what well you want to image
            plateRowOffset = plateRowOffset + rDir * (Row * PlateList[count].RowSpacing);
            plateColumnOffset = plateColumnOffset + cDir * (Column * PlateList[count].ColumnSpacing);


            // Row Positions
            double rowOffset = info.RowOffset + rDir * plateRowOffset; // Reference and Plate Offset Combined

            for (int j = 0; j < row; j++)
            {
                rowPosition[j] = rowOffset + rDir * (j * RowStep);
            }


            // Column Positions
            double columnOffset = info.ColumnOffset + cDir * plateColumnOffset;

            for (int j = 0; j < column; j++)
            {
                columnPosition[j] = columnOffset + cDir * (j * ColumnStep);

            }

        }

        public void SetCurrentPlate(int value)
        {
            // Initialize new current Plate class
            plate = new Plate();
            motor = new Motor();

            // Set the class to current plate selected
            plate = PlateList[value];
            motor = MotorList[value];

        }




    }
}
