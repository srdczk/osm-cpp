using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms.DataVisualization.Charting;

namespace OSM
{
    class Util
    {
        // turn on or off report
        public static bool kReport = false;
        // ped sum, to stop report when there is not ped in space
        public static int kPedSum = (kFloorNum - 1) * (kInitPedNum);
        // timeout mill seconds
        public static int kTimeout = 1000;
        // initial ped num
        public static int kInitPedNum = 5;
        // peds per floor
        public static int kFloorPedNum = 100;
        // max ped's num per floor
        public static int kMaxFloorPedNum = 105;
        // min and max floors
        public static int kMinFloor = 3;
        public static int kMaxFloor = 50;
        // step split to 3
        public static int kStepSp = 3;
        // id of peds
        public static int kId = 0;
        // static floor field's scale
        public static double kSffScale = 50.0;
        // circle corner
        public static double kCornerVal = 180.0;
        // circle split
        public static int kCircleSp = 20;
        // step length of ped
        public static double kStepLen = 0.3;
        // params of model ped
        public static double kMuP = 10000.0;
        public static double kVP = 0.4;
        public static double kAP = 1.0;
        public static double kBP = 0.2;
        public static double kGP = 0.4;
        public static double kHP = 1.0;
        // params of model wall
        public static double kMuO = 10000.0;
        public static double kVO = 0.2;
        public static double kAO = 3.0;
        public static double kBO = 2.0;
        public static double kHO = 6.0;
        // util class Node: len and s
        class Node
        {
            public double Len { get; set; }
            public double S { get; set; }
            public Node(double len, double s)
            {
                Len = len;
                S = s;
            }
        }
        // the variable of floor's stair 
        // enter's width and height
        public static double kEnterWidth = 1.5;
        public static double kEnterHeight = 1.5;
        // the coner's width and height
        public static double kCornerWidth = 1.5;
        public static double kCornerHeight = 1.5;
        // stair's length and width
        public static double kStairWidth = 1.5;
        public static double kStairLength = 0.3;
        // number of stair
        public static int kStairNum = 12;
        // Interval len
        public static double kIntervalLength = 0.5;
        // Exit's size
        public static double kExitLength = 1.5;
        // k add width and height
        public static double kAddWidth = 5.0;
        public static double kAddHeight = 5.0;
        // draw text need add x, add y
        public static double kTextAddX = 1.5;
        public static double kTextAddY = -1.2;
        // all floors
        public static int kFloorNum = 20;
        // scale 
        public static double kScale = 14.0;
        // width and height
        public static double kWidth = 0.0;
        public static double kHeight = 15.0;
        // PI / 2
        private static double kFi = Math.PI / 2.0;
        // radius of ped
        public static double kR = 0.20;
        // util class, only static
        public static bool IsZero(double d)
        {
            return Math.Abs(d) < 1e-6;
        }
        // define is same vector
        public static bool IsSame(Vector a, Vector b)
        {
            return IsZero(a.X - b.X) && IsZero(a.Y - b.Y);
        }
        // draw ped
        public static void DrawPed(Graphics g, Ped ped)
        {
            // ellipse draw x, y, w, h
            // to distinguish pedestrians on different floors
            if (ped.StartFloor % 3 == 0)
            {
                g.FillEllipse(new SolidBrush(Color.Red), (float)((ped.CurPos.X - kR) * kScale + kWidth * kScale), (float)((ped.CurPos.Y - kR) * kScale + kHeight * kScale),
                (float)((2 * kR) * kScale), (float)((2 * kR) * kScale));
            }
            else if (ped.StartFloor % 3 == 1)
            {
                g.FillEllipse(new SolidBrush(Color.Blue), (float)((ped.CurPos.X - kR) * kScale + kWidth * kScale), (float)((ped.CurPos.Y - kR) * kScale + kHeight * kScale),
                (float)((2 * kR) * kScale), (float)((2 * kR) * kScale));
            }
            else
            {
                g.FillEllipse(new SolidBrush(Color.Green), (float)((ped.CurPos.X - kR) * kScale + kWidth * kScale), (float)((ped.CurPos.Y - kR) * kScale + kHeight * kScale),
                (float)((2 * kR) * kScale), (float)((2 * kR) * kScale));                
            }
            g.DrawEllipse(new Pen(Color.Black), (float)((ped.CurPos.X - kR) * kScale + kWidth * kScale), (float)((ped.CurPos.Y - kR) * kScale + kHeight * kScale),
                (float)((2 * kR) * kScale), (float)((2 * kR) * kScale));
        }
        //draw wall
        public static void DrawWall(Graphics g, Wall wall)
        {

            Pen pen;
            if (wall.Green)
            {
                pen = new Pen(Color.Green, 2.5f);
            }
            else
            {
                pen = new Pen(Color.Black);
            }
            g.DrawLine(pen, (float)(wall.Begin.X * kScale + kWidth * kScale), (float)(wall.Begin.Y * kScale + kHeight * kScale), 
                (float)(wall.End.X * kScale + kWidth * kScale), (float)(wall.End.Y * kScale + kHeight * kScale));
        }
        // draw text
        public static void DrawText(Graphics g, string text, double x, double y)
        {
            // consolas to write text
            Font font = new Font("Consolas", (float)(kScale / 2.0), FontStyle.Bold);

            g.DrawString(text, font, Brushes.Black, (float)(x * kScale + kWidth * kScale), (float)(y * kScale + kHeight * kScale));
        }
        // draw floor
        public static void DrawFloor(Graphics g, Floor floor)
        {
            // draw virtual walls which represent stairs
            foreach (var wall in floor.Virtual)
            {
                DrawWall(g, wall);
            }

            // draw walls which in size, let green add to black
            foreach (var wall in floor.Walls)
            {
                DrawWall(g, wall);
            }

            if (floor.Start)
                DrawText(g, (floor.Number + 0.5).ToString() + "-" + (floor.Number + 1).ToString(), floor.AddX + kTextAddX, floor.AddY + kTextAddY);
            else if (floor.End)
                DrawText(g, (floor.Number + 1).ToString() + "-" + (floor.Number + 1.5).ToString(), floor.AddX + kTextAddX, floor.AddY + kTextAddY);
            else
                DrawText(g, (floor.Number + 0.5).ToString() + "-" + (floor.Number + 1.5).ToString(), floor.AddX + kTextAddX, floor.AddY + kTextAddY);


        }
        // corner width of evacuation
        public static double GetCornerSff(double max, double min, double tanX)
        {
            var theta = Math.Atan(tanX);

            return min + (max - min) * theta / Math.PI * 2.0;
        }
        // generate coner field val
        public static double[][] GenerateField(double x)
        {
            int len = (int)(x * 10);
            // 100 angle
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
            // some will be calculate many times
            for (int k = 1; k <= n / 2; k++)
            {
                // < 45
                double tanA = Math.Tan(Math.PI / 2.0 / n * k);
                double sinA = Math.Sin(Math.PI / 2.0 / n * k);
                double cosA = Math.Cos(Math.PI / 2.0 / n * k);
                if (k < n / 2)
                {
                    
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
                                nodes[i - 1][s].Add(new Node(dx / cosA, ((double)k / n) * kFi * x));
                                nodes[s][i - 1].Add(new Node(dx / cosA, ((double)(n - k) / n) * kFi * x));
                            }
                            else
                            {
                                nodes[i - 1][s].Add(new Node(py / sinA, ((double)k / n) * kFi * x));
                                nodes[s][i - 1].Add(new Node(py / sinA, ((double)(n - k) / n) * kFi * x));
                            }
                        }
                    }
                }
                else
                {
                    // == 45
                    for (int i = 0; i < len; i++)
                    {
                        nodes[i][i].Add(new Node(dx / Math.Cos(Math.PI / 4), 0.5 * kFi * x));
                    }
                }
            }
            // == 90
            for (int i = 0; i < len; i++)
            {
                nodes[0][i].Add(new Node(dx, kFi * x));
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
                            p += node.Len * node.S;
                            q += node.Len;
                        }

                        res[i][j] = p / q;
                    }
                }
            }
            return res;
        }
        // calculate from Wall
        public static double CalculateFromWall(Vector vector, Wall wall)
        {
            // if not in wall 
            if (wall.InWall(vector, kR / 2.0))
            {
                var dis = wall.DistanceTo(vector);
                if (dis < kGP / 2.0)
                    return kMuO;
                else if (dis >= kGP / 2.0 && dis <= kHO)
                    return kVO * Math.Exp(-kAO * Math.Pow(dis, kBO));
                else
                    return 0.0;
            }
            return 0.0;
        }
        // calculate from ped
        public static double CalculateFromPed(Vector vector, Ped ped)
        {
            var dis = vector.DistanceTo(ped.CurPos);
            if (dis <= kGP)
                return kMuP;
            else if (dis > kGP && dis <= kGP + kHP)
                return kVP * Math.Exp(-kAP * Math.Pow(dis, kBP));
            else
                return 0.0;
        }
        // from ped -> next floor, modify ped pos to calculate
        public static double CalculateFromPed(Vector vector, Vector pedPos)
        {
            var dis = vector.DistanceTo(pedPos);
            if (dis <= kGP)
                return kMuP;
            else if (dis > kGP && dis <= kGP + kHP)
                return kVP * Math.Exp(-kAP * Math.Pow(dis, kBP));
            else
                return 0.0;
        }
        // get a random number between 0 1
        public static double GetRandom()
        {
            // random seed
            var seed = Guid.NewGuid().GetHashCode();
            var r = new Random(seed);
            var i = r.Next(0, 100000);
            return ((double)i) / 100000.0;
        }

        // which block
        public static Block InWhichBlock(Vector pos, Floor floor)
        {
            var vector = pos.NewSubtract(floor.AddX, floor.AddY);
            if (floor.Start)
            {
                if (vector.X >= kR && vector.X <= kEnterWidth - kR && 
                    vector.Y <= 0 && vector.Y >= -kEnterHeight)
                    return Block.StartBlock;

                if (vector.X >= kR && vector.X <= kEnterWidth - kR && 
                    vector.Y >= 0 && vector.Y <= kCornerHeight)
                    return Block.FirstCorner;

                if (vector.X >= kR && vector.X <= kEnterWidth - kR && 
                    vector.Y >= kCornerHeight && vector.Y <= kCornerHeight + kStairLength * kStairNum)
                    return Block.FirstStair;
                if (vector.X >= kR && vector.X <= kEnterWidth && 
                    vector.Y >= kCornerHeight + kStairLength * kStairNum && vector.Y <= 2 * kCornerHeight + kStairLength * kStairNum - kR)
                    return Block.SecondCorner;
                if (vector.X >= kEnterWidth && vector.X <= kCornerWidth + kIntervalLength && 
                    vector.Y >= kCornerHeight + kStairLength * kStairNum + kR && vector.Y <= 2 * kCornerHeight + kStairLength * kStairNum - kR)
                    return Block.FirstInterval;
                if (vector.X >= kCornerWidth + kIntervalLength && vector.X <= 2 * kCornerWidth + kIntervalLength - kR &&
                    vector.Y >= kCornerHeight + kStairLength * kStairNum && vector.Y <= 2 * kCornerHeight + kStairLength * kStairNum - kR)
                    return Block.ThirdCorner;
                if (vector.X >= kCornerWidth + kIntervalLength + kR && vector.X <= 2 * kCornerWidth + kIntervalLength - kR &&
                    vector.Y <= kCornerHeight + kStairNum * kStairLength /*should not run over one step*/ && vector.Y >= kCornerHeight + kStairNum * kStairLength - kStepLen)
                    return Block.SecondStair;
                return Block.OutOfSize;
            }
            else if (floor.End)
            {
                if (vector.X >= kCornerWidth + kIntervalLength + kR && vector.X <= 2 * kCornerWidth + kIntervalLength - kR &&
                    vector.Y <= kCornerHeight + kStairLength * kStairNum && vector.Y >= kCornerHeight)
                    return Block.SecondStair;

                if (vector.X >= kCornerWidth + kIntervalLength + kR && vector.X <= 2 * kCornerWidth + kIntervalLength &&
                    vector.Y >= kR && vector.Y <= kCornerHeight)
                    return Block.FourthCorner;

                if (vector.X >= 2 * kCornerWidth + kIntervalLength && vector.Y >= kR && vector.Y <= kCornerHeight - kR)
                    return Block.ExitBlock;

                return Block.OutOfSize;
            }
            else
            {
                if (vector.X >= kR && vector.X <= kEnterWidth - kR &&
                    vector.Y <= 0 && vector.Y >= -kEnterHeight)
                    return Block.StartBlock;

                if (vector.X >= kR && vector.X <= kEnterWidth &&
                    vector.Y >= 0 && vector.Y <= kCornerHeight)
                    return Block.FirstCorner;

                if (vector.X >= kR && vector.X <= kEnterWidth - kR &&
                    vector.Y >= kCornerHeight && vector.Y <= kCornerHeight + kStairLength * kStairNum)
                    return Block.FirstStair;

                if (vector.X >= kR && vector.X <= kEnterWidth &&
                    vector.Y >= kCornerHeight + kStairLength * kStairNum && vector.Y <= 2 * kCornerHeight + kStairLength * kStairNum - kR)
                    return Block.SecondCorner;

                if (vector.X >= kCornerWidth && vector.X <= kCornerWidth + kIntervalLength &&
                    vector.Y >= kCornerHeight + kStairLength * kStairNum + kR && vector.Y <= 2 * kCornerHeight + kStairLength * kStairNum - kR)
                    return Block.FirstInterval;

                if (vector.X >= kCornerWidth + kIntervalLength && vector.X <= 2 * kCornerWidth + kIntervalLength - kR &&
                    vector.Y >= kCornerHeight + kStairLength * kStairNum && vector.Y <= 2 * kCornerHeight + kStairLength * kStairNum - kR)
                    return Block.ThirdCorner;

                if (vector.X >= kCornerWidth + kIntervalLength + kR && vector.X <= 2 * kCornerWidth + kIntervalLength - kR &&
                    vector.Y >= kCornerHeight && vector.Y <= kCornerHeight + kStairLength * kStairNum)
                    return Block.SecondStair;

                if (vector.X >= kIntervalLength + kCornerWidth && vector.X <= 2 * kCornerWidth + kIntervalLength - kR &&
                    vector.Y >= kR && vector.Y <= kCornerHeight)
                    return Block.FourthCorner;

                if (vector.X >= kCornerWidth && vector.X <= kCornerWidth + kIntervalLength && 
                    vector.Y >= kR && vector.Y <= kCornerHeight - kR)
                    return Block.SecondInterval;

                return Block.OutOfSize;
            }
        }
        // calculate the floor field
        public static double GetSff(Ped ped, Vector target)
        {

            var atFloor = ped.GetAtFloor();
            var curBlock = InWhichBlock(ped.CurPos, atFloor);
            var targetBlock = InWhichBlock(target, atFloor);
            if (targetBlock == Block.OutOfSize) return Double.MaxValue;

            var vector = target.NewSubtract(atFloor.AddX, atFloor.AddY);
            if (atFloor.Start)
            {
                
                switch (targetBlock)
                {
                    
                    case Block.StartBlock:
                        // by curBlock to judge
                        if (curBlock != Block.StartBlock) return Double.MaxValue;
                        return -vector.Y * kSffScale;
                    case Block.FirstCorner:
                        if (curBlock != Block.StartBlock && curBlock != Block.FirstCorner) return Double.MaxValue;
                        return -vector.Y * kSffScale;
                    case Block.FirstStair:
                        if (curBlock != Block.FirstStair && curBlock != Block.FirstCorner) return Double.MaxValue;
                        return -vector.Y * kSffScale;
                    case Block.SecondCorner:
                        if (curBlock != Block.SecondCorner && curBlock != Block.FirstStair) return Double.MaxValue;
                        // second corner in this target
                        var tanX = (kCornerWidth - vector.X) / (double)(vector.Y - kCornerHeight - kStairNum * kStairLength);
                        return GetCornerSff(-(kCornerHeight + kStairNum * kStairLength) * kSffScale, 
                            -(kCornerHeight + kStairNum * kStairLength) * kSffScale - kCornerVal, tanX);
                    case Block.FirstInterval:
                        if (curBlock != Block.FirstInterval && curBlock != Block.SecondCorner) return Double.MaxValue;
                        return -(kCornerHeight + kStairNum * kStairLength) * kSffScale - kCornerVal - (vector.X - kCornerWidth) * kSffScale;
                    case Block.ThirdCorner:
                        if (curBlock != Block.ThirdCorner && curBlock != Block.FirstInterval) return Double.MaxValue;
                        tanX = (vector.Y - kCornerHeight - kStairNum * kStairLength) / (double)(vector.X - kCornerWidth - kIntervalLength);
                        return GetCornerSff(-(kCornerHeight + kStairNum * kStairLength) * kSffScale - kCornerVal - kIntervalLength * kSffScale, 
                            -(kCornerHeight + kStairNum * kStairLength) * kSffScale - kCornerVal - kIntervalLength * kSffScale - kCornerVal, tanX);
                    case Block.SecondStair:
                        if (curBlock != Block.ThirdCorner && curBlock != Block.FirstInterval) return Double.MaxValue;
                        return -(kCornerHeight + kStairNum * kStairLength) * kSffScale -
                            kCornerVal - kIntervalLength * kSffScale - kCornerVal + (vector.Y - kCornerHeight - kStairNum * kStairLength) * kSffScale;

                }
            }
            else if (atFloor.End)
            {

                switch (targetBlock)
                {
                    case Block.SecondStair:
                        if (curBlock != Block.SecondStair) return Double.MaxValue;
                        return vector.Y * kSffScale;
                    case Block.FourthCorner:
                        if (curBlock != Block.FourthCorner && curBlock != Block.SecondStair) return Double.MaxValue;
                        var tanX = (2 * kCornerWidth + kIntervalLength - vector.X) / (double)(kCornerHeight - vector.Y);
                        return GetCornerSff(kCornerHeight * kSffScale, kCornerHeight * kSffScale - kCornerVal, tanX);
                    case Block.ExitBlock:
                        if (curBlock != Block.ExitBlock && curBlock != Block.FourthCorner) return Double.MaxValue;
                        return kCornerHeight * kSffScale - kCornerVal - (vector.X - kCornerWidth - kIntervalLength) * kSffScale;
                }
            }
            else
            {

                switch (targetBlock)
                {
                    // judge by cur's block
                    case Block.StartBlock:
                        if (ped.StartFloor != atFloor.Number) return Double.MaxValue;
                        if (curBlock != Block.StartBlock) return Double.MaxValue;
                        return -vector.Y * kSffScale;
                    case Block.FirstCorner:
                        if (ped.StartFloor == atFloor.Number)
                        {
                            if (curBlock != Block.StartBlock && curBlock != Block.FirstCorner) return Double.MaxValue;
                            return -vector.Y * kSffScale;
                        }
                        if (curBlock != Block.SecondInterval && curBlock != Block.FirstCorner) return Double.MaxValue;
                        var tanX = (kCornerHeight - vector.Y) / (double)(kCornerWidth - vector.X);
                        return GetCornerSff(-kCornerHeight * kSffScale + kCornerVal, -kCornerHeight * kSffScale, tanX);

                    case Block.FirstStair:
                        if (curBlock != Block.FirstStair && curBlock != Block.FirstCorner && curBlock != Block.SecondInterval) return Double.MaxValue;
                        if (ped.StartFloor == atFloor.Number)
                        {
                            if (curBlock != Block.FirstStair && curBlock != Block.FirstCorner) return Double.MaxValue;
                        }
                        return -vector.Y * kSffScale;

                    case Block.SecondCorner:
                        // second corner in this target
                        if (curBlock != Block.SecondCorner && curBlock != Block.FirstStair) return Double.MaxValue;
                        tanX = (kCornerWidth - vector.X) / (double)(vector.Y - kCornerHeight - kStairNum * kStairLength);
                        return GetCornerSff(-(kCornerHeight + kStairNum * kStairLength) * kSffScale,
                            -(kCornerHeight + kStairNum * kStairLength) * kSffScale - kCornerVal, tanX);

                    case Block.FirstInterval:
                        if (curBlock != Block.SecondCorner && curBlock != Block.FirstInterval) return Double.MaxValue;
                        if (curBlock != Block.FirstInterval && curBlock != Block.SecondCorner && curBlock != Block.FirstStair) return Double.MaxValue;
                        return -(kCornerHeight + kStairNum * kStairLength) * kSffScale - kCornerVal - (vector.X - kCornerWidth) * kSffScale;

                    case Block.ThirdCorner:
                        if (curBlock != Block.ThirdCorner && curBlock != Block.FirstInterval) return Double.MaxValue;
                        tanX = (vector.Y - kCornerHeight - kStairNum * kStairLength) / (double)(vector.X - kCornerWidth - kIntervalLength);
                        return GetCornerSff(-(kCornerHeight + kStairNum * kStairLength) * kSffScale - kCornerVal - kIntervalLength * kSffScale,
                            -(kCornerHeight + kStairNum * kStairLength) * kSffScale - 2 * kCornerVal - kIntervalLength * kSffScale, tanX);

                    case Block.SecondStair:

                        if (curBlock == Block.ThirdCorner || curBlock == Block.FirstInterval)
                        {
                            return -(kCornerHeight + kStairNum * kStairLength) * kSffScale -
                               kCornerVal - kIntervalLength * kSffScale - kCornerVal + (vector.Y - kCornerHeight - kStairNum * kStairLength) * kSffScale;

                        }
                        else
                        {
                            if (curBlock != Block.SecondStair)
                                return Double.MaxValue;
                            return -kCornerHeight * kSffScale + 2 * kCornerVal + kIntervalLength * kSffScale + (vector.Y - kCornerHeight) * kSffScale;
                        }

                    case Block.FourthCorner:
                        if (ped.StartFloor == atFloor.Number) return Double.MaxValue;
                        if (curBlock != Block.FourthCorner && curBlock != Block.SecondStair) return Double.MaxValue;
                        tanX = (vector.X - kCornerWidth - kIntervalLength) / (double)(kCornerHeight - vector.Y);
                        return GetCornerSff(-kCornerHeight * kSffScale + 2 * kCornerVal + kIntervalLength * kSffScale, -kCornerHeight * kSffScale + kCornerVal + kIntervalLength * kSffScale, tanX);

                    case Block.SecondInterval:
                        if (ped.StartFloor == atFloor.Number) return Double.MaxValue;
                        if (curBlock != Block.SecondInterval && curBlock != Block.FourthCorner) return Double.MaxValue;
                        return -kCornerHeight * kSffScale + kCornerVal + (vector.X - kCornerWidth) * kSffScale;

                }
            }

            return Double.MaxValue;
        }
        // bool can move in thie func
        public static bool CanMove(Ped ped, Vector target, Floor floor)
        {
            foreach (var p in floor.Peds)
            {
                if (p.Equals(ped)) continue;
                if (target.DistanceTo(p.CurPos) < 2 * kR) return false;
            }

            foreach (var wall in floor.Walls)
            {
                if (wall.Green) continue;
                if (wall.InWall(target, kR / 2.0))
                {
                    if (wall.DistanceTo(target) < kR) return false;
                }
            }
            return true;
        }

        // calculate to next floor
        public static double FromNextPeds(Ped ped, Floor nextFloor)
        {
            double res = 0.0;
            var pedVector = ped.CurPos.NewSubtract(ped.GetAtFloor().AddX, ped.GetAtFloor().AddY);
            foreach (var nextPed in nextFloor.Peds)
            {
                if (InWhichBlock(nextPed.CurPos, nextFloor) == Block.SecondStair)
                {
                    var nextVector = nextPed.CurPos.NewSubtract(nextFloor.AddX, nextFloor.AddY);
                    res += CalculateFromPed(nextVector, pedVector);
                }
            }
            return res;
        }

        // enhance judge
        public static bool CanMove(Ped ped, Vector target, Space space)
        {
            var atFloor = ped.GetAtFloor();
            // ped's curBlock
            var curBlock = InWhichBlock(ped.CurPos, atFloor);
            foreach (var p in atFloor.Peds)
            {
                if (p.Equals(ped)) continue;
                if (curBlock == Block.FirstInterval || curBlock == Block.ThirdCorner) 
                {
                    var pedBlock = InWhichBlock(p.CurPos, atFloor);
                    // the ped in second stair should not be calculated
                    if (pedBlock == Block.SecondStair) continue;
                }
                if (target.DistanceTo(p.CurPos) < 2 * kR) return false;
            }

            foreach (var wall in atFloor.Walls)
            {
                if (wall.Green) continue;
                if (wall.InWall(target, kR / 2.0))
                {
                    if (wall.DistanceTo(target) < kR) return false;
                }
            }


            if ((curBlock == Block.ThirdCorner || curBlock == Block.FirstInterval) && atFloor.Number > 0)
            {
                var nextFloor = space.Floors[atFloor.Number - 1];
                var vector = target.NewSubtract(atFloor.AddX, atFloor.AddY);
                // var ped in next floor
                foreach (var nextPed in nextFloor.Peds)
                {
                    if (InWhichBlock(nextPed.CurPos, nextFloor) == Block.SecondStair)
                    {
                        var pedVector = nextPed.CurPos.NewSubtract(nextFloor.AddX, nextFloor.AddY);
                        if (pedVector.DistanceTo(vector) < 2 * kR) return false;
                    }
                }
            }

            return true;

        }
        // add random ped to start block
        public static void InitRandomPed(Floor floor, Space space)
        {
            // add init ped count to every floor
            for (int i = 0; i < kInitPedNum; i++)
            {
                double x = floor.AddX + GetRandom() * (kCornerWidth - 2 * kR) + kR;
                double y = floor.AddY - GetRandom() * (kCornerWidth - 2 * kR) - kR;
                bool can = true;
                while (can)
                {
                    can = false;
                    foreach (var ped in floor.Peds)
                    {
                        if (ped.CurPos.DistanceTo(x, y) < 2 * kR)
                        {
                            can = true;
                            x = floor.AddX + GetRandom() * (kCornerWidth - 2 * kR) + kR;
                            y = floor.AddY - GetRandom() * (kCornerWidth - 2 * kR) - kR;
                        }
                    }
                }
                floor.Add(new Ped(x, y, floor.Number, space));
            }
        }

        // add random ped to floor(judge cnt to avoid dead loop)
        public static bool AddRandomPed(Floor floor, Space space)
        {
            double x = floor.AddX + GetRandom() * (kCornerWidth - 2 * kR) + kR;
            double y = floor.AddY - GetRandom() * (kCornerWidth - 2 * kR) - kR;
            int cnt = 0;
            bool can = true;
            while (can && cnt < 5)
            {
                can = false;
                foreach (var ped in floor.Peds)
                {
                    if (ped.CurPos.DistanceTo(x, y) < 2 * kR)
                    {
                        can = true;
                        cnt++;
                        x = floor.AddX + GetRandom() * (kCornerWidth - 2 * kR) + kR;
                        y = floor.AddY - GetRandom() * (kCornerWidth - 2 * kR) - kR;
                    }
                }
            }
            if (!can)
            {
                floor.Add(new Ped(x, y, floor.Number, space));
                return true;
            }
            return false;
        }

        // read int from input
        public static int StringToInt(string input, int max, int min)
        {
            int result;
            if (int.TryParse(input.Trim(), out result))
            {
                if (result < min) return min;
                if (result > max) return max;
                return result;
            }
            return min;
        }
        // set system's size
        public static void SetChartLength(Chart chart)
        {
            chart.Size = new Size(10 * 50, 300);
        }

    }
}
