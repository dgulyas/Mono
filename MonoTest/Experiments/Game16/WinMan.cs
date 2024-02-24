using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace HoleGame.WindowMan
{
    //Window Manager class
    //Right now it just is a bunch of squares you can drag around,
    //reordering them so that the one that's clicked on is on top.
    public class WinMan : Game
    {
        private SpriteBatch _spriteBatch;
        private readonly GraphicsDeviceManager _graphics;
        private Texture2D _squareTxt;
        private Window _draggedWindow;
        private readonly List<Window> _windows = new List<Window>();

        private readonly int screenSize = 1500;
        private Color[] _colors = new Color[] { Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Blue, Color.Indigo, Color.Purple };

        public WinMan()
        {
            _graphics = new GraphicsDeviceManager(this);
        }

        protected override void Initialize()
        {
            SetupGraphics();
            for (int i = 0; i < 21; i++)
            {
                _windows.Add(new Window { Rectangle = new Rectangle(50 + i * 50, 50 + i * 50, 400, 400), Color = _colors[i % _colors.Length] });
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

            _squareTxt = new Texture2D(GraphicsDevice, 1, 1);
            _squareTxt.SetData(0, new Rectangle(0, 0, 1, 1), new[] { Color.White }, 0, 1);
        }

        protected override void Update(GameTime gameTime)
        {
            var ms = Mouse.GetState();

            if(ms.LeftButton == ButtonState.Released)
            {
                _draggedWindow = null;
            }
            else if(_draggedWindow == null) //mouse button is down, just started dragging
            {
                var clickedIndex = -1;
                for (int i = _windows.Count - 1; i >= 0; i--)
                {
                    if (_windows[i].Rectangle.Contains(ms.Position))
                    {
                        clickedIndex = i;
                        break;
                    }
                }

                if(clickedIndex >= 0) //clicked on a window
                {
                    _draggedWindow = _windows[clickedIndex];
                    _draggedWindow.MouseOffset = ms.Position - _draggedWindow.Rectangle.Location;


                    if (clickedIndex < _windows.Count - 1) //window isn't on top
                    {
                        _windows.RemoveAt(clickedIndex);
                        _windows.Add(_draggedWindow);
                    }
                }

            }
            else //in the middle of dragging
            {
                _draggedWindow.Rectangle.Location = ms.Position - _draggedWindow.MouseOffset;
            }

            base.Update(gameTime);
        }

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