using Abyss.Globals;
using Abyss.Levels;
using Abyss.Magic;
using Abyss.Sprites;
using Abyss.Utility;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System.Collections.Generic;

namespace Abyss.Entities
{
    internal class Entity
    {
        public SpriteSheet sprite;
        private protected int width, height;

        // declare max values for the entity
        private protected readonly double max_speed = 2;
        private protected readonly double max_accel = 10;
        private protected readonly double friction = 25;

        // declare the entity's stats
        private protected double speed = 1;
        private protected double max_health;

        // current stats
        private protected double health;

        // declare the entity's substats
        private protected double damage;
        private protected double defense;

        // declare position variables
        private protected Vector2 target_vector = Vector2.Zero;
        private protected Vector2 position;
        private protected Vector2 velocity = Vector2.Zero; // starts off not moving

        // declare the status effects to be applied
        public List<StatusEffect> statuses;

        // time elapsed
        private protected double time_elapsed = 0;
        public double regen_timer = 0;

        // last damage applied
        public double last_damage = 0;

        public Entity(SpriteSheet sprite, float x, float y)
        {
            this.sprite = sprite;
            this.width = sprite.width - 1;
            this.height = sprite.height - 1;
            this.position = new Vector2(x, y);
        }

        public Entity(float x, float y)
        {
            this.position = new Vector2(x, y);
        }

        public Entity(SpriteSheet sprite)
        {
            this.statuses = new List<StatusEffect>();
            this.sprite = sprite;
            this.width = this.sprite.width - 1;
            this.height = this.sprite.height - 1;
        }

        public virtual void Load()
        {
            this.sprite = _Sprites.TestBox;
            this.width = this.sprite.width - 1;
            this.height = this.sprite.height - 1;
        }


        /// <summary>
        /// moves the entity based on its target vector
        /// </summary>
        /// <param name="collision_layer"></param>
        /// <param name="delta"></param>
        public virtual void Move(Layer collision_layer, double delta)
        {
            // if the movement vector is not zero then the entity must be trying to move
            if (target_vector != Vector2.Zero)
            {
                Vector2 target = target_vector * (float)max_speed * (float)speed;
                velocity = Math0.MoveToward(velocity, target, max_accel * delta);
            }
            else velocity = Math0.MoveToward(velocity, Vector2.Zero, friction * delta);

            Vector2 max_velocity = velocity.NormalizedCopy() * (float)max_speed * (float)speed;
            velocity = Vector2.Clamp(velocity, -Math0.Absolute(max_velocity), Math0.Absolute(max_velocity));

            //velocity = velocity_temp;
            // handle collision, if they are about to collide we must alter our velocity before we move forward
            if (velocity.X != 0 && this.CollisionCheck(collision_layer, new Vector2(velocity.X, 0)))
                velocity.X = 0;
            if (velocity.Y != 0 && this.CollisionCheck(collision_layer, new Vector2(0, velocity.Y)))
                velocity.Y = 0;
            position += velocity * new Vector2((float)(delta * Variables.FRAME_FACTOR));
        }


        /// <summary>
        /// determines if the entity is colliding with a collision tile
        /// </summary>
        /// <param name="collision_layer"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public bool CollisionCheck(Layer collision_layer, Vector2 offset)
        {
            Vector[] tile_coords = AdjacentTiles();
            foreach (var corner in tile_coords)
            {
                Vector tile_pos = Math0.ClampToTileMap(corner.To2() + offset.NormalizedCopy());
                Tile target_tile = collision_layer.GetTiles()[tile_pos.y, tile_pos.x];
                if (!target_tile.NULL && target_tile.Colliding(this, offset)) return true;
            }
            return false;
        }

        public void ClampPosition()
        {
            position = Vector2.Clamp(position, new Vector2(-1), new Vector2(16 * 16 - 16 + 1));
        }

        /// <summary>
        /// generates a list of tiles the entity is on and adjacent to
        /// </summary>
        /// <returns></returns>
        public Vector[] AdjacentTiles()
        {
            /**
            if (width < Globals.TILE_SIZE && height < Globals.TILE_SIZE)
            {
                Vector origin = MathUtil.CoordsToTileCoords(GetPosition() + new Vector2(width / 2, height / 2));
                List<Vector> tile_positions = new List<Vector> { MathUtil.Clamp(origin.To2()) };
                if (vel.X < 0) tile_positions.Add(MathUtil.Clamp(origin.To2() + new Vector2(-1, 0)));
                if (vel.X > 0) tile_positions.Add(MathUtil.Clamp(origin.To2() + new Vector2(1, 0)));

                if (vel.Y < 0) tile_positions.Add(MathUtil.Clamp(origin.To2() + new Vector2(0, -1)));
                if (vel.Y > 0) tile_positions.Add(MathUtil.Clamp(origin.To2() + new Vector2(0, 1)));

                return tile_positions.ToArray();
            }
            */
            List<Vector> tile_positions = new List<Vector>();
            if (target_vector.X > 0)
            {
                tile_positions.Add(Math0.CoordsToTileCoords(position + new Vector2(width / 2, 0)));
                tile_positions.Add(Math0.CoordsToTileCoords(position + new Vector2(width / 2, height / 2)));
                tile_positions.Add(Math0.CoordsToTileCoords(position + new Vector2(width / 2, height)));
            }
            if (target_vector.X < 0)
            {
                tile_positions.Add(Math0.CoordsToTileCoords(position + new Vector2(0, 0)));
                tile_positions.Add(Math0.CoordsToTileCoords(position + new Vector2(0, height / 2)));
                tile_positions.Add(Math0.CoordsToTileCoords(position + new Vector2(0, height)));
            }
            if (target_vector.Y > 0)
            {
                tile_positions.Add(Math0.CoordsToTileCoords(position + new Vector2(0, height / 2)));
                tile_positions.Add(Math0.CoordsToTileCoords(position + new Vector2(width / 2, height / 2)));
                tile_positions.Add(Math0.CoordsToTileCoords(position + new Vector2(width, height / 2)));
            }
            if (target_vector.Y < 0)
            {
                tile_positions.Add(Math0.CoordsToTileCoords(position + new Vector2(0, 0)));
                tile_positions.Add(Math0.CoordsToTileCoords(position + new Vector2(width / 2, 0)));
                tile_positions.Add(Math0.CoordsToTileCoords(position + new Vector2(width, 0)));
            }
            return tile_positions.ToArray();
        }


        /// <summary>
        /// reduces the entity's health by a given amount
        /// </summary>
        /// <param name="amount"></param>
        public void ReduceHealth(double amount)
        {
            health -= amount / defense;
            if (health < 0) { health = 0; }
        }


        /// <summary>
        /// determines if the entity is colliding with another given entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool CollidesWith(Entity entity)
        {
            return Math0.RectangleCollisionCheck(this.position, this.GetSize(), entity.GetPosition(), entity.GetSize());
        }


        /// <summary>
        /// determines if the entity hits another entity and returns its damage
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public double DealDamage(Entity entity)
        {
            double damage = this.CalculateDamage();
            if (entity.CollidesWith(this))
            {
                return damage;
            }
            return 0;
        }


        /// <summary>
        /// calculates the entity's damage
        /// </summary>
        /// <param name="base_damage"></param>
        /// <returns></returns>
        public virtual double CalculateDamage(double base_damage = 1) 
        {
            // do the temp variable
            double damage = base_damage * this.damage;

            // find any required statuses and apply the status effects
            if (statuses != null)
            {
                StatusEffect? damage_effect = statuses.Find(effect => effect.application_id == 2);

                // if the efffects existed then apply the effect
                if (damage_effect.HasValue) damage += damage_effect.Value.value;
            }
            this.last_damage = damage;
            return damage;
        }

        public void SetPosition(Vector2 _new)
        {
            this.position = _new;
        }
        public double GetMaxHealth() { return max_health; }
        public double GetHealth() { return health; }
        public Vector2 GetPosition() { return position; }
        public Vector2 GetVelocity() { return velocity; }
        public int GetWidth() { return width; }
        public int GetHeight() { return height; }
        public Vector2 GetSize(bool x = true, bool y = true)
        {
            Vector2 size = new Vector2(width, height);
            if (!x) size.X = 0;
            if (!y) size.Y = 0;
            return size;
        }


        // clones the entity
        public virtual Entity Clone()
        {
            return new Entity(this.sprite, this.position.X * 16, this.position.Y * 16);
        }

        internal Rectangle CreateRectangle()
        {
            Vector rounded_position = Vector.Convert(this.position, true);
            return new Rectangle(rounded_position.x, rounded_position.y, width, height);
        }
    }
}
