using Abyss.Entities;
using Abyss.Master;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.Map
{
    /**
     * A class for managing the current map, and other maps for a level or area
     * 
     */
    internal class Level
    {
        // TileMaps to cycle between
        private Map[] maps;
        private string tileset;
        private TileMap[] Maps;
        private int length;

        // The current tile map to draw and read from
        private TileMap current;
        private List<Entity> current_entities;

        /**
         * Constructs a map manager
         * 
         * @param   TileMap     the current tile map to be read
         */
        public Level(Map[] maps, string tileset)
        {
            this.length = maps.Length;
            this.maps = maps;
            this.tileset = tileset;
        }

        public void LoadLevel(ContentManager Content, int current_index)
        {
            Texture2D _tileset = Content.Load<Texture2D>(tileset);
            Maps = new TileMap[length];
            for (int i = 0; i < length; i++)
            {
                Maps[i] = new TileMap(maps[i], _tileset);
            }

            current = Maps[current_index];
            current_entities = Maps[current_index].GetEntities().ToList();
        }

        public TileMap[] GetMaps() { return this.Maps; }

        /**
         * Gets the current tile map
         */
        public TileMap GetCurrent()
        {
            return current;
        }

        public List<Entity> GetEntities()
        {
            return this.current_entities;
        }

        /**
         * Changes the current tile map to the given
         * 
         * @param   TileMap     which map to change to
         */
        public void SetCurrent(TileMap _new)
        {
            current = _new;
            current_entities = _new.GetEntities().ToList();
        }

        public void SetCurrent(int index)
        {
            current = Maps[index];
            current_entities = Maps[index].GetEntities().ToList();
        }

        public int GetNextMap(Side side)
        {
            switch(side)
            {
                case Side.LEFT: return 3;
                case Side.RIGHT: return 1;
                case Side.TOP: return 0;
                case Side.BOTTOM: return 2;
            }
            return -1;
        }

        public int GetIndex()
        {
            return Array.IndexOf(Maps, current);
        }

        /** Gets the level's maximum position
         * within the window
         */
        public Vector2 Max()
        {
            return current.getMax();
        }

        /** Gets the level's minimum position
         * Not 0, because its within the window
         * 
         */
        public Vector2 Min()
        {
            return current.getMin();
        }
    }
}
