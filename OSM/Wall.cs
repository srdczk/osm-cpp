using System;

namespace OSM
{
    class Wall
    {
        // begin and end's point
        public Vector Begin { get; }
        public Vector End { get; }
        public bool Green { get; set; }
        // init
        public Wall(double beginX, double beginY, double endX, double endY)
        {
            Begin = new Vector(beginX, beginY);
            End = new Vector(endX, endY);
            Green = false;
        }
        public Wall(Vector begin, Vector end)
        {
            Begin = new Vector(begin.X, begin.Y);
            End = new Vector(end.X, end.Y);
        }
        // calculate cross point
        public Vector CrossPoint(Vector vector)
        {
            // if is vertical 
            if (Util.IsZero(Begin.X - End.X))
            {
                return new Vector(Begin.X, vector.Y);
            }
            // if is horizontal
            if (Util.IsZero(Begin.Y - End.Y))
            {
                return new Vector(vector.X, Begin.Y);
            }
            // calculate
            double aW = (End.Y - Begin.Y) / (End.X - Begin.X), 
                bW = Begin.Y - aW * Begin.X, 
                aV = (-1) / aW, 
                bV = vector.Y - aV * vector.X;
            // cross point
            return new Vector((bV - bW) / (aW - aV), aW * ((bV - bW) / (aW - aV)) + bW);
        }
        // the distance from wall to point
        public double DistanceTo(Vector vector)
        {
            return CrossPoint(vector).DistanceTo(vector);
        }
        // define is in the wall by r
        public bool InWall(Vector vector, double r)
        {
            var metaDir = End.Normalize(Begin).Multiply(r / 2.0);
            var xBegin = Begin.NewSubtract(metaDir);
            var xEnd = End.NewAdd(metaDir);
            // x, y should > min && < max
            return vector.X > Math.Min(xBegin.X, xEnd.X) &&
                vector.X < Math.Max(xBegin.X, xEnd.X) &&
                vector.Y > Math.Min(xBegin.Y, xEnd.Y) &&
                vector.Y < Math.Max(xBegin.Y, xEnd.Y);
        }
    }
}
