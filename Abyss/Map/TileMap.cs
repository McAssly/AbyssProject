using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.Map
{
    /// <summary>
    /// stores map segments for level data
    /// </summary>
    internal struct Map
    {
        // Represents the assortment of tiles within different layers (lower the layer the higher load priority)
        public Layer[] tile_layers;
        public Layer collision_layer;
        // the map to go to based on the direction given, 0 = north, 1 = east, 2 = south, 3 = west
        public int[] index_locations;

        /// <summary>
        /// constructs the map segement with its associated data
        /// </summary>
        /// <param name="tile_layers"></param>
        /// <param name="collision_layer"></param>
        /// <param name="z_index"></param>
        /// <param name="north_index"></param>
        /// <param name="east_index"></param>
        /// <param name="south_index"></param>
        /// <param name="west_index"></param>
        public Map(Layer[] tile_layers, Layer collision_layer, int north_index = -1, int east_index = -1, int south_index = -1, int west_index = -1)
        {
            this.tile_layers = tile_layers;
            this.collision_layer = collision_layer;
            this.index_locations = new int[4]
            {
                north_index, east_index, south_index, west_index
            };
        }
    }

    /// <summary>
    /// The tile map used for drawing and collision
    /// </summary>
    internal class TileMap
    {
        // The tileset layers to be loaded
        private MapLayer[] layers = new MapLayer[16];
        // Set the collision layer
        private MapLayer collision_layer;
        // The width and height of the map (default: 1x1)
        private int width, height = 1;
        // map directions
        private int[] next_maps;

        /// <summary>
        /// constructs the tile map
        /// </summary>
        /// <param name="map"></param>
        /// <param name="tileset"></param>
        public TileMap(Map map, Texture2D tileset)
        {
            List<MapLayer> map_layers = new List<MapLayer>();
            // Next construct the maplayer array
            for (uint i = 0; i < map.tile_layers.Length; i++)
            {
                map_layers.Add(new MapLayer(map.tile_layers[i], tileset));
            }
            layers = map_layers.ToArray();

            // set the collisoon layer
            collision_layer = new MapLayer(map.collision_layer);

            // set the next map indicies
            this.next_maps = map.index_locations;

            // Next we need the width and height of the tilemap
            // first grab any layer doesn't matter and grab their respective width/height
            this.width = this.layers[0].GetWidth();
            this.height = this.layers[0].GetHeight();
        }

        public MapLayer GetCollisionLayer() { return this.collision_layer; }

        public int[] GetNext() { return this.next_maps; }
        public int GetWidth() { return width; }
        public int GetHeight() { return height; }
        public MapLayer[] GetLayers()
        {
            return layers;
        }

        public Vector2 getMax()
        {
            return new Vector2(width * Globals.TILE_SIZE - Globals.TILE_SIZE, height * Globals.TILE_SIZE - Globals.TILE_SIZE) + getMin();
        }

        public Vector2 getMin()
        {
            return new Vector2(Globals.WindowW / width, Globals.WindowH / height);
        }
    }
}
