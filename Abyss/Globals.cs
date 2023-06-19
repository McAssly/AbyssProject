using Abyss.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss
{
    internal static class Globals
    {
        // GAME VARIABLES
        public static double WindowSize = 1.5; // inverse size (bigger = smaller)
        public static int WindowW = (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / WindowSize), WindowH = (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / WindowSize);
        public static readonly double FRAME_SPEED = 1.2;

        // GAME CONSTANTS
        public static readonly int TILE_SIZE = 16;
        public static readonly int GameWindow_size = 256;
        public static readonly int UIWindow_size = 224;
        public static float game_scale = 4 / (float)WindowSize;

        // UI CONSTANTS
        public static readonly Vector2 MessageLocation = new Vector2(264, 8);
        public static readonly float MessageScale = (float)0.4;

        // DEBUG
        public static Texture2D TESTBOX;

        // TEXTURES / fonts
        public static SpriteFont FONT;


        /** Update the game scale when the window is resized:
         */
        public static void UpdateGameScale()
        {
            game_scale = 4 / (float)WindowSize;
        }
    }
}
