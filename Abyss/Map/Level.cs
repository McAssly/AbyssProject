using Abyss.Master;
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
        private Map[] maps;
        private string tileset;
        private TileMap[] Maps;
        private int length;

        // The current tile map to draw and read from
        private TileMap Current;

        /**
         * Constructs a map manager
         * 
         * @param   TileMap     the current tile map to be read
         */
        public Level(Map[] _Maps, string tileset)
        {
            this.length = _Maps.Length;
            this.maps = _Maps;
            this.tileset = tileset;
        }

        public void LoadLevel(ContentManager Content, int currentIndex)
        {
            Texture2D _tileset = Content.Load<Texture2D>(tileset);
            Maps = new TileMap[length];
            for (int i = 0; i < length; i++)
            {
                Maps[i] = new TileMap(maps[i], _tileset);
            }

            Current = Maps[currentIndex];
        }

        public TileMap[] GetMaps() { return this.Maps; }

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

        public void SetCurrent(int index)
        {
            Current = Maps[index];
        }

        public void SetCurrent(Side? side)
        {
            switch (side.Value)
            {
                case Side.LEFT: Current = Maps[Current.GetNext()[3]]; break;
                case Side.RIGHT: Current = Maps[Current.GetNext()[1]]; break;
                case Side.TOP: Current = Maps[Current.GetNext()[0]]; break;
                case Side.BOTTOM: Current = Maps[Current.GetNext()[2]]; break;
            }
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
