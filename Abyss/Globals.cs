using Abyss.Map;
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
        public static int WindowW = 512, WindowH = 512;
        public static readonly double FRAME_SPEED = 1.2;

        // GAME CONSTANTS
        public static readonly int TILE_SIZE = 16;
        public static readonly float GAME_SCALE = WindowW / (TILE_SIZE * 16);

        // DEBUG
        public static Texture2D TESTBOX;
    }
}
