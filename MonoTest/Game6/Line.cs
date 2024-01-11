using Microsoft.Xna.Framework;
using System;

namespace MonoTest.Game6
{
    //https://thirdpartyninjas.com/blog/2008/10/07/line-segment-intersection/
    //Line segments intersection math

    public class Line
    {
        public Vector2 P1;
        public Vector2 P2;

        public Line(Vector2 p1, Vector2 p2)
        {
            P1 = p1;
            P2 = p2;
        }

        public Line() { }

        public Vector2? FindIntersection(Line l)
        {
            //This implements a set theory approach for finding line segment intersections.
            //https://thirdpartyninjas.com/blog/2008/10/07/line-segment-intersection/

            var precision = 0.0000001f;

            var P3 = l.P1;
            var P4 = l.P2;

            //den = (y4-y3)(x2-x1)-(x4-x3)(y2-y1)
            var denominator = (P4.Y - P3.Y) * (P2.X - P1.X) - (P4.X - P3.X) * (P2.Y - P1.Y);

            if(Math.Abs(denominator) < precision)
            {
                //Line segments are parallel
                return null;
            }

            //Ua = (x4-x3)(y1-y3)-(y4-y3)(x1-x3)
            //Ub = (x2-x1)(y1-y3)-(y2-y1)(x1-x3)
            var Ua = ((P4.X - P3.X) * (P1.Y - P3.Y) - (P4.Y - P3.Y) * (P1.X - P3.X)) / denominator;
            var Ub = ((P2.X - P1.X) * (P1.Y - P3.Y) - (P2.Y - P1.Y) * (P1.X - P3.X)) / denominator;

            if(Ua <= 0 || Ua >= 1 || Ub <= 0 || Ub >= 1)
            {
                //Line segments don't cover the intersection point
                return null;
            }

            var x = P1.X + Ua * (P2.X - P1.X);
            var y = P1.Y + Ua * (P2.Y - P1.Y);

            return new Vector2(x, y);
        }
    }

    

}
