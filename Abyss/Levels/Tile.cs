using Abyss.Entities;
using Abyss.Utility;
using Microsoft.Xna.Framework;

namespace Abyss.Levels
{
    internal struct Tile
    {
        public readonly Rectangle rectangle;
        public readonly Vector2 position;
        public readonly bool NULL;

        public Tile(bool NULL, Vector2 position = new Vector2(), Rectangle rectangle = new Rectangle())
        {
            this.rectangle = rectangle;
            this.position = position;
            this.NULL = NULL;
        }

        /// <summary>
        /// determines if the given entity is colliding with the tile
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="position_offset"></param>
        /// <returns></returns>
        public bool Colliding(Entity entity, Vector2 position_offset)
        {
            Vector2 target_position = entity.GetPosition() + position_offset;
            return Math0.RectangleCollisionCheck(this.position, new Vector2(16, 16), target_position, entity.GetSize());
        }
    }
}
