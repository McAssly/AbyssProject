using System.Collections.Generic;
using System;
using Abyss.Utility;
using Microsoft.Xna.Framework;

namespace Abyss.Levels.data
{
    internal class Dungeon

    {
        public static int fail_count = 0;


        /// <summary>
        /// the maximum width that the dungeon can be, (dungeon spawn is in the center, width/2) MUST BE ODD
        /// </summary>
        internal int width;

        /// <summary>
        /// the maximum height of the dungeon's level
        /// </summary>
        internal int height;


        /// <summary>
        /// the maximum number of paths that can exist at any given point MUST BE GREATER THAN 2
        /// </summary>
        internal int path_count;


        /// <summary>
        /// the rate at which path generation diverges/converges;  divergence (+value);  convergence (-value)
        /// </summary>
        internal double divergence_rate;

        /// <summary>
        /// the map length for build index
        /// </summary>
        internal int map_length = 0;

        /// <summary>
        /// the finalized array of nodes within the dungeon
        /// </summary>
        internal MapNode[] nodes;

        /// <summary>
        /// the map used for generating
        /// </summary>
        internal MapNode[,] map;

        /// <summary>
        /// the paths for the system, each path is only the leading node for each path
        /// </summary>
        internal Path[] paths;


        /// <summary>
        /// for divergence checks
        /// </summary>
        private Random rand = new Random();

        /// <summary>
        /// initializes a new dungeon
        /// </summary>
        public Dungeon(int width, int height, int path_count, double divergence_rate)
        {
            this.height = height;
            this.width = width;
            this.path_count = path_count;
            this.divergence_rate = divergence_rate;

            // set the map
            map = new MapNode[height, width];

            // add the entrance node
            this.AddNode(new MapNode(width / 2, 0, (byte)(path_count + 1), map_length, null));

            // set the paths
            this.paths = InitializePaths(path_count);
            this.nodes = this.Generate();
        }

        private Path[] InitializePaths(int path_count)
        {
            // grab the entrance node
            Vector origin = new Vector(width / 2, 0);

            // initialize the paths
            List<Path> paths = new List<Path>(path_count);

            int path_min = path_count;
            if (path_count > 3) path_min = 3;
            int zone_split = width / path_min;

            // loop through and initialize each path
            for (int i = 0; i < path_count; i++)
            {
                // set the 1st 3 path origins
                Vector path_origin = origin;
                Vector path_range = new Vector(i * zone_split, (i + 1) * zone_split);
                switch (i)
                {
                    case 0: path_origin = new Vector(origin.x - 1, origin.y); break;
                    case 1: path_origin = new Vector(origin.x, origin.y + 1); break;
                    case 2: path_origin = new Vector(origin.x + 1, origin.y); break;
                } // for the others, keep null and store for later for building
                if (i < 3) paths.Add(new Path(path_origin, path_range, i, this));
                else paths.Add(new Path());
            }
            return paths.ToArray();
        }


        /// <summary>
        /// generates the dungeon level comepletely
        /// </summary>
        /// <returns></returns>
        private MapNode[] Generate()
        {

            /**
             * DIVERGENCE SECTION ---------------------------------------------
             */
            // DIVERGE UNTIL ALL PATHS REACH THE MEDIAN
            while (PathsAreBeforeMedian())
            {
                // loop through each path
                for (int i = 0; i < paths.Length; i++)
                {
                    if (!paths[i].Exists()) continue;
                    paths[i].MoveBeforeMedian(this);
                    //this.Print();
                }
            }

            /**
             * CONVERGENCE SECTION -----------------------------------------
             */
            // WHILE THERE ARE STILL PATHS EXISTING 

            this.SetPathsToFinalDestination();
            while (PathsUnfinished())
            {
                // loop through each path
                foreach (var path in paths)
                {
                    path.Move(this, false, true);
                }
            }


            return new MapNode[0];
        }


        private void SetPathsToFinalDestination()
        {
            foreach (Path path in paths)
            {
                path.destination = new Vector2(width / 2, height - 1);
            }
        }


        private bool PathsUnfinished()
        {
            foreach (Path path in paths)
            {
                if (path.head != path.destination) return true;
            }
            return false;
        }

        /// <summary>
        /// determines whether or not EVERY path is behind the median or not
        /// </summary>
        /// <returns></returns>
        private bool PathsAreBeforeMedian()
        {
            foreach (var path in paths)
            {
                if (!path.Exists()) continue;
                if (path.GetY() < GetMedian()) return true;
            }
            return false;
        }

        internal int GetFirstNullPath()
        {
            for (int i = 0; i < paths.Length; i++)
            {
                if (!paths[i].Exists()) { return i; }
            }
            return -1;
        }


        /// <summary>
        /// creates a new node in the given direction and links it to the previous; 0: left -> 1: right -> 2: forward -> 3: backward
        /// </summary>
        /// <returns></returns>
        internal void CreateNode(Vector position, MapNode previous, int path_index)
        {
            map[position.y, position.x] = new MapNode(position.x, position.y, (byte)path_index, map_length, previous);
            map_length++;
        }


        /// <summary>
        /// adds a new node to the node map
        /// </summary>
        /// <param name="node"></param>
        private void AddNode(MapNode node)
        {
            map[node.position.y, node.position.x] = node;
            map_length++;
        }


        /// <summary>
        /// calculates the median line which seperates the dungeon between its path's divergence or convergence
        /// </summary>
        /// <returns></returns>
        internal int GetMedian()
        {
            return (height - 1) / 2;
        }

        /// <summary>
        /// calculates the current chance to diverege or converge the path based on the given y value
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        internal double CalculateDiveregenceRate(int y, bool decreasing = false)
        {
            // OLD FUNCTION: return Math.Cbrt((y - GetMedian()) / (-GetMedian() / Math.Pow(divergence_rate, 3)));
            if (decreasing)
            {
                return (divergence_rate - 1) / Math.Pow(height - 1, 2) * y * y + 1;
            }
            return (1 - divergence_rate) / Math.Pow(height - 1, 2) * y * y + divergence_rate;
        }


        /// <summary>
        /// checks if the node diverges (or converges)
        /// </summary>
        /// <param name="y"></param>
        /// <param name="modifier"></param>
        /// <returns></returns>
        internal bool Diverged(int y, bool decreasing = false)
        {
            double divergence_rate = CalculateDiveregenceRate(y, decreasing);
            return rand.NextDouble() < divergence_rate;
        }
    }
}
