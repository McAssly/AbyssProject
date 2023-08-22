using Abyss.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.Levels.data
{
    internal class MapNode
    {
        /// <summary>
        /// the position of the node
        /// </summary>
        internal Vector position;


        /// <summary>
        /// whether or not the node is converged to the center
        /// </summary>
        internal bool converged;


        /// <summary>
        /// the type of room it is;
        /// 0 = generic; 1 = entrance; 2 = midpoint; 3 = end; etc.
        /// </summary>
        internal byte type;


        /// <summary>
        /// the adjacent nodes that they direct to; index of -1 means blocked off; index of -2 means open but disconnected
        /// </summary>
        // bottom = 0; left = 1; right = 2; top = 3;
        internal int[] next_nodes = new int[4];


        /// <summary>
        /// create a new map node
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="path_id"></param>
        /// <param name="nodes"></param>
        public MapNode(int x, int y, byte path_id, byte type, int path, Dungeon dungeon, MapNode previous = null)
        {
            this.type = type;
            position = new Vector(x, y);

            if (previous == null)
                next_nodes = new int[4] { -1, -2, -2, -2 };
            else
                next_nodes = Connect(previous, dungeon, path);
        }



        /// <summary>
        /// connects the map node to the previous and its surroundings
        /// </summary>
        /// <returns></returns>
        private int[] Connect(MapNode builder, Dungeon dungeon, int path)
        {
            // completely open
            next_nodes = new int[4] { -2, -2, -2, -2 };

            // if there is a path to the left or right of the node; BLOCK IT
            if (!converged)
            {
                MapNode[] adjacent_paths = dungeon.AdjacentPath(this, path);

                // check the left side
                if (adjacent_paths[0] is not null) next_nodes[1] = -1;

                // check the right side
                if (adjacent_paths[1] is not null) next_nodes[2] = -1;
            }

            // if the previous was next it;             BLOCK THE BOTTOM
            if (builder.position.y == this.position.y)
            {
                next_nodes[0] = -1;

                if (builder.position.x > this.position.x) next_nodes[2] = dungeon.IndexOf(builder);
            }

        }



        /// <summary>
        /// returns which connection type the node is
        /// </summary>
        /// <returns></returns>
        // 0 = all; 1 = bottom-left; 2 = bottom-right; 3 = bottom; 4 = end; 5 = left-right; 6 = midpoint; 7 = top-bottom; 8 = top-left; 9 = top-right; 10 = floating;
        public int GetConnectionType()
        {
            switch (type)
            {
                case 0: // <---- placeholder
                    {
                        return 1;
                    }
                case 1: return 0;
                case 2: return 6;
                case 3: return 4;
            }
            return 10;
        }
    }
}
