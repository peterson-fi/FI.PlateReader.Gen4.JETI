using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.PlateReader.Gen4.JETI
{
    class PlateSetup
    {
       
        // If wells are selected in well selection chart
        public bool WellsSelected { get; set; }


        // Input Variables (Well Selection Chart)
        public int RowSelection1 { get; set; }
        public int RowSelection2 { get; set; }
        public int ColumnSelection1 { get; set; }
        public int ColumnSelection2 { get; set; }

        public int RowMin { get; set; }
        public int RowMax { get; set; }
        public int ColumnMin { get; set; }
        public int ColumnMax { get; set; }


        // Export Variables
        public bool[] ActiveRow { get; set; }
        public bool[] ActiveColumn { get; set; }
        public int ActiveWells { get; set; }
   


        // Methods
        public void SetActiveWells(int row, int column)
        {
            // Create array of active wells 
            ActiveRow = new bool[row];
            ActiveColumn = new bool[column];

            // Min and Max values from the User
            RowMin = Math.Min(RowSelection1, RowSelection2);
            RowMax = Math.Max(RowSelection1, RowSelection2);

            ColumnMin = Math.Min(ColumnSelection1, ColumnSelection2);
            ColumnMax = Math.Max(ColumnSelection1, ColumnSelection2);

            // Compute Active Row (Rows you are going to scan in the static scan method)
            int rowCount = 0;

            for (int i = 0; i < row; i++)
            {
                if (i < RowMin || i > RowMax)
                {
                    ActiveRow[i] = false;
                }
                else
                {
                    rowCount++;
                    ActiveRow[i] = true;
                }
            }
            
            // Compute Active Columns (Columns that will be scanned in static scan method)
            int columnCount = 0;

            for (int i = 0; i < column; i++)
            {
                if (i < ColumnMin || i > ColumnMax)
                {
                    ActiveColumn[i] = false;
                }
                else
                {
                    columnCount++;
                    ActiveColumn[i] = true;
                }
            }

            // Compute the number of active wells for data export 
            ActiveWells = rowCount * columnCount;
        }


       


    }
}
