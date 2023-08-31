using Abyss.Levels;
using Microsoft.Xna.Framework.Graphics;

namespace Abyss.Globals
{
    internal class _Levels
    {
        // TILESETS
        private static readonly string TS_EASTWOODS = "tilesets/eastwoods";

        // MAPS
        public static Level Eastwoods = new Level(Levels.overworld.Eastwoods.Maps, TS_EASTWOODS);
    }

    internal class _Dungeons
    {
        // TILESETS
        public static Texture2D TestDungeon;

        // DUNGEON DATA

        // DUNGEON PIECES
    }
}
