using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.Levels.data
{
    /// <summary>
    /// Layer data storage. Acts as a single tile layer storing very basic tile information. 
    /// The tiles array is a grid of the tiles within the layer representing the index of which tile to use
    /// blocked determines if said layer has collision or not
    /// </summary>
    internal struct TileLayer
    {
        public uint[,] tiles;

        
        public TileLayer(uint[,] tiles)
        {
            this.tiles = tiles;
        }
    }
}
