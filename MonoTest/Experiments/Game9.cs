using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTest.Common;
using System.Collections.Generic;

namespace MonoTest.Experiments
{
    //Still no trig :)
    //This works by finding the slope of the wall, rotating it 90 degrees (-1/slope)
    //and adding it to the un-reflected point. This gives 2 ends of a line. The
    //intersection between that line and the wall is found. Then the un-reflected
    //point and the intersection are used to find the reflected point.
    //Corners of walls aren't handled very well, if the reflected point ends up behind
    //a different wall.

    public class Game9 : Game
    {
        readonly GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D canvas;

        int screenSize = 1200;

        List<Vector2> Points;
        List<Vector2> pDirs;
        List<Color> Rainbow = new List<Color> {
            Color.Red, Color.Orange, Color.Yellow,
            Color.Green, Color.Blue, Color.Indigo};
        List<Line> walls;

        public Game9()
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
            Points = new List<Vector2>();
            pDirs = new List<Vector2>();

            for (int i = 0; i < Rainbow.Count * 4; i++)
            {
                Points.Add(new Vector2(120 + 18 * i, 50));
                pDirs.Add(new Vector2(1, 2.1f));
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
            walls.Add(new Line(new Vector2(screenSize - 40, 0), new Vector2(screenSize - 40, screenSize)));
            walls.Add(new Line(new Vector2(0, 50), new Vector2(screenSize, 50)));
            walls.Add(new Line(new Vector2(0, screenSize - 50), new Vector2(screenSize, screenSize - 50)));
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            canvas = new Texture2D(GraphicsDevice, screenSize, screenSize);
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
            spriteBatch.Dispose();
            canvas.Dispose();
        }

        protected override void Update(GameTime gameTime)
        {
            for (int i = 0; i < Points.Count; i++)
            {
                var p = Points[i];
                var pDir = pDirs[i];
                var newPos = p + pDir;

                foreach (var wall in walls)
                {
                    var intersection = Helpers.FindIntersection(wall, new Line(p, newPos));

                    if (intersection != null)
                    {
                        var reflPos = Helpers.ReflectPoint(wall, newPos);
                        pDir = Helpers.FindPointAlongRay(intersection.Value,
                            reflPos,
                            pDir.Length());
                        pDir -= intersection.Value;

                        newPos = reflPos;
                    }
                }

                p = newPos;
                Points[i] = p;
                pDirs[i] = pDir;

                var color = Rainbow[i % 6];

                canvas.SetData(0, new Rectangle((int)p.X, (int)p.Y, 2, 2),
                    new[] { color, color, color, color }, 0, 4);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            //foreach(var wall in walls)
            //{
            //    spriteBatch.DrawLine(wall, Color.Brown);
            //}
            //foreach(var p in Points)
            //{
            //    spriteBatch.DrawCircle(p, 20, 20, Color.Red);
            //}
            spriteBatch.Draw(canvas, new Vector2(0, 0), Color.White);
            spriteBatch.End();
        }


    }
}
