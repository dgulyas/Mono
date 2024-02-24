using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace HoleGame.WindowMan
{
    //Window Manager class
    public class WinMan : Game
    {
        private SpriteBatch _spriteBatch;
        private readonly GraphicsDeviceManager _graphics;
        private Texture2D _squareTxt;

        private readonly int screenSize = 1500;

        private readonly List<Window> _windows = new List<Window>();

        private MouseState _previousMouseState;
        private Window _draggedWindow;

        Color _color = Color.Orange;
        private Color[] Colors = new Color[] { Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Blue, Color.Indigo, Color.Purple };

        public WinMan()
        {
            _graphics = new GraphicsDeviceManager(this);
        }

        protected override void Initialize()
        {
            SetupGraphics();
            var rand = new Random();
            for (int i = 0; i < 4; i++)
            {
                var rPoint = new Vector2(rand.Next(screenSize / 2), rand.Next(screenSize / 2));
                _windows.Add(new Window { Rectangle = new Rectangle(50 + i * 50, 50 + i * 50, 400, 400), Color = Colors[i % Colors.Length] });
            }

            base.Initialize();
        }

        private void SetupGraphics()
        {
            IsMouseVisible = true;

            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = screenSize;
            _graphics.PreferredBackBufferHeight = screenSize;
            _graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Content.RootDirectory = "Content";
            //_assets.LoadAssets(Content);
            _squareTxt = new Texture2D(GraphicsDevice, 1, 1);
            _squareTxt.SetData(0, new Rectangle(0, 0, 1, 1), new[] { Color.White }, 0, 1);
        }

        protected override void Update(GameTime gameTime)
        {
            var cl = GetClickLocation();
            if (cl != null)
            {
                var clickedIndex = -1;
                for (int i = _windows.Count - 1; i >= 0; i--)
                {
                    if (_windows[i].Rectangle.Contains(cl.Value))
                    {
                        clickedIndex = i;
                        break;
                    }
                }

                if (clickedIndex >= 0)
                {
                    _draggedWindow = _windows[clickedIndex];
                    _draggedWindow.DragStart = _draggedWindow.Rectangle.Location;


                    if (clickedIndex < _windows.Count - 1)
                    {
                        _windows.RemoveAt(clickedIndex);
                        _windows.Add(_draggedWindow);
                    }
                }
            }




            base.Update(gameTime);
        }

        private Point? GetClickLocation()
        {
            Point? clickLocation = null;

            var mouseState = Mouse.GetState();
            if (_previousMouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed)
            {
                clickLocation = new Point(mouseState.X, mouseState.Y);
                //mouseState.
            }
            _previousMouseState = mouseState;
            return clickLocation;
        }

        //This isn't working so well. I want to create a state machine that both tracks dragging
        //and clicking.


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            for (int i = 0; i < _windows.Count; i++)
            {
                var w = _windows[i];
                _spriteBatch.Draw(_squareTxt, w.Rectangle.Location.ToVector2(), w.Rectangle, w.Color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}