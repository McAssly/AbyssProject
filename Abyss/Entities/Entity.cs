using Abyss.Globals;
using Abyss.Levels;
using Abyss.Magic;
using Abyss.Sprites;
using Abyss.Utility;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Abyss.Entities
{
    internal class Entity
    {
        public SpriteSheet sprite;
        private protected int width, height;

        // declare the entity's stats
        private protected double max_speed = 2;
        private protected double max_accel = 10;
        private protected double friction = 10;
        private protected double speed = 1;
        private protected double max_health;

        private protected Timer invulnerability = new Timer(0.25);

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
            Vector2 velocity = this.velocity;
            // if the movement vector is not zero then the entity must be trying to move
            Vector2 target = target_vector * (float)max_speed * (float)speed;
            if (Math.Abs(velocity.X) < Math.Abs(target.X)) velocity.X = (float)Math0.MoveToward0(velocity.X, target.X, delta * max_speed * max_accel * speed);
            if (Math.Abs(velocity.Y) < Math.Abs(target.Y)) velocity.Y = (float)Math0.MoveToward0(velocity.Y, target.Y, delta * max_speed * max_accel * speed);
            velocity = Math0.MoveToward(velocity, Vector2.Zero, friction * delta);

            this.velocity = velocity;

            Collide(collision_layer);

            if (!this.CollisionCheck(collision_layer, true) && !this.CollisionCheck(collision_layer, false))
                position += this.velocity * new Vector2((float)(delta * Variables.FRAME_FACTOR));
        }


        public void Collide(Layer collision_layer)
        {
            // handle collision, if they are about to collide we must alter our velocity before we move forward
            if (velocity.X != 0 && this.CollisionCheck(collision_layer, true))
                velocity.X = 0;
            if (velocity.Y != 0 && this.CollisionCheck(collision_layer, false))
                velocity.Y = 0;
        }


        /// <summary>
        /// determines if the entity is colliding with a collision tile
        /// </summary>
        /// <param name="collision_layer"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public bool CollisionCheck(Layer collision_layer, bool x_axis)
        {
            // loop through each corner of the entity
            foreach (var tile_pos in GetNextTiles(x_axis))
            {
                // get the tile at the target position
                Vector tile_pos_ = Math0.ClampToTileMap(tile_pos.To2());
                Tile target_tile = collision_layer.GetTiles()[tile_pos_.y, tile_pos_.x];

                // if there is a collision tile preset, return true
                if (!target_tile.NULL && target_tile.Colliding(this, velocity)) return true;
            }
            // otherwise return false
            return false;
        }


        public void Clamp()
        {
            Vector2 position = new Vector2(this.position.X, this.position.Y);
            position = Math0.Clamp(position, Vector2.Zero, new Vector2(240, 240));
            this.position = new Vector2(position.X, position.Y);
        }


        /// <summary>
        /// get the positions of tiles that the entity will run into
        /// </summary>
        /// <returns></returns>
        public Vector[] GetNextTiles(bool x_axis)
        {
            List<Vector> next = new List<Vector>();

            // set the direction to move in
            Vector2 direction = velocity.NormalizedCopy();
            if (x_axis) direction.Y = 0;
            else direction.X = 0;
            if (direction.X + direction.Y > 0) direction.Ceiling();
            else direction.Floor();

            // get the tile coordinates
            foreach (var tile_pos in CurrentTilePositions(x_axis))
                next.Add(Math0.ClampToTileMap(tile_pos.To2() + direction));
            return next.ToArray();
        }

        /// <summary>
        /// generates a list of tiles the entity is on and adjacent to
        /// </summary>
        /// <returns></returns>
        public Vector[] CurrentTilePositions(bool x_axis)
        {
            Vector2 position = new Vector2(this.position.X, this.position.Y);
            position.Floor();
            List<Vector> tile_positions = new List<Vector>() 
            { 
                Math0.CoordsToTileCoords(position + GetSize() / 2), // CENTRAL POSITION
                Math0.CoordsToTileCoords(position + new Vector2(0, 0)), // TOP LEFT CORNER
                Math0.CoordsToTileCoords(position + new Vector2(0, height)), // BOTTOM LEFT CORNER
                Math0.CoordsToTileCoords(position + new Vector2(width, 0)), // TOP RIGHT CORNER
                Math0.CoordsToTileCoords(position + new Vector2(width, height)) // BOTTOM RIGHT CORNER
            };
            if (x_axis) // get left/right positions
            {
                tile_positions.Add(Math0.CoordsToTileCoords(position + new Vector2(0, height / 2))); // LEFT CENTER
                tile_positions.Add(Math0.CoordsToTileCoords(position + new Vector2(width, height / 2))); // RIGHT CENTER
            }
            else // get top/bottom positions
            {
                tile_positions.Add(Math0.CoordsToTileCoords(position + new Vector2(width / 2, 0))); // TOP CENTER
                tile_positions.Add(Math0.CoordsToTileCoords(position + new Vector2(width / 2, height))); // BOTTOM CENTER
            }
            /*
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
            */
            return tile_positions.ToArray();
        }


        /// <summary>
        /// reduces the entity's health by a given amount
        /// </summary>
        /// <param name="amount"></param>
        public void TakeDamage(double amount)
        {
            if (!IsInvulnerable())
            {
                health -= amount / defense;
                if (health < 0) { health = 0; }
                invulnerability.Start();
            }
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
            if (entity.CollidesWith(this)) return damage;
            return 0;
        }


        /// <summary>
        /// checks if the entity is invulnerable
        /// </summary>
        /// <returns></returns>
        public bool IsInvulnerable()
        {
            return invulnerability.IsRunning();
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

        public void AddVelocity(Vector2 velocity)
        {
            this.velocity += velocity;
        }


        public void SetTargetVector(Vector2 vector)
        {
            this.target_vector = vector;
        }

        public double GetMaxSpeed()
        {
            return this.max_speed;
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

        internal SpriteSheet GetSprite()
        {
            return sprite;
        }

        internal Vector2 GetTargetVector()
        {
            return target_vector;
        }
    }
}
