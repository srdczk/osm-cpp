
using System.Collections.Generic;

namespace OSM
{
    class Space
    {
        public List<Floor> Floors { get; }
        public Space()
        {
            Floors = new List<Floor>();
        }
        // init floors
        public Space Init()
        {
            Floors.Clear();
            Util.kId = 0;
            for (int i = 0; i < Util.kFloorNum; i++)
            {
                var floor = new Floor(i);
                if (i > 0)
                    Util.InitRandomPed(floor, this);
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
                Util.kWidth = (Util.kFloorNum - 10) * 5.0;
                Util.kHeight = 20.0 + (Util.kFloorNum - 10) * 2.0;
            }
            
            return this;
        }
    }
}