﻿using Abyss.Entities;
using Abyss.Globals;
using Abyss.Levels;
using Abyss.Sprites;
using Abyss.Utility;
using Microsoft.Xna.Framework;

namespace Abyss.Magic
{
    /// <summary>
    /// Allows for global particle constructor control within each grimoire
    /// </summary>
    internal struct ParticleController
    {
        public readonly double accel;
        public readonly double lifetime;
        public readonly double base_damage;
        public readonly double base_speed;
        public readonly double mana_cost;
        public Timer cooldown;
        public bool pierce;
        public bool ignore_collision;
        public bool lock_to_player;
        public double radius;

        public ParticleController(
            double lifetime, double base_damage, double base_speed,
            double mana_cost, double accel, double cooldown_max,
            double radius = 1,
            bool pierce = false, bool ignore_collision = false, bool lock_to_player = false)
        {
            this.lifetime = lifetime;
            this.base_damage = base_damage;
            this.base_speed = base_speed;
            this.mana_cost = mana_cost;
            this.accel = accel;
            this.cooldown = new Timer(cooldown_max);
            this.pierce = pierce;
            this.ignore_collision = ignore_collision;
            this.lock_to_player = lock_to_player;
            this.radius = radius;
        }
    }

    /// <summary>
    /// A magic particle
    /// </summary>
    internal class Particle
    {
        // instance variables
        private protected Entity parent;
        public AnimatedSprite sprite;
        public Vector2 position;
        public Vector2 velocity;
        public double accel;
        public double lifetime;
        public double damage;
        public double rotation;
        public bool pierce;
        public bool ignore_collision;
        public bool lock_to_player;
        public double radius;
        public double damage_multiplier;


        /** PARTICLE CONSTRUCTOR */
        public Particle(Entity parent, 
            Vector2 position, Vector2 velocity, ParticleController pc, 
            double damage, double rotation, AnimatedSprite sprite, double damage_mult)
        {
            this.parent = parent;
            this.position = position;
            this.velocity = velocity;
            accel = pc.accel;
            lifetime = pc.lifetime;
            this.damage = damage;
            this.rotation = rotation;
            this.sprite = sprite;
            this.pierce = pc.pierce;
            this.ignore_collision = pc.ignore_collision;
            this.lock_to_player = pc.lock_to_player;
            this.radius = pc.radius;
            this.damage_multiplier = damage_mult;
        }


        public double GetDamage()
        {
            return damage * damage_multiplier;
        }



        /// <summary>
        /// in the case of a particle reducing its damage (typically when piercing a target)
        /// </summary>
        public void ReduceDamage()
        {
            this.damage = this.damage * 0.77;
            if (this.damage < 0.01)
                this.lifetime = 0;
        }


        /// <summary>
        /// determines if the particle is colliding with an entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool IsColliding(Entity entity)
        {
            if (this.radius <= 1)
                return Math0.RectangleCollisionCheck(position, new Vector2(1, 1), entity.GetPosition(), entity.GetSize());
            else
                return Math0.RectangleToCircleCollisionCheck(position, radius, entity.GetPosition(), entity.GetSize());
        }

        /// <summary>
        /// determines if the particle is colliding with a tile
        /// </summary>
        /// <param name="tile"></param>
        /// <returns></returns>
        public bool IsColliding(Tile tile)
        {
            return Math0.WithinRectangle(position, tile.position, new Vector2(16, 16));
        }

        /// <summary>
        /// determines if said particle is outside the game's boundaries
        /// </summary>
        /// <returns></returns>
        public bool IsOutside()
        {
            return position.X < 0 || position.X > 16 * 16 || position.Y < 0 || position.Y > 16 * 16;
        }


        /**  UPDATE/DRAW methods   */

        /// <summary>
        /// Updates the particle
        /// </summary>
        /// <param name="delta"></param>
        public void Update(double delta)
        {
            if (sprite != null) sprite.Update(delta);
            if (!lock_to_player)
            {
                velocity = Math0.ApplyAcceleration(velocity, accel * delta);
                position += velocity * new Vector2((float)(delta * Variables.FRAME_FACTOR));
            }
            else
            {
                this.position = parent.GetPosition() + new Vector2(parent.sprite.width / 2, parent.sprite.height / 2);
            }
            lifetime -= Variables.PARTICLE_SUBTRACTOR * delta;
        }
    }
}
