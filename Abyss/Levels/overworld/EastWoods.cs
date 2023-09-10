﻿
using Abyss.Entities;
using Abyss.Entities.enemies;
using Abyss.Levels.data;

namespace Abyss.Levels.overworld
{
    internal class Eastwoods
	{
		public static TileMap[] Maps = new TileMap[2]
            {
                new TileMap(
                    new TileLayer[2]
                    {
                    new TileLayer(new uint[16,16]
                        {
                            {4,11,4,11,11,11,11,12,5,10,11,11,11,11,11,11},
                            {11,11,11,11,11,11,11,12,5,10,11,11,4,11,11,11},
                            {11,11,11,11,4,11,11,12,5,10,11,11,11,11,11,11},
                            {11,11,11,11,11,11,11,12,5,10,11,11,11,11,11,4},
                            {11,11,11,11,11,11,11,12,5,10,11,11,11,11,11,11},
                            {4,11,11,11,11,11,4,12,5,10,11,4,11,11,11,11},
                            {11,11,11,11,11,11,11,12,5,10,11,11,11,11,11,11},
                            {11,11,4,11,11,11,11,12,5,10,11,11,11,11,11,4},
                            {11,11,11,11,20,20,20,21,5,10,11,11,11,11,11,11},
                            {20,20,20,21,5,5,5,5,5,10,11,11,11,11,11,11},
                            {5,5,5,5,5,1,2,3,5,10,11,11,11,11,11,11},
                            {2,2,2,2,2,4,11,21,5,10,11,11,11,11,11,11},
                            {11,11,11,11,11,11,12,5,5,10,4,11,11,11,4,11},
                            {11,11,4,11,11,11,12,5,1,11,11,11,11,11,11,11},
                            {4,11,11,11,11,11,12,5,10,11,11,11,11,11,11,11},
                            {11,11,11,11,11,11,12,5,10,11,11,11,4,11,11,11}
                        }),
                    new TileLayer(new uint[16,16]
                        {
                            {37,29,0,0,0,0,0,0,0,0,0,0,0,0,38,37},
                            {37,29,0,0,0,0,0,0,0,0,0,0,0,0,38,37},
                            {37,29,0,0,0,0,0,0,0,0,0,7,0,0,38,37},
                            {37,29,0,0,0,0,0,0,0,0,0,0,0,0,38,37},
                            {37,29,0,0,0,0,0,0,0,0,0,0,0,0,38,37},
                            {29,46,0,0,0,0,0,0,0,0,0,0,0,0,38,37},
                            {46,0,0,0,0,0,0,0,0,0,0,0,0,0,38,37},
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,38,37},
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,38,37},
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,38,37},
                            {0,0,0,0,0,0,0,0,0,0,0,7,0,28,38,37},
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,38,37,37},
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,38,37,37},
                            {28,0,0,0,0,0,0,0,0,0,0,0,0,38,37,37},
                            {29,28,0,7,0,0,0,0,0,0,0,0,0,38,37,37},
                            {37,29,0,0,0,0,0,0,0,0,0,0,0,38,37,37}
                        }),
                    },
                    new TileLayer(new uint[16,16]
                        {
                            {0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0},
                            {0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0},
                            {0,1,0,0,0,0,0,0,0,0,0,1,0,0,1,0},
                            {0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0},
                            {0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0},
                            {1,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0},
                            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0},
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0},
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0},
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0},
                            {0,0,0,0,0,0,0,0,0,0,0,1,0,1,1,0},
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0},
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0},
                            {1,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0},
                            {1,1,0,1,0,0,0,0,0,0,0,0,0,1,0,0},
                            {0,1,0,0,0,0,0,0,0,0,0,0,0,1,0,0}
                        }),
                    new Enemy[3]{
                        new Rat(5, 7),
                        new Rat(4, 12),
                        new Rat(10, 9)
                    },
                    -1, 1, -1, -1),
                new TileMap(
                    new TileLayer[2]
                    {
                    new TileLayer(new uint[16,16]
                        {
                            {11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11},
                            {11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11},
                            {11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11},
                            {11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11},
                            {11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11},
                            {11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11},
                            {11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11},
                            {11,11,11,4,11,11,11,11,11,11,4,11,11,11,11,11},
                            {11,11,11,11,11,20,11,11,11,11,11,11,11,4,11,11},
                            {11,11,11,11,12,5,10,4,11,11,11,11,11,11,11,11},
                            {11,11,11,11,12,5,19,11,11,11,11,11,11,11,11,11},
                            {11,11,11,11,12,5,5,19,11,11,11,4,11,11,11,11},
                            {11,11,11,11,11,3,5,5,19,11,11,11,11,11,11,11},
                            {11,11,4,11,11,11,3,5,5,10,11,11,11,11,11,11},
                            {11,11,11,11,11,11,11,3,5,10,4,11,11,11,11,11},
                            {11,11,11,11,11,4,11,12,5,10,11,11,11,4,11,11}
                        }),
                    new TileLayer(new uint[16,16]
                        {
                            {37,37,37,37,30,47,31,37,37,37,37,37,37,37,37,37},
                            {37,37,37,37,30,48,31,37,37,37,37,37,37,37,37,37},
                            {37,37,37,37,30,47,31,37,37,37,37,37,37,37,37,37},
                            {37,37,37,37,30,48,31,37,37,37,37,37,37,37,37,37},
                            {37,37,37,37,30,47,31,37,37,37,37,37,37,37,37,37},
                            {37,37,29,46,30,48,31,46,46,46,38,37,37,37,37,37},
                            {37,29,46,0,39,49,40,0,0,0,46,46,46,46,38,37},
                            {37,29,0,0,0,0,0,0,0,0,0,0,0,0,38,37},
                            {37,29,9,0,0,0,0,0,0,0,0,7,0,0,38,37},
                            {37,29,0,0,0,0,0,0,0,0,0,0,0,0,38,37},
                            {37,29,0,0,0,0,0,0,0,0,0,0,0,0,38,37},
                            {37,29,0,0,0,0,0,0,0,0,0,0,0,0,38,37},
                            {37,29,0,0,0,0,0,0,0,0,0,0,0,0,38,37},
                            {37,29,0,0,0,0,0,0,0,0,0,0,0,0,38,37},
                            {37,29,0,8,0,0,0,0,0,0,0,0,0,0,38,37},
                            {37,29,0,0,0,0,0,0,0,0,0,0,0,0,38,37}
                        }),
                    },
                    new TileLayer(new uint[16,16]
                        {
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            {0,0,1,1,1,1,1,1,1,1,1,0,0,0,0,0},
                            {0,1,1,0,1,2,1,0,0,0,1,1,1,1,1,0},
                            {0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0},
                            {0,1,1,0,0,0,0,0,0,0,0,1,0,0,1,0},
                            {0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0},
                            {0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0},
                            {0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0},
                            {0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0},
                            {0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0},
                            {0,1,0,1,0,0,0,0,0,0,0,0,0,0,1,0},
                            {0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0}
                        }),
                    new Enemy[0]{},
                    -1, -1, -1, 0),
                };
};
}