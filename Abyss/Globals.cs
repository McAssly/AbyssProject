using Abyss.Map;
using Abyss.Sprite;
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
        public static readonly double FRAME_SPEED = 1.5;
        public static readonly double ANIMATION_SPEED = 100;

        // GAME CONSTANTS
        public static readonly int TILE_SIZE = 16;
        public static double GameScale = Math.Sqrt(TILE_SIZE) / (float)WindowSize;
        public static readonly double GAME_WINDOW_SIZE = 256*GameScale;
        public static Vector3 DrawPosition = new Vector3(
                (float)((WindowW - GAME_WINDOW_SIZE)/(2*GameScale)), 
                (float)((WindowH - GAME_WINDOW_SIZE) / (2 * GameScale)),0);

        public static readonly double FRAME_FACTOR = 50;
        public static readonly double PARTICLE_SUBTRACTOR = 0.5;

        // UI CONSTANTS
        public static Color Black = new Color(48, 41, 39);
        public static readonly Vector2 MESSAGE_LOCATION = new Vector2(TILE_SIZE/2, TILE_SIZE / 2);
        public static readonly float MESSAGE_SCALE = 0.4f;

        // dialogue
        public static Vector2 DialoguePosition = new Vector2(DrawPosition.X + 16, DrawPosition.Y + 32);
        public static Vector2 DialogueSize = new Vector2((float)GAME_WINDOW_SIZE/(float)GameScale - 32, 32);
        public static Vector2[] OptionOffset = new Vector2[3]
        {
            new Vector2(0, DialogueSize.Y * 3),
            new Vector2(0, DialogueSize.Y * 4),
            new Vector2(0, DialogueSize.Y * 5)
        };

        // DRAW LAYERS
        public static readonly float floor_z = 0.05f;
        public static readonly float deco_z = 0.04f;
        public static readonly float entity_z = 0.03f;
        public static readonly float wall_z = 0.02f;
        public static readonly float exterior1_z = 0.01f;
        public static readonly float exterior2_z = 0.0f;


        /** Update the game scale when the window is resized:
         */
        public static void UpdateGameScale()
        {
            GameScale = 4 / (float)WindowSize;
        }
    }
}
