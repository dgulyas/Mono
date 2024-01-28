using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MonoTest
{
    public class Game12 : Game
    {
        readonly GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Queue<Texture2D> canvasQ;
        Texture2D ReadCanvas;
        Texture2D WriteCanvas;
        Color[] bits = new Color[1200 * 1200];

        int screenSize = 1200;
        List<Line> walls;
        List<Particle> Particles;
        List<Color> Rainbow = new List<Color> {
            Color.Red, Color.Orange, Color.Yellow,
            Color.Green, Color.Blue, Color.Indigo};
        
        public Game12()
        {
            graphics = new GraphicsDeviceManager(this);
            
        }

        protected override void Initialize()
        {
            SetupGraphics();
            SetupPoints();
            SetupWalls();

            canvasQ = new Queue<Texture2D>();

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
                    Color = Rainbow[i % Rainbow.Count]
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
            ReadCanvas = new Texture2D(GraphicsDevice, screenSize, screenSize);
            WriteCanvas = new Texture2D(GraphicsDevice, screenSize, screenSize);
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
            spriteBatch.Dispose();
            //canvas.Dispose();
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

            WriteCanvas = new Texture2D(GraphicsDevice, screenSize, screenSize);
            ReadCanvas.GetData(bits);
            WriteCanvas.SetData(bits);

            foreach (var p in Particles)
            {
                var color = p.Color;
                WriteCanvas.SetData(0, new Rectangle((int)p.Position.X, (int)p.Position.Y, 2, 2),
                    new[] { color, color, color, color }, 0, 4);
            }
            
            canvasQ.Enqueue(ReadCanvas);
            ReadCanvas = WriteCanvas;
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            //foreach (var wall in walls)
            //{
            //    spriteBatch.DrawLine(wall, Color.Brown);
            //}
            if (canvasQ.Count > 0)
            {
                spriteBatch.Draw(canvasQ.Dequeue(), Vector2.Zero, Color.White);
            }
            spriteBatch.End();
        }
    }
}
