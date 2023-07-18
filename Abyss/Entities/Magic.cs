
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

        public ParticleController(Element element, double lifetime, double base_damage, double base_speed, double mana_cost, double accel, double cooldown_max)
        {
            this.element = element;
            this.lifetime = lifetime;
            this.base_damage = base_damage;
            this.base_speed = base_speed;
            this.mana_cost = mana_cost;
            this.accel = accel;
            this.cooldown_max = cooldown_max;
            this.cooldown = 0;
        }
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
        public Particle(Entity parent, Vector2 position, Vector2 velocity, ParticleController pc, double damage, double rotation)
        {
            this.parent = parent;
            this.position = position;
            this.velocity = velocity;
            this.accel = pc.accel;
            this.lifetime = pc.lifetime;
            this.element = pc.element;
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
        private ParticleController primary = new ParticleController(Element.NULL, 0.5, 1, 5, 1, 0.25, 0.1);
        private ParticleController secondary = new ParticleController(Element.NULL, 0.4, 1, 5, 5, 0.25, 0.3);




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
                    if (primary.cooldown <= 0)
                    {
                        Primary(parent, targetPos);
                        parent.ReduceMana(primary.mana_cost);
                        primary.cooldown = primary.cooldown_max;
                    }
                        break;
                case 2: // secondary
                    if (secondary.cooldown <= 0)
                    {
                        Secondary(parent, targetPos);
                        parent.ReduceMana(secondary.mana_cost);
                        secondary.cooldown = secondary.cooldown_max;
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
            Vector2 target = MathUtil.MoveToward(parent.Position(), targetPos, primary.base_speed);
            Particles.Add(new Particle(
                parent, position + new Vector2(8, 8),
                Vector2.Subtract(target, position),
                primary, parent.CalculateDamage(primary.base_damage),
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
            Vector2 target = MathUtil.MoveToward(parent.Position(), targetPos, secondary.base_speed);
            double rotation = Math.Atan2(target.Y - position.Y, target.X - position.X);
            // central particle
            Particles.Add(new Particle(
                parent, position + new Vector2(8, 8),
                Vector2.Subtract(target, position),
                secondary, parent.CalculateDamage(secondary.base_damage),
                rotation
                ));

            double length = Math.Sqrt(Math.Pow(target.X - position.X, 2) + Math.Pow(target.Y - position.Y, 2));

            // left particle
            double leftRotation = rotation - 0.18;
            Vector2 leftTarget = new Vector2((float)(position.X + length * Math.Cos(leftRotation)), (float)(position.Y + length * Math.Sin(leftRotation)));
            Particles.Add(new Particle(
                parent, position + new Vector2(8, 8),
                Vector2.Subtract(leftTarget, position),
                secondary, parent.CalculateDamage(secondary.base_damage),
                leftRotation
                ));

            // right particle
            double rightRotation = rotation + 0.18;
            Vector2 rightTarget = new Vector2((float)(position.X + length * Math.Cos(rightRotation)), (float)(position.Y + length * Math.Sin(rightRotation)));
            Particles.Add(new Particle(
                parent, position + new Vector2(8, 8),
                Vector2.Subtract(rightTarget, position),
                secondary, parent.CalculateDamage(secondary.base_damage),
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

            if (primary.cooldown > 0) primary.cooldown -= Globals.ParticleSubtractor * delta;
            if (secondary.cooldown > 0) secondary.cooldown -= Globals.ParticleSubtractor * delta;
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
