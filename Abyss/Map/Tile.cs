using Abyss.Entities;
using Abyss.Entities.Magic;
using Abyss.Master;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.Map
{
    /// <summary>
    /// Stores basic tile data for drawing and collision mapping
    /// </summary>
    internal struct Tile
    {
        public readonly Rectangle rect;
        public readonly Vector2 pos;
        public readonly bool NULL;
        public Tile(bool NULL, Vector2 pos = new Vector2(), Rectangle rect = new Rectangle())
        {
            this.rect = rect;
            this.pos = pos;
            this.NULL = NULL;
        }

        /// <summary>
        /// determines if the given entity's target position is colliding with the tile
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Colliding(Entity entity)
        {
            Vector2 target_pos = entity.GetPosition() + entity.GetVelocity();
            // 1st: Get the four corners of the target object
            Vector2 p0 = target_pos;                                                          // TOP LEFT CORNER
            Vector2 p1 = target_pos + new Vector2(entity.GetWidth(), 0);                      // TOP RIGHT
            Vector2 p2 = target_pos + new Vector2(0, entity.GetHeight());                     // BOTTOM LEFT
            Vector2 p3 = target_pos + new Vector2(entity.GetWidth(), entity.GetHeight());     // BOTTOM RIGHT

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


        public bool Colliding(Particle particle)
        {
            return MathUtil.IsWithin(particle.position, pos.X, pos.X + Globals.TILE_SIZE, pos.Y, pos.Y + Globals.TILE_SIZE);
        }
    }
}
