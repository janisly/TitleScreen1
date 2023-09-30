using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TitleScreen1;
using SharpDX.MediaFoundation;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using TitleScreen1.Screens;
using TitleScreen1.StateManagement;

namespace TitleScreen1
{

    /// <summary>
    /// A game demonstrating a title screen
    /// </summary>
    public class TitlePage : Game
    {
        private GraphicsDeviceManager _graphics;
        private readonly ScreenManager _screenManager;
        private SpriteBatch spriteBatch;

        /// <summary>
        /// Constructs the game
        /// </summary>
        public TitlePage()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            var screenFactory = new ScreenFactory();
            Services.AddService(typeof(IScreenFactory), screenFactory);

            _screenManager = new ScreenManager(this);
            Components.Add(_screenManager);

            AddInitialScreens();
        }

        private void AddInitialScreens()
        {
            _screenManager.AddScreen(new BackgroundScreen(), null);
            _screenManager.AddScreen(new MainMenuScreen(), null);
        }

        /// <summary>
        /// Initializes the game
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Constants.GAME_WIDTH = GraphicsDevice.Viewport.Width;
            Constants.GAME_HEIGHT = GraphicsDevice.Viewport.Height;
            

            base.Initialize();
        }

        /// <summary>
        /// Loads game content
        /// </summary>
        protected override void LoadContent()
        {
        }

        /// <summary>
        /// Updates the game world
        /// </summary>
        /// <param name="gameTime">The measured game time</param>
        protected override void Update(GameTime gameTime)
        {
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
            base.Draw(gameTime);
        }
    }
}