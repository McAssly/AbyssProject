using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.Map
{
    /**
     * Stores a basic tile for drawing
     * 
     * this tile does not however directly cover collisions, that is covered by the layer class
     */
    internal struct Tile
    {
        public Rectangle rect;
        public bool NULL;
        public Tile(Rectangle Rect, bool NULL)
        {
            this.rect = Rect;
            this.NULL = NULL;
        }
    }
}
