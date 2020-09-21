using System;
using System.Collections.Generic;
using System.Drawing;

namespace OSM
{
    // Util class, only util functions
    public class CustomizeUtil
    {
        public static bool IsZero(double d)
        {
            return Math.Abs(d) <= 1e-6;
        }

        public static double MAX_FIELD = 1500000;
       
        
        public static bool IsSameVector(Vector a, Vector b)
        {
            return IsZero(a.GetX() - b.GetX())
                   && IsZero(a.GetY() - b.GetY());
        }
        
        public static Vector CalXY(int floor) 
        {
            int x = floor / 10, y = floor % 10;
            return new Vector(5 + (9 - y) * (5), 5 + x * (10));
        }
        

        // judge which block
        public static Block GetBlock(Vector vector, int floor) 
        {
            double x = vector.GetX(), y = vector.GetY();
            if (x >= 0.15 && x <= 1.35 && y <= 0 && y >= -1.35)
                return Block.StartBlock;
            if (x >= 0.15 && x <= 1.35 && y >= 0 && y <= 1.5)
                return Block.FirstCorner;
            if (x >= 0.15 && x <= 1.35 && y >= 1.5 && y <= 5.1)
                return Block.FirstStair;
            // 5.1 + 1.5 -> -0.1
            if (x >= 0.15 && x <= 1.5 && y >= 5.1 && y <= 6.45)
                return Block.SecondCorner;
            if (x >= 1.5 && x <= 2.0 && y >= 5.1 && y <= 6.45)
                return Block.FirstInterval;
            if (x >= 2.0 && x <= 3.35 && y >= 5.1 && y <= 6.45)
                return Block.ThirdCorner;
            if (x >= 2.15 && x <= 3.35 && y <= 5.1 && y >= 1.5)
                return Block.SecondStair;
            if (x >= 2.0 && x <= 3.35 && y <= 1.5 && y >= 0.2)
                return Block.FourthCorner;
            if (x >= 1.5 && x <= 2.0 && y >= 0.2 && y <= 1.35)
                return Block.SecondInterval;
            // only floor == 0 have exit block
            if (floor == 0 && x >= 3.4 && y > 0.2 && y <= 1.35)
                return Block.ExitBlock;
            // not in scene
            return Block.OutOfSize;
        }

        public static double GetDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }

        // 生成指定正方形区域的场域值
        // 测试场域值
        public static double[][] GenerateField(double x)
        {
            int len = (int)(x * 10);
            //分成100个角度
            int n = 200;
            double dx = x / len;
            double[][] res = new double[len][];
            List<Node>[][] nodes = new List<Node>[len][];
            for (int i = 0; i < len; i++) 
            {
                nodes[i] = new List<Node>[len];
                res[i] = new double[len];
                for (int j = 0; j < len; j++)
                {
                    nodes[i][j] = new List<Node>();
                }
            }
            //计算第i条线会和哪些格点相交, 只需要计算一半
            //由于对称性,只需要计算
            //k == 0 时候, 不计算
            for (int k = 1; k <= n / 2; k++) 
            {
                //当小于45度时候
                double tanA = Math.Tan(Math.PI / 2.0 / n * k);
                double sinA = Math.Sin(Math.PI / 2.0 / n * k);
                double cosA = Math.Cos(Math.PI / 2.0 / n * k);
                if (k < n / 2) {
                    //计算和格点相交
                    for (int i = 1; i <= len; i++)
                    {
                        double dy = (i * dx) * tanA;
                        int s = (int)(dy / dx);
                        double pd = dx * tanA;
                        double py = dy - s * dx;
                        if (!IsZero(py))
                        {
                            if (py > pd)
                            {
                                nodes[i - 1][s].Add(new Node(dx / cosA, ((double)k / n) * Config.FI * x));
                                nodes[s][i - 1].Add(new Node(dx / cosA, ((double)(n - k) / n) * Config.FI * x));
                            }
                            else
                            {
                                nodes[i - 1][s].Add(new Node(py / sinA, ((double)k / n) * Config.FI * x));
                                nodes[s][i - 1].Add(new Node(py / sinA, ((double)(n - k) / n) * Config.FI * x));
                            }
                        }
                    }
                }
                else
                {
                    //当等于45度的时候
                    for (int i = 0; i < len; i++)
                    {
                        nodes[i][i].Add(new Node(dx / Math.Cos(Math.PI / 4), 0.5 * Config.FI * x));
                    }
                }
            }
            //当角度等于90度的时候, 还未计算
            for (int i = 0; i < len; i++)
            {
                nodes[0][i].Add(new Node(dx, Config.FI * x));
            }

            for (int i = 0; i < len; i++)
            {
                for (int j = 0; j < len; j++)
                {
                    if (nodes[i][j].Count == 0)
                    {
                        res[i][j] = 0;
                    }
                    else
                    {
                        double p = 0;
                        double q = 0;
                        foreach (Node node in nodes[i][j])
                        {
                            p += node.len * node.s;
                            q += node.len;
                        }
           
                        res[i][j] = p / q;
                    }
                }
            }
            return res;
        }
        
        // generate random 0 - 1
        public static double GetRandom()
        {
            // random seed
            var seed = Guid.NewGuid().GetHashCode();
            var r = new Random(seed);
            var i = r.Next(0, 100000);
            return  ((double)i) / 100000.0;
        }

        public static bool IsEmpty(string s)
        {
            if (s.Trim().Equals("")) return true;

            return false;
        }

        public static double CalculateFromPed(Vector vector, Ped ped)
        {
            var dis = vector.DistanceTo(ped.GetCurPos());
            if (dis <= Config.gP)
            {
                return Config.miuP;
            }
            else if (dis > Config.gP && dis <= Config.gP + Config.hP)
            {
                return Config.vP * Math.Exp(-Config.aP * Math.Pow(dis, Config.bP));
            }
            else
            {
                return 0;
            }
        }
        
        
        public static void DrawWall(Graphics g, Wall wall)
        {
            Pen pen = new Pen(Color.Black);
            g.DrawLine(pen, (float) (Config.SCALE * Config.WIDTH + wall.GetBegin().GetX() * Config.SCALE)
                , (float)(Config.SCALE * Config.HEIGHT + wall.GetBegin().GetY() * Config.SCALE)
                , (float)(Config.SCALE * Config.WIDTH + wall.GetEnd().GetX() * Config.SCALE)
                , (float)(Config.SCALE * wall.GetEnd().GetY() + Config.SCALE * Config.HEIGHT));
        }
        
        
        // 绘制文本
        public static void DrawText(Graphics g, string text, double x, double y)
        {
            Font font = new Font("Consolas", 12, FontStyle.Bold);
            
            g.DrawString(text, font, Brushes.Black, (float) (Config.SCALE * Config.WIDTH + x * Config.SCALE), (float)(Config.SCALE * Config.HEIGHT + y * Config.SCALE));
        }


        public static void DrawPed(Graphics g, Ped ped) 
        {
            if (ped.IsGetTarget()) return;
            // 颜色, 根据起始楼层开始划分
            int floor = ped.GetStartFloor();
            // add black line
            g.DrawEllipse(new Pen(Color.Black), (float)(Config.SCALE * Config.WIDTH + (ped.GetCurPos().GetX() - ped.GetRadius()) * Config.SCALE)
                    , (float)(Config.SCALE * Config.HEIGHT + (ped.GetCurPos().GetY() - ped.GetRadius()) * Config.SCALE)
                    , (float)(2 * ped.GetRadius() * Config.SCALE)
                    , (float)(2 * ped.GetRadius() * Config.SCALE));
            if (floor % 3 == 0)
            {
                g.FillEllipse(new SolidBrush(Color.Red), (float) (Config.SCALE * Config.WIDTH + (ped.GetCurPos().GetX() - ped.GetRadius()) * Config.SCALE)
                    , (float)(Config.SCALE * Config.HEIGHT + (ped.GetCurPos().GetY() - ped.GetRadius()) * Config.SCALE)
                    , (float)(2 * ped.GetRadius() * Config.SCALE)
                    , (float)(2 * ped.GetRadius() * Config.SCALE));
            }
            else if (floor % 3 == 1)
            {
                g.FillEllipse(new SolidBrush(Color.Green), (float) (Config.SCALE * Config.WIDTH + (ped.GetCurPos().GetX() - ped.GetRadius()) * Config.SCALE)
                    , (float)(Config.SCALE * Config.HEIGHT + (ped.GetCurPos().GetY() - ped.GetRadius()) * Config.SCALE)
                    ,(float)( 2 * ped.GetRadius() * Config.SCALE)
                    , (float)(2 * ped.GetRadius() * Config.SCALE));
            }
            else
            {
                g.FillEllipse(new SolidBrush(Color.Blue), (float) (Config.SCALE * Config.WIDTH + (ped.GetCurPos().GetX() - ped.GetRadius()) * Config.SCALE)
                    , (float)(Config.SCALE * Config.HEIGHT + (ped.GetCurPos().GetY() - ped.GetRadius()) * Config.SCALE)
                    , (float)(2 * ped.GetRadius() * Config.SCALE)
                    , (float)(2 * ped.GetRadius() * Config.SCALE));
            }
        }


        public static void DrawFloor(Graphics g, Floor floor)
        {
            foreach (var wall in floor.GetWalls())
            {
                DrawWall(g, wall);
            }
            foreach (var wall in floor.GetVirtual())
            {
                DrawWall(g, wall);
            }
            
            DrawText(g, (floor.GetFloor() + 1).ToString(), floor.GetStartX() + 2, floor.GetStartY() - 1.2);
        }
        
        public static double CalculateFromWall(Vector vector, Wall wall)
        {
            // 墙作用力的大小
            if (wall.IsIn(vector, 0)) {
                double dis = wall.DistanceTo(vector);
                if (dis < Config.gP / 2.0) {
                    return Config.miuO;
                } else if (dis >= Config.gP / 2.0 && dis <= Config.hO) {
                    return Config.vO * Math.Exp(-Config.aO * Math.Pow(dis, Config.bO));
                } else {
                    return 0;
                }
            } else return 0;
        }

        // calculate which step the ped stand in
        public static int WhichStep(Block block, Vector pos)
        {
            if (block != Block.FirstStair && block != Block.SecondStair)
            {
                return int.MaxValue;
            }
            if (block == Block.FirstStair)
            {
                return (int)((pos.GetY() - 1.5) / Config.stepLen) + 1;
            }
            // block == Second Stair

            return (int)((5.1 - pos.GetY()) / Config.stepLen) + 1;
        }
      
        
        public static double GetField(Vector des, Vector now, int floor, bool isStart)
        {
        // 根据行人所在的位置判断场域值的大小
            Block desBlock = GetBlock(des, floor);
            Block nowBlock = GetBlock(now, floor);


            if (isStart && floor == 0)
            {
                // 如果是起始位置
                switch (desBlock)
                {
                    case Block.StartBlock:
                        if (nowBlock != Block.StartBlock) return MAX_FIELD;
                        else return MAX_FIELD - (des.GetY() + 1.5) * 100;
                    case Block.FirstCorner:
                        // 是 start, 不作为转角
                        if (nowBlock != Block.StartBlock && nowBlock != Block.FirstCorner && nowBlock != Block.SecondInterval) return MAX_FIELD;
                        else return MAX_FIELD - (des.GetY() + 1.5) * 100;
                    case Block.FirstStair:
                        if (nowBlock != Block.FirstStair && nowBlock != Block.FirstCorner) return MAX_FIELD;
                        else return MAX_FIELD - (des.GetY() + 1.5) * 100;
                    case Block.SecondCorner:
                        if (nowBlock != Block.SecondCorner && nowBlock != Block.FirstStair) return MAX_FIELD;
                        // 计算转角场域的值
                        int x = (int) ((des.GetY() - 5.1) * 10), y = (int) ((1.5 - des.GetX()) * 10);
                        if (x > 14 || y > 14) return MAX_FIELD;
                        return 1400000 + Space.sff[x][y] * 50;
                    case Block.FirstInterval:
                        if (nowBlock != Block.SecondCorner && nowBlock != Block.FirstInterval) return MAX_FIELD;
                        else return 1400000 - (des.GetX() - 1.5) * 100;
                    case Block.ThirdCorner:
                        if (nowBlock != Block.FirstInterval && nowBlock != Block.ThirdCorner) return MAX_FIELD;
                        x = (int) ((des.GetX() - 2.0) * 10);
                        y = (int) ((des.GetY() - 5.1) * 10);
                        if (x > 14 || y > 14) return MAX_FIELD;
                        return 1300000 + Space.sff[x][y] * 50;
                    case Block.SecondStair:
                        if (nowBlock != Block.ThirdCorner && nowBlock != Block.SecondStair) return MAX_FIELD;
                        else return 1300000 - (5.1 - des.GetY()) * 100;
                    case Block.FourthCorner:
                        if (nowBlock != Block.SecondStair && nowBlock != Block.FourthCorner) return MAX_FIELD;
                        x = (int) ((1.5 - des.GetY()) * 10);
                        y = (int) ((3.5 - des.GetX()) * 10);
                        if (x > 14 || y > 14) return MAX_FIELD;
                        return 1200000 + Space.sff[x][y] * 50;
                    case Block.SecondInterval:
                        return MAX_FIELD;
                    case Block.ExitBlock:
                        if (nowBlock != Block.FourthCorner && nowBlock != Block.ExitBlock) return MAX_FIELD;
                        else return 1200000 - (des.GetX() - 3.5) * 100;
                    case Block.OutOfSize:
                        return MAX_FIELD;
                }
            } else if (!isStart && floor == 0) {
                switch (desBlock) {
                    case Block.StartBlock:
                        // 不能够进入 Block.StartBlock
                        return MAX_FIELD;
                    case Block.FirstCorner:
                        if (nowBlock != Block.FirstCorner && nowBlock != Block.SecondInterval) return MAX_FIELD;
                        int x = (int) ((1.5 - des.GetX()) * 10), y = (int) ((1.5 - des.GetY()) * 10);
                        if (x > 14 || y > 14) return MAX_FIELD;
                        return 1400000 + Space.sff[x][y] * 50;
                    case Block.FirstStair:
                        if (nowBlock != Block.FirstStair && nowBlock != Block.FirstCorner) return MAX_FIELD;
                        else return 1400000 - (des.GetY() - 1.5) * 100;
                    case Block.SecondCorner:
                        if (nowBlock != Block.SecondCorner && nowBlock != Block.FirstStair) return MAX_FIELD;
                        x = (int) ((des.GetY() - 5.1) * 10);
                        y = (int) ((1.5 - des.GetX()) * 10);
                        if (x > 14 || y > 14) return MAX_FIELD;
                        return 1300000 + Space.sff[x][y] * 50;
                        // 计算转角场域的值
                    case Block.FirstInterval:
                        if (nowBlock != Block.FirstInterval && nowBlock != Block.SecondCorner) return MAX_FIELD;
                        return 1300000 - (des.GetX() - 1.5) * 100;
                    case Block.ThirdCorner:
                        if (nowBlock != Block.ThirdCorner && nowBlock != Block.FirstInterval) return MAX_FIELD;
                        x = (int) ((des.GetX() - 2.0) * 10);
                        y = (int) ((des.GetY() - 5.1) * 10);
                        if (x > 14 || y > 14) return MAX_FIELD;
                        return 1200000 + Space.sff[x][y] * 50;
                    case Block.SecondStair:
                        if (nowBlock != Block.SecondStair && nowBlock != Block.ThirdCorner) return MAX_FIELD;
                        else return 1200000 - (5.1 - des.GetY()) * 100;
                    case Block.FourthCorner:
                        // 进入 Block.FourthCorner
                        if (nowBlock != Block.FourthCorner && nowBlock != Block.SecondStair) return MAX_FIELD;
                        x = (int) ((1.5 - des.GetY()) * 10);
                        y = (int) ((3.5 - des.GetX()) * 10);
                        if (x > 14 || y > 14) return MAX_FIELD;
                        return 1100000 + Space.sff[x][y] * 50;
                    case Block.SecondInterval:
                        if (nowBlock != Block.SecondInterval) return MAX_FIELD;
                        return MAX_FIELD - (2.0 - des.GetX()) * 100;
                    case Block.ExitBlock:
                        if (nowBlock != Block.ExitBlock && nowBlock != Block.FourthCorner) return MAX_FIELD;
                        return 1100000 - (des.GetX() - 3.5) * 100;
                    case Block.OutOfSize:
                        return MAX_FIELD;
                }
            } else if (isStart) {
                switch (desBlock) {
                    case Block.StartBlock:
                        if (nowBlock != Block.StartBlock) return MAX_FIELD;
                        else return MAX_FIELD - (des.GetY() + 1.5) * 100;
                    case Block.FirstCorner:
                        // 是 start, 不作为转角
                        if (nowBlock != Block.StartBlock && nowBlock != Block.FirstCorner && nowBlock != Block.SecondInterval) return MAX_FIELD;
                        else return MAX_FIELD - (des.GetY() + 1.5) * 100;
                    case Block.FirstStair:
                        if (nowBlock != Block.FirstStair && nowBlock != Block.FirstCorner) return MAX_FIELD;
                        else return MAX_FIELD - (des.GetY() + 1.5) * 100;
                    case Block.SecondCorner:
                        if (nowBlock != Block.SecondCorner && nowBlock != Block.FirstStair) {
    //                        System.out.println("NIMA");
                            return MAX_FIELD;
                        }
                        // 计算转角场域的值
                        int x = (int) ((des.GetY() - 5.1) * 10), y = (int) ((1.5 - des.GetX()) * 10);
    //                    System.out.println(x + "," + y);
                        if (x > 14 || y > 14) return MAX_FIELD;
                        return 1400000 + Space.sff[x][y] * 50;
                    case Block.FirstInterval:
                        if (nowBlock != Block.SecondCorner && nowBlock != Block.FirstInterval) {
                            return MAX_FIELD;
                        } else {
                            return 1400000 - (des.GetX() - 1.5) * 100;
                        }
                    case Block.ThirdCorner:
                        if (nowBlock != Block.FirstInterval && nowBlock != Block.ThirdCorner) {
                            return MAX_FIELD;
                        }
                        x = (int) ((des.GetX() - 2.0) * 10);
                        y = (int) ((des.GetY() - 5.1) * 10);
                        if (x > 14 || y > 14) return MAX_FIELD;
                        return 1300000 + Space.sff[x][y] * 50;
                    case Block.SecondStair:
                        if (nowBlock != Block.ThirdCorner && nowBlock != Block.SecondStair) return MAX_FIELD;
                        else return 1300000 - (5.1 - des.GetY()) * 100;
                    case Block.FourthCorner:
                        if (nowBlock != Block.SecondStair && nowBlock != Block.FourthCorner) return MAX_FIELD;
                        x = (int) ((1.5 - des.GetY()) * 10);
                        y = (int) ((des.GetX() - 2.0) * 10);
                        if (x > 14 || y > 14) return MAX_FIELD;
                        return 1200000 + Space.sff[x][y] * 50;
                    case Block.SecondInterval:
                        if (nowBlock != Block.FourthCorner && nowBlock != Block.SecondInterval) return MAX_FIELD;
                        if (nowBlock == Block.FourthCorner) return 1200000 - (2.0 - des.GetX()) * 100;
                        return MAX_FIELD - (2.0 - des.GetX()) * 100;
                    case Block.ExitBlock:
                        return MAX_FIELD;
                    case Block.OutOfSize:
                        return MAX_FIELD;
                }
            } else {
                // floor != 0 && !isStart
                switch (desBlock)
                {
                    case Block.StartBlock:
                        // 不能够进入 Block.StartBlock
                        return MAX_FIELD;
                    case Block.FirstCorner:
                        if (nowBlock != Block.FirstCorner && nowBlock != Block.SecondInterval)
                            return MAX_FIELD;
                        int x = (int) ((1.5 - des.GetX()) * 10), y = (int) ((1.5 - des.GetY()) * 10);
                        if (x > 14 || y > 14) return MAX_FIELD;
                        return 1400000 + Space.sff[x][y] * 50;
                    case Block.FirstStair:
                        if (nowBlock != Block.FirstStair && nowBlock != Block.FirstCorner) return MAX_FIELD;
                        else return 1400000 - (des.GetY() - 1.5) * 100;
                    case Block.SecondCorner:
                        if (nowBlock != Block.SecondCorner && nowBlock != Block.FirstStair) return MAX_FIELD;
                        x = (int) ((des.GetY() - 5.1) * 10);
                        y = (int) ((1.5 - des.GetX()) * 10);
                        if (x > 14 || y > 14) return MAX_FIELD;
                        return 1300000 + Space.sff[x][y] * 50;
                    // 计算转角场域的值
                    case Block.FirstInterval:
                        if (nowBlock != Block.FirstInterval && nowBlock != Block.SecondCorner) return MAX_FIELD;
                        return 1300000 - (des.GetX() - 1.5) * 100;
                    case Block.ThirdCorner:
                        if (nowBlock != Block.ThirdCorner && nowBlock != Block.FirstInterval) return MAX_FIELD;
                        x = (int) ((des.GetX() - 2.0) * 10);
                        y = (int) ((des.GetY() - 5.1) * 10);
                        if (x > 14 || y > 14) return MAX_FIELD;
                        return 1200000 + Space.sff[x][y] * 50;
                    case Block.SecondStair:
                        if (nowBlock != Block.SecondStair && nowBlock != Block.ThirdCorner) return MAX_FIELD;
                        else return 1200000 - (5.1 - des.GetY()) * 100;
                    case Block.FourthCorner:
                        // 进入 Block.FourthCorner
                        if (nowBlock != Block.FourthCorner && nowBlock != Block.SecondStair) return MAX_FIELD;
                        x = (int) ((1.5 - des.GetY()) * 10);
                        y = (int) ((des.GetX() - 2.0) * 10);
                        if (x > 14 || y > 14) return MAX_FIELD;
                        return 1100000 + Space.sff[x][y] * 50;
                    case Block.SecondInterval:
                        if (nowBlock != Block.FourthCorner && nowBlock != Block.SecondInterval) return MAX_FIELD;
                        if (nowBlock == Block.FourthCorner) return 1100000 - (2.0 - des.GetX()) * 100;
                        return MAX_FIELD - (2.0 - des.GetX()) * 100;
                    case Block.ExitBlock:
                        return MAX_FIELD;
                    case Block.OutOfSize:
                        return MAX_FIELD;
                }
            }
            return MAX_FIELD;
        }
        

        // define the move of ped
        public static bool CanMove(Floor floor, Floor nextFloor, Ped p, Vector des)
        {

            var sub = CustomizeUtil.CalXY(floor.GetFloor());
            var now = p.GetCurPos().NewSubtract(sub);

            // now Block, and some pos to avoid 
            var pBlock = CustomizeUtil.GetBlock(now, floor.GetFloor());

            // 是否贴墙
            foreach (var wall in floor.GetWalls())
            {
                if (wall.IsIn(des, Config.R / 2.0) && wall.DistanceTo(des) < Config.R)
                    // break in thisA
                    return false;
            }
            


            foreach (var ped in floor.GetPeds())
            {
                if (p.Equals(ped)) continue;
                // do not calculate the ped get target
                if (ped.IsGetTarget()) continue;

                if (pBlock == Block.FourthCorner)
                {
                    var pedBlock = CustomizeUtil.GetBlock(ped.GetCurPos().NewSubtract(sub), floor.GetFloor());
                    if (pedBlock == Block.SecondInterval) continue;
                }
                
                // the distance to ped should not > 1.4 * R
                if (des.DistanceTo(ped.GetCurPos()) < Config.R + 0.1)
                    return false;
            }

            //if (pBlock == Block.FourthCorner && nextFloor != null)
            //{
                
            //    foreach (var ped in nextFloor.GetFromUp())
            //    {
                    
            //        if (ped.IsGetTarget()) continue;
            //        if (des.DistanceTo(GetUpPosition(floor.GetFloor() - 1, ped.GetCurPos())) < Config.R + 0.1)
            //            return false;
            //    }
            //}

            return true;

        }

        public static Vector GetUpPosition(int floor, Vector pos)
        {
            // vector should sub
            var sub = CalXY(floor);
            // vector should add
            var add = CalXY(floor + 1);
            // return add's position
            return pos.NewSubtract(sub).Add(add);
        }
        
    }
}