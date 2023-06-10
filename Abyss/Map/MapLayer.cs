using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.Map
{
    /**
     * Layer
     * 
     * For storage of levels
     * Acts as a single tile layer storing very basic tile information
     * 
     * the tiles array is a grid of the tiles within the layer representing the index of which tile to use
     * blocked determines if said layer has collision or not
     */
    internal struct Layer
    {
        public bool blocked;
        public uint[,] tiles;

        /**
         * Constructor for the layer structure
         * 
         * @param   bool    determines if the layer has collision or not
         * @param   uint[,] the grid of tile indicies
         */
        public Layer(bool blocked, uint[,] tiles)
        {
            this.blocked = blocked;
            this.tiles = tiles;
        }
    }

    /**
     * More advanced implementation of a layer
     * 
     */
    internal class MapLayer
    {
        // determines if the layer has collision or not
        private bool blocked;

        // tileset for this map layer to draw to
        private Texture2D tileset;

        // the list of tile textures at their corresponding index
        private List<Rectangle> tileTexture;

        // Main tile map
        private Tile[,] tiles;

        /** 
         * Constructor for a maplayer
         * 
         * @param   Layer       takes in a basic layer and converts it to maplayer
         * @param   Texture2D   takes in a tileset for this map layer to draw to
         */
        public MapLayer(Layer layer, Texture2D tileset) 
        {
            // First create the tileTexture array
            this.tileset = tileset;
            this.tileTexture = new List<Rectangle>();
            // loop through the each tile within the tileset and place them in order within the list
            for (int y = 0; y < this.tileset.Height / Globals.TILE_SIZE; y++)
            {
                for (int x = 0; x < this.tileset.Width / Globals.TILE_SIZE; x++)
                {
                    /**
                     * The rectangle constructor first takes in position (x,y)
                     * Then it takes in the size (w, h)
                     * 
                     * In this case the tile is positioned @ x * its width, y * height
                     * And its width and height are the global tile size (16px)
                     */
                    tileTexture.Add(new Rectangle(x * Globals.TILE_SIZE, y * Globals.TILE_SIZE, Globals.TILE_SIZE, Globals.TILE_SIZE));
                }
            }
        }
    }
}
