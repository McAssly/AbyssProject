using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Abyss.Levels.data
{
    internal class Dungeon

    {
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
        internal int path_limit;


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
        internal List<MapNode> paths;


        /// <summary>
        /// for divergence checks
        /// </summary>
        private Random rand = new Random();

        /// <summary>
        /// initializes a new dungeon with the given dataset
        /// </summary>
        /// <param name="data"></param>
        public Dungeon(DungeonData data)
        {
            this.height = data.height;
            this.width = data.width;
            // make sure the width is ODD
            if (this.width % 2 == 0) this.width += 1;
            this.path_limit = data.path_limit;
            // make sure the path limit is not too small, minimum is 3
            if (this.path_limit < 3) this.path_limit = 3;
            this.divergence_rate = data.divergence_rate;

            this.nodes = this.Generate();
        }

        /// <summary>
        /// initializes a new dungeon
        /// </summary>
        public Dungeon(int width, int height, int path_limit, double divergence_rate)
        {
            this.height = height;
            this.width = width;
            // make sure the width is ODD
            if (this.width % 2 == 0) this.width += 1;
            this.path_limit = path_limit;
            // make sure the path limit is not too small, minimum is 3
            if (this.path_limit < 3) this.path_limit = 3;
            this.divergence_rate = divergence_rate;

            this.paths = new List<MapNode>();

            this.nodes = this.Generate();
        }


        /// <summary>
        /// generates the dungeon level comepletely
        /// </summary>
        /// <returns></returns>
        private MapNode[] Generate()
        {
            // initialize the map
            map = new MapNode[height, width];

            // add the entrance node
            map[0, width / 2] = new MapNode(width/2, 0, 0, map_length, null);
            map_length++;

            // generate the first 3 paths
            for (byte d = 0; d < 3; d++) // left -> right -> forward
                if (CheckDirectionWithinBounds(GetEntranceNode(), d))
                    CreateNode(GetEntranceNode(), false, d, null);

            /**
             * DIVERGENCE SECTION ---------------------------------------------
             */

            // DIVERGE UNTIL ALL PATHS REACH THE MEDIAN
            while (PathsAreBeforeMedian())
            {
                List<MapNode> new_paths = new List<MapNode>(paths);
                // loop through each path
                foreach (var node in paths)
                {
                    // skip any paths that have already reached the median
                    if (node.position.y == GetMedian()) continue;

                    // otherwise continue generating the path
                    for (byte d = 0; d < 3; d++)
                    {
                        bool passed = false;
                        // first check if we still need to make more paths
                        bool path_override = false;
                        if (paths.Count >= path_limit) path_override = true;
                        if (node.IsClosed(d))
                            passed = MovePath(node, d, path_override, new_paths);
                        if (passed) break;
                    }

                    this.Print();
                }
                this.paths = new_paths;
                //this.Print();
            }


            this.Print();

            /**
             * CONVERGENCE SECTION -----------------------------------------
             */

            // WHILE THERE ARE STILL PATHS EXISTING 


            return new MapNode[0];
        }


        /// <summary>
        /// moves the path in the given direction if it passes the divergence check
        /// </summary>
        /// <param name="previous"></param>
        /// <param name="direction_flag"></param>
        /// <param name="path_override"></param>
        private bool MovePath(MapNode previous, byte direction_flag, bool path_override, List<MapNode> new_paths)
        {
            // if there is no previous node to build off of dont do anything
            if (previous == null || (previous.position.x == 0 && direction_flag == 0) || (previous.position.x == width - 1 && direction_flag == 1)) return false;

            // check for the next node that the path will move onto

            // if the next node is empty we can try moving
            if (GetNextNode(previous, direction_flag) == null)
            {
                // if divergence passes move to the node and create one
                if (Diverged(previous.position.y))
                {
                    CreateNode(previous, path_override, direction_flag, new_paths);
                    return true;
                }
            }
            else
            {
                // otherwise there is already a node there and if divergence passes, connect
                previous.Connect(GetNextNode(previous, direction_flag));
                if (!paths.Contains(GetNextNode(previous, direction_flag)))
                {
                    GetNextNode(previous, direction_flag).path_id = previous.path_id;
                    new_paths[previous.path_id] = GetNextNode(previous, direction_flag);
                }
            }
            return false;
        }


        /// <summary>
        /// check if the next node in the given direction will be within the map boundaries; 0: left -> 1: right -> 2: forward -> 3: backward
        /// </summary>
        /// <param name="node"></param>
        /// <param name="direction_flag"></param>
        /// <returns></returns>
        private bool CheckDirectionWithinBounds(MapNode node, byte direction_flag)
        {
            switch (direction_flag)
            {
                case 0: // left
                    return node.position.x - 1 >= 0;
                case 1: // right
                    return node.position.x + 1 < width;
                case 2: // forward
                    return node.position.y + 1 < height;
                case 3: // backward
                    return node.position.y - 1 >= 0;
            }
            return false;
        }



        /// <summary>
        /// determines whether or not EVERY path is behind the median or not
        /// </summary>
        /// <returns></returns>
        private bool PathsAreBeforeMedian()
        {
            foreach (var node in paths)
            {
                if (node.position.y < GetMedian()) return true;
            }
            return false;
        }


        /// <summary>
        /// creates a new node in the given direction and links it to the previous; 0: left -> 1: right -> 2: forward -> 3: backward
        /// </summary>
        /// <param name="map_length"></param>
        /// <param name="previous"></param>
        /// <param name="override_path"></param>
        /// <param name="direction_flag"></param>
        /// <returns></returns>
        private void CreateNode(MapNode previous, bool override_path, byte direction_flag, List<MapNode> paths)
        {
            if (!CheckDirectionWithinBounds(previous, direction_flag)) return;
            map_length++;
            int x = previous.position.x;
            int y = previous.position.y;
            switch (direction_flag)
            {
                case 0:  x -= 1; break; // left
                case 1:  x += 1; break; // right
                case 2:  y += 1; break; // forward
                case 3:  y -= 1; break; // backward
            }
            MapNode new_node = new MapNode(x, y, (byte)this.paths.Count(), map_length, previous);
            if (override_path)
            {
                new_node.path_id = previous.path_id;
                paths[new_node.path_id] = new_node;
            }
            else if (paths != null) paths.Add(new_node);
            else this.paths.Add(new_node);
            this.AddNode(new_node);
        }


        /// <summary>
        /// gets the next node in the given direction; 0: left -> 1: right -> 2: forward -> 3: backward
        /// </summary>
        /// <param name="node"></param>
        /// <param name="direction_flag"></param>
        /// <returns></returns>
        private MapNode GetNextNode(MapNode node, byte direction_flag)
        {
            MapNode next = null;
            if (!CheckDirectionWithinBounds(node, direction_flag)) return next;
            switch (direction_flag)
            {
                case 0: // LEFT
                    next = map[node.position.y, node.position.x - 1]; break;
                case 1: // RIGHT
                    next = map[node.position.y, node.position.x + 1]; break;
                case 2: // FORWARD
                    next = map[node.position.y + 1, node.position.x]; break;
                case 3: // BACKWARD
                    next = map[node.position.y - 1, node.position.x]; break;
            }
            return next;
        }


        /// <summary>
        /// gets the entrance node
        /// </summary>
        /// <returns></returns>
        private MapNode GetEntranceNode()
        {
            return map[0, width / 2];
        }


        /// <summary>
        /// adds a new node to the node map
        /// </summary>
        /// <param name="node"></param>
        private void AddNode(MapNode node)
        {
            map[node.position.y, node.position.x] = node;
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
        internal double CalculateDiveregenceRate(int y)
        {
            return Math.Cbrt((y - GetMedian()) / (-GetMedian() / Math.Pow(divergence_rate, 3)));
        }


        /// <summary>
        /// checks if the node diverges (or converges)
        /// </summary>
        /// <param name="y"></param>
        /// <param name="modifier"></param>
        /// <returns></returns>
        internal bool Diverged(int y, double modifier = 1)
        {
            double divergence_rate = CalculateDiveregenceRate(y) * modifier;
            return rand.NextDouble() < divergence_rate;
        }



        /// <summary>
        /// simply prints out the generated map to the debug console
        /// </summary>
        private void Print()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (map[y, x] is null) Debug.Write("  ");
                    else if (paths.Contains(map[y, x])) Debug.Write("* ");
                    else Debug.Write(map[y,x].path_id + " ");
                }
                Debug.Write("\n");
            }
        }
    }
}
