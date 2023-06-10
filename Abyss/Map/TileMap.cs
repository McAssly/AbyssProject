using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.Map
{
    /**
     * This strcture is intended for storing map segments, or full map levels
     * 
     */
    internal struct Map
    {
        // Represents the ID for the map or level to be loaded
        public uint Id;
        // Reperesents the sprite map (or tileset) to be used graphically
        public Texture2D TileSet;
        // Represents the assortment of tiles within different layers (lower the layer the higher load priority)
        public Layer[] TileLayers;

        /**
         * Constructs a Map structure for basic storage of levels (might be a bit slow)
         * 
         * @param   uint            Id for the level
         * @param   Texture2D       Tileset for the level (Can be null)
         * @param   uint[,,]        3D array of tileIds, (1st element is the layer id)
         */
        public Map(uint id, Texture2D tileSet , Layer[] tileLayers)
        {
            Id = id;
            TileSet = tileSet;
            TileLayers = tileLayers;
        }
    }

    internal class TileMap
    {
        // the tile map Id for referencing required tile entities to be loaded
        public uint Id;

        // The tileset layers to be loaded
        private List<MapLayer> layers = new List<MapLayer>();
        // The width and height of the map (default: 1x1)
        private uint width, height = 1;

        /**
         * TileMap constructor loads a given map into a usable tilemap object
         * 
         * @param   Map     takes the given level (map) to be loaded
         */
        public TileMap(Map map)
        { 
            // Start by setting the appropriate instance variables
            this.Id = map.Id;

            // Next construct the maplayer array
            for (uint i = 0; i < map.TileLayers.Length; i++)
            {
                layers.Add(new MapLayer(map.TileLayers[i], map.TileSet));
            }
        }
    }
}
