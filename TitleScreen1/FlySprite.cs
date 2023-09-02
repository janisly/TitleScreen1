using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TitleScreen1
{
    public enum Direction
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3,
    }

    /// <summary>
    /// A class representing a fly
    /// </summary>
    internal class FlySprite
    {
        private Texture2D texture;

        private double directionTimer;

        private double animationTimer;

        private short animationFrame = 1;


        //The direction of the fly
        public Direction Direction;

        //The position of the fly
        public Vector2 Position;

        /// <summary>
        /// Loads the fly sprite texture
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("fly_1");
        }

        /// <summary>
        /// Updates the fly sprite to fly in a pattern
        /// </summary>
        /// <param name="gameTime">The game time</param>
        public void Update(GameTime gameTime)
        {
            //Update the direction timer
            directionTimer += gameTime.ElapsedGameTime.TotalSeconds;

            //Switch directions every two seconds
            if (directionTimer > 2.0)
            {
                switch (Direction)
                {
                    case Direction.Up:
                        Direction = Direction.Right;
                        break;
                    case Direction.Down:
                        Direction = Direction.Left;
                        break;
                    case Direction.Right:
                        Direction = Direction.Down;
                        break;
                    case Direction.Left:
                        Direction = Direction.Up;
                        break;
                }
                directionTimer -= 2.0;
            }

            //Move the fly in the direction it is flying
            switch (Direction)
            {
                case Direction.Up:
                    Position += new Vector2(0, -1) * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;
                case Direction.Down:
                    Position += new Vector2(0, 1) * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;
                case Direction.Right:
                    Position += new Vector2(-1, 0) * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;
                case Direction.Left:
                    Position += new Vector2(1, 0) * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;
            }
        }

        /// <summary>
        ///  Draws the animated fly sprite
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The SpriteBatch to draw with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            int X = 0;
            int Y = 0;
            //Draws the sprite
            switch (Direction)
            {
                case Direction.Up:
                    X = 0;
                    Y = 0;
                    break;
                case Direction.Down:
                    X = 9;
                    Y = 9;
                    break;
                case Direction.Right:
                    X = 9;
                    Y = 0;
                    break;
                case Direction.Left:
                    X = 0;
                    Y = 9;
                    break;
            }
            var source = new Rectangle(X, Y, 9, 9);
            spriteBatch.Draw(texture, Position, source, Color.White);
        }
    }
}
