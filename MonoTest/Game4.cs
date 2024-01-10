using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame;
using System;

namespace MonoTest
{
    public class Game4 : Game
    {
        readonly GraphicsDeviceManager graphics;

        int numThings = 300000;
        int screenSize = 2000;

        int[] x;
        int[] y;

        Random rand = new Random();


        public Game4()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            x = new int[numThings];
            y = new int[numThings];
            for (int i = 0; i < x.Length; i++)
            {
                x[i] = screenSize / 2;
                y[i] = screenSize / 2;
            }
        }

        protected override void Initialize()
        {
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = screenSize;
            graphics.PreferredBackBufferHeight = screenSize;
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

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            for(int i = 0; i < x.Length; i++)
            {
                x[i] += rand.Next(-5, 6);
                y[i] += rand.Next(-4, 5);
                if (x[i] < 0 || x[i] >= screenSize || y[i] < 0 || y[i] >= screenSize)
                {
                    x[i] = screenSize / 2;
                    y[i] = screenSize / 2;
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            for (int i = 0; i < x.Length; i++)
            {
                spriteBatch.Draw(whiteRectangle, new Rectangle(x[i], y[i], 2, 2), Color.Black);
            }

            spriteBatch.End();
        }
    }
}