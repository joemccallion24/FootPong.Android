using FootPong.Android.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;

namespace FootPong.Android.Sprites
{
    public class Player : Sprite
    {
        TouchCollection touchCollection;
        public Player(Texture2D texture) : base(texture)
        {
            speed = 9f;
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            //if (input == null) throw new Exception("give input");
            touchCollection = TouchPanel.GetState();
            if (touchCollection.Count > 0)
            {
                int touchPosition = ((int)touchCollection[0].Position.Y);
                int touchX = ((int)touchCollection[0].Position.X);
                if (touchX < Game1.screenWidth / 2)
                {
                    if ((position.Y + _texture.Height / 2) < touchPosition)
                    {
                        velocity.Y = speed;
                    }
                    else if (position.Y > (touchPosition - _texture.Height / 2.7))
                    {
                        velocity.Y = -speed;
                    }
                    else if ((position.Y + _texture.Height / 2) == touchPosition)
                    {
                        velocity.Y = 0;
                    }
                }
            }

            position += velocity;
            position.Y = MathHelper.Clamp(position.Y, 0, GameState.screenHeight - _texture.Height);

            velocity = Vector2.Zero;
        }
    }
}