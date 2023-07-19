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
        // Represents the assortment of tiles within different layers (lower the layer the higher load priority)
        public Layer[] tile_layers;
        public Layer collision_layer;
        // the map to go to based on the direction given, 0 = north, 1 = east, 2 = south, 3 = west
        public int[] index_locations;

        /**
         * Constructs a Map structure for basic storage of levels (might be a bit slow)
         * 
         * @param   uint            Id for the level
         * @param   uint[,,]        3D array of tileIds, (1st element is the layer id)
         */
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

    internal class TileMap
    {
        // The tileset layers to be loaded
        private List<MapLayer> layers = new List<MapLayer>();
        // Set the collision layer
        private MapLayer collision_layer;
        // The width and height of the map (default: 1x1)
        private int width, height = 1;
        // map directions
        private int[] next_maps;

        /**
         * TileMap constructor loads a given map into a usable tilemap object
         * 
         * @param   Map     takes the given level (map) to be loaded
         */
        public TileMap(Map map, Texture2D tileset)
        {
            // Next construct the maplayer array
            for (uint i = 0; i < map.tile_layers.Length; i++)
            {
                layers.Add(new MapLayer(map.tile_layers[i], tileset));
            }

            // set the collisoon layer
            collision_layer = new MapLayer(map.collision_layer);

            // set the next map indicies
            this.next_maps = map.index_locations;

            // Next we need the width and height of the tilemap
            // first grab any layer doesn't matter and grab their respective width/height
            this.width = this.layers[0].GetWidth();
            this.height = this.layers[0].GetHeight();
        }

        /**
         * Draws the map to the screen
         * 
         * @param   SpriteBatch     the sprite batch to draw to
         * @param   Vector2         the position to map the draw to
         */
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (MapLayer layer in layers)
            {
                layer.Draw(spriteBatch);
            }
        }

        public MapLayer GetCollisionLayer() { return this.collision_layer; }

        public int[] GetNext() { return this.next_maps; }
        public int GetWidth() { return width; }
        public int GetHeight() { return height; }
        public List<MapLayer> GetLayers()
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
