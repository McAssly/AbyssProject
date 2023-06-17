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
        public Vector2 pos;
        public bool NULL;
        public Tile(Rectangle rect, Vector2 pos, bool NULL)
        {
            this.rect = rect;
            this.pos = pos;
            this.NULL = NULL;
        }

        /**
         * Determines if the tile is colliding with the given object
         * 
         * @param   Vector2     the position of said object
         * @param   Vector2     the size of said object
         */
        public bool Colliding(Vector2 pos, Vector2 size)
        {
            // 1st: Get the four corners of the target object
            Vector2 p0 = pos;                           // TOP LEFT CORNER
            Vector2 p1 = pos + new Vector2(size.X, 0);  // TOP RIGHT
            Vector2 p2 = pos + new Vector2(0, size.Y);  // BOTTOM LEFT
            Vector2 p3 = pos + size;                    // BOTTOM RIGHT

            // 2nd: Get the four bounds of this tile
            float left = this.pos.X;
            float right = this.pos.X + Globals.TILE_SIZE;
            float top = this.pos.Y;
            float bottom = this.pos.Y + Globals.TILE_SIZE;

            return MathUtil.IsWithin(p0, left, right, top, bottom)
                || MathUtil.IsWithin(p1, left, right, top, bottom)
                || MathUtil.IsWithin(p2, left, right, top, bottom)
                || MathUtil.IsWithin(p3, left, right, top, bottom);
        }
    }
}
