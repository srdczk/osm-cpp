using System.Collections.Generic;

namespace OSM
{
    class Floor
    {
        // the number of floor
        public int Number { get; }
        // the set of peds
        public HashSet<Ped> Peds { get; }
        // the set of walls
        public HashSet<Wall> Walls { get; }
        // the set of virtual walls
        public HashSet<Wall> Virtual { get; }
        // to del's ped, may be out of bounds or 
        public HashSet<Ped> ToDel { get; }
        // is start or end
        public bool Start { get; }
        public bool End { get; }
        // addX and addY
        public double AddX { get; }
        // addY
        public double AddY { get; }

        // the file to print
        public int Cnt { get; set; }

        // add ped to floor
        public void Add(Ped ped)
        {
            Peds.Add(ped);
        }

        // clear ped from ped's set
        public void ClearRemove()
        {
            ToDel.Clear();
        }

        // add To Del
        public void AddToDel(Ped ped)
        {
            ToDel.Add(ped);
        }

        public Floor(int floor)
        {
            // cnt to generate 
            Cnt = 0;
            // init all
            Peds = new HashSet<Ped>();
            Walls = new HashSet<Wall>();
            Virtual = new HashSet<Wall>();
            ToDel = new HashSet<Ped>();
            Number = floor;
            Start = floor == Util.kFloorNum - 1;
            End = floor == 0;
            // if !Number is end
            var addX = Util.kAddWidth + (9 - (floor % 10)) * 5;
            var addY = Util.kAddHeight + (floor / 10) * 10;
            // set addx and addy to draw text
            AddX = addX;
            AddY = addY;
            // to modify easy, number to variable
            if (Start)
            {
                Walls.Add(new Wall(addX, addY - Util.kEnterHeight, 
                    addX, addY + Util.kStairNum * Util.kStairLength + 2 * Util.kCornerHeight));
                Walls.Add(new Wall(addX, addY + Util.kStairNum * Util.kStairLength + 2 * Util.kCornerHeight,
                    addX + 2 * Util.kCornerWidth + Util.kIntervalLength, addY + Util.kStairNum * Util.kStairLength + 2 * Util.kCornerHeight));
                Walls.Add(new Wall(addX + 2 * Util.kCornerWidth + Util.kIntervalLength, addY + Util.kStairNum * Util.kStairLength + 2 * Util.kCornerHeight,
                    addX + 2 * Util.kCornerWidth + Util.kIntervalLength, addY + Util.kStairNum * Util.kStairLength + Util.kCornerHeight));
                Walls.Add(new Wall(addX + Util.kCornerWidth, addY - Util.kEnterHeight,
                    addX + Util.kCornerWidth, addY + Util.kStairNum * Util.kStairLength + Util.kCornerHeight));
                Walls.Add(new Wall(addX + Util.kCornerWidth, addY + Util.kStairNum * Util.kStairLength + Util.kCornerHeight,
                    addX + Util.kCornerWidth + Util.kIntervalLength, addY + Util.kStairNum * Util.kStairLength + Util.kCornerHeight));
                // set this wall green
                var greenWall = new Wall(addX + Util.kCornerWidth + Util.kIntervalLength, addY + Util.kStairNum * Util.kStairLength + Util.kCornerHeight,
                    addX + 2 * Util.kCornerWidth + Util.kIntervalLength, addY + Util.kStairNum * Util.kStairLength + Util.kCornerHeight);
                greenWall.Green = true;
                Walls.Add(greenWall);
                for (int i = 0; i <= Util.kStairNum; i++)
                {
                    Virtual.Add(new Wall(addX, addY + Util.kCornerHeight + i * Util.kStairLength, addX + Util.kCornerWidth, addY + Util.kCornerHeight + i * Util.kStairLength));
                }
            }
            else if (End)
            {
                var greenWall = new Wall(addX + Util.kCornerWidth + Util.kIntervalLength, addY + Util.kStairNum * Util.kStairLength + Util.kCornerHeight,
                    addX + 2 * Util.kCornerWidth + Util.kIntervalLength, addY + Util.kStairNum * Util.kStairLength + Util.kCornerHeight);
                greenWall.Green = true;
                Walls.Add(greenWall);

                Walls.Add(new Wall(addX + Util.kCornerWidth + Util.kIntervalLength, addY,
                    addX + Util.kCornerWidth + Util.kIntervalLength, addY + Util.kCornerHeight + Util.kStairLength * Util.kStairNum));
                Walls.Add(new Wall(addX + 2 * Util.kCornerWidth + Util.kIntervalLength, addY + Util.kCornerHeight + Util.kStairLength * Util.kStairNum,
                    addX + 2 * Util.kCornerWidth + Util.kIntervalLength, addY + Util.kCornerHeight));
                Walls.Add(new Wall(addX + Util.kCornerWidth + Util.kIntervalLength, addY, 
                    addX + 2 * Util.kCornerWidth + Util.kIntervalLength + Util.kExitLength, addY));
                Walls.Add(new Wall(addX + 2 * Util.kCornerWidth + Util.kIntervalLength, addY + Util.kCornerHeight,
                    addX + 2 * Util.kCornerWidth + Util.kIntervalLength + Util.kExitLength, addY + Util.kCornerHeight));
                // the floor to outsize

                // to add the stair
                for (int i = 0; i <= Util.kStairNum; i++)
                {
                    Virtual.Add(new Wall(addX + Util.kCornerWidth + Util.kIntervalLength, addY + Util.kCornerHeight + i * Util.kStairLength,
                        addX + 2 * Util.kCornerWidth + Util.kIntervalLength, addY + Util.kCornerHeight + i * Util.kStairLength));
                }
            }
            else
            {
                // add green wall from last floor
                var greenWall = new Wall(addX + Util.kCornerWidth + Util.kIntervalLength, addY + Util.kStairNum * Util.kStairLength + Util.kCornerHeight,
                    addX + 2 * Util.kCornerWidth + Util.kIntervalLength, addY + Util.kStairNum * Util.kStairLength + Util.kCornerHeight);
                greenWall.Green = true;
                Walls.Add(greenWall);

                Walls.Add(new Wall(addX, addY - Util.kEnterHeight, 
                    addX, addY + 2 * Util.kCornerHeight + Util.kStairLength * Util.kStairNum));
                Walls.Add(new Wall(addX, addY + 2 * Util.kCornerHeight + Util.kStairLength * Util.kStairNum,
                    addX + 2 * Util.kCornerWidth + Util.kIntervalLength, addY + 2 * Util.kCornerHeight + Util.kStairLength * Util.kStairNum));
                Walls.Add(new Wall(addX + 2 * Util.kCornerWidth + Util.kIntervalLength, addY + 2 * Util.kCornerHeight + Util.kStairLength * Util.kStairNum,
                    addX + 2 * Util.kCornerWidth + Util.kIntervalLength, addY));
                Walls.Add(new Wall(addX + 2 * Util.kCornerWidth + Util.kIntervalLength, addY, addX + Util.kCornerWidth, addY));
                Walls.Add(new Wall(addX + Util.kCornerWidth, addY, addX + Util.kCornerWidth, addY - Util.kEnterHeight));

                Walls.Add(new Wall(addX + Util.kCornerWidth, addY + Util.kCornerHeight, 
                    addX + Util.kCornerWidth, addY + Util.kCornerHeight + Util.kStairLength * Util.kStairNum));
                Walls.Add(new Wall(addX + Util.kCornerWidth, addY + Util.kCornerHeight + Util.kStairLength * Util.kStairNum,
                    addX + Util.kCornerWidth + Util.kIntervalLength, addY + Util.kCornerHeight + Util.kStairLength * Util.kStairNum));
                Walls.Add(new Wall(addX + Util.kCornerWidth + Util.kIntervalLength, addY + Util.kCornerHeight + Util.kStairLength * Util.kStairNum,
                    addX + Util.kCornerWidth + Util.kIntervalLength, addY + Util.kCornerHeight));
                Walls.Add(new Wall(addX + Util.kCornerWidth + Util.kIntervalLength, addY + Util.kCornerHeight,
                    addX + Util.kCornerWidth, addY + Util.kCornerHeight));

                for (int i = 0; i <= Util.kStairNum; i++)
                {
                    Virtual.Add(new Wall(addX, addY + Util.kCornerHeight + i * Util.kStairLength, 
                        addX + Util.kCornerWidth, addY + Util.kCornerHeight + i * Util.kStairLength));
                    Virtual.Add(new Wall(addX + Util.kCornerWidth + Util.kIntervalLength, addY + Util.kCornerHeight + i * Util.kStairLength,
                        addX + 2 * Util.kCornerWidth + Util.kIntervalLength, addY + Util.kCornerHeight + i * Util.kStairLength));
                }

            }
        }

    }

}
