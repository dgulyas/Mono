using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MonoTest
{
    public class Game11 : Game
    {
        readonly GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        int screenSize = 1200;
        List<Line> walls;
        List<Particle> Particles;
        List<Color> Rainbow = new List<Color> {
            Color.Red, Color.Orange, Color.Yellow,
            Color.Green, Color.Blue, Color.Indigo};
        
        public Game11()
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

            for(int i = 0; i < Rainbow.Count * 20; i++)
            {
                Particles.Add(new Particle()
                {
                    Position = new Vector2(121 + 8 * i, 51),
                    Direction = new Vector2(2.1f, 4.1f),
                    Color = Rainbow[i % Rainbow.Count],
                    Texture = new Texture2D(GraphicsDevice, screenSize, screenSize)
            });
            }            
        }

        private void SetupGraphics()
        {
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = screenSize;
            graphics.PreferredBackBufferHeight = screenSize;
            graphics.ApplyChanges();
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

                //var color = particle.Color;
                //particle.Texture.SetData(0, 
                //    new Rectangle((int)particle.Position.X, (int)particle.Position.Y, 2, 2),
                //    new[] { color, color, color, color }, 0, 4);
            });

            foreach (var p in Particles)
            {
                var color = p.Color;
                p.Texture.SetData(0, new Rectangle((int)p.Position.X, (int)p.Position.Y, 2, 2),
                    new[] { color, color, color, color }, 0, 4);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            foreach(var p in Particles)
            {
                spriteBatch.Draw(p.Texture, new Vector2(0, 0), Color.White);
            }

            spriteBatch.End();
        }
    }

}
