using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.Levels.dungeons
{
    internal struct DungeonData
    {
        /// <summary>
        /// the maximum width that the dungeon can be, (dungeon spawn is in the center, width/2)
        /// </summary>
        internal int width;

        /// <summary>
        /// the maximum height of the dungeon's level
        /// </summary>
        internal int end_y;


        /// <summary>
        /// the maximum number of paths that can exist at any given point
        /// </summary>
        internal int max_path;


        /// <summary>
        /// the rate at which path generation diverges/converges;  divergence (+value);  convergence (-value)
        /// </summary>
        internal double divergence_rate;


        /// <summary>
        /// calculates the median line which seperates the dungeon between its path's divergence or convergence
        /// </summary>
        /// <returns></returns>
        internal int GetMedian()
        {
            return (end_y - 1) / 2;
        }

        /// <summary>
        /// calculates the current chance to diverege or converge the path based on the given y value
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public double CalculateDiveregenceRate(int y)
        {
            if (y >= end_y || y < 0) return -1;
            return Math.Cbrt((y - GetMedian()) / (-GetMedian() / Math.Pow(divergence_rate, 3)));
        }
    }
}
