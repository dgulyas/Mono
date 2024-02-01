using Microsoft.Xna.Framework;

namespace MonoTest.Common
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
    }
}
