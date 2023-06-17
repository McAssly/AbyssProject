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

            // 2nd: Get the four bounds of this tiled
            float left = this.pos.X;
            float right = this.pos.X + Globals.TILE_SIZE;
            float top = this.pos.Y;
            float bottom = this.pos.Y + Globals.TILE_SIZE;

            return MathUtil.IsWithin(p0, left, right, top, bottom)
                || MathUtil.IsWithin(p1, left, right, top, bottom)
                || MathUtil.IsWithin(p2, left, right, top, bottom)
                || MathUtil.IsWithin(p3, left, right, top, bottom);
        }

        /**
         * Determines which side the given position is closest to
         * 
         * @param   Vector2     the position of the obj
         * @param   Vector2     the size of the obj
         */
        public SIDES ClosestSide(Vector2 pos, Vector2 size)
        {
            // 1st: Get two corners of the target object
            Vector2 p0 = pos;                           // TOP LEFT CORNER
            Vector2 p3 = pos + size;                    // BOTTOM RIGHT

            // now the corner labels clearly show which side it should be closest to, IE top left would be close to either the bottom or the right side
            // etc. etc.

            // So if we have p0, that corner should be checking the distance between p0 and the bottom side and the right side
            // etc. etc.

            // 2nd: Get the four bounds of this tile
            float left = this.pos.X;
            float right = this.pos.X + Globals.TILE_SIZE;
            float top = this.pos.Y;
            float bottom = this.pos.Y + Globals.TILE_SIZE;

            // 3rd: get the distances between each side
            float[] dist = new float[4]
            { // THE ORDER HERE MATTERS AS IN THE ENUM (see the enum on MathUtil.cs in global)
                Math.Abs(left - p3.X),      // distance between the left side
                Math.Abs(right - p0.X),     // distance between the right side
                Math.Abs(top - p3.Y),       // distance between the top side
                Math.Abs(bottom - p0.Y)     // distance between the bottom side
            };

            // 4th: find the largest distance
            float max = dist[0]; // grab the first distance (right side)
            for (int i=1; i<4; i++) // loop through to find the max
            { // we don't need to start at 0 since we already yoinked that one
                if (dist[i] > max)
                { // if the current dist is greater than update our maximum distance
                    max = dist[i];
                }
            }

            // 5th: find and return the correct side:
            for (int i=0; i<4; i++) // loop through each distance again
            {
                if (dist[i] == max)
                { // return the corresponding enum
                    return (SIDES)Enum.ToObject(typeof(SIDES), i);
                }
            }
            return SIDES.LEFT; // return the left side as a default: why not what could go wrong right?.....
        }
    }
}
