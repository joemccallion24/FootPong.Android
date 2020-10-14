using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;

namespace FootPong.Android.Controls
{
    public class Button : Component
    {
        private TouchLocationState _currentTouch;
        private SpriteFont _font;
        private bool _isHovering;
        private TouchLocationState _previousTouch;
        private Texture2D _texture;
        private TouchCollection touchCollection;

        public event EventHandler Click;
        public bool Clicked { get; private set; }
        public Color PenColour { get; set; }
        public Vector2 Position { get; set; }

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
            }
        }

        public string Text { get; set; }

        public Button(Texture2D texture, SpriteFont font)
        {
            _texture = texture;
            _font = font;
            PenColour = Color.Black;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var colour = Color.White;
            if (_isHovering)
                colour = Color.Gray;
            spriteBatch.Draw(_texture, Rectangle, colour);

            if (!string.IsNullOrEmpty(Text))
            {
                var x = (Rectangle.X + (Rectangle.Width / 2)) - (_font.MeasureString(Text).X / 2);
                var y = (Rectangle.Y + (Rectangle.Height / 2)) - (_font.MeasureString(Text).Y / 2);

                spriteBatch.DrawString(_font, Text, new Vector2(x, y), PenColour);
            }
        }

        public override void Update(GameTime gameTime)
        {
            touchCollection = TouchPanel.GetState();

            foreach (TouchLocation tl in touchCollection)
            {
                _previousTouch = _currentTouch;

                touchCollection = TouchPanel.GetState();
                if (touchCollection.Count > 0)
                {
                    int touchPosition = ((int)touchCollection[0].Position.Y);
                    int touchX = ((int)touchCollection[0].Position.X);

                    var mouseRectangle = new Rectangle(touchX, touchPosition, 1, 1);
                    // var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

                    _isHovering = false;

                    if (mouseRectangle.Intersects(Rectangle))
                    {
                        _isHovering = true;
                        _currentTouch = tl.State;

                        //if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
                        if (_currentTouch == TouchLocationState.Released)
                        {
                            Click?.Invoke(this, new EventArgs());

                        }
                    }
                }
            }
        }
    }
}