using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace TitleScreen1.Collisions
{
    public struct BoundingRectangle
    {
        /// <summary>
        /// The X-coordinate of the BoundingRectangle
        /// </summary>
        public float X;

        /// <summary>
        /// The Y-coordinate of the BoundingRectangle
        /// </summary>
        public float Y;

        /// <summary>
        /// The Width of the BoundingRectangle
        /// </summary>
        public float Width;

        /// <summary>
        /// The Height of the BoundingRectangle
        /// </summary>
        public float Height;

        /// <summary>
        /// The Left-coordinate of the BoundingRectangle
        /// </summary>
        public float Left => X;

        /// <summary>
        /// The Right-coordinate of the BoundingRectangle
        /// </summary>
        public float Right => X + Width;

        /// <summary>
        /// The Top-coordinate of the BoundingRectangle
        /// </summary>
        public float Top => Y;

        /// <summary>
        /// The Bottom-coordinate of the BoundingRectangle
        /// </summary>
        public float Bottom => Y + Height;

        /// <summary>
        /// Constructs a new BoundingRectangle
        /// </summary>
        /// <param name="x">The X-coordinate of the BoundingRectangle</param>
        /// <param name="y">The Y-coordinate of the BoundingRectangle</param>
        /// <param name="width">The Width of the BoundingRectangle</param>
        /// <param name="height">The Height of the BoundingRectangle</param>
        public BoundingRectangle(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Constructs a new BoundingRectangle
        /// </summary>
        /// <param name="position">The position of the BoundingRectangle</param>
        /// <param name="width">The Width of the BoundingRectangle</param>
        /// <param name="height">The Height of the BoundingRectangle</param>
        public BoundingRectangle(Vector2 position, float width, float height)
        {
            X = position.X;
            Y = position.Y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Tests for a collision between this and another bounding object
        /// </summary>
        /// <param name="other">The other bounding object</param>
        /// <returns>true for collision, false otherwise</returns>
        public bool CollidesWith(BoundingRectangle other)
        {
            return CollisionHelper.Collides(this, other);
        }

        /// <summary>
        /// Tests for a collision between this and another bounding object
        /// </summary>
        /// <param name="other">The other bounding object</param>
        /// <returns>true for collision, false otherwise</returns>
        public bool CollidesWith(BoundingCircle other)
        {
            return CollisionHelper.Collides(other, this);
        }
    }
}
