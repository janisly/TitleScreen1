using System;
using System.Threading;
using Microsoft.Win32;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Flycatcher.Particles;
using Flycatcher.StateManagement;

namespace Flycatcher.Screens
{
    // This screen implements the actual game logic. It is just a
    // placeholder to get the idea across: you'll probably want to
    // put some more interesting gameplay in here!
    public class GameplayScreen : GameScreen
    {
        private ContentManager _content;
        private SpriteFont _gameFont;

        private Vector2 _playerPosition = new Vector2(100, 100);
        private Vector2 _enemyPosition = new Vector2(100, 100);

        private readonly Random _random = new Random();

        private float _pauseAlpha;
        private readonly InputAction _pauseAction;
        public bool _shaking { get; set; } = false;
        public bool _celebrating { get; set; } = false;

        private Texture2D flyTexture;
        private FlySprite[] flies;
        private FlycatcherSprite flycatcher;
        private SpriteFont bangers;
        private int stage = 0;
        private int level = 0;
        private int caught = 0;
        private Song backgroundMusic;
        private SoundEffect victoryTrill;
        private SoundEffect DeathBit;
        private float _shakeTime = 0;
        private float _celebrationTime = 0;
        private float _recordTime = 0;
        private float _oldRecord = 0;
        private float _speedMod = 0;
        private float _highSpeed;
        private PixieParticleSystem pixie;
        private Color[] levelColor = { Color.CornflowerBlue, Color.Crimson, Color.Cyan, Color.SeaGreen, Color.Gainsboro,
        Color.Yellow, Color.Orange, Color.DarkBlue, Color.Fuchsia, Color.Gold};
        // The cube to draw 
        Cube cube;

        private Texture2D ball;



        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            _pauseAction = new InputAction(
                new[] { Buttons.Start, Buttons.Back },
                new[] { Keys.Back, Keys.Escape }, true);
        }

        public void LoadLevel(int level)
        {
            
            if (level == 0)
            {
                flycatcher = new FlycatcherSprite();
                flies = new FlySprite[]
                {
                new FlySprite( new Vector2(Constants.GAME_WIDTH / 16, Constants.GAME_HEIGHT / 16)) { Direction = Direction.Right },
                new FlySprite( new Vector2((Constants.GAME_WIDTH / 16)*15, (Constants.GAME_HEIGHT / 16))) { Direction = Direction.Down },
                new FlySprite( new Vector2((Constants.GAME_WIDTH / 16)*15, (Constants.GAME_HEIGHT / 16)*14)) { Direction = Direction.Left },
                new FlySprite( new Vector2((Constants.GAME_WIDTH / 16), (Constants.GAME_HEIGHT / 16)*14)) { Direction = Direction.Up },
                };
            }
            if (level >= 1)
            {
                flycatcher.ResetPosition(new Vector2(Constants.GAME_WIDTH / 2, Constants.GAME_HEIGHT / 2));
                flycatcher.SpeedMod = (float)(_speedMod * 0.4);
                flies = new FlySprite[]
                {
                new FlySprite( new Vector2(Constants.GAME_WIDTH / 8, Constants.GAME_HEIGHT / 8)) { Direction = Direction.Right },
                new FlySprite( new Vector2((Constants.GAME_WIDTH / 8)*7, (Constants.GAME_HEIGHT / 8))) { Direction = Direction.Down },
                new FlySprite( new Vector2((Constants.GAME_WIDTH / 8)*7, (Constants.GAME_HEIGHT / 8)*7)) { Direction = Direction.Left },
                new FlySprite( new Vector2((Constants.GAME_WIDTH / 8), (Constants.GAME_HEIGHT / 8)*7)) { Direction = Direction.Up },
                };
            }
            foreach (var fly in flies) fly.LoadContent(_content);
            flycatcher.LoadContent(_content);
        }

        // Load graphics content for the game
        public override void Activate()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            LoadLevel(0);
            bangers = _content.Load<SpriteFont>("bangers");
            ball = _content.Load<Texture2D>("ball");
            victoryTrill = _content.Load<SoundEffect>("victoryTrill");
            DeathBit = _content.Load<SoundEffect>("DeathBit");
            backgroundMusic = _content.Load<Song>("John Bartmann - ominous-night-master");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(backgroundMusic);

            // Create the cube
            cube = new Cube(Constants.PARENT);

            pixie = new PixieParticleSystem(Constants.PARENT, flycatcher);
            Constants.PARENT.Components.Add(pixie);

            // A real game would probably have more content than this sample, so
            // it would take longer to load. We simulate that by delaying for a
            // while, giving you a chance to admire the beautiful loading screen.
            Thread.Sleep(1000);

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }


        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override void Unload()
        {
            _content.Unload();
        }

        // This method checks the GameScreen.IsActive property, so the game will
        // stop updating when the pause menu is active, or if you tab away to a different application.
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            //Time at which winning occurs
            float timeWon = 0;
            
            //Exits game (Back or ESC)
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))    ExitScreen();

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                _pauseAlpha = Math.Min(_pauseAlpha + 1f / 32, 1);
            else
                _pauseAlpha = Math.Max(_pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                // update the cube 
                cube.Update(gameTime);

                //Updates the ongoing timer
                if (stage < 2 && stage > 0) { _recordTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds; }


                //Advances Game Stage (A or Space)
                if ((GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Space) ||
                    Keyboard.GetState().IsKeyDown(Keys.A)) && stage == 0)
                {
                    stage = 1;
                    if (level >= 1)
                    {
                        LoadLevel(level);
                    }
                }
                // TODO: Add your update logic here
                foreach (var fly in flies) fly.Update(gameTime);
                if (stage == 1) flycatcher.Update(gameTime);

                //Detect and process collisions
                flycatcher.Color = Color.White;
                foreach (var fly in flies)
                {
                    if (!fly.Caught && fly.Bounds.CollidesWith(flycatcher.Bounds))
                    {
                        DeathBit.Play();
                        flycatcher.Color = Color.LimeGreen;
                        fly.Caught = true;
                        caught++;
                        _shaking = true;
                    }
                }
                if (caught == 4)
                {
                    if (stage == 1)
                    {
                        stage = 2;
                        //Takes the difference in milliseconds between rounds, converts to seconds
                        float recordDifference = (_recordTime - _oldRecord) / 1000;
                        //Calculates Time under ten seconds achieved
                        float timeSaved = 10 - recordDifference;
                        //Uses a square root to return a speed boost between 0 and 10 with diminishing returns
                        if (recordDifference < 10)
                        {
                            _speedMod = (float)Math.Pow((timeSaved/10), 0.7)*10;
                        }
                        else { _speedMod = 0; }
                        _highSpeed = Math.Max(_highSpeed, _speedMod);
                        _oldRecord = _recordTime;
                    }
                    if (_celebrating)
                    {
                        _celebrationTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                        if (_celebrationTime >= 2000)
                        {
                            _celebrationTime = 0;
                            stage = 0;
                            level += 1;
                            caught = 0;
                            _celebrating = false;
                        }
                    }
                    
                }

            }
        }

        // Unlike the Update method, this will only be called when the gameplay screen is active.
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            var keyboardState = input.CurrentKeyboardStates[playerIndex];
            var gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected && input.GamePadWasConnected[playerIndex];

            PlayerIndex player;
            if (_pauseAction.Occurred(input, ControllingPlayer, out player) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else
            {
                // Otherwise move the player position.
                var movement = Vector2.Zero;

                if (keyboardState.IsKeyDown(Keys.Left))
                    movement.X--;

                if (keyboardState.IsKeyDown(Keys.Right))
                    movement.X++;

                if (keyboardState.IsKeyDown(Keys.Up))
                    movement.Y--;

                if (keyboardState.IsKeyDown(Keys.Down))
                    movement.Y++;

                var thumbstick = gamePadState.ThumbSticks.Left;

                movement.X += thumbstick.X;
                movement.Y -= thumbstick.Y;

                if (movement.Length() > 1)
                    movement.Normalize();

                _playerPosition += movement * 8f;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background initially. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, levelColor[level % 10], 0, 0);

            var spriteBatch = ScreenManager.SpriteBatch;
            float flyScareRotation = 0;
            Color flyCatcherExultation = Color.White;
            Color bloodColor = Color.Black;

            Matrix shakeTransform = Matrix.Identity;
            if (_shaking)
            {
                _shakeTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                shakeTransform = Matrix.CreateTranslation(6 * MathF.Sin(_shakeTime), 6 * MathF.Cos(_shakeTime), 0);
                flyScareRotation = MathF.Sin(_shakeTime);
                bloodColor = Color.OrangeRed;
                if (_shakeTime > 400) _shaking = false;
            }
            else 
            { 
                _shakeTime = 0;
                flyScareRotation = 0;
                bloodColor = Color.Black;
            }

            spriteBatch.Begin(transformMatrix: shakeTransform);

            foreach (var fly in flies)
            {
                fly.Draw(gameTime, spriteBatch, flyScareRotation);
                /* //Fly Hitbox Helper
                var rect = new Rectangle((int)(fly.Bounds.Center.X - fly.Bounds.Radius),
                    (int)(fly.Bounds.Center.Y - fly.Bounds.Radius),
                    (int)(2 * fly.Bounds.Radius), (int)(2 * fly.Bounds.Radius));
                spriteBatch.Draw(ball, rect, Color.White);
                */
            }

            /* //Flycatcher Hitbox Helper
            var rectG = new Rectangle((int)(flycatcher.Bounds.X),
                    (int)(flycatcher.Bounds.Y),
                    (int)(flycatcher.Bounds.Width), (int)(flycatcher.Bounds.Height));
            spriteBatch.Draw(ball, rectG, Color.White);
            */

            spriteBatch.DrawString(bangers, "Press the escape key or the back button to exit", new Vector2(2, 2),
                Color.Black, 0, new Vector2(0, 0), 0.7f, SpriteEffects.None, 0);
            spriteBatch.DrawString(bangers, "(WASD to move)", new Vector2(2, 20),
                Color.Black, 0, new Vector2(0, 0), 0.5f, SpriteEffects.None, 0);
            spriteBatch.DrawString(bangers, "Record Time: " + Math.Round((_recordTime/1000), 3), new Vector2(2, 35),
                levelColor[(level + 1) % 10], 0, new Vector2(0, 0), 0.85f, SpriteEffects.None, 0);
            spriteBatch.DrawString(bangers, "Level: " + (level + 1), new Vector2(2, 56),
                levelColor[(level + 1) % 10], 0, new Vector2(0, 0), 0.6f, SpriteEffects.None, 0);
            spriteBatch.DrawString(bangers, "Speed: " + Math.Round((1 + _speedMod), 1), new Vector2(2, 72),
                levelColor[(level + 1) % 10], 0, new Vector2(0, 0), 0.6f, SpriteEffects.None, 0);
            spriteBatch.DrawString(bangers, "Highest Speed: " + Math.Round((1 + _highSpeed), 1), new Vector2(2, 98),
                levelColor[(level + 2) % 10], 0, new Vector2(0, 0), 0.6f, SpriteEffects.None, 0);
            if (stage == 0)
            {
                if (level == 0)
                {
                    spriteBatch.DrawString(bangers, "FLYCATCHER", new Vector2((Constants.GAME_WIDTH / 16) * 6, (Constants.GAME_HEIGHT / 16) * 7),
                        Color.Gold, 0, new Vector2(0, 0), 1.2f, SpriteEffects.None, 0);
                    spriteBatch.DrawString(bangers, "Press A or Space to start", new Vector2((Constants.GAME_WIDTH / 32) * 11, Constants.GAME_HEIGHT / 2),
                        Color.Red, 0, new Vector2(0, 0), 0.8f, SpriteEffects.None, 0);
                }
                else
                {
                    spriteBatch.DrawString(bangers, "Press A or Space to start", new Vector2((Constants.GAME_WIDTH / 16) * 5, (Constants.GAME_HEIGHT / 16) * 7),
                        levelColor[(level + 1) % 10], 0, new Vector2(0, 0), 0.9f, SpriteEffects.None, 0);
                    spriteBatch.DrawString(bangers, "Speed Boost For Next Level: " + Math.Round(_speedMod, 1), new Vector2((Constants.GAME_WIDTH / 16) * 6, (Constants.GAME_HEIGHT / 16) * 8),
                        levelColor[(level + 1) % 10], 0, new Vector2(0, 0), 0.7f, SpriteEffects.None, 0);
                }
            }
            if (stage == 1)
            {
                flycatcher.Draw(gameTime, spriteBatch, flyCatcherExultation);
                pixie.Draw(gameTime, spriteBatch, bloodColor);
            }
            if (stage >= 2)
            {
                if (stage == 2)
                {
                    victoryTrill.Play();
                    _celebrating = true;
                }
                if(level != 9)
                {
                    spriteBatch.DrawString(bangers, "PREY SLAUGHTERED", new Vector2((Constants.GAME_WIDTH / 16) * 5, (Constants.GAME_HEIGHT / 16) * 7),
                        Color.Gold, 0, new Vector2(0, 0), 1.7f, SpriteEffects.None, 0);
                }
                else
                {
                    spriteBatch.DrawString(bangers, "PREY SLAUGHTERED", new Vector2((Constants.GAME_WIDTH / 16) * 5, (Constants.GAME_HEIGHT / 16) * 7),
                        Color.LimeGreen, 0, new Vector2(0, 0), 1.7f, SpriteEffects.None, 0);
                }
                stage = 3;
            }

            if(level == 9 || (level) % 10 == 9)
            {
                // draw the cube
                cube.Draw();
            }

            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || _pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, _pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }
    }
}
