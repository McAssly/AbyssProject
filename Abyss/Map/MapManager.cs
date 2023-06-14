using Microsoft.Xna.Framework;
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
    internal class MapManager
    {
        // TileMaps to cycle between
        public static TileMap TestMap;

        // The current tile map to draw and read from
        private TileMap Current;

        /**
         * Constructs a map manager
         * 
         * @param   TileMap     the current tile map to be read
         */
        public MapManager(TileMap current)
        {
            Current = current;
        }

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
