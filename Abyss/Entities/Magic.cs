
using Abyss.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.Xna.Framework.Graphics;
using Abyss.Map;

namespace Abyss.Entities
{
    /// <summary>
    /// The elemental type of a particle
    /// </summary>
    internal enum Element
    {
        NULL = 0, 
    }

    /// <summary>
    /// A magic particle
    /// </summary>
    internal class Particle
    {
        // instance variables
        private protected Entity parent;
        public Vector2 position;
        public Vector2 velocity;
        public double accel;
        public double lifetime;
        public readonly Element element;
        public double damage;
        public double rotation;


        /** PARTICLE CONSTRUCTOR */
        public Particle(Entity parent, Vector2 position, Vector2 velocity, double accel, double lifetime, Element element, double damage, double rotation)
        {
            this.parent = parent;
            this.position = position;
            this.velocity = velocity;
            this.accel = accel;
            this.lifetime = lifetime;
            this.element = element;
            this.damage = damage;
            this.rotation = rotation;
        }


        /// <summary>
        /// determines if the particle is colliding with an entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool IsColliding(Entity entity)
        {
            return false;
        }

        /// <summary>
        /// determines if the particle is colliding with a tile
        /// </summary>
        /// <param name="tile"></param>
        /// <returns></returns>
        public bool IsColliding(Tile tile)
        {
            return true;
        }


        /**  UPDATE/DRAW methods   */

        /// <summary>
        /// Updates the particle
        /// </summary>
        /// <param name="delta"></param>
        public void Update(double delta)
        {
            velocity = MathUtil.ApplyAcceleration(velocity, accel * delta);
            position += velocity;
            lifetime -= Globals.ParticleSubtractor * delta;
        }

        /// <summary>
        /// Draws the particle
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="texture"></param>
        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            spriteBatch.Draw(texture, this.position, null, Color.White, (float)rotation, new Vector2(texture.Width/2, texture.Height/2), 1, SpriteEffects.None, 0);
        }
    }

    internal class Grimoire
    {
        public List<Particle> Particles = new List<Particle>();
        private readonly double accel = 0.25;
        private readonly double lifetime = 0.5;
        private readonly Element element = Element.NULL;
        private readonly double base_damage = 1;
        private readonly double base_speed = 5;

        private readonly double mana_cost_1 = 1;
        private readonly double mana_cost_2 = 5;

        private readonly double cooldown_max_1 = 0.1;
        private readonly double cooldown_max_2 = 0.3;

        private double cooldown_1 = 0.0;
        private double cooldown_2 = 0.0;




        /**    ATTACK SEQUENCE    */


        /// <summary>
        /// Simply directs the grimoire to the given attack sequence type. Also handles cooldowns
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="targetPos"></param>
        /// <param name="type"></param>
        public void Attack(Entity parent, Vector2 targetPos, int type)
        {
            switch (type)
            {
                case 1: // primary
                    if (cooldown_1 <= 0)
                    {
                        Primary(parent, targetPos);
                        parent.ReduceMana(mana_cost_1);
                        cooldown_1 = cooldown_max_1;
                    }
                        break;
                case 2: // secondary
                    if (cooldown_2 <= 0)
                    {
                        Secondary(parent, targetPos);
                        parent.ReduceMana(mana_cost_2);
                        cooldown_2 = cooldown_max_2;
                    }
                    break;
                default: return;
            }
        }

        /// <summary>
        /// the primary attack of the base grimoire
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="targetPos"></param>
        public void Primary(Entity parent, Vector2 targetPos)
        {
            Vector2 position = parent.Position();
            Vector2 target = MathUtil.MoveToward(parent.Position(), targetPos, base_speed);
            Particles.Add(new Particle(
                parent, position + new Vector2(8, 8),
                Vector2.Subtract(target, position),
                accel, lifetime, element, parent.CalculateDamage(base_damage),
                Math.Atan2(target.Y - position.Y, target.X - position.X)
                ));
        }

        /// <summary>
        /// the secondary attack of the base grimoire
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="targetPos"></param>
        public void Secondary(Entity parent, Vector2 targetPos)
        {
            Vector2 position = parent.Position();
            Vector2 target = MathUtil.MoveToward(parent.Position(), targetPos, base_speed);
            double rotation = Math.Atan2(target.Y - position.Y, target.X - position.X);
            // central particle
            Particles.Add(new Particle(
                parent, position + new Vector2(8, 8),
                Vector2.Subtract(target, position),
                accel, lifetime, element, parent.CalculateDamage(base_damage),
                rotation
                ));

            double length = Math.Sqrt(Math.Pow(target.X - position.X, 2) + Math.Pow(target.Y - position.Y, 2));

            // left particle
            double leftRotation = rotation - 0.18;
            Vector2 leftTarget = new Vector2((float)(position.X + length * Math.Cos(leftRotation)), (float)(position.Y + length * Math.Sin(leftRotation)));
            Particles.Add(new Particle(
                parent, position + new Vector2(8, 8),
                Vector2.Subtract(leftTarget, position),
                accel, lifetime, element, parent.CalculateDamage(base_damage),
                leftRotation
                ));

            // right particle
            double rightRotation = rotation + 0.18;
            Vector2 rightTarget = new Vector2((float)(position.X + length * Math.Cos(rightRotation)), (float)(position.Y + length * Math.Sin(rightRotation)));
            Particles.Add(new Particle(
                parent, position + new Vector2(8, 8),
                Vector2.Subtract(rightTarget, position),
                accel, lifetime, element, parent.CalculateDamage(base_damage),
                rightRotation
                ));
        }




        /**    UPDATE/DRAW methods    */

        /// <summary>
        /// clears the particles
        /// </summary>
        public void Clear()
        {
            Particles.Clear();
        }

        /// <summary>
        /// updates the grimoire
        /// </summary>
        /// <param name="delta"></param>
        public void Update(double delta)
        {
            foreach (Particle particle in Particles) particle.Update(delta);
            // remove all particles that have run out of life
            Particles.RemoveAll(particle => particle.lifetime <= 0);

            if (cooldown_1 > 0) cooldown_1 -= Globals.ParticleSubtractor * delta;
            if (cooldown_2 > 0) cooldown_2 -= Globals.ParticleSubtractor * delta;
        }

        /// <summary>
        /// draws the grimoire
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (Globals.BaseSpell == null) return;
            foreach (var particle in Particles)
            {
                particle.Draw(spriteBatch, Globals.BaseSpell);
            }
        }




        /**    STATIC methods    */

        /// <summary>
        /// determines which string formatted grimoire was given
        /// </summary>
        /// <param name="grimoire"></param>
        /// <returns></returns>
        public static Grimoire Which(string grimoire)
        {
            switch (grimoire)
            {
                case "base":
                    return new Grimoire();
                case "water":
                    return new WaterGrimoire();
                case "wind":
                    return new WindGrimoire();
                case "earth":
                    return new EarthGrimoire();
                case "fire":
                    return new FireGrimoire();
                case "lightning":
                    return new LightningGrimoire();
                default: return new Grimoire();
            }
        }

        /// <summary>
        /// converts the grimoire to a formatted string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "base";
        }
    }

    internal class WaterGrimoire : Grimoire
    {
        public override string ToString()
        {
            return "water";
        }
    }

    internal class WindGrimoire : Grimoire
    {
        public override string ToString()
        {
            return "wind";
        }
    }

    internal class EarthGrimoire : Grimoire
    {
        public override string ToString()
        {
            return "earth";
        }
    }

    internal class FireGrimoire : Grimoire
    {
        public override string ToString()
        {
            return "fire";
        }
    }

    internal class LightningGrimoire : Grimoire
    {
        public override string ToString()
        {
            return "lightning";
        }
    }

}
