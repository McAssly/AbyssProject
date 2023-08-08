﻿using Abyss.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.Globals
{
    internal class _Levels
    {
        // TILESETS
        private static readonly string TS_EASTWOODS = "tilesets/eastwoods";

        // MAPS
        public static Level Eastwoods = new Level(Levels.overworld.Eastwoods.Maps, TS_EASTWOODS);
    }
}