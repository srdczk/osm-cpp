using System;

namespace OSM
{
    // 行人类, 描述行人
    public class Ped
    {
        // 行人的楼层数
        private int floor;
        
        // 行人的起始楼层
        private int startFloor;
        
        // 是否到达终点(出口)
        private bool getTarget;
        
        private Vector curPos;

        private Vector dir;

        private double radius;

        private double stepLen;

        private int cnt;

        // id 行人索引
        private int id;
        // space 指针
        private Space space;

        

        public Ped(int id, double x, double y
                , double sl, double r
                , double dirX, double dirY
                , Space s, int f) 
        {
            this.id = id;
            curPos = new Vector(x, y);
            dir = new Vector(dirX, dirY);
            radius = r;
            stepLen = sl;
            space = s;
            getTarget = false;

            floor = f;
            startFloor = f;
            // 不同楼层的人用不同的颜色来表示
            // 颜色直接在绘图的时候确定
            // cnt should be count
            cnt = 0;
        }

        public int GetStartFloor()
        {
            return startFloor;
        }

        public void Move()
        {
           

            var target = curPos.NewAdd(dir.NewMultiply(stepLen));
            var sub = CustomizeUtil.CalXY(floor);
            var des = target.NewSubtract(sub);
            var now = curPos.NewSubtract(sub);

            // destination block, current block 
            var desBlock = CustomizeUtil.GetBlock(des, floor);
            var nowBlock = CustomizeUtil.GetBlock(now, floor);


            Floor x = (Floor) (space.GetMap()[floor]);


            Floor nextFloor = floor > 0 ? (Floor)(space.GetMap()[floor]) : null;

            if (!CustomizeUtil.CanMove(x, nextFloor, this, target))
            {
                // if a ped's move should not be allowed
                if (nowBlock == Block.SecondInterval)
                {
                    //if (cnt++ == 5)
                    //{
                    //    if (Config.delCnt < Config.floorPedSum * 0.20)
                    //    {
                    //        if (CustomizeUtil.GetRandom() > 0.5)
                    //        {
                    //            ++Config.delCnt;
                    //            x.AddDel(this);
                    //        }
                    //    }

                    //    cnt = 0;
                    //}

                    //if (cnt++ == 5)
                    //{
                    //    if (CustomizeUtil.GetRandom() > 0.55)
                    //    {
                    //        x.AddDel(this);
                    //    }
                    //    cnt = 0;
                    //}

                }
                else
                {
                    cnt = 0;
                }
                
                //if (cnt++ == 5)
                //{
                //    x.AddDel(this);
                //}
                return;
 
            }

            cnt = 0;

            if (floor == 0 && des.GetX() > 4.8)
            {
                Console.WriteLine(++Config.getTarget);
                getTarget = true;
                // space one's remove
                // remove ped from the whole map
                ((Floor)(space.GetMap()[floor])).wantRemove.Add(this);
                return;
            }

            if (desBlock == Block.OutOfSize) return;

            
            // next floor, add and remove 
            if (desBlock == Block.SecondInterval && nowBlock == Block.FourthCorner)
            {
                
                var newSub = CustomizeUtil.CalXY(--floor);
                curPos = newSub.Add(des);
                ((Floor)(space.GetMap()[floor + 1])).wantRemove.Add(this);
                
            }
            else
            {
                curPos = target;
            }
            
            // if update should be calculate
            if (x.IsFromUp(this) && desBlock != Block.SecondInterval)
            {
                x.FromUpRemove(this);
            }

        }
        
        // 更新 方向, 每个时间步做一次, OSM 模型的定义
        public void UpdateDir()
        {
            var at = ((Floor) space.GetMap()[floor]); 
            var sub = CustomizeUtil.CalXY(floor);
            var now = curPos.NewSubtract(sub);
            Block block = CustomizeUtil.GetBlock(now, floor);
            var p = new Vector(0, 1);

            switch (block)
            {
                case Block.StartBlock:
                    p = new Vector(0, 1);
                    break;
                case Block.FirstCorner:
                    p = new Vector(0, 1);
                    break;
                case Block.FirstStair:
                    p = new Vector(0, 1);
                    break;
                case Block.SecondCorner:
                    p = new Vector(Math.Sqrt(2) / 2.0, Math.Sqrt(2) / 2.0);
                    break;
                case Block.FirstInterval:
                    p = new Vector(1, 0);
                    break;
                case Block.ThirdCorner:
                    p = new Vector(Math.Sqrt(2) / 2.0, -Math.Sqrt(2) / 2.0);
                    break;
                case Block.SecondStair:
                    p = new Vector(0, -1);
                    break;
                case Block.FourthCorner:
                    p = new Vector(-Math.Sqrt(2) / 2.0, -Math.Sqrt(2) / 2.0);
                    break;
                case Block.SecondInterval:
                    p = new Vector(-1, 0);
                    break;
                case Block.ExitBlock:
                    p = new Vector(1, 0);
                    break;
                case Block.OutOfSize:
                    p = new Vector(1, 0);
                    break;
            }

            int step = CustomizeUtil.WhichStep(block, now);
            if (step >= Config.maxStep)
            {
                //set stepLen = Config
                stepLen = Config.stepLen;
                // modify to Customize update
                //CustomizeUpdate(p, at);
                DefaultUpdate(p, at);
            }
            else
            {
                // update stair
                if (CustomizeUtil.GetRandom() < 0.75)
                {
                    stepLen = Config.stepLen;

                    DefaultUpdate(p, at);
                }
                else
                {
                    StairUpdate(at, block == Block.FirstStair);
                }
            }
            
        }

        // calculate all peds on stair
        private void StairUpdate(Floor at, bool up)
        {
            Vector resTarget;
            var sub = CustomizeUtil.CalXY(floor);
            var now = curPos.NewSubtract(sub);
            var res = Double.MaxValue;

            Floor nextFloor = floor > 0 ? (Floor)(space.GetMap()[floor - 1]) : null;


            if (up)
            {
                resTarget = new Vector(curPos.GetX(), curPos.GetY() + Config.stairLen);
                var begin = new Vector(1, 0).Rotate(Math.PI / 3.0);
                // next pos's y -> 
                var nextY = curPos.GetY() + Config.stairLen;
                for (int i = 0; i < Config.Q + 1; i++)
                {

                    var nextX = curPos.GetX() + begin.GetX() * Config.stairLen / begin.GetY(); 

                    var target = new Vector(nextX, nextY);

                    double field = 0;

                    begin.Rotate(1.0 / 3.0 * Math.PI / (double) Config.Q);
                    // if can't move
                    if (!CustomizeUtil.CanMove(at, nextFloor, this, target)) continue;

                    foreach (var ped in at.GetPeds())
                    {
                        if (ped.Equals(this))
                            continue;
                        field += CustomizeUtil.CalculateFromPed(target, this);
                    }
                    foreach (var wall in at.GetWalls())
                    {
                        if (wall.IsIn(target, Config.R / 2.0))
                        {
                            field += CustomizeUtil.CalculateFromWall(target, wall);
                        }
                    }

                    // calculate field value
                    var des = target.NewSubtract(sub);
                    field += CustomizeUtil.GetField(des, now, floor, startFloor == floor);

                    if (field < res)
                    {
                        res = field;
                        resTarget = target;
                    }
                }
            }
            else
            {
                resTarget = new Vector(curPos.GetX(), curPos.GetY() - Config.stairLen);
                var begin = new Vector(1, 0).Rotate(Math.PI / 3.0);
                // next pos's y -> 
                var nextY = curPos.GetY() - Config.stairLen;
                for (int i = 0; i < Config.Q + 1; i++)
                {

                    var nextX = curPos.GetX() + begin.GetX() * Config.stairLen / begin.GetY();

                    var target = new Vector(nextX, nextY);

                    double field = 0;

                    begin.Rotate(1.0 / 3.0 * Math.PI / (double)Config.Q);
                    // if can't move
                    if (!CustomizeUtil.CanMove(at, nextFloor, this, target)) continue;

                    foreach (var ped in at.GetPeds())
                    {
                        if (ped.Equals(this))
                            continue;

                        field += CustomizeUtil.CalculateFromPed(target, this);
                    }
                    foreach (var wall in at.GetWalls())
                    {
                        if (wall.IsIn(target, Config.R / 2.0))
                        {
                            field += CustomizeUtil.CalculateFromWall(target, wall);
                        }
                    }

                    // calculate field value
                    var des = target.NewSubtract(sub);
                    field += CustomizeUtil.GetField(des, now, floor, startFloor == floor);

                    if (field < res)
                    {
                        res = field;
                        resTarget = target;
                    }
                }

            }
            // from resPos to this
            dir = resTarget.Normalize(curPos);
            stepLen = resTarget.DistanceTo(curPos);
        }

        private void CustomizeUpdate(Vector p, Floor at)
        {
            // update by split
            var floor = at.GetFloor();
            var sub = CustomizeUtil.CalXY(floor);
            var now = curPos.NewSubtract(sub);

            var begin = p.Rotate(2 * Math.PI / Config.Q * CustomizeUtil.GetRandom());

            // now Block, and some pos to avoid 
            var curBlock = CustomizeUtil.GetBlock(now, floor);

            // minimal field value
            var res = Double.MaxValue;
            // default result target 
            var resTarget = curPos.NewAdd(p.NewMultiply(stepLen));

            Floor nextFloor = floor > 0 ? (Floor)(space.GetMap()[floor]) : null;

            for (int i = 0; i < Config.Q; i++)
            {
                begin.Rotate(2 * Math.PI / (double)Config.Q);
                // split five 
                for (int j = 1; j <= Config.Sp; j++)
                {
                    // every single pattern
                    double r = Config.stepLen * j / (double)Config.Sp;
                    // calculate the target point
                    var target = curPos.NewAdd(begin.NewMultiply(r));
                    // simplyfy target
                    var des = target.NewSubtract(sub);
                    // calculate the destination point's block
                    var desBlock = CustomizeUtil.GetBlock(des, floor);
                    // forbid the second interval's ped to fourth corner
                    if ((curBlock == Block.SecondInterval) && (desBlock == Block.FourthCorner)) continue;
                    // if can't move, this point should not be calculated
                    if (!CustomizeUtil.CanMove(at, nextFloor, this, target)) continue;
                    // calculate field value
                    double field = 0.0;
                    // calculate the field from other peds
                    foreach (var ped in at.GetPeds())
                    {
                        if (ped.Equals(this))
                            continue;

                        field += CustomizeUtil.CalculateFromPed(target, this);
                    }

                    // calculate the field from walls
                    foreach (var wall in at.GetWalls())
                    {
                        // if wall can impact on ped
                        if (wall.IsIn(target, Config.R / 2.0))
                        {
                            field += CustomizeUtil.CalculateFromWall(target, wall);
                        }
                    }
                    // if start floor == floor, field's calculation
                    field += CustomizeUtil.GetField(des, now, floor, startFloor == floor);
                    // minimal result
                    if (field < res)
                    {
                        res = field;
                        resTarget = target;
                    }

                }
            }
            // update the step's length and ped's dir
            dir = resTarget.Normalize(curPos);
            stepLen = resTarget.DistanceTo(curPos);
        }

        private void DefaultUpdate(Vector p, Floor at)
        {
            var resTarget = curPos.NewAdd(p.NewMultiply(stepLen));
            // Generate a angle random
            var begin = p.Rotate(2 * Math.PI / Config.Q * CustomizeUtil.GetRandom());

            var res = Double.MaxValue;
            var sub = CustomizeUtil.CalXY(floor);
            var now = curPos.NewSubtract(sub);

            var curBlock = CustomizeUtil.GetBlock(now, floor);

            Floor nextFloor = floor > 0 ? (Floor)(space.GetMap()[floor]) : null;

            for (int i = 0; i < Config.Q; ++i)
            {
                var vector = curPos.NewAdd(begin.Rotate(2 * Math.PI / Config.Q).NewMultiply(stepLen));

                var des = vector.NewSubtract(sub);

                // define rule to avoid the problem
                var desBlock = CustomizeUtil.GetBlock(des, floor);

                // add the judge 
                if (((curBlock == Block.SecondInterval) && (desBlock == Block.FourthCorner))) continue;

                if (!CustomizeUtil.CanMove(at, nextFloor, this, vector)) continue;
                double field = 0;

                foreach (var ped in at.GetPeds())
                {
                    if (ped.Equals(this))
                        continue;

                    field += CustomizeUtil.CalculateFromPed(vector, ped);
                }

                foreach (var wall in at.GetWalls())
                {
                    if (wall.IsIn(vector, Config.R / 2.0))
                    {
                        field += CustomizeUtil.CalculateFromWall(vector, wall);
                    }
                }
                
                // should not can move
                field += CustomizeUtil.GetField(des, now, floor, startFloor == floor);

                if (field < res)
                {
                    res = field;
                    resTarget = vector;
                }

            }

            dir = resTarget.Normalize(curPos);
        }

        public double GetRadius() 
        {
            return radius;
        }
        
        public int GetId()
        {
            return id;
        }

        public Vector GetCurPos()
        {
            return curPos;
        }
        
        public void SetFloor(int f)
        {
            floor = f;
        }

        public int GetFloor()
        {
            return floor;
        }

        public void SetCurX(double x)
        {
            curPos.SetX(x);
        }

        // 判断是否是起始楼层
        public bool IsStart()
        {
            return startFloor == floor;
        }

        public bool IsGetTarget()
        {
            return getTarget;
        }
    }
}