using Abyss.Levels.data;
using Abyss.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.Levels
{
    internal class Layer
    {
        // tileset for this map layer to draw to
        private Texture2D tileset;

        // the list of tile textures at their corresponding index
        private Rectangle[] tile_sprite_map;

        // Main tile map
        private Tile[,] tiles = new Tile[16, 16];

        // Size (width, height)
        private int width, height = 1;

        
        /// <summary>
        /// constructs the layer, if given no tileset its therefore a non-draw layer
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="tileset"></param>
        public Layer(TileLayer layer, Texture2D tileset = null)
        {
            if (tileset != null)
            {
                this.tileset = tileset;
                this.tile_sprite_map = Sprite.GenerateSpriteMapping(tileset, 16, 16, 16, 16);
            }

            // get the width/height
            width = layer.tiles.GetLength(0);
            height = layer.tiles.GetLength(1);

            // Lastly set the actual tile map
            GenerateMapLayer(layer, tileset == null);
        }

        /** generates the layer mapping
         */
        private void GenerateMapLayer(TileLayer layer, bool ignored)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    // Get the tile index from the layer of tiles
                    int tile_index = (int)layer.tiles[x, y] - 1;
                    // if the tile index is valid insert the right tile
                    if (tile_index >= 0)
                    {
                        if (ignored) tiles[x, y] = new Tile(false, new Vector2(y * 16, x * 16));
                        else tiles[x, y] = new Tile(false, new Vector2(y * 16, x * 16), tile_sprite_map[tile_index]);
                    }
                    // otherwise the tile will be empty
                    else
                    {
                        tiles[x, y] = new Tile(true);
                    }
                }
            }
        }

        
        /// <summary>
        /// returns the width of the layer
        /// </summary>
        /// <returns></returns>
        public int GetWidth()
        {
            return width;
        }

        /// <summary>
        /// returns the height of the layer
        /// </summary>
        /// <returns></returns>
        public int GetHeight()
        {
            return height;
        }

        /// <summary>
        /// returns the tile grid
        /// </summary>
        /// <returns></returns>
        public Tile[,] GetTiles()
        {
            return tiles;
        }

        /// <summary>
        /// returns the tileset that it draws to
        /// </summary>
        /// <returns></returns>
        public Texture2D GetTileset()
        {
            return tileset;
        }
    }
}
