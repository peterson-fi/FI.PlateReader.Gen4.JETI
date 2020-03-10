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
        Stopwatch stopwatch;

        public string StartDate { get; set; }       // Date of when experiment was started
        public string StartPlateTime { get; set; }  // Start of Plate
        public string EndPlateTime { get; set; }    // End of Plate
        public string PlateTime { get; set; }       // Current Plate time for time label


        // Time Methods
        public void StartTime()
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();

            StartDate = DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.ToString("dd");
            StartPlateTime = DateTime.Now.ToString("HH") + ":" + DateTime.Now.ToString("mm") + ":" + DateTime.Now.ToString("ss");
        }

        public void GetTime()
        {
            PlateTime = stopwatch.Elapsed.ToString(@"m\:ss");

        }

        public void EndTime()
        {
            EndPlateTime = DateTime.Now.ToString("HH") + ":" + DateTime.Now.ToString("mm") + ":" + DateTime.Now.ToString("ss");

        }

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
