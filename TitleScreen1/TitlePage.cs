using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TitleScreen1;
using SharpDX.MediaFoundation;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace TitleScreen1
{

    /// <summary>
    /// A game demonstrating a title screen
    /// </summary>
    public class TitlePage : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch spriteBatch;

        private Texture2D flyTexture;
        private FlySprite[] flies;
        private FlycatcherSprite flycatcher;
        private SpriteFont bangers;
        private int stage = 0;
        private int caught = 0;
        private Song backgroundMusic;
        private SoundEffect victoryTrill;
        private SoundEffect DeathBit;

        private Texture2D ball;

        /// <summary>
        /// Constructs the game
        /// </summary>
        public TitlePage()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// Initializes the game
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Constants.GAME_WIDTH = GraphicsDevice.Viewport.Width;
            Constants.GAME_HEIGHT = GraphicsDevice.Viewport.Height;
            flycatcher = new FlycatcherSprite();
            flies = new FlySprite[]
            {
                new FlySprite( new Vector2(Constants.GAME_WIDTH / 16, Constants.GAME_HEIGHT / 16)) { Direction = Direction.Right },
                new FlySprite( new Vector2((Constants.GAME_WIDTH / 16)*15, (Constants.GAME_HEIGHT / 16))) { Direction = Direction.Down },
                new FlySprite( new Vector2((Constants.GAME_WIDTH / 16)*15, (Constants.GAME_HEIGHT / 16)*14)) { Direction = Direction.Left },
                new FlySprite( new Vector2((Constants.GAME_WIDTH / 16), (Constants.GAME_HEIGHT / 16)*14)) { Direction = Direction.Up },
            };

            base.Initialize();
        }

        /// <summary>
        /// Loads game content
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            foreach (var fly in flies) fly.LoadContent(Content);
            flycatcher.LoadContent(Content);
            bangers = Content.Load<SpriteFont>("bangers");
            ball = Content.Load<Texture2D>("ball");
            victoryTrill = Content.Load<SoundEffect>("victoryTrill");
            DeathBit = Content.Load<SoundEffect>("DeathBit");
            backgroundMusic = Content.Load<Song>("John Bartmann - ominous-night-master");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(backgroundMusic);
        }

        /// <summary>
        /// Updates the game world
        /// </summary>
        /// <param name="gameTime">The measured game time</param>
        protected override void Update(GameTime gameTime)
        {
            //Exits game (Back or ESC)
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //Advances Game Stage (A or Space)
            if ((GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Space) ||
                Keyboard.GetState().IsKeyDown(Keys.A)) && stage == 0)
                stage = 1;

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
                }
            }
            if (caught == 4)
            {
                stage = 2;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the game world
        /// </summary>
        /// <param name="gameTime">The measured game time</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LimeGreen);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            foreach (var fly in flies)
            {
                fly.Draw(gameTime, spriteBatch);
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
                Color.Black, 0, new Vector2(0,0), 0.7f, SpriteEffects.None, 0);
            spriteBatch.DrawString(bangers, "(WASD to move)", new Vector2(2, 20),
                Color.Black, 0, new Vector2(0, 0), 0.5f, SpriteEffects.None, 0);
            if (stage == 0) { 
                spriteBatch.DrawString(bangers, "FLYCATCHER", new Vector2((Constants.GAME_WIDTH / 16)*6, (Constants.GAME_HEIGHT / 16)*7), 
                    Color.Gold, 0, new Vector2(0, 0), 1.2f, SpriteEffects.None, 0); 
                spriteBatch.DrawString(bangers, "Press A or Space to start", new Vector2((Constants.GAME_WIDTH / 32)*12, Constants.GAME_HEIGHT / 2),
                    Color.Red, 0, new Vector2(0, 0), 0.8f, SpriteEffects.None, 0);
            }
            if(stage == 1)
            {
                flycatcher.Draw(gameTime, spriteBatch);
            }
            if (stage >= 2)
            {
                if(caught == 4)
                {
                    victoryTrill.Play();
                    caught++;
                }
                spriteBatch.DrawString(bangers, "PREY SLAUGHTERED", new Vector2((Constants.GAME_WIDTH / 16) * 5, (Constants.GAME_HEIGHT / 16)*7),
                    Color.Gold, 0, new Vector2(0, 0), 1.7f, SpriteEffects.None, 0);
                stage++;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}