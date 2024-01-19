//Finds the intersection point between 2 arbitrary line segments.
//Lines sweep across the field and a circle is drawn on the intersection.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame;

namespace MonoTest
{
    public class Game6 : Game
    {
        readonly GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        int screenSize = 600;

        Line l1;
        Line l2;
        Vector2 v1;

        public Game6()
        {
            graphics = new GraphicsDeviceManager(this);
        }

        protected override void Initialize()
        {
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = screenSize;
            graphics.PreferredBackBufferHeight = screenSize;
            graphics.ApplyChanges();

            l1 = new Line(new Vector2(5, 5), new Vector2(400, 400));
            l2 = new Line(new Vector2(5, 400), new Vector2(400, 5));
            v1 = Helpers.FindIntersection(l1, l2).Value;

            base.Initialize();
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
            l1.P2.X += 4;
            if (l1.P2.X > 600)
            {
                l1.P2.X = 200;
            }

            l1.P2.Y = 800 - l1.P2.X;

            l2.P1.Y += 1;
            if (l2.P1.Y > 500)
            {
                l2.P1.Y = 350;
            }

            l2.P2.X -= 2;
            if (l2.P2.X < 350)
            {
                l2.P2.X = 500;
            }

            v1 = Helpers.FindIntersection(l1, l2).Value;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.DrawLine(l1.P1, l1.P2, Color.Brown);
            spriteBatch.DrawLine(l2.P1, l2.P2, Color.Brown);
            spriteBatch.DrawCircle(v1, 20, 10, Color.Purple);
            spriteBatch.End();
        }
    }
}