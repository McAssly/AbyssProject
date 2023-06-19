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
        public readonly Rectangle rect;
        public readonly Vector2 pos;
        public readonly bool NULL;
        public List<SIDE> ignores = new List<SIDE>();
        public Tile(bool NULL, Vector2 pos = new Vector2(), Rectangle rect = new Rectangle())
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
         * @param   Vector2     the position
         */
        public SideDistance ClosestSide(Vector2 pos)
        {
            List<Vector2> SidePositions = this.SidePositions();
            List<SideDistance> distances = new List<SideDistance>();
            for (int i = 0; i < SidePositions.Count(); i++) {
                SIDE side = (SIDE)Enum.ToObject(typeof(SIDE), i);
                if (!ignores.Contains(side))
                    distances.Add(new SideDistance((pos - SidePositions[i]).Length(), side));
            }

            // in case there are no sides throw a new exception
            if (distances.Count == 0)
                return new SideDistance();

            // get the minimum distance, and return its side
            SideDistance minSide = distances[0];
            foreach (SideDistance sd in distances)
                if (minSide.dist > sd.dist)
                    minSide = sd;

            return minSide;
        }

        /**
         * Returns what side the given position is colliding with (assuming full tile size)
         */
        public SIDE CollisionSide(Vector2 pos) 
        {
            List<Vector2> sidePos = this.SidePositions();
            List<SideDistance> distances = new List<SideDistance>();
            for (int i=0; i<8; i++)
            {
                Vector2 offset_pos = pos + MathUtil.offsets[i];
                distances.Add(ClosestSide(offset_pos));
            }

            SideDistance minSide = distances[0];
            foreach (SideDistance sd in distances)
                if (minSide.dist > sd.dist)
                    minSide = sd;

            return minSide.side;
        }

        /** returns all the positions of each side of the tile
         */
        private List<Vector2> SidePositions()
        {
            // Initialize a list of side positions (order: 0->L, 1->R, 2->T, 3->B)
            List<Vector2> sidePos = new List<Vector2>();
            // calculate and add each side at their respective offsets
            for (int i = 4; i < 8; i++)
                sidePos.Add(this.pos + MathUtil.offsets[i]);

            return sidePos;
        }
    }
}
