
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FootPong.Android
{
    public class Score
    {
        public int playerScore;
        public int AIscore; //Maybe Player 2 score in future?

        private SpriteFont _font;

        public Score(SpriteFont font)
        {
            _font = font;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, playerScore.ToString(), new Vector2(Game1.screenWidth / 2 - 200, 90), Color.White);
            spriteBatch.DrawString(_font, AIscore.ToString(), new Vector2(Game1.screenWidth / 2 + 200, 90), Color.White);
        }
    }
}