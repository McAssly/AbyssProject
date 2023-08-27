using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.Levels.data
{
    /// <summary>
    /// stores basic dungeon data
    /// </summary>
    internal struct DungeonData
    {
        internal int width, height, path_limit;
        internal double divergence_rate;

        public DungeonData(int width, int height, int path_limit, double divergence_rate)
        {
            this.width = width;
            this.height = height;
            this.path_limit = path_limit;
            this.divergence_rate = divergence_rate;
        }
    }
}
