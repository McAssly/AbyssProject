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
        // GAME CONSTANTS
        public static readonly int TILE_SIZE = 16;

        // TILESETS
        public static Texture2D TILESET0;

        // MAPS
        public static Map.Map LEVEL0 = new Map.Map(
            0, TILESET0,
            new Layer[]
            {
                new Layer(false, new uint[,]{ { } }),
                new Layer(true, new uint[,]{ { } })
            });

        // DEBUG
        public static Texture2D TESTBOX;
    }
}
