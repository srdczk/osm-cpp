using System;

namespace OSM
{
    public class Vector
    {
        private double x, y;
        
        public Vector(double p, double q)
        {
            x = p;
            y = q;
        }
        
        public Vector(float p, float q)
        {
            x = p;
            y = q;
        }
        
        public double Length() 
        {
            return Math.Sqrt(x * x + y * y);
        }
        
        public double DistanceTo(Vector vector) 
        {
            return Math.Sqrt((x - vector.x) * (x - vector.x)
                             + (y - vector.y) * (y - vector.y));
        }
        // 申请新对象操作和本地对象操作
        public Vector NewAdd(Vector vector) 
        {
            return new Vector(x + vector.x, y + vector.y);
        }
        
        public Vector Add(Vector vector) 
        {
            x += vector.x;
            y += vector.y;
            return this;
        }
        
        public Vector NewSubtract(Vector vector) 
        {
            return new Vector(x - vector.x, y - vector.y);
        }

        public Vector Subtract(Vector vector) 
        {
            x -= vector.x;
            y -= vector.y;
            return this;
        }
        
        public Vector NewMultiply(Vector vector) 
        {
            return new Vector(x * vector.x, y * vector.y);
        }

        public Vector Multiply(Vector vector) 
        {
            x *= vector.x;
            y *= vector.y;
            return this;
        }
        
        public Vector NewMultiply(double d) 
        {
            return new Vector(x * d, y * d);
        }

        public Vector Multiply(double d) 
        {
            x *= d;
            y *= d;
            return this;
        }
        
        public Vector Normalize(Vector vector)
        {
            if (CustomizeUtil.IsSameVector(vector, this)) return new Vector(1, 0);
            double distance = DistanceTo(vector);
            double p = this.x - vector.x;
            double q = this.y - vector.y;
            p /= distance;
            q /= distance;
            return new Vector(p, q);
        }
        // 逆时针 旋转 角度制的值
        public Vector Rotate(int angle) 
        {
            return Rotate((angle / 180.0 * Math.PI));
        }

        // 逆时针旋转弧度制的值
        public Vector Rotate(double angle) 
        {
            double tx = x * Math.Cos(angle) - y * Math.Sin(angle);
            double ty = y * Math.Cos(angle) + x * Math.Sin(angle);
            x = tx;
            y = ty;
            return this;
        }
        
        public Vector NewRotate(int angle) 
        {
            return NewRotate((angle / 180.0 * Math.PI));
        }

        public Vector NewRotate(double angle)
        {
            return new Vector(x * Math.Cos(angle) - y * Math.Sin(angle)
                , y * Math.Cos(angle) + x * Math.Sin(angle));
        }

        public void SetX(double x)
        {
            this.x = x;
        }

        public double GetX() {
            return x;
        }

        public double GetY() 
        {
            return y;
        }

    }
}