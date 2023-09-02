using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        private SpriteFont bangers;

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

            flies = new FlySprite[]
            {
                new FlySprite() { Position = new Vector2(240, 20), Direction = Direction.Right },
                new FlySprite() { Position = new Vector2(240, 400), Direction = Direction.Up },
                new FlySprite() { Position = new Vector2(500, 20), Direction = Direction.Down },
                new FlySprite() { Position = new Vector2(500, 400), Direction = Direction.Left },
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
            bangers = Content.Load<SpriteFont>("bangers");
        }

        /// <summary>
        /// Updates the game world
        /// </summary>
        /// <param name="gameTime">The measured game time</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            foreach (var fly in flies) fly.Update(gameTime);

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
            foreach (var fly in flies) fly.Draw(gameTime, spriteBatch);
            spriteBatch.DrawString(bangers, "Press the escape key or the back button to exit", new Vector2(2, 2), Color.Black);
            spriteBatch.DrawString(bangers, "Insect Observation Idle 2023", new Vector2(250, 200), Color.Gold);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}