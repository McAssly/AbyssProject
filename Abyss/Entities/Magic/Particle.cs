using Abyss.Draw;
using Abyss.Map;
using Abyss.Master;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.Entities.Magic
{
    /// <summary>
    /// The elemental type of a particle
    /// </summary>
    internal enum Element
    {
        NULL = 0, water = 1, fire = 2, earth = 3, wind = 4, lightning = 5
    }

    /// <summary>
    /// Allows for global particle constructor control within each grimoire
    /// </summary>
    internal struct ParticleController
    {
        public readonly double accel;
        public readonly double lifetime;
        public readonly Element element;
        public readonly double base_damage;
        public readonly double base_speed;
        public readonly double mana_cost;
        public readonly double cooldown_max;
        public double cooldown;
        public bool pierce;

        public ParticleController(Element element, double lifetime, double base_damage, double base_speed, double mana_cost, double accel, double cooldown_max, bool pierce = false)
        {
            this.element = element;
            this.lifetime = lifetime;
            this.base_damage = base_damage;
            this.base_speed = base_speed;
            this.mana_cost = mana_cost;
            this.accel = accel;
            this.cooldown_max = cooldown_max;
            cooldown = 0;
            this.pierce = pierce;
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
        public readonly Element element;
        public double damage;
        public double rotation;
        public bool pierce;


        /** PARTICLE CONSTRUCTOR */
        public Particle(Entity parent, Vector2 position, Vector2 velocity, ParticleController pc, double damage, double rotation, AnimatedSprite sprite, Particle? connection_point = null)
        {
            this.parent = parent;
            this.position = position;
            this.velocity = velocity;
            accel = pc.accel;
            lifetime = pc.lifetime;
            element = pc.element;
            this.damage = damage;
            this.rotation = rotation;
            this.sprite = sprite;
            this.pierce = pc.pierce;
        }


        /// <summary>
        /// determines if the particle is colliding with an entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool IsColliding(Entity entity)
        {
            return MathUtil.IsWithin(position, entity.GetPosition().X, entity.GetPosition().X + entity.GetWidth(), entity.GetPosition().Y, entity.GetPosition().Y + entity.GetHeight());
        }

        /// <summary>
        /// determines if the particle is colliding with a tile
        /// </summary>
        /// <param name="tile"></param>
        /// <returns></returns>
        public bool IsColliding(Tile tile)
        {
            return tile.Colliding(this);
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
            sprite.Update(delta);
            velocity = MathUtil.ApplyAcceleration(velocity, accel * delta);
            position += velocity * new Vector2((float)(delta * Globals.FRAME_FACTOR));
            lifetime -= Globals.PARTICLE_SUBTRACTOR * delta;
        }
    }
}
