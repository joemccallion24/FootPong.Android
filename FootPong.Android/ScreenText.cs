
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FootPong.Android
{
    public class ScreenText
    {
        private SpriteFont _font;

        public ScreenText(SpriteFont font)
        {
            _font = font;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, "FIRST TO FIVE", new Vector2(Game1.screenWidth / 2 - 200, 20), Color.Black);
        }
    }
}