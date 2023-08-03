using Abyss.Entities.Enemies;
using Abyss.Map;
using Abyss.Master;
using Abyss.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Vector = Abyss.Master.Vector;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Abyss.Entities
{
    internal class Entity
    {
        public SpriteSheet sprite;

        private protected int width, height;

        // declare max values for the entity
        private protected readonly int max_speed = 1;
        private protected readonly int max_accel = 50;
        private protected readonly int friction = 50;

        // declare the entity's stats
        private protected double speed = 1;
        private protected double max_health;
        private protected double max_mana;

        // current stats
        private protected double health;
        private protected double mana;

        // declare the entity's substats
        private protected double damage;
        private protected double crit_dmg; // percentage to increase damage by
        private protected double crit_rate; // between 0 and 1, a percentage value
        private protected double defense;    // damage resistence

        // declare position variables
        private protected Vector2 movement_vec = Vector2.Zero;
        private protected Vector2 pos;
        private protected Vector2 vel = Vector2.Zero; // starts off not moving

        // declare the enemy attack cooldown
        public double attack_cooldown;
        public double attack_cooldown_max;

        // declare the status effects to be applied
        public List<StatusEffect> statuses;

        // time elapsed
        private protected double time_elapsed = 0;
        public double regen_timer = 0;
        private protected double idle_timer_max = 5;
        private protected double idle_timer = 0;

        // last damage applied
        public double last_damage = 0;

        // for crits
        private protected static readonly Random random = new Random();

        public Entity(SpriteSheet sprite, float x, float y)
        {
            this.sprite = sprite;
            this.width = this.sprite.width - 1;
            this.height = this.sprite.height - 1;
            this.pos = new Vector2(x, y);
        }

        public Entity(float x, float y)
        {
            this.pos = new Vector2(x, y);
        }

        public Entity(SpriteSheet sprite)
        {
            this.statuses = new List<StatusEffect>();
            this.sprite = sprite;
            this.width = this.sprite.width - 1;
            this.height = this.sprite.height - 1;
        }

        public bool CollidesWith(Entity entity)
        {
            return MathUtil.RectangleCollisionCheck(this.pos, this.GetSize(), entity.GetPosition(), entity.GetSize());
        }

        public double Hits(Player player)
        {
            double damage = this.CalculateDamage(1);
            if (player.CollidesWith(this))
            {
                return damage;
            }
            return 0;
        }

        public double CalculateDamage(double base_dmg)
        {
            // do the temp variables
            double crit_dmg = this.crit_dmg;
            double crit_rate = this.crit_rate;
            double damage = base_dmg * this.damage;

            // find any required statuses and apply the status effects
            if (statuses != null)
            {
                StatusEffect? crit_rate_effect = statuses.Find(effect => effect.application_id == 0);
                StatusEffect? crit_dmg_effect = statuses.Find(effect => effect.application_id == 1);
                StatusEffect? damage_effect = statuses.Find(effect => effect.application_id == 2);

                // if the efffects existed then apply the effect
                if (crit_rate_effect.HasValue) crit_rate += crit_rate_effect.Value.value;
                if (crit_dmg_effect.HasValue) crit_dmg += crit_dmg_effect.Value.value;
                if (damage_effect.HasValue) damage += damage_effect.Value.value;
            }

            // calculate the actual damage values
            int iterations = 0;
            while (random.NextDouble() < crit_rate && iterations < 3)
            {
                damage = damage + (damage * crit_dmg / (iterations + 1));
                iterations++;
            }
            this.last_damage = damage;
            return damage;
        }

        public void ReduceHealth(double amount)
        {
            health -= amount / defense;
            if (health < 0) health = 0;
        }

        public void ReduceMana(double cost)
        {
            mana -= cost;
            if (mana < 0) mana = 0;
        }

        public void AddMana(double amount)
        {
            mana += amount;
        }

        /**
         * Moves the entity based on the current movement vector (already normalized)
         * 
         * @param TileMap   the current map level the entity is playing in
         * @param double    the time it took the last frame to load (seconds)
         */
        public void Move(TileMap map, double delta)
        {
            // if the movement vector is not zero then the entity must be trying to move
            if (movement_vec != Vector2.Zero)
                vel = MathUtil.MoveToward(vel, movement_vec * max_speed * (float)speed, max_accel * delta);
            else // otherwise it is not trying to move at all so slow it down to zero
                vel = MathUtil.MoveToward(vel, Vector2.Zero, friction * delta);

            // handle collision, if they are about to collide we must alter our velocity before we move forward
            if (vel.X != 0 && this.CollisionCheck(map, new Vector2(vel.X, 0)))
                vel.X = 0;
            if (vel.Y != 0 && this.CollisionCheck(map, new Vector2(0, vel.Y)))
                vel.Y = 0;
            pos += vel * new Vector2((float)(delta * Globals.FRAME_FACTOR));
        }

        public bool CollisionCheck(TileMap map, Vector2 offset)
        {
            Vector[] tile_coords = GenerateTilePositions();
            foreach (var corner in tile_coords)
            {
                Vector tile_pos = MathUtil.Clamp(corner.To2() + offset.NormalizedCopy());
                Tile target_tile = map.GetCollisionLayer().GetTiles()[tile_pos.y, tile_pos.x];
                if (!target_tile.NULL && target_tile.Colliding(this, offset)) return true;
            }
            return false;
        }

        public Vector[] GenerateTilePositions()
        {
            Vector origin = MathUtil.CoordsToTileCoords(GetPosition() + new Vector2(width / 2, height / 2));
            List<Vector> tile_positions = new List<Vector> {MathUtil.Clamp(origin.To2())};
            if (vel.X < 0) tile_positions.Add(MathUtil.Clamp(origin.To2() + new Vector2(-1, 0)));
            if (vel.X > 0) tile_positions.Add(MathUtil.Clamp(origin.To2() + new Vector2(1, 0)));

            if (vel.Y < 0) tile_positions.Add(MathUtil.Clamp(origin.To2() + new Vector2(0, -1)));
            if (vel.Y > 0) tile_positions.Add(MathUtil.Clamp(origin.To2() + new Vector2(0, 1)));

            return tile_positions.ToArray();
        }


        // set the entities position
        public void SetPosition(float? x, float? y)
        {
            if (x != null) pos.X = x.Value;
            if (y != null) pos.Y = y.Value;
        }

        /**
         * Getters/Setters
         */
        public double GetMaxHealth() { return max_health; }
        public double GetMaxMana() { return max_mana; }
        public double GetHealth() { return health; }
        public double GetMana() { return mana; }
        public Vector2 GetPosition() { return pos; }
        public Vector2 GetVelocity() { return vel; }
        public int GetWidth() { return width; }
        public int GetHeight() { return height; }
        public Vector2 GetSize(bool x = true, bool y = true)
        {
            Vector2 size = new Vector2(width, height);
            if (!x) size.X = 0;
            if (!y) size.Y = 0;
            return size;
        }
        /**
         * Simply updates the draw object's position
         */
        public void ClampPosition()
        {
            pos = Vector2.Clamp(pos, new Vector2(-1), new Vector2(16 * 16 - 16 + 1));
        }

        public virtual void Load()
        {
            this.sprite = Sprites.TestBox;
            this.width = this.sprite.width - 1;
            this.height = this.sprite.height - 1;
        }



        // clones the entity
        public virtual Entity Clone()
        {
            return new Entity(this.sprite, this.pos.X * 16, this.pos.Y * 16);
        }

        internal Rectangle CreateRectangle()
        {
            Vector rounded_position = Vector.Round(this.pos);
            return new Rectangle(rounded_position.x, rounded_position.y, width, height);
        }
    }
}
