using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;


namespace FI.PlateReader.Gen4.JETI
{
    class Time
    {

        // Time Variables
        Stopwatch plateStopwatch;
        Stopwatch scanStopwatch;


        public string StartDate { get; set; }       // Date of when experiment was started
        
        public string StartPlateTime { get; set; }  // Start of Plate
        public string EndPlateTime { get; set; }    // End of Plate
        public string PlateTime { get; set; }       // Current Plate Time

        public string StartScanTime { get; set; }   // Start of Scan
        public string EndScanTime { get; set; }     // End of Scan
        public string ScanTime { get; set; }        // Total Scan time



        

        // Plate Time Methods
        public void StartPlateStopwatch()
        {
            plateStopwatch = new Stopwatch();
            plateStopwatch.Start();

            StartPlateTime = DateTime.Now.ToString("HH") + ":" + DateTime.Now.ToString("mm") + ":" + DateTime.Now.ToString("ss");
        }

        public void EndPlateStopwatch()
        {
            EndPlateTime = DateTime.Now.ToString("HH") + ":" + DateTime.Now.ToString("mm") + ":" + DateTime.Now.ToString("ss");

        }

        public void GetPlateTime()
        {
            PlateTime = plateStopwatch.Elapsed.ToString(@"m\:ss");
        }


        // Scan Time Methods
        public void StartScanStopwatch()
        {
            scanStopwatch = new Stopwatch();
            scanStopwatch.Start();

            StartDate = DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.ToString("dd");
            StartScanTime = DateTime.Now.ToString("HH") + ":" + DateTime.Now.ToString("mm") + ":" + DateTime.Now.ToString("ss");
        }

        public void EndScanStopwatch()
        {
            EndScanTime = DateTime.Now.ToString("HH") + ":" + DateTime.Now.ToString("mm") + ":" + DateTime.Now.ToString("ss");
        }
               
        public void GetScanTime()
        {
            ScanTime = scanStopwatch.Elapsed.ToString(@"m\:ss");
        }


        // Delay
        public void Delay(int ms)
        {
            /*
             * Thread.Sleep(ms)
             * Task.Delay(ms).Wait();
             */

            int timeout = 10000;

            var t = Task.Run(async () =>
            {
                await Task.Delay(ms);
            });

            t.Wait(timeout);
            
        }



        
    }
}
