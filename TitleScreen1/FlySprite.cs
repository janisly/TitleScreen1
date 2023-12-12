using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Flycatcher.Collisions;

namespace Flycatcher
{
    public enum Direction
    {
        Right = 0,
        Down = 1,
        Left = 2,
        Up = 3,
    }

    /// <summary>
    /// A class representing a fly
    /// </summary>
    public class FlySprite
    {
        private Texture2D texture;

        private double directionTimer;

        private double animationTimer;

        private short animationFrame = 1;

        private bool flap = false;

        private BoundingCircle bounds;
        
        /// <summary>
        /// The current rotation of the fly. Default 0.
        /// </summary>
        public float Rotation;

        public bool Caught { get; set; } = false;


        //The direction of the fly
        public Direction Direction;

        //The position of the fly
        public Vector2 Position;

        /// <summary>
        /// The bounding volume of the sprite
        /// </summary>
        public BoundingCircle Bounds => bounds;

        /// <summary>
        /// Creates a new fly sprite
        /// </summary>
        /// <param name="position">The position of the sprite in the game</param>
        public FlySprite(Vector2 position)
        {
            this.Position = position;
            this.bounds = new BoundingCircle(position + new Vector2(18, 18), 18);
        }

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
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            //Switch animation frame every second
            if (animationTimer > 0.5)
            {
                flap = !flap;
                animationTimer -= 0.5;
            }

            //Switch directions every half second
            if (directionTimer > 2.0)
            {
                Circle(1);
                directionTimer -= 2.0;
            }

            //Move the fly in the direction it is flying
            switch (Direction)
            {
                case Direction.Up:
                    Position += new Vector2(0, -1) * 80 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;
                case Direction.Down:
                    Position += new Vector2(0, 1) * 80 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;
                case Direction.Right:
                    Position += new Vector2(1, 0) * 80 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;
                case Direction.Left:
                    Position += new Vector2(-1, 0) * 80 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;
            }
            bounds = new BoundingCircle(Position + new Vector2(18, 18), 18);
        }

        public void Circle(int type)
        {
            switch (type)
            {
                case 1:
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
                    break;
            }
            
        }

        /// <summary>
        ///  Draws the animated fly sprite
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The SpriteBatch to draw with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, float flyScareRotation)
        {
            if (Caught) return;
            int X = 0;
            int Y = 0;
            //Draws the sprite
            Rotation = flyScareRotation;
            switch (Direction)
            {
                case Direction.Up:
                    X = 0;
                    Y = 0;
                    break;
                case Direction.Down:
                    X = 36;
                    Y = 36;
                    break;
                case Direction.Right:
                    X = 36;
                    Y = 0;
                    break;
                case Direction.Left:
                    X = 0;
                    Y = 36;
                    break;
            }
            int x = 0;
            if (!flap) x = 72;
            var source = new Rectangle(X + x, Y, 36, 36);
            spriteBatch.Draw(texture, Position, source, Color.White, Rotation, new Vector2(0,0), 1, SpriteEffects.None, 0);
        }
    }
}
