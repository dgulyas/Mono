using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoTest.Common;
using System.Collections.Generic;
using System.Linq;

namespace MonoTest.Experiments
{

    public class Base : Game
    {
        readonly GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        ContentManager content;
        int screenSize = 1200;

        List<Vector2> circles;
        MouseState previousMouseState;
        Vector2 Center;
        int score = 0;

        public Base()
        {
            graphics = new GraphicsDeviceManager(this);
            circles = new List<Vector2>();
            content = new ContentManager(Services);

            Center = new Vector2(screenSize / 2, screenSize / 2);
        }

        protected override void Initialize()
        {
            SetupGraphics();

            previousMouseState = Mouse.GetState();
            this.IsMouseVisible = true;
            IsFixedTimeStep = false;



            base.Initialize();
        }

        private void SetupGraphics()
        {
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = screenSize;
            graphics.PreferredBackBufferHeight = screenSize;
            graphics.ApplyChanges();

            GraphicsDevice.Clear(Color.Black);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            spriteBatch = new SpriteBatch(GraphicsDevice);

            var CourierNew = content.Load<SpriteFont>("Swansea");
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
            spriteBatch.Dispose();
        }

        protected override void Update(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();


            if (previousMouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed)
            {
                circles.Add(new Vector2(mouseState.X, mouseState.Y));
            }

            score += circles.Count(c => Vector2.Distance(c, Center) < 100);
            circles.RemoveAll(c => Vector2.Distance(c, Center) < 100);
            for(int i = 0; i < circles.Count; i++)
            {
                var circle = circles[i];

                var dir = Center - circle;
                dir.Normalize();

                circles[i] += dir * 5;
            }


            previousMouseState = mouseState;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            //spriteBatch.DrawString();

            foreach (var p in circles)
            {
                spriteBatch.DrawCircle(p, 20, 20, Color.Orange);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
