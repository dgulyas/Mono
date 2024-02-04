using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoTest.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MonoTest.Experiments
{
    //This is the first actually interesting things I've made :)

    public class Base : Game
    {
        readonly GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        int screenSize = 1200;
        List<Line> walls;
        List<Particle> Particles;

        public Base()
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

            AddMoreParticles();
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
            walls.Add(new Line(new Vector2(1, 1), new Vector2(1, screenSize - 1)));
            walls.Add(new Line(new Vector2(screenSize -1, 1), new Vector2(screenSize - 1, screenSize - 1)));
            walls.Add(new Line(new Vector2(1, 1), new Vector2(screenSize - 1, 1)));
            walls.Add(new Line(new Vector2(1, screenSize - 1), new Vector2(screenSize, screenSize - 1)));
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
            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Space))
            {
                UpdateParticlePosition();
            }
        }

        private void UpdateParticlePosition()
        {
            Parallel.ForEach(Particles, particle =>
            {
                var p = particle.Position;
                var pDir = particle.Direction;
                var newPos = p + pDir;

                if (newPos.X < 1)
                {
                    newPos.X = 1;
                    pDir.X *= -1;
                }
                else if (newPos.X > screenSize - 1)
                {
                    newPos.X = screenSize - 1;
                    pDir.X *= -1;
                }

                if (newPos.Y < 1)
                {
                    newPos.Y = 1;
                    pDir.Y *= -1;
                }
                else if (newPos.Y > screenSize - 1)
                {
                    newPos.Y = screenSize - 1;
                    pDir.Y *= -1;
                }

                //foreach (var wall in walls)
                //{
                //    var intersection = Helpers.FindIntersection(wall, new Line(p, newPos));

                //    if (intersection != null)
                //    {
                //        if (wall.P1.X == wall.P2.X)
                //        {
                //            pDir.X *= -1;

                //            var diff = newPos.X - wall.P1.X;
                //            newPos.X = wall.P1.X - diff;
                //        }
                //        else
                //        {
                //            pDir.Y *= -1;

                //            var diff = newPos.Y - wall.P1.Y;
                //            newPos.Y = wall.P1.Y - diff;
                //        }

                //    }
                //}

                particle.Position = newPos;
                particle.Direction = pDir;
            });
        }

        private void AddMoreParticles()
        {
            for (int i = 0; i < 2000; i++)
            {
                var angle = Math.PI / 1000 * i + .01;

                Particles.Add(new Particle()
                {
                    Position = new Vector2(600, 600),
                    Direction = new Vector2(1.9f * (float)Math.Cos(angle), 1.9f * (float)Math.Sin(angle)),
                    Color = Helpers.HSL2RGB((double)i / 2000, 0.5, 0.5)
                });
            }
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
