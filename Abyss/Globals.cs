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
        public static Map.Map TESTLEVEL = new Map.Map(
            0, TILESET0,
            new Layer[]
            {
                new Layer(false, new uint[,] // FLOOR LAYER
                {
                    { 4,4,4,4,0,4,4,4 },
                    { 4,4,4,4,0,4,4,4 },
                    { 4,4,4,4,0,4,4,4 },
                    { 4,4,4,4,0,4,4,4 },
                    { 4,4,4,4,0,4,4,4 },
                    { 4,4,4,4,3,4,4,4 },
                    { 4,4,4,4,0,4,4,4 },
                    { 4,4,4,4,0,4,4,4 }
                }),
                new Layer(true, new uint[,] // WALL LAYER
                {
                    { 0,0,0,0,5,0,0,0 },
                    { 0,0,0,0,5,0,0,0 },
                    { 0,0,0,0,5,0,0,0 },
                    { 0,0,0,0,5,0,0,0 },
                    { 0,0,0,0,5,0,0,0 },
                    { 0,0,0,0,0,0,0,0 },
                    { 0,0,0,0,5,0,0,0 },
                    { 0,0,0,0,5,0,0,0 }
                }),
                new Layer(false, new uint[,] // OBJECT LAYER (unblocked)
                {
                    { 1,9,9,9,0,1,1,1 },
                    { 1,0,0,0,0,0,0,1 },
                    { 1,0,2,0,0,0,0,1 },
                    { 1,0,0,0,0,27,28,1 },
                    { 1,0,0,0,0,0,0,1 },
                    { 1,2,0,0,0,0,0,1 },
                    { 1,0,0,9,0,0,0,1 },
                    { 1,1,1,1,0,1,1,1 }
                }),
                new Layer(true, new uint[,] // OBJECT LAYER (blocked)
                {
                    { 0,0,0,0,0,6,7,0 },
                    { 0,0,0,16,0,13,14,0 },
                    { 0,0,0,0,0,20,21,0 },
                    { 0,0,0,0,0,0,0,0 },
                    { 0,8,0,0,0,16,0,0 },
                    { 0,0,0,0,0,0,0,0 },
                    { 0,0,0,0,0,0,8,0 },
                    { 0,0,0,0,0,0,0,0 }
                })
            });

        // DEBUG
        public static Texture2D TESTBOX;
    }
}
