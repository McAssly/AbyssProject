using Abyss.Entities.Enemies;
using Abyss.Map;
using Abyss.Master;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.Entities
{
    internal class Entity
    {
        private protected Rectangle draw_obj;
        private protected Texture2D texture;
        private protected int draw_size = Globals.TILE_SIZE; // by default we will use the global tile size
        // offsets for the entity, useful for knowing where each corner and face are located on the entity's draw object
        private protected readonly Vector2[] _offsets;

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

        // last damage applied
        public double last_damage = 0;

        // for crits
        private protected static readonly Random random = new Random();

        public Entity()
        {

        }

        public Entity(float x, float y)
        {
            this.pos = new Vector2(x * 16, y * 16);
        }

        public Entity(Texture2D texture)
        {
            this.statuses = new List<StatusEffect>();
            this.texture = texture;
            _offsets = new Vector2[8]
                {
                    Vector2.Zero,
                    new Vector2(draw_size),
                    new Vector2(draw_size, 0),
                    new Vector2(0, draw_size),

                    new Vector2(0, draw_size / 2),
                    new Vector2(draw_size, draw_size / 2),
                    new Vector2(draw_size / 2, 0),
                    new Vector2(draw_size / 2, draw_size)
                };
        }

        public bool CollidesWith(Entity entity)
        {
            foreach (Vector2 offset in _offsets)
            {
                if (MathUtil.IsWithin(pos + offset, entity.GetPosition().X, entity.GetPosition().X + entity.GetWidth(), entity.GetPosition().Y, entity.GetPosition().Y + entity.GetHeight()))
                    return true;
            }
            return false;
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
            if (this.WillCollideX(map) && vel.X != 0)
                vel.X = 0;
            if (this.WillCollideY(map) && vel.Y != 0)
                vel.Y = 0;
            pos += vel * new Vector2((float)(delta * Globals.FRAME_FACTOR));
        }

        public bool WillCollideX(TileMap map)
        {
            // the only offsets needed for the x axis are the left and right offsets
            // therefore ignore offsets @ i = 6 & 7
            for (int i = 0; i < 8; i++)
            {
                if (i == 6 || i == 7) continue;
                if (vel.X > 0 && (i == 0 || i == 4 || i == 3)) continue; // ignore left side
                if (vel.X < 0 && (i == 1 || i == 5 || i == 2)) continue; // ignore right side
                if (CollisionCheckX(map, i)) return true;
            }
            return false;
        }
        public bool WillCollideY(TileMap map)
        {
            // the only offsets needed for the y axis are the top and bottom offsets
            // therefore ignore offsets @ i = 4 & 5
            for (int i = 0; i < 8; i++)
            {
                if (i == 4 || i == 5) continue;
                if (vel.Y > 0 && (i == 0 || i == 6 || i == 2)) continue; // ignore top side
                if (vel.Y < 0 && (i == 3 || i == 7 || i == 1)) continue; // ignore bottom side
                if (CollisionCheckY(map, i)) return true;
            }
            return false;
        }

        public bool CollisionCheckX(TileMap map, int offset)
        {
            Vector2 tilePos = Vector2.Clamp(MathUtil.CoordsToTileCoords(pos + new Vector2(vel.X, 0) + MathUtil.offsets[offset]), Vector2.Zero, new Vector2(map.GetWidth() - 1, map.GetHeight() - 1));
            Tile targetTile = map.GetCollisionLayer().GetTiles()[(int)tilePos.Y, (int)tilePos.X];
            return !targetTile.NULL && targetTile.Colliding(this);
        }
        public bool CollisionCheckY(TileMap map, int offset)
        {
            Vector2 tilePos = Vector2.Clamp(MathUtil.CoordsToTileCoords(pos + new Vector2(0, vel.Y) + MathUtil.offsets[offset]), Vector2.Zero, new Vector2(map.GetWidth() - 1, map.GetHeight() - 1));
            Tile targetTile = map.GetCollisionLayer().GetTiles()[(int)tilePos.Y, (int)tilePos.X];
            return !targetTile.NULL && targetTile.Colliding(this);
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
        public Rectangle GetDrawObj() { return draw_obj; }
        public int GetWidth() { return draw_obj.Width; }
        public int GetHeight() { return draw_obj.Height; }
        /**
         * Simply updates the draw object's position
         */
        public void UpdateDrawObj()
        {
            pos = Vector2.Clamp(pos, new Vector2(-1), new Vector2(16 * 16 - 16 + 1));
            // if the player's position updated then therefore so does the draw object's
            if (draw_obj.X != pos.X)
                draw_obj.X = (int)pos.X;

            // the same goes for the y-axis
            if (draw_obj.Y != pos.Y)
                draw_obj.Y = (int)pos.Y;
        }
    }
}
