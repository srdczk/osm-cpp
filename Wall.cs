using System;

namespace OSM
{
    public class Wall
    {
        private Vector begin, end;

        public Wall(Vector p, Vector q)
        {
            begin = p;
            end = q;
        }

        public Wall(double x, double y, double p, double q)
        {
            begin = new Vector(x, y);
            end = new Vector(p, q);
        }
        
        // 计算点在直线上的交点
        public Vector CrossPoint(Vector vector)
        {
            // 直线方程 y = a * x + b
            // 考虑 斜率为 0  或者 无穷大
            if (CustomizeUtil.IsZero(begin.GetX() - end.GetX())) 
            {
                // 直线的方程 : x = begin.GetX();
                return new Vector(begin.GetX(), vector.GetY());
            } 
            else if (CustomizeUtil.IsZero(begin.GetY() - end.GetY())) 
            {
                // 直线 方程: y = begin.GetY();
                return new Vector(vector.GetX(), begin.GetY());
            }
            else
            {
                double aW = (end.GetY() - begin.GetY()) / (end.GetX() - begin.GetX())
                    , bW = begin.GetY() - aW * begin.GetX()
                    , aV =  (-1) / aW
                    , bV = vector.GetY() - aV * vector.GetX();
                // 交点
                return new Vector((bV - bW) / (aW - aV), aW * ((bV - bW) / (aW - aV)) + bW);
            }
        }
        
        public bool IsIn(Vector vector, double r) 
        {
            Vector metaDir = end.Normalize(begin).Multiply(r / 2.0);
            Vector xBegin = begin.NewSubtract(metaDir), xEnd = end.NewAdd(metaDir);

            return vector.GetX() >= Math.Min(xBegin.GetX(), xEnd.GetX())
                   && vector.GetX() <= Math.Max(xBegin.GetX(), xEnd.GetX())
                   && vector.GetY() >= Math.Min(xBegin.GetY(), xEnd.GetY())
                   && vector.GetY() <= Math.Max(xBegin.GetY(), xEnd.GetY());

        }
        
        public double DistanceTo(Vector vector) 
        {
            // 点到线的距离
            return vector.DistanceTo(CrossPoint(vector));
        }
        
        
        public Vector GetBegin() 
        {
            return begin;
        }

        public Vector GetEnd() 
        {
            return end;
        }

        
    }
}