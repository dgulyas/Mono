using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame;

namespace MonoTest
{
    public class Game7 : Game
    {
        readonly GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        int screenSize = 600;

        Vector2 p;
        Vector2 pDir;
        Line wall;

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
            wall = new Line(new Vector2(50,50), new Vector2(50, 550));

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
            var newPos = p + pDir;
            
            var intersection = Helpers.FindIntersection(wall, new Line(p, newPos));

            if(intersection != null)
            {
                var reflPos = Helpers.FindReflectionPoint(wall, intersection.Value, newPos);
            }

            p = newPos;
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.DrawLine(wall, Color.Brown);
            spriteBatch.DrawCircle(p, 20, 10, Color.Purple);
            spriteBatch.End();
        }


    }
}
