﻿using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.Map
{
    /**
     * A class to store levels in
     * 
     */
    internal class Levels
    {
        // TILESETS
        private static readonly string TS_OVERWORLD = "tsOverworld";

        // MAPS
        public static Map MP_START0 = new Map(
            0, null,
            new Layer[]
            {
                new Layer(false, new uint[,] // floor layer
                {
                    { 9, 9, 9, 9, 9, 9, 9, 11, 14, 13, 9, 9, 9, 9, 9, 9 },
                    { 9, 9, 9, 9, 9, 9, 9, 11, 14, 13, 9, 9, 9, 9, 9, 9 },
                    { 9, 9, 9, 9, 9, 9, 9, 11, 14, 13, 9, 9, 9, 9, 9, 9 },
                    { 9, 9, 9, 9, 9, 9, 9, 11, 14, 13, 9, 9, 9, 9, 9, 9 },
                    { 9, 9, 9, 9, 9, 9, 9, 11, 14, 13, 9, 9, 9, 9, 9, 9 },
                    { 9, 9, 9, 9, 9, 9, 9, 11, 14, 13, 9, 9, 9, 9, 9, 9 },
                    { 9, 9, 9, 9, 9, 9, 9, 11, 14, 13, 9, 9, 9, 9, 9, 9 },
                    { 9, 9, 9, 9, 9, 9, 9, 11, 14, 13, 9, 9, 9, 9, 9, 9 },
                    { 9, 9, 9, 9, 9, 9, 9, 11, 14, 13, 9, 9, 9, 9, 9, 9 },
                    { 9, 9, 9, 9, 9, 9, 10, 24, 14, 32, 10, 9, 9, 9, 9, 9 },
                    { 9, 9, 9, 9, 9, 11, 14, 14, 14, 14, 14, 13, 9, 9, 9, 9 },
                    { 9, 9, 9, 9, 9, 11, 14, 14, 14, 14, 14, 13, 9, 9, 9, 9 },
                    { 9, 9, 9, 9, 9, 11, 14, 14, 14, 14, 14, 13, 9, 9, 9, 9 },
                    { 9, 9, 9, 9, 9, 9, 12, 12, 12, 12, 12, 9, 9, 9, 9, 9 },
                    { 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9 },
                    { 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9 }
                }),

                new Layer(true, new uint[,]
                {
                    {44, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 44},
                    {44, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 44},
                    {44, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 44},
                    {44, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 44},
                    {44, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 44},
                    {44, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 44},
                    {44, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 44},
                    {44, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 44},
                    {44, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 44},
                    {44, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 44},
                    {44, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 44},
                    {44, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 44},
                    {44, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 44},
                    {44, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 44},
                    {44, 44, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 44, 44},
                    {44, 44, 44, 44, 44, 44, 44, 44, 44, 44, 44, 44, 44, 44, 44, 44}
                }),

                new Layer(true, new uint[,]
                {
                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 2, 0, 8, 4, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, 0, 0},
                    {0, 0, 0, 8, 0, 0, 2, 0, 0, 0, 2, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 2, 0, 0, 0, 2, 0, 0, 0, 2, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 2, 0, 0, 0, 0, 0},
                    {0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0},
                    {0, 0, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, 0, 0},
                    {0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
                }),

                new Layer(false, new uint[,]
                {
                    {0, 42, 0, 33, 0, 0, 0, 0, 37, 34, 0, 0, 0, 33, 43, 0},
                    {0, 42, 0, 0, 0, 0, 33, 0, 37, 0, 0, 35, 0, 0, 43, 0},
                    {0, 42, 0, 36, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 43, 0},
                    {0, 42, 0, 0, 0, 34, 0, 0, 0, 0, 0, 0, 0, 0, 43, 0},
                    {0, 42, 0, 0, 0, 0, 0, 0, 37, 0, 0, 0, 0, 0, 43, 0},
                    {0, 42, 0, 0, 33, 0, 0, 0, 0, 0, 0, 33, 0, 0, 43, 0},
                    {0, 42, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 36, 0, 43, 0},
                    {0, 42, 0, 35, 0, 0, 0, 0, 37, 0, 0, 0, 0, 0, 43, 0},
                    {0, 42, 33, 0, 0, 0, 0, 0, 0, 35, 0, 0, 0, 0, 43, 0},
                    {0, 42, 0, 0, 0, 36, 0, 0, 0, 0, 0, 0, 36, 0, 43, 0},
                    {0, 42, 0, 0, 0, 0, 0, 0, 0, 37, 0, 0, 33, 0, 43, 0},
                    {0, 42, 0, 0, 0, 0, 37, 0, 0, 0, 0, 0, 0, 0, 43, 0},
                    {0, 42, 0, 0, 0, 0, 0, 0, 0, 37, 0, 0, 0, 0, 43, 0},
                    {0, 46, 0, 33, 0, 0, 0, 0, 34, 0, 0, 0, 0, 0, 47, 0},
                    {0, 0, 46, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 47, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
                })
            });

        public static void LoadTileSets(ContentManager Content)
        {
            MP_START0.TileSet = Content.Load<Texture2D>(TS_OVERWORLD);
        }
    }
}
