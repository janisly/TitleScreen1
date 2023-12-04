using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TitleScreen1.Collisions;
using TitleScreen1.Particles;

namespace TitleScreen1
{
    /// <summary>
    /// A class representing a flycatcher creature
    /// </summary>
    public class FlycatcherSprite: IParticleEmitter
    {
        private GamePadState gamePadState;

        private KeyboardState keyboardState;

        private Texture2D texture;

        private bool flipped;

        private bool moving;

        private int speed = 2;

        private Vector2 position = new Vector2(Constants.GAME_WIDTH / 2, Constants.GAME_HEIGHT / 2);

        private float rotation = 0;

        private BoundingRectangle bounds = 
            new BoundingRectangle(new Vector2(((Constants.GAME_WIDTH / 2) - 8), ((Constants.GAME_HEIGHT / 2)) - 9), 16, 18);

        private Color spriteColor;

        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }

        private Vector2 oldPosition = new Vector2(0, 0);

        /// <summary>
        /// The bounding volume of the sprite
        /// </summary>
        public BoundingRectangle Bounds => bounds;

        /// <summary>
        /// The color to blend with the ghost
        /// </summary>
        public Color Color { get; set; } = Color.White;

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("flycatcher");
        }

        public void ResetPosition(Vector2 position)
        {
            this.position = position;
            this.oldPosition = position;
        }

        /// <summary>
        /// Updates the sprite's position based on user input
        /// </summary>
        /// <param name="gameTime">The GameTime</param>
        public void Update(GameTime gameTime)
        {
            gamePadState = GamePad.GetState(0);
            keyboardState = Keyboard.GetState();

            moving = false;
            rotation = 0;

            // Apply the gamepad movement with inverted Y axis
            position += gamePadState.ThumbSticks.Left * new Vector2(1, -1);
            if (gamePadState.ThumbSticks.Left.X < 0) flipped = true;
            if (gamePadState.ThumbSticks.Left.X > 0) flipped = false;

            // Apply keyboard movement
            if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
            {
                position += new Vector2(0, -1 * speed);
                moving = true;
                if (flipped) { rotation = 0.10f; } else { rotation = -0.20f; }
            }
            if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
            {
                position += new Vector2(0, 1 * speed);
                moving = true;
                if (flipped) { rotation = -0.10f; } else { rotation = 0.20f; }
            }
            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            {
                position += new Vector2(-1 * speed, 0);
                flipped = true;
                moving = true;
            }
            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                position += new Vector2(1 * speed, 0);
                flipped = false;
                moving = true;
            }
            bounds.X = position.X - 8;
            bounds.Y = position.Y - 9;

            Position = position;// new Vector2(Constants.GAME_WIDTH / 2, Constants.GAME_HEIGHT / 2);
            Velocity = Position - oldPosition;
            oldPosition = position;
        }

        /// <summary>
        /// Draws the sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Color recolor)
        {
            Color = recolor;
            SpriteEffects spriteEffects = (flipped) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            int x = (moving) ? 66 : 0;
            var source = new Rectangle(0 + x, 0, 66, 72);
            spriteBatch.Draw(texture, position, source, Color, rotation, new Vector2(33, 36), 1, spriteEffects, 0);
        }
    }
}
