using System;

namespace OSM
{
    class Ped
    {
        // the position of ped
        public Vector CurPos { get; set; }
        // start floor
        public int StartFloor { get; }
        // floor at
        public int AtFloor { get; set; }
        // if is get target
        public bool GetTarget { get; set; }

        private Space space;


        // ped's init function, generate ped's position and start floor
        public Ped(double x, double y, int floor, Space s)
        {
            CurPos = new Vector(x, y);
            // can judge if is start floor
            StartFloor = floor;
            AtFloor = floor;
            // not need dir or target pos
            GetTarget = false;
            space = s;
        }


        // single step length's move(in stairs)
        private void SingleStep(Block curBlock)
        {
            double resSff = Double.MaxValue;
            Vector resTarget = null;
            // around find min, 20; 
            var rotate = 2 * Math.PI / Util.kCircleSp;

            var dir = new Vector(1, 0).Rotate(rotate * Util.GetRandom());

            for (int i = 0; i < Util.kCircleSp; i++)
            {
                var target = CurPos.NewAdd(dir.NewMultiply(Util.kStepLen));


                // calculate target's 
                double sff = Util.GetSff(this, target);

                var atFloor = space.Floors[AtFloor];
                // all sff from ped
                foreach (var ped in atFloor.Peds)
                {

                    if (ped.Equals(this)) continue;

                    if (curBlock == Block.ThirdCorner || curBlock == Block.FirstInterval)
                    {
                        // should not calculate second stair
                        var pedBlock = Util.InWhichBlock(ped.CurPos, ped.GetAtFloor());
                        if (pedBlock == Block.SecondStair) continue;
                    }
                    sff += Util.CalculateFromPed(target, ped);
                }

                if (curBlock == Block.ThirdCorner || curBlock == Block.FirstInterval)
                {
                    if (AtFloor > 0)
                    {
                        var nextFloor = space.Floors[AtFloor - 1];
                        sff += Util.FromNextPeds(this, nextFloor);
                    }
                }

                // calculate all wall's
                // Green wall not(sign)
                foreach (var wall in atFloor.Walls)
                {
                    if (wall.Green) continue;
                    if (wall.InWall(target, Util.kR / 2.0)) sff += Util.CalculateFromWall(target, wall);
                }



                // else set for dir
                if (sff < resSff)
                {
                    resSff = sff;
                    resTarget = target;
                }
                dir.Rotate(rotate);
            }

            // bool can move or can't move
            if (resTarget != null)
            {

                if (Util.CanMove(this, resTarget, space)) CurPos = resTarget;
            }
        }

        // multi step length's move
        private void MultiStep(Block curBlock)
        {
            // should be split to 3
            double resSff = Double.MaxValue;
            Vector resTarget = null;
            var rotate = 2 * Math.PI / Util.kCircleSp;
            var dir = new Vector(1, 0).Rotate(rotate * Util.GetRandom());
            
            for (int i = 0; i < Util.kCircleSp; i++)
            {

                for (int j = 0; j < Util.kStepSp; j++)
                {
                    var target = CurPos.NewAdd(dir.NewMultiply((j + 1) * Util.kStepLen / Util.kStepSp));
                    double sff = Util.GetSff(this, target);

                    var atFloor = GetAtFloor();
                    // all sff from ped
                    foreach (var ped in atFloor.Peds)
                    {

                        if (ped.Equals(this)) continue;
                        // if in coner or interval, don't calculate from the last floor's peds
                        if (curBlock == Block.ThirdCorner || curBlock == Block.FirstInterval)
                        {
                            // should not calculate second stair
                            var pedBlock = Util.InWhichBlock(ped.CurPos, ped.GetAtFloor());
                            if (pedBlock == Block.SecondStair) continue;
                        }
                        sff += Util.CalculateFromPed(target, ped);
                    }
                    // if in corner or interval, should calculate next floor's peds 
                    if (curBlock == Block.ThirdCorner || curBlock == Block.FirstInterval)
                    {
                        if (AtFloor > 0)
                        {
                            var nextFloor = space.Floors[AtFloor - 1];
                            sff += Util.FromNextPeds(this, nextFloor);
                        }
                    }

                    // calculate all wall's
                    // Green wall not(sign)
                    foreach (var wall in atFloor.Walls)
                    {
                        if (wall.Green) continue;
                        if (wall.InWall(target, Util.kR / 2.0)) sff += Util.CalculateFromWall(target, wall);
                    }

                    // update result target
                    if (sff < resSff)
                    {
                        resSff = sff;
                        resTarget = target;
                    }

                }

                // dir rotate in i loop, not in j
                dir.Rotate(rotate);
            }

            // the same to move
            if (resTarget != null)
            {
                if (Util.CanMove(this, resTarget, space))
                    CurPos = resTarget;
            }

        }

        public void Move()
        {

            var curBlock = Util.InWhichBlock(CurPos, GetAtFloor());

            if (curBlock == Block.SecondStair || curBlock == Block.FirstStair)
            {
                SingleStep(curBlock);
            }
            else
            {
                MultiStep(curBlock);
            }
            

            // calculate from other's position
            var desBlock = Util.InWhichBlock(CurPos, GetAtFloor());
            var desVector = CurPos.NewSubtract(GetAtFloor().AddX, GetAtFloor().AddY);
            // if move to next position
            if ((curBlock == Block.FirstInterval || curBlock == Block.ThirdCorner) && desBlock == Block.SecondStair)
            {
                var atFloor = GetAtFloor();
                var nextFloor = space.Floors[--AtFloor];
                CurPos.Subtract(atFloor.AddX, atFloor.AddY).Add(nextFloor.AddX, nextFloor.AddY);
                atFloor.AddToDel(this);
            }
            // if in exit block, get to target
            if (AtFloor == 0 && desBlock == Block.ExitBlock)
            {
                var atFloor = GetAtFloor();
                var vector = CurPos.NewSubtract(atFloor.AddX, atFloor.AddY);
                if (vector.X > Util.kCornerWidth * 2 + Util.kIntervalLength + Util.kEnterWidth)
                {
                    GetTarget = true;
                    GetAtFloor().AddToDel(this);
                    // this ped should be del
                    Util.kPedSum--;
                }
            }
        }

        public void SetGetTarget()
        {
            GetTarget = true;
        }

        // give an interface to GetFloor
        public Floor GetAtFloor()
        {
            return space.Floors[AtFloor];
        }

    }


}
