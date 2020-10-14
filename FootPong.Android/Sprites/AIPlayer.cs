using FootPong.Android.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;

namespace FootPong.Android.Sprites
{
    public class AIPlayer : Player
    {
        TouchCollection touchCollection;
        public AIPlayer(Texture2D texture) : base(texture)
        {
            speed = 9f;
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            position += velocity;
            position.Y = MathHelper.Clamp(position.Y, 0, GameState.screenHeight - _texture.Height - 100);

            velocity = Vector2.Zero;
        }

        public void Player2Move()
        {
            touchCollection = TouchPanel.GetState();
            if (touchCollection.Count > 0)
            {
                int touchPosition = ((int)touchCollection[0].Position.Y);
                int touchX = ((int)touchCollection[0].Position.X);
                if (touchX > Game1.screenWidth / 2)
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
        }
    }
}