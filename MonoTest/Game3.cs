using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoTest
{
    public class Game3 : Game
    {
        readonly GraphicsDeviceManager graphics;

        int[][] sand;
        int width = 256;
        int height = 256;

        public Game3()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            sand = new int[height][];
            for(int j = 0; j < sand.Length; j++)
            {
                sand[j] = new int[width];
                for(int i = 0; i < sand[j].Length; i++)
                {
                    sand[j][i] = i * j % 255;
                }
            }
        }

        protected override void Initialize()
        {
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = width*6+50;
            graphics.PreferredBackBufferHeight = height*6+50;
            graphics.ApplyChanges();

            base.Initialize();
        }

        SpriteBatch spriteBatch;
        Texture2D whiteRectangle;

        protected override void LoadContent()
        {
            base.LoadContent();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // Create a 1px square rectangle texture that will be scaled to the
            // desired size and tinted the desired color at draw time
            whiteRectangle = new Texture2D(GraphicsDevice, 1, 1);
            whiteRectangle.SetData(new[] { Color.White });
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
            spriteBatch.Dispose();

            // If you are creating your texture (instead of loading it with
            // Content.Load) then you must Dispose of it
            whiteRectangle.Dispose();
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            for (int y = 0; y < sand.Length; y++)
            {
                for (int x = 0; x < sand[y].Length; x++)
                {
                    var v = sand[y][x];

                    spriteBatch.Draw(whiteRectangle, new Rectangle(x * 6 + 25, y * 6 + 25, 4, 4), new Color(v,v,v));
                }
            }

            spriteBatch.End();
        }
    }
}