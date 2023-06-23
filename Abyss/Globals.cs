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
        public static double game_scale = Math.Sqrt(TILE_SIZE) / (float)WindowSize;
        public static readonly double GameWindow_size = 256*game_scale;
        public static Vector3 DrawPosition = new Vector3(
                (float)((WindowW - GameWindow_size)/(2*game_scale)), 
                (float)((WindowH - GameWindow_size) / (2 * game_scale)),0);

        // UI CONSTANTS
        public static Color Black = new Color(48, 41, 39);
        public static readonly Vector2 MessageLocation = new Vector2(TILE_SIZE/2, TILE_SIZE / 2);
        public static readonly float MessageScale = 0.4f;

        // dialogue
        public static Vector2 DialoguePosition = new Vector2(DrawPosition.X + 16, DrawPosition.Y + 32);
        public static Vector2 DialogueSize = new Vector2((float)GameWindow_size/(float)game_scale - 32, 32);
        public static Vector2[] OptionOffset = new Vector2[3]
        {
            new Vector2(0, DialogueSize.Y * 3),
            new Vector2(0, DialogueSize.Y * 4),
            new Vector2(0, DialogueSize.Y * 5)
        };

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
