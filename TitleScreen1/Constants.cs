using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flycatcher
{
    /// <summary>
    /// A class defining constants for the game
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The game width
        /// </summary>
        public static int GAME_WIDTH { get; set; }

        /// <summary>
        /// The game height
        /// </summary>
        public static int GAME_HEIGHT { get; set; }

        /// <summary>
        /// The game
        /// </summary>
        public static Game PARENT { get; set; }
    }
}
