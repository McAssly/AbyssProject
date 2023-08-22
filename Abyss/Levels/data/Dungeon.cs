using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.Levels.data
{
    internal class Dungeon

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
        /// the array of nodes within the dungeon
        /// </summary>
        internal MapNode[] central;

        /// <summary>
        /// the paths for the system
        /// </summary>
        internal MapPath[] paths;



        /// <summary>
        /// initializes a new dungeon
        /// </summary>
        public Dungeon(int end_y, int width, int max_path, double divergence_rate)
        {
            this.end_y = end_y;
            this.width = width;
            this.max_path = max_path;
            this.divergence_rate = divergence_rate;

            this.nodes = Generate();
        }




        private MapNode[] Generate()
        {

        }


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
        internal double CalculateDiveregenceRate(int y)
        {
            if (y >= end_y || y < 0) return -1;
            return Math.Cbrt((y - GetMedian()) / (-GetMedian() / Math.Pow(divergence_rate, 3)));
        }
    }
}
