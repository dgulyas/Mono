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
    //Corners of walls aren't handles very well, if the reflected point ends up behind
    //a different wall. Game 9 should try to deal with that.

    public class Game8 : Game
    {
        readonly GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D canvas;

        int screenSize = 1200;

        Vector2 p;
        Vector2 pDir;
        List<Line> walls;

        public Game8()
        {
            graphics = new GraphicsDeviceManager(this);
        }

        protected override void Initialize()
        {
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = screenSize;
            graphics.PreferredBackBufferHeight = screenSize;
            graphics.ApplyChanges();

            p = new Vector2(40, 50);
            pDir = new Vector2(5.3f, 5.3f);

            walls = new List<Line>();
            walls.Add(new Line(new Vector2(40, 0), new Vector2(40, screenSize)));
            walls.Add(new Line(new Vector2(screenSize - 40, 0), new Vector2(screenSize - 40, screenSize)));
            walls.Add(new Line(new Vector2(0, 50), new Vector2(screenSize, 50)));
            walls.Add(new Line(new Vector2(0, screenSize - 50), new Vector2(screenSize, screenSize - 50)));

            base.Initialize();
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

            canvas.SetData(0, new Rectangle((int)p.X, (int)p.Y, 2, 2),
                new[] { Color.Red, Color.Red, Color.Red, Color.Red }, 0, 4);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            foreach (var wall in walls)
            {
                spriteBatch.DrawLine(wall, Color.Brown);
            }
            spriteBatch.DrawCircle(p, 20, 20, Color.Red);
            spriteBatch.Draw(canvas, new Vector2(0, 0), Color.White);
            spriteBatch.End();
        }


    }
}
