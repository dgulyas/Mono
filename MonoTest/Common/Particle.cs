using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoTest.Common
{
    public class Particle
    {
        public Vector2 Position { get; set; }
        public Vector2 Direction { get; set; }
        public Color Color { get; set; }
        public Texture2D Texture { get; set; }
    }
}
