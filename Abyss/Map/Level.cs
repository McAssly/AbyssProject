using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private Map[,] maps;
        private string tileset;
        private TileMap[,] Maps;
        private int width, height;

        // The current tile map to draw and read from
        private TileMap Current;

        /**
         * Constructs a map manager
         * 
         * @param   TileMap     the current tile map to be read
         */
        public Level(Map[,] _Maps, string tileset)
        {
            this.width = _Maps.GetLength(0);
            this.height = _Maps.GetLength(1);
            this.maps = _Maps;
            this.tileset = tileset;
        }

        public void LoadLevel(ContentManager Content, int current_x, int current_y)
        {
            Texture2D _tileset = Content.Load<Texture2D>(tileset);
            Maps = new TileMap[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Maps[i, j] = new TileMap(maps[i, j], _tileset);
                }
            }

            Current = Maps[current_x, current_y];
        }

        public TileMap[,] GetMaps() { return this.Maps; }

        /**
         * Gets the current tile map
         */
        public TileMap GetCurrent()
        {
            return Current;
        }

        /**
         * Changes the current tile map to the given
         * 
         * @param   TileMap     which map to change to
         */
        public void SetCurrent(TileMap _new)
        {
            Current = _new;
        }

        /** Gets the level's maximum position
         * within the window
         */
        public Vector2 Max()
        {
            return Current.getMax();
        }

        /** Gets the level's minimum position
         * Not 0, because its within the window
         * 
         */
        public Vector2 Min()
        {
            return Current.getMin();
        }
    }
}
