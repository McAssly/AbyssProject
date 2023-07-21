
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
using System.Reflection.PortableExecutable;

namespace Abyss.Entities.Magic
{
    /// <summary>
    /// The elemental type of a particle
    /// </summary>
    internal enum Element
    {
        NULL = 0, water = 1
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
            cooldown = 0;
        }
    }

    internal struct SubParticle
    {
        public Vector2 displacement;
        public Vector2 velocity;
        public readonly double accel;
        public double rotation;
        public readonly double angular_vel;

        public SubParticle(double x, double y, double vx, double vy, double accel, double angular_vel)
        {
            displacement = new Vector2((float)x, (float)y);
            velocity = new Vector2((float)vx, (float)vy);
            this.accel = accel;
            rotation = 0;
            this.angular_vel = angular_vel;
        }

        public void Update(double delta)
        {
            rotation += angular_vel;
            if (accel > 0) velocity += MathUtil.ApplyAcceleration(velocity, accel * delta);
            displacement += velocity * new Vector2((float)(delta * Globals.FRAME_FACTOR));
            displacement = MathUtil.Rotate(Vector2.Zero, displacement, rotation);
        }
    }

    /// <summary>
    /// A magic particle
    /// </summary>
    internal class Particle
    {
        // instance variables
        private protected Entity parent;
        public SubParticle[] particles;
        public Vector2 position;
        public Vector2 velocity;
        public double accel;
        public double lifetime;
        public readonly Element element;
        public double damage;
        public double rotation;


        /** PARTICLE CONSTRUCTOR */
        public Particle(Entity parent, Vector2 position, Vector2 velocity, ParticleController pc, double damage, double rotation, SubParticle[] sub_particles)
        {
            this.parent = parent;
            this.position = position;
            this.velocity = velocity;
            accel = pc.accel;
            lifetime = pc.lifetime;
            element = pc.element;
            this.damage = damage;
            this.rotation = rotation;
            particles = sub_particles;
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
            for (int i = 0; i < particles.Length; i++)
                particles[i].Update(delta);
            velocity = MathUtil.ApplyAcceleration(velocity, accel * delta);
            position += velocity * new Vector2((float)(delta * Globals.FRAME_FACTOR));
            lifetime -= Globals.PARTICLE_SUBTRACTOR * delta;
        }
    }




    /// <summary>
    /// This is the base grimoire its nothing special but it works
    /// </summary>
    internal class Grimoire
    {
        public List<Particle> Particles = new List<Particle>();
        private protected ParticleController primary;
        private protected ParticleController secondary;

        private protected SubParticle[] sub_particles;

        public Grimoire() 
        {
            primary = new ParticleController(Element.NULL, 0.5, 1, 5, 1, 0.25, 0.1);
            secondary = new ParticleController(Element.NULL, 0.4, 1, 5, 5, 0.25, 0.3);

            sub_particles = new SubParticle[2]
                {
                    new SubParticle(0, 0, 0, 0, 0, 0),
                    new SubParticle(1, 1, 0, 0, 0, 0.3)
                };
    }

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
                    if (primary.cooldown <= 0 && parent.GetMana() >= primary.mana_cost)
                    {
                        Primary(parent, targetPos);
                        parent.ReduceMana(primary.mana_cost);
                        primary.cooldown = primary.cooldown_max;
                    }
                    break;
                case 2: // secondary
                    if (secondary.cooldown <= 0 && parent.GetMana() >= secondary.mana_cost)
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
        /// <param name="target_pos"></param>
        public virtual void Primary(Entity parent, Vector2 target_pos)
        {
            Vector2 target = MathUtil.MoveToward(parent.GetPosition(), target_pos, primary.base_speed);
            GenerateParticle(parent, Vector2.Subtract(target, parent.GetPosition()), 0, Math.Atan2(target.Y - parent.GetPosition().Y, target.X - parent.GetPosition().X));
        }

        /// <summary>
        /// the secondary attack of the base grimoire
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="target_pos"></param>
        public virtual void Secondary(Entity parent, Vector2 target_pos)
        {
            Vector2 position = parent.GetPosition();
            Vector2 target = MathUtil.MoveToward(parent.GetPosition(), target_pos, secondary.base_speed);
            double rotation = Math.Atan2(target.Y - position.Y, target.X - position.X);
            // central particle
            GenerateParticle(parent, Vector2.Subtract(target, position), 1, rotation);

            // left particle
            double left_rotation = rotation - 0.18;
            Vector2 left_target = MathUtil.Rotate(position, target, left_rotation);
            GenerateParticle(parent, Vector2.Subtract(left_target, position), 1, left_rotation);

            // right particle
            double right_rotation = rotation + 0.18;
            Vector2 right_target = MathUtil.Rotate(position, target, right_rotation);
            GenerateParticle(parent, Vector2.Subtract(right_target, position), 1, right_rotation);
        }

        public void GenerateParticle(Entity parent, Vector2 velocity, byte type, double rotation)
        {
            if (parent == null) return;
            switch (type)
            {
                case 0:
                    Particles.Add(new Particle(parent, parent.GetPosition() + new Vector2(4, 4), velocity, primary, parent.CalculateDamage(primary.base_damage), rotation, (SubParticle[])sub_particles.Clone()));
                    break;
                case 1:
                    Particles.Add(new Particle(parent, parent.GetPosition() + new Vector2(4, 4), velocity, secondary, parent.CalculateDamage(secondary.base_damage), rotation, (SubParticle[])sub_particles.Clone()));
                    break;
                default: break;
            }
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
        public void Update(double delta, GameMaster game_state)
        {
            foreach (Particle particle in Particles) particle.Update(delta);
            // remove all particles that have run out of life
            Particles.RemoveAll(particle => particle.lifetime <= 0 || particle.IsOutside());

            List<Particle> _particles = new List<Particle>();

            // get adjacent collision tiles
            foreach (Particle particle in Particles)
            {
                Vector2 tile_pos = Vector2.Clamp(MathUtil.CoordsToTileCoords(particle.position), Vector2.Zero, new Vector2(16 - 1, 16 - 1));
                Tile tile = game_state.GetCurrentTileMap().GetCollisionLayer().GetTiles()[(int)tile_pos.Y, (int)tile_pos.X];
                if (particle.IsColliding(tile) && !tile.NULL)
                {
                    _particles.Add(particle);
                    break;
                }
            }

            // remove any colliding with a tile
            Particles.RemoveAll(particle => _particles.Contains(particle));

            if (primary.cooldown > 0) primary.cooldown -= Globals.PARTICLE_SUBTRACTOR * delta;
            if (secondary.cooldown > 0) secondary.cooldown -= Globals.PARTICLE_SUBTRACTOR * delta;
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
}
