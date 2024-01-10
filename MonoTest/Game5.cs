using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoTest
{
    public class Game5 : Game
    {
        readonly GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D canvas;

        int screenSize = 1000;

        Point p;
        Point v; //vector that's added to p each tick

        public Game5()
        {
            graphics = new GraphicsDeviceManager(this);

            p = new Point(5, 5);
            v = new Point(1, 1);
        }

        protected override void Initialize()
        {
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = screenSize;
            graphics.PreferredBackBufferHeight = screenSize;
            graphics.ApplyChanges();

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
            p = p + v;

            canvas.SetData(0, new Rectangle(p.X, p.Y, 2, 2), 
                new[] { Color.Red, Color.Red, Color.Red, Color.Red }, 0, 4);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(canvas, new Vector2(0,0), Color.White);
            spriteBatch.End();
        }
    }
}