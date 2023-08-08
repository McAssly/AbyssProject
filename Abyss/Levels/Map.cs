using Abyss.Entities;
using Abyss.Globals;
using Abyss.Levels.data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.Levels
{
    internal class Map
    {
        // The tileset layers to be loaded
        private Layer[] layers = new Layer[16];
        // Set the collision layer
        private Layer collision_layer;
        // The width and height of the map (default: 1x1)
        private int width, height = 1;
        // map directions
        private int[] next_maps;
        // the entities on the map
        private protected readonly Enemy[] enemies;

        /// <summary>
        /// constructs the tile map
        /// </summary>
        /// <param name="map"></param>
        /// <param name="tileset"></param>
        public Map(TileMap map, Texture2D tileset)
        {
            List<Layer> map_layers = new List<Layer>();
            // Next construct the maplayer array
            for (uint i = 0; i < map.tile_layers.Length; i++)
            {
                map_layers.Add(new Layer(map.tile_layers[i], tileset));
            }
            layers = map_layers.ToArray();

            // set the collisoon layer
            collision_layer = new Layer(map.collision_layer);

            // set the next map indicies
            this.next_maps = map.index_locations;

            // set the entities
            this.enemies = (Enemy[])map.enemies.Clone();

            // Next we need the width and height of the tilemap
            // first grab any layer doesn't matter and grab their respective width/height
            this.width = this.layers[0].GetWidth();
            this.height = this.layers[0].GetHeight();
        }

        public Enemy[] GetEnemies(bool clone = true)
        {
            if (clone)
            {
                List<Enemy> enemies = new List<Enemy>();
                foreach (Enemy e in this.enemies)
                {
                    enemies.Add(e.Clone());
                }
                return enemies.ToArray();
            }
            return enemies.ToArray();
        }

        public Layer GetCollisionLayer() { return this.collision_layer; }

        public int[] GetNext() { return this.next_maps; }
        public int GetWidth() { return width; }
        public int GetHeight() { return height; }
        public Layer[] GetLayers()
        {
            return layers;
        }

        public Vector2 getMax()
        {
            return new Vector2(width * 16 - 16, height * 16 - 16) + getMin();
        }

        public Vector2 getMin()
        {
            return new Vector2(Variables.WindowW / width, Variables.WindowH / height);
        }
    }
}
