using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.Map
{
    internal struct SideDistance
    {
        public float dist;
        public SIDE side;

        public SideDistance(float dist, SIDE side)
        {
            this.dist = dist;
            this.side = side;
        }
    }

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
        public List<SIDE> ignores = new List<SIDE>();
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
         * Determines which sides the tile should ignore in collision mapping
         */
        public void SidesToIgnore(MapLayer layer)
        {
            // initialize a list of sides to ignore
            List<SIDE> ignores = new List<SIDE>();

            // get the current map position of this tile
            Vector2 mapPosition = MathUtil.CoordsToTileCoords(pos);

            // get the adjacent tile positions
            Vector2 Left = mapPosition - new Vector2(1, 0);
            Vector2 Right = mapPosition - new Vector2(-1, 0);
            Vector2 Top = mapPosition - new Vector2(0, 1);
            Vector2 Bottom = mapPosition - new Vector2(0, -1);

            // LEFT SIDE
            if (IgnoreSide(Left.X, Left, 0, layer, true))
                ignores.Add(SIDE.LEFT);

            // RIGHT SIDE
            if (IgnoreSide(Right.X, Right, layer.GetWidth()-1, layer, false))
                ignores.Add(SIDE.RIGHT);

            // TOP SIDE
            if (IgnoreSide(Top.Y, Top, 0, layer, true))
                ignores.Add(SIDE.TOP);

            // BOTTOM SIDE
            if (IgnoreSide(Bottom.Y, Bottom, layer.GetWidth()-1, layer, false))
                ignores.Add(SIDE.BOTTOM);

            this.ignores = ignores;
        }

        /** 
         * Determines whether the given side should be ignored
         */
        private static bool IgnoreSide(float axis_position, Vector2 pos, float bound, MapLayer layer, bool isLowerBound)
        {
            if (isLowerBound)
            {
                if (axis_position < bound) // its out of bounds so therefore it needs to be ignored
                    return true;
                else if (!layer.GetTiles()[(int)pos.Y, (int)pos.X].NULL) // even though these have the same result we do not want an outta bounds error from indexing
                    return true;
            }
            else // same here just changing how we interact with our bounds
            {
                if (axis_position > bound)
                    return true;
                else if (!layer.GetTiles()[(int)pos.Y, (int)pos.X].NULL)
                    return true;
            }
            // if no checks passed then the tile shouldn't be ignored
            return false;
        }

        /**
         * Determines which side the given position is closest to
         * 
         * @param   Vector2     the position of the obj
         * @param   Vector2     the size of the obj
         */
        public SIDE ClosestSide(Vector2 pos, Vector2 size, TileMap map)
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

            // 3rd: get the distances between each side but only add them if they aren't ignored
            List<SideDistance> dist = new List<SideDistance>()
            {
                new SideDistance(Math.Abs(left - p3.X), SIDE.LEFT),
                new SideDistance(Math.Abs(right - p0.X), SIDE.RIGHT),
                new SideDistance(Math.Abs(top - p3.Y), SIDE.TOP),
                new SideDistance(Math.Abs(bottom - p0.Y), SIDE.BOTTOM)
            };

            // 4th: find the smallest distance
            SideDistance min = dist[0]; // grab the first distance (right side)
            for (int i=1; i<dist.Count; i++) // loop through to find the min
            { // we don't need to start at 0 since we already yoinked that one
                if (dist[i].dist < min.dist)
                { // if the current dist is greater than update our minimum distance
                    min = dist[i];
                }
            }

            // 5th: find and return the correct side:
            for (int i=0; i<dist.Count; i++) // loop through each distance again
            {
                if (dist[i].dist == min.dist)
                { // return the corresponding enum
                    return dist[i].side;
                }
            }
            return SIDE.LEFT; // return the left side as a default: why not what could go wrong right?.....
        }
    }
}
