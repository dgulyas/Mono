using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame;
using System;
using System.Collections.Generic;
using System.Threading;

namespace MonoTest
{
    //There's a bug in the bouncing code. I'm not sure if it's due to everything being floats, not doubles.
    //The new pDir is wrong, and other things might be as well.
    //I wonder if it's maybe because something's getting converted into ints and then back to
    //floats.
    public class Game7 : Game
    {
        readonly GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D canvas;

        int screenSize = 1200;

        Vector2 p;
        Vector2 pDir;
        List<Line> walls;

        public Game7()
        {
            graphics = new GraphicsDeviceManager(this);
        }

        protected override void Initialize()
        {
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = screenSize;
            graphics.PreferredBackBufferHeight = screenSize;
            graphics.ApplyChanges();

            p = new Vector2(203, 50);
            pDir = new Vector2(-5, 5);

            walls = new List<Line>();
            walls.Add(new Line(new Vector2(50,0), new Vector2(50, screenSize)));
            walls.Add(new Line(new Vector2(screenSize - 50, 0), new Vector2(screenSize - 50, screenSize)));
            walls.Add(new Line(new Vector2(0, 50), new Vector2(screenSize, 50)));
            walls.Add(new Line(new Vector2(0, screenSize - 50), new Vector2(screenSize, screenSize-50)));

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
                    var reflPos = Helpers.FindReflectionPoint(wall, intersection.Value, newPos);
                    pDir = Helpers.FindPointAlongRay(intersection.Value,
                        reflPos,
                        pDir.Length()) - intersection.Value;
                    newPos = reflPos;
                }
            }

            p = newPos;

            canvas.SetData(0, new Rectangle((int)p.X, (int)p.Y, 2, 2),
                new[] { Color.Red, Color.Red, Color.Red, Color.Red }, 0, 4);

            Console.WriteLine(pDir);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            foreach(var wall in walls)
            {
                spriteBatch.DrawLine(wall, Color.Brown);
            }
            spriteBatch.Draw(canvas, new Vector2(0, 0), Color.White);
            spriteBatch.End();
        }


    }
}
