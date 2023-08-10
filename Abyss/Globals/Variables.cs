using Abyss.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.Globals
{
    internal class Variables
    {
        // game frame rates / animation speeds
        public static readonly double FRAME_FACTOR = 50;
        public static readonly double FRAME_SPEED = 1.0;
        public static readonly double ANIMATION_SPEED = 100.0;
        internal static readonly double PARTICLE_SUBTRACTOR = 0.5;

        // window sizing
        public static int WindowW = (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / Config.WindowScalar);
        public static int WindowH = (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / Config.WindowScalar);

        // draw positions and scaling
        public static float GameScale = 4 / (float)Config.WindowScalar;
        public static Vector3 DrawPosition = Math0.CenterWithinRectangle(WindowW, WindowH, 256, 256, GameScale).To3();


        /// <summary>
        /// Whenever the window scalar is updated we need to update some more variables so here we do this.
        /// </summary>
        public static void UpdateGameScaling(GraphicsDeviceManager _graphics)
        {
            GameScale = 4 / (float)Config.WindowScalar;
            WindowW = (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / Config.WindowScalar);
            WindowH = (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / Config.WindowScalar);
            DrawPosition = Math0.CenterWithinRectangle(WindowW, WindowH, 256, 256, GameScale).To3();

            UpdateWindowSize(_graphics);
        }

        public static void UpdateWindowSize(GraphicsDeviceManager _graphics)
        {
            _graphics.PreferredBackBufferWidth = WindowW;
            _graphics.PreferredBackBufferHeight = WindowH;
            _graphics.ApplyChanges(); // apply changes to the screen
        }


        // debug controllers
        public static bool DebugCollision = false;
        public static bool DebugDraw = true;



        // dialogue
        public static Vector2 DialoguePosition = new Vector2(DrawPosition.X + 16, DrawPosition.Y + 32);
        public static Vector2 DialogueSize = new Vector2(256 / GameScale - 32, 32);
        public static Vector2[] OptionOffset = new Vector2[3]
        {
            new Vector2(0, DialogueSize.Y * 3),
            new Vector2(0, DialogueSize.Y * 4),
            new Vector2(0, DialogueSize.Y * 5)
        };
    }
}
