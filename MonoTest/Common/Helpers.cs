using Microsoft.Xna.Framework;
using System;

namespace MonoTest.Common
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
        public static Vector2 FindReflCircPoint(Line wall, Vector2 impactPos, Vector2 endPos)
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
            else if (dist == 0 && rad1 == rad2)
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
            if (Vector2.Distance(endPos, P1) > Vector2.Distance(endPos, P2))
            {
                return P1;
            }
            else
            {
                return P2;
            }

        }

        public static Vector2? FindIntersection(Line l1, Line l2, bool mustCoverIntersection = true)
        {
            //This implements a set theory approach for finding line segment intersections.
            //https://thirdpartyninjas.com/blog/2008/10/07/line-segment-intersection/

            var precision = 0.0000001f;

            var L1P1 = l1.P1;
            var L1P2 = l1.P2;
            var L2P1 = l2.P1;
            var L2P2 = l2.P2;

            //den = (y4-y3)(x2-x1)-(x4-x3)(y2-y1)
            var denominator = (L2P2.Y - L2P1.Y) * (L1P2.X - L1P1.X) - (L2P2.X - L2P1.X) * (L1P2.Y - L1P1.Y);

            if (Math.Abs(denominator) < precision)
            {
                //Line segments are parallel
                return null;
            }

            //Ua = (x4-x3)(y1-y3)-(y4-y3)(x1-x3)
            //Ub = (x2-x1)(y1-y3)-(y2-y1)(x1-x3)
            var Ua = ((L2P2.X - L2P1.X) * (L1P1.Y - L2P1.Y) - (L2P2.Y - L2P1.Y) * (L1P1.X - L2P1.X)) / denominator;
            var Ub = ((L1P2.X - L1P1.X) * (L1P1.Y - L2P1.Y) - (L1P2.Y - L1P1.Y) * (L1P1.X - L2P1.X)) / denominator;

            if ((Ua <= 0 || Ua > 1 || Ub <= 0 || Ub > 1) && mustCoverIntersection)
            {
                //Line segments don't cover the intersection point
                return null;
            }

            var x = L1P1.X + Ua * (L1P2.X - L1P1.X);
            var y = L1P1.Y + Ua * (L1P2.Y - L1P1.Y);

            return new Vector2(x, y);
        }

        /// <summary>
        /// Given a ray with origin start and passing through end, returns a
        /// point on the ray length distance from start
        /// </summary>
        /// <param name="start">The beginning of the ray</param>
        /// <param name="end">A point on the ray defining the rays direction</param>
        /// <param name="length">The distance the desired point is from start</param>
        public static Vector2 FindPointAlongRay(Vector2 start, Vector2 end, float length)
        {
            //https://math.stackexchange.com/a/2109383
            var distStartEnd = Vector2.Distance(start, end);

            var x = start.X - length * (start.X - end.X) / distStartEnd;
            var y = start.Y - length * (start.Y - end.Y) / distStartEnd;

            return new Vector2(x, y);
        }

        public static Vector2 ReflectPoint(Line wall, Vector2 origPoint)
        {
            var wallSlopeX = wall.P1.X - wall.P2.X;
            var wallSlopeY = wall.P1.Y - wall.P2.Y;

            var secondPoint = origPoint + new Vector2(-1 * wallSlopeY, wallSlopeX);

            var inter = FindIntersection(wall, new Line(origPoint, secondPoint), false);
            var reflectedPoint = 2 * inter.Value - origPoint;
            return reflectedPoint;
        }

        //This function was taken from 
        //https://geekymonkey.com/Programming/CSharp/RGB2HSL_HSL2RGB.htm
        public static Color HSL2RGB(double h, double sl, double l)
        {
            double v;
            double r, g, b;

            r = l;   // default to gray
            g = l;
            b = l;
            v = l <= 0.5 ? l * (1.0 + sl) : l + sl - l * sl;
            if (v > 0)
            {
                double m;
                double sv;
                int sextant;
                double fract, vsf, mid1, mid2;

                m = l + l - v;
                sv = (v - m) / v;
                h *= 6.0;
                sextant = (int)h;
                fract = h - sextant;
                vsf = v * sv * fract;
                mid1 = m + vsf;
                mid2 = v - vsf;
                switch (sextant)
                {
                    case 0:
                        r = v;
                        g = mid1;
                        b = m;
                        break;
                    case 1:
                        r = mid2;
                        g = v;
                        b = m;
                        break;
                    case 2:
                        r = m;
                        g = v;
                        b = mid1;
                        break;
                    case 3:
                        r = m;
                        g = mid2;
                        b = v;
                        break;
                    case 4:
                        r = mid1;
                        g = m;
                        b = v;
                        break;
                    case 5:
                        r = v;
                        g = m;
                        b = mid2;
                        break;
                }
            }
            Color rgb = new Color();
            rgb.R = Convert.ToByte(r * 255.0f);
            rgb.G = Convert.ToByte(g * 255.0f);
            rgb.B = Convert.ToByte(b * 255.0f);
            rgb.A = 255;
            return rgb;
        }

    }
}
