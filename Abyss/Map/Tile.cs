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
        public bool Colliding(Entity entity, Vector2 offset)
        {
            Vector2 target_pos = entity.GetPosition() + offset;
            return MathUtil.RectangleCollisionCheck(this.pos, new Vector2(16,16), target_pos, entity.GetSize());
        }
    }
}
