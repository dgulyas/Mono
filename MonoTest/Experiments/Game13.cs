using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTest.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MonoTest.Experiments
{
    //This now has 1200 points and runs pretty fast.
    //Previously when I didn't call GraphicsDevice.Clear() every
    //frame it wouldn't work. I'm not sure what's different
    //between now and then. I can start doing more interesting
    //things now :)

    public class Game13 : Game
    {
        readonly GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        int screenSize = 1200;
        List<Line> walls;
        List<Particle> Particles;
        List<Color> Rainbow = new List<Color> {
            Color.Red, Color.Orange, Color.Yellow,
            Color.Green, Color.Blue, Color.Indigo};

        public Game13()
        {
            graphics = new GraphicsDeviceManager(this);
        }

        protected override void Initialize()
        {
            SetupGraphics();
            SetupPoints();
            SetupWalls();

            IsFixedTimeStep = false;
            base.Initialize();
        }

        private void SetupPoints()
        {
            Particles = new List<Particle>();

            for (int i = 0; i < Rainbow.Count * 20; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Particles.Add(new Particle()
                    {
                        Position = new Vector2(121 + 8 * i, 51),
                        Direction = new Vector2(2.1f, 1.1f + j),
                        Color = Rainbow[i % Rainbow.Count]
                    });
                }
            }
        }

        private void SetupGraphics()
        {
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = screenSize;
            graphics.PreferredBackBufferHeight = screenSize;
            graphics.ApplyChanges();

            GraphicsDevice.Clear(Color.Black);
        }

        private void SetupWalls()
        {
            walls = new List<Line>();
            walls.Add(new Line(new Vector2(40, 0), new Vector2(40, screenSize)));
            walls.Add(new Line(new Vector2(screenSize - 50, 0), new Vector2(screenSize - 50, screenSize)));
            walls.Add(new Line(new Vector2(0, 50), new Vector2(screenSize, 50)));
            walls.Add(new Line(new Vector2(0, screenSize - 50), new Vector2(screenSize, screenSize - 50)));
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
            spriteBatch.Dispose();
        }

        protected override void Update(GameTime gameTime)
        {
            Parallel.ForEach(Particles, particle =>
            {
                var p = particle.Position;
                var pDir = particle.Direction;
                var newPos = p + pDir;

                foreach (var wall in walls)
                {
                    var intersection = Helpers.FindIntersection(wall, new Line(p, newPos));

                    if (intersection != null)
                    {
                        if (wall.P1.X == wall.P2.X)
                        {
                            pDir.X *= -1;

                            var diff = newPos.X - wall.P1.X;
                            newPos.X = wall.P1.X - diff;
                        }
                        else
                        {
                            pDir.Y *= -1;

                            var diff = newPos.Y - wall.P1.Y;
                            newPos.Y = wall.P1.Y - diff;
                        }

                    }
                }

                particle.Position = newPos;
                particle.Direction = pDir;
            });
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            spriteBatch.Begin();
            foreach (var p in Particles)
            {
                spriteBatch.DrawRectangle(
                    new Rectangle((int)p.Position.X, (int)p.Position.Y, 2, 2),
                    p.Color);
            }
            spriteBatch.End();
        }
    }
}
