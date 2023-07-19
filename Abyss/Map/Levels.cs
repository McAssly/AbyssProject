using Abyss.Map.levels;
using Microsoft.Xna.Framework.Content;
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
        private static readonly string TS_EASTWOODS = "tilesets/eastwoods";

        // MAPS
        public static Level Eastwoods = new Level(levels.Eastwoods.Maps, TS_EASTWOODS);
    }
}
