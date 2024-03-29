﻿using Abyss.Utility;
using System.Collections.Generic;

namespace Abyss.Levels.data
{
    internal class MapNode
    {
        /// <summary>
        /// the position of the node on the node map
        /// </summary>
        internal Vector position;


        /// <summary>
        /// the path this node is in
        /// </summary>
        internal byte path_id;


        /// <summary>
        /// the map index of this node (*more like an ID)
        /// </summary>
        internal int map_index;


        /// <summary>
        /// the next map nodes that his node is connected to (the index); north east south west
        /// </summary>
        internal int[] next_nodes;



        private bool exists;

        /// <summary>
        /// creates a new map node
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="path_id"></param>
        /// <param name="last_index"></param>
        /// <param name="previous"></param>
        public MapNode(int x, int y, byte path_id, int last_index, MapNode? previous)
        {
            this.next_nodes = new int[4] { -1, -1, -1, -1 };
            this.position = new Vector(x, y);
            this.path_id = path_id;
            this.map_index = last_index;

            exists = true;

            if (previous != null)
                this.Connect(previous);
        }

        /// <summary>
        /// creates a new map node (used for cloning)
        /// </summary>
        /// <param name="position"></param>
        /// <param name="path_id"></param>
        /// <param name="map_index"></param>
        /// <param name="next_nodes"></param>
        public MapNode(Vector position, byte path_id, int map_index, int[] next_nodes)
        {
            this.position = position;
            this.path_id = path_id;
            this.map_index = map_index;
            this.next_nodes = next_nodes;

            exists = true;
        }


        internal Vector NextPosition(byte d)
        {
            switch (d)
            {
                case 0: return new Vector(this.position.x - 1, position.y);
                case 1: return new Vector(this.position.x + 1, position.y);
                case 2: return new Vector(this.position.x, position.y + 1);
                case 3: return new Vector(this.position.x, position.y - 1);
            }
            return position;
        }




        /// <summary>
        /// connects the given node to this node
        /// </summary>
        /// <param name="node"></param>
        internal void Connect(MapNode node)
        {
            switch (GetDirection(node))
            {
                case 0: // LEFT
                    {
                        next_nodes[3] = node.map_index;
                        node.next_nodes[1] = this.map_index;
                        break;
                    }
                case 1: // RIGHT
                    {
                        next_nodes[1] = node.map_index;
                        node.next_nodes[3] = this.map_index;
                        break;
                    }
                case 2: // DOWN
                    {
                        next_nodes[2] = node.map_index;
                        node.next_nodes[0] = this.map_index;
                        break;
                    }
                case 3: // UP
                    {
                        next_nodes[0] = node.map_index;
                        node.next_nodes[2] = this.map_index;
                        break;
                    }
            }
        }



        /// <summary>
        /// gets the direction the other node is in from this node
        /// </summary>
        /// <param name="node"></param>
        private byte GetDirection(MapNode node)
        {
            if (node.position.x < this.position.x) // WEST
                return 0;
            else if (node.position.x > this.position.x) // EAST
                return 1;
            else if (node.position.y < this.position.y) // SOUTH
                return 2;
            else if (node.position.y > this.position.y) // NORTH
                return 3;
            return 0;
        }


        /// <summary>
        /// determines if the given direction is closed or not
        /// </summary>
        /// <param name="direction_flag"></param>
        /// <returns></returns>
        public bool IsClosed(byte direction_flag)
        {
            switch (direction_flag)
            {
                case 0: return next_nodes[3] == -1; // left
                case 1: return next_nodes[1] == -1; // right
                case 2: return next_nodes[0] == -1; // up
                case 3: return next_nodes[2] == -1; // down
            }
            return true;
        }


        /// <summary>
        /// Gets the directions that the node hasn't opened
        /// </summary>
        /// <returns></returns>
        internal List<byte> GetClosedDirections(bool exclude_south = false)
        {
            List<byte> result = new List<byte>();
            for (byte d = 0; d < 4; d++)
            {
                if (exclude_south && d == 3) continue;
                if (this.IsClosed(d)) result.Add(d);
            }
            return result;
        }


        /// <summary>
        /// clones the node
        /// </summary>
        /// <returns></returns>
        internal MapNode Clone(byte? path_override = null)
        {
            if (path_override.HasValue) path_id = path_override.Value;
            return new MapNode(position, path_id, map_index, next_nodes);
        }

        public override string ToString()
        {
            return $"{{{position.x}, {position.y}}}";
        }
    }
}
