using System;

namespace OSM
{
    class Vector
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }
        // length of vector
        public double Length()
        {
            return Math.Sqrt(X * X + Y * Y);
        }
        // distance to another vector
        public double DistanceTo(Vector vector)
        {
            return Math.Sqrt((vector.X - X) * (vector.X - X) + 
                (vector.Y - Y) * (vector.Y - Y));
        }
        // add a vector, 4
        public Vector Add(Vector vector)
        {
            X += vector.X;
            Y += vector.Y;
            return this;
        }

        public Vector Add(double x, double y)
        {
            X += x;
            Y += y;
            return this;
        }

        public Vector NewAdd(Vector vector)
        {
            return new Vector(X + vector.X, Y + vector.Y);
        }

        public Vector NewAdd(double x, double y)
        {
            return new Vector(X + x, Y + y);
        }
        // Subtract a vector, 4
        public Vector Subtract(Vector vector)
        {
            X -= vector.X;
            Y -= vector.Y;
            return this;
        }

        public Vector Subtract(double x, double y)
        {
            X -= x;
            Y -= y;
            return this;
        }

        public Vector NewSubtract(Vector vector)
        {
            return new Vector(X - vector.X, Y - vector.Y);
        }

        public Vector NewSubtract(double x, double y)
        {
            return new Vector(X - x, Y - y);
        }
        // multiply a vector, 6
        public Vector Multiply(Vector vector)
        {
            X *= vector.X;
            Y *= vector.Y;
            return this;
        }

        public Vector Multiply(double x, double y)
        {
            X *= x;
            Y *= y;
            return this;
        }

        public Vector Multiply(double d)
        {
            X *= d;
            Y *= d;
            return this;
        }

        public Vector NewMultiply(Vector vector)
        {
            return new Vector(X * vector.X, Y * vector.Y);
        }

        public Vector NewMultiply(double x, double y)
        {
            return new Vector(X * x, Y * y);
        }

        public Vector NewMultiply(double d)
        {
            return new Vector(d * X, d * Y);
        }
        // rotate theta, radian
        public Vector Rotate(double theta)
        {
            double resX = X * Math.Cos(theta) - Y * Math.Sin(theta);
            double resY = Y * Math.Cos(theta) + X * Math.Sin(theta);
            X = resX;
            Y = resY;
            return this;
        }

        public Vector NewRotate(double theta)
        {
            return new Vector(X * Math.Cos(theta) - Y * Math.Sin(theta), 
                Y * Math.Cos(theta) + X * Math.Sin(theta));
        }
        
        public Vector Normalize(Vector vector)
        {
            // if is same vector
            if (Util.IsSame(this, vector)) return new Vector(1, 0);
            double distance = DistanceTo(vector);
            return new Vector((X - vector.X) / distance, (Y - vector.Y) / distance);
        }

        public double DistanceTo(double x, double y)
        {
            return Math.Sqrt((x - X) * (x - X) + (y - Y) * (y - Y));
        }

    }
}
