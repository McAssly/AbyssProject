
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
using System.Diagnostics;
using System.Threading;
using Abyss.Draw;

namespace Abyss.Entities.Magic
{
    /// <summary>
    /// This is the base grimoire its nothing special but it works
    /// </summary>
    internal class Grimoire
    {
        public List<Particle> Particles = new List<Particle>();
        private protected ParticleController primary;
        private protected ParticleController secondary;

        private protected AnimatedSprite sprite;

        public Grimoire() 
        {
            primary = new ParticleController(Element.NULL, 0.5, 1, 5, 1, 0.25, 0.1);
            secondary = new ParticleController(Element.NULL, 0.4, 1, 5, 5, 0.25, 0.3);

            sprite = Globals.BaseSpell;
        }

        /**    ATTACK SEQUENCE    */


        /// <summary>
        /// Simply directs the grimoire to the given attack sequence type. Also handles cooldowns
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="targetPos"></param>
        /// <param name="type"></param>
        public virtual void Attack(Entity parent, Vector2 targetPos, int type)
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


        /// <summary>
        /// Detects if the entity is hit by any particle the grimoire casted, and returns the damage that particle deals then removes said particle
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Particle Hits(Entity entity)
        {
            Particle dealer = Particles.Find(x => x.IsColliding(entity));
            if (dealer != null)
            {
                if (!dealer.pierce) Particles.Remove(dealer);
                return dealer;
            }
            return null;
        }

        public void GenerateParticle(Entity parent, Vector2 velocity, byte type, double rotation)
        {
            if (parent == null) return;
            switch (type)
            {
                case 0:
                    Particles.Add(new Particle(parent, parent.GetPosition() + new Vector2(8, 8), velocity, primary, parent.CalculateDamage(primary.base_damage), rotation, sprite.Clone()));
                    break;
                case 1:
                    Particles.Add(new Particle(parent, parent.GetPosition() + new Vector2(8, 8), velocity, secondary, parent.CalculateDamage(secondary.base_damage), rotation, sprite.Clone()));
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
                Vector tile_pos = MathUtil.CoordsToTileCoords(particle.position);
                Tile tile = game_state.GetCurrentTileMap().GetCollisionLayer().GetTiles()[tile_pos.y, tile_pos.x];
                if (particle.IsColliding(tile) && !tile.NULL)
                {
                    game_state.Burst(particle.position);
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
