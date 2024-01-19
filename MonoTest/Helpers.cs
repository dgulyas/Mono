using Microsoft.Xna.Framework;
using System;

namespace MonoTest
{
    public static class Helpers
    {

        /// <summary>
        /// Every tick, a point moves along it's path a certain distance. If it would
        /// cross over a wall, it should bounce off it instead.
        /// </summary>
        /// <param name="wall">A line representing the wall the point is bouncing off of</param>
        /// <param name="impactPos">The coordinates on the wall where the point hits it</param>
        /// <param name="endPos">The ending coordinates of the point if it didn't bounce off the wall</param>
        /// <returns>The position fo the point if it had bounced off the wall</returns>
        public  static Vector2 FindReflectionPoint(Line wall, Vector2 impactPos, Vector2 endPos)
        {
            //This works by taking the intersection between 2 circles.
            //The circle centered on one end of the wall going through the endPos
            //And the other cnetered on the impactPos, going through the endPos
            //So one of the intersection points will be the endPos
            //and the other will be the reflected point we want.
            var wallP = wall.P1;

            // Find the distance between the centers.
            float dist = Vector2.Distance(impactPos, wall.P1);

            var rad1 = Vector2.Distance(wallP, endPos);
            var rad2 = Vector2.Distance(impactPos, endPos);

            // See how many solutions there are.
            if (dist > rad1 + rad2)
            {   // The circles don't touch/overlap.
                // This should be impossible since they share a common point.
                throw new Exception();
            }
            else if (dist < Math.Abs(rad2 - rad1))
            {
                // No solutions, one circle contains the other.
                // This should be impossible since they share a common point.
                throw new Exception();
            }
            else if ((dist == 0) && (rad1 == rad2))
            {
                // No solutions, the circles coincide.
                // This would happen if the impact point is exactly on the end.
                //Hopefully this never happens.
                throw new Exception();
            }
            
            float a = (rad1 * rad1 -
                rad2 * rad2 + dist * dist) / (2 * dist);
            float h = (float)Math.Sqrt(rad1 * rad1 - a * a);

            float cx2 = wallP.X + a * (impactPos.X - wallP.X) / dist;
            float cy2 = wallP.Y + a * (impactPos.Y - wallP.Y) / dist;

            //These two points are where the circles intersect.
            var P1 = new Vector2(
                cx2 + h * (impactPos.Y - wallP.Y) / dist,
                cy2 - h * (impactPos.X - wallP.X) / dist);
            var P2 = new Vector2(
                cx2 - h * (impactPos.Y - wallP.Y) / dist,
                cy2 + h * (impactPos.X - wallP.X) / dist);

            //The reflection point will be the one farthest away from the endPos
            if(Vector2.Distance(endPos, P1) > Vector2.Distance(endPos, P2))
            {
                return P1;
            }
            else
            {
                return P2;
            }

        }

        public static Vector2? FindIntersection(Line l1, Line l2)
        {
            //This implements a set theory approach for finding line segment intersections.
            //https://thirdpartyninjas.com/blog/2008/10/07/line-segment-intersection/

            var precision = 0.0000001f;

            var P1 = l1.P1;
            var P2 = l1.P2;
            var P3 = l2.P1;
            var P4 = l2.P2;

            //den = (y4-y3)(x2-x1)-(x4-x3)(y2-y1)
            var denominator = (P4.Y - P3.Y) * (P2.X - P1.X) - (P4.X - P3.X) * (P2.Y - P1.Y);

            if (Math.Abs(denominator) < precision)
            {
                //Line segments are parallel
                return null;
            }

            //Ua = (x4-x3)(y1-y3)-(y4-y3)(x1-x3)
            //Ub = (x2-x1)(y1-y3)-(y2-y1)(x1-x3)
            var Ua = ((P4.X - P3.X) * (P1.Y - P3.Y) - (P4.Y - P3.Y) * (P1.X - P3.X)) / denominator;
            var Ub = ((P2.X - P1.X) * (P1.Y - P3.Y) - (P2.Y - P1.Y) * (P1.X - P3.X)) / denominator;

            if (Ua <= 0 || Ua >= 1 || Ub <= 0 || Ub >= 1)
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
