using FootPong.Android.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Button = FootPong.Android.Controls.Button;

namespace FootPong.Android.State
{
    public class GameState : State
    {
        private SpriteBatch _spriteBatch;

        public static int screenWidth = Game1.screenWidth;
        public static int screenHeight = Game1.screenHeight;
        public static Random random = Game1.random;
        private Ball ball;
        private AIPlayer AIplayer;
        private Player Player1;
        private List<Sprite> sprites;
        private List<Component> _components, _componentsPause;
        private Score score;
        private ScreenText screenFont;
        private int difficultyCase;
        private float currentYPosition, timer = 0;
        private bool Pause, Multiplayer;


        public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
        : base(game, graphicsDevice, content)
        {
            difficultyCase = 0;
            _spriteBatch = new SpriteBatch(_graphicsDevice);
            //use this.Content to load your game content here
            var ballTexture = _content.Load<Texture2D>("Ball");
            var playerTexture = _content.Load<Texture2D>("Player1");
            var player2Texture = _content.Load<Texture2D>("player2");
            var buttonTexture = _content.Load<Texture2D>("home");
            var buttonFont = _content.Load<SpriteFont>("ButtonFont");
            var DifficultybuttonTexture = _content.Load<Texture2D>("difficultyButton");
            var DifficultybuttonFont = _content.Load<SpriteFont>("ButtonDifficultyFont");
            var pausebuttonTexture = _content.Load<Texture2D>("Pause");

            score = new Score(_content.Load<SpriteFont>("File"));
            screenFont = new ScreenText(_content.Load<SpriteFont>("ScreenFont"));

            var HomeButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(Game1.screenWidth / 20, screenHeight - screenHeight / 10),
                Text = "",
            };
            HomeButton.Click += HomeButton_Click;

            var PauseButton = new Button(pausebuttonTexture, buttonFont)
            {
                Position = new Vector2(screenWidth / 2, screenHeight - screenHeight / 20),
                Text = "",
            };
            PauseButton.Click += PauseButton_Click;

            var DifficultyButton = new Button(DifficultybuttonTexture, DifficultybuttonFont)
            {
                Position = new Vector2(Game1.screenWidth / 8, screenHeight - screenHeight / 10),
                Text = "Play Style",
            };
            DifficultyButton.Click += DifficultyButton_Click;

            var MultiplayerButton = new Button(DifficultybuttonTexture, DifficultybuttonFont)
            {
                Position = new Vector2((Game1.screenWidth / 4) * 3, screenHeight - screenHeight / 10),
                Text = "Player 2",
            };
            MultiplayerButton.Click += MultiplayerButton_Click;

            _componentsPause = new List<Component>()
        {
            HomeButton,
            DifficultyButton,
            PauseButton,
            MultiplayerButton,
        };
            _components = new List<Component>()
        {
            PauseButton,
        };

            ball = new Ball(ballTexture)
            {
                position = new Vector2((screenWidth / 2) - (ballTexture.Width / 2), (screenHeight / 2) - (ballTexture.Height / 2)), //positions the ball in the centre of the screen
                score = score,
            };

            AIplayer = new AIPlayer(player2Texture)
            {
                position = new Vector2(Game1.screenWidth - (Game1.screenWidth / 10), (screenHeight / 2) - (playerTexture.Height / 2)),
            };

            Player1 = new Player(playerTexture)
            {
                position = new Vector2(Game1.screenWidth / 15, (screenHeight / 2) - (playerTexture.Height / 2)),
            };

            //load in the sprites
            sprites = new List<Sprite>()
        {
            ball,
            Player1,
            AIplayer,
        };

            Pause = false;
            Multiplayer = false;
        }

        public override void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (!Pause && timer > Constant.START_GAME_DELAY)
            {
                foreach (var sprite in sprites)
                {
                    sprite.Update(gameTime, sprites);
                }

                foreach (var component in _components)
                    component.Update(gameTime);

                if (!Multiplayer)
                {
                    AIMove();
                }
                else if (Multiplayer)
                {
                    AIplayer.Player2Move();
                }

                if (score.playerScore == Constant.WIN_SCORE)
                {
                    _game.changeState(new Endgame(_game, _graphicsDevice, _content));
                }
                else if (score.AIscore == Constant.WIN_SCORE)
                {
                    _game.changeState(new EndGameLost(_game, _graphicsDevice, _content));
                }

                ball.isPlaying = true;
            }
            else
            {
                foreach (var component in _componentsPause)
                    component.Update(gameTime);
                ball.isPlaying = false;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _graphicsDevice.Clear(Color.Green);

            _spriteBatch.Begin();

            _spriteBatch.Draw(_content.Load<Texture2D>("background"),
                new Rectangle(0, 0, Game1.screenWidth, Game1.screenHeight),
                new Rectangle(0, 0, 2020, 980),
                Color.White);

            foreach (var sprite in sprites) sprite.Draw(_spriteBatch);
            if (!Pause)
            {
                foreach (var component in _components)
                    component.Draw(gameTime, _spriteBatch);
            }
            else if (Pause)
            {
                foreach (var component in _componentsPause)
                    component.Draw(gameTime, _spriteBatch);
                screenFont.Draw(_spriteBatch);
            }

            score.Draw(_spriteBatch);
            if (ball.speed < 7 && score.playerScore == 0 && score.AIscore == 0)
            {
                screenFont.Draw(_spriteBatch); //drawn for the first 5 seconds 
            }

            _spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public void AIMove()
        {
            if (ball.position.Y > AIplayer.position.Y && ball.position.X > screenWidth / 2)
            {
                AIplayer.velocity.Y = AIplayer.speed;
            }
            else if (ball.position.Y < AIplayer.position.Y && ball.position.X > screenWidth / 2)
            {
                AIplayer.velocity.Y = -AIplayer.speed;
            }
            else if (ball.position.Y == AIplayer.position.Y && ball.position.X > screenWidth / 2)
            {
                AIplayer.velocity.Y = 0;
            }
        }

        private void HomeButton_Click(object sender, EventArgs e)
        {
            _game.changeState(new MenuState(_game, _graphicsDevice, _content));
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            Pause = !Pause;
        }

        private void MultiplayerButton_Click(object sender, EventArgs e)
        {
            Multiplayer = !Multiplayer;
        }

        private void DifficultyButton_Click(object sender, EventArgs e)
        {

            currentYPosition = Player1.position.Y;
            if (difficultyCase >= 2)
            {
                difficultyCase = 0;
                setDifficulty();
            }
            else
            {
                difficultyCase++;
                setDifficulty();
            }
        }

        public void setDifficulty()
        {
            switch (difficultyCase)
            {
                case 0:
                    Player1.position.X = Game1.screenWidth / 15;
                    Player1.position.Y = currentYPosition;
                    break;
                case 1:
                    Player1.position.X = Game1.screenWidth / 100 * 25;
                    Player1.position.Y = currentYPosition;
                    break;
                case 2:
                    Player1.position.X = Game1.screenWidth / 100 * 45;
                    Player1.position.Y = currentYPosition;
                    break;
            }
        }
    }
}