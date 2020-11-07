using System;
using System.IO;
using System.Collections.Generic;


namespace OSM
{
    class Space
    {
        public List<Floor> Floors { get; }

        // fileName
        public string FileName { get; set; }

        private Reporter reporter;

        public Space()
        {
            Floors = new List<Floor>();
        }

        // init floors
        public Space Init(bool report)
        {
            Util.kAllPed = 0;
            Util.kCumPeds = 0;
            //Util.kTimerTick = 0;
            Floors.Clear();
            Util.kId = 0;
            for (int i = 0; i < Util.kFloorNum; i++)
            {
                var floor = new Floor(i);
                //if (i > 0)
                //    Util.InitRandomPed(floor, this);
                Floors.Add(floor);
            }
            if (Util.kFloorNum >= 10)
                Util.kScale = 14.0;
            else
                Util.kScale = 14.0 + (10 - Util.kFloorNum) * 2.0;




            if (Util.kFloorNum > 40)
            {
                Util.kWidth = 0;
                Util.kHeight = 0;
            }
            else if (Util.kFloorNum > 30)
            {
                Util.kWidth = 0;
                Util.kHeight = 7.0;
            }
            else if (Util.kFloorNum > 20)
            {
                Util.kWidth = 0;
                Util.kHeight = 9.0;
            }
            else if (Util.kFloorNum > 10)
            {
                Util.kWidth = 0;
                Util.kHeight = 15.0;
            }
            else
            {
                Util.kWidth = (Util.kFloorNum - 10) * 10.0;
                Util.kHeight = 20.0 + (Util.kFloorNum - 10) * 2.0;
            }

            Util.kPedSum = (Util.kInitPedNum) * (Util.kFloorNum - 1);

            if (report)
                // generate file name for this
                GenerateFileName();

            if (reporter != null)
            {
                reporter.Stop();
            }


            if (report)
                reporter = new Reporter(FileName, Util.kFloorNum);
            else
                reporter = null;

            return this;
        }

        private void GenerateFileName()
        {
            var path = "./report/";
            var now = DateTime.Now;
            path += now.Year.ToString().Substring(2) + "-" + now.Month.ToString() + "-" +
                now.Day.ToString() + "-" + now.Hour.ToString() + "-" + now.Minute.ToString() + "f" + Util.kFloorNum + "p" + Util.kFloorPedNum;
            FileName = path + "/";
            
            if (!Directory.Exists(FileName))
            {
                Directory.CreateDirectory(FileName);
            }
        }

        // add to report, every timer will do this
        public void AddToReporter(string line)
        {
            reporter.AddReport(line);
        }

        // stop report when close
        public void StopReport()
        {
            if (reporter != null)
                reporter.Stop();
        }

    }
}