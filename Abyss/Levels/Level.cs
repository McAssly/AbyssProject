using Abyss.Entities;
using Abyss.Levels.data;
using Abyss.Utility;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Abyss.Levels
{
    internal class Level
    {
        // the levels connected to it
        internal Vector[] map_connections; // x: prev_level, y: next_map


        // tile maps
        private protected Map[] maps;
        private protected readonly string tileset_location;
        private protected TileMap[] tilemaps;
        private int length;

        private int map_index;
        private List<Enemy> current_enemies;

        public Level(TileMap[] tilemaps, string tileset_location)
        {
            this.tileset_location = tileset_location;
            this.length = tilemaps.Length;
            this.tilemaps = tilemaps;
        }

        /// <summary>
        /// Loads a map and its content
        /// </summary>
        /// <param name="Content"></param>
        public void Load(ContentManager Content)
        {
            Texture2D _tileset = Content.Load<Texture2D>(this.tileset_location);
            maps = new Map[length];
            // generate the level from the tile map data
            for (int i = 0; i < length; i++)
            {
                maps[i] = new Map(tilemaps[i], _tileset);
                foreach (Enemy enemy in maps[i].GetEnemies(false))
                    enemy.Load();
            }

            // by default, gets updated via save data
            map_index = 0;
            current_enemies = maps[map_index].GetEnemies().ToList();
        }


        /// <summary>
        /// returns the map array
        /// </summary>
        /// <returns></returns>
        public Map[] GetMaps() { return maps; }

        /// <summary>
        /// gets the current map
        /// </summary>
        /// <returns></returns>
        public Map GetCurrent() { return maps[map_index]; }

        /// <summary>
        /// returns the list of enemies
        /// </summary>
        /// <returns></returns>
        public List<Enemy> GetEnemies() { return current_enemies; }


        /// <summary>
        /// based on the given index update the current map
        /// </summary>
        /// <param name="index"></param>
        public void SetCurrent(int index)
        {
            int prev = map_index;
            if (prev != index)
            {
                map_index = index;
                ResetEnemies();
            }
        }


        /// <summary>
        /// Resets all the enemies within the current map
        /// </summary>
        public void ResetEnemies()
        {
            current_enemies = maps[map_index].GetEnemies().ToList();
        }

        /// <summary>
        /// returns the current map index
        /// </summary>
        /// <returns></returns>
        public int GetIndex()
        {
            return map_index;
        }


        public int GetMapConnection(int prev_level)
        {
            foreach (Vector c in map_connections)
            {
                if (c.x == prev_level) return c.y;
            }
            return -1;
        }



        public static int ConvertToLevelIndex(int map_index)
        {
            return -(map_index + 2);
        }
    }
}
