﻿using Abyss.Entities;
using Abyss.Globals;
using Abyss.Levels;
using Abyss.Magic.Grimoires;
using Abyss.Master;
using Abyss.Sprites;
using Abyss.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Abyss.Magic
{
    /// <summary>
    /// This is the base grimoire its nothing special but it works
    /// </summary>
    internal class Grimoire : Magic
    {
        private protected ParticleController primary;
        private protected ParticleController secondary;
        private protected double dmg_multiplier_1 = 1;
        private protected double dmg_multiplier_2 = 1;
        private protected AnimatedSprite sprite;
        private protected AnimatedSprite sprite_2;

        public Grimoire()
        {
            primary = new ParticleController(0.5, 1, 5, 1, 0.25, 0.1);
            secondary = new ParticleController(0.4, 5, 5, 5, 0.25, 0.3);

            sprite = _Sprites.BaseSpell;
            sprite_2 = sprite;
        }

        /**    ATTACK SEQUENCE    */


        /// <summary>
        /// Simply directs the grimoire to the given attack sequence type. Also handles cooldowns
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="targetPos"></param>
        /// <param name="type"></param>
        public virtual void Attack(Player parent, Vector2 targetPos, int type, double delta)
        {
            switch (type)
            {
                case 1: // primary
                    if (!primary.cooldown.IsRunning() && parent.GetMana() >= primary.mana_cost && PrimaryCheck(parent))
                    {
                        Primary(parent, targetPos, delta);
                        parent.ReduceMana(primary.mana_cost);
                        primary.cooldown.Start();
                    }
                    break;
                case 2: // secondary
                    if (!secondary.cooldown.IsRunning() && parent.GetMana() >= secondary.mana_cost && SecondaryCheck(parent))
                    {
                        Secondary(parent, targetPos, delta);
                        parent.ReduceMana(secondary.mana_cost);
                        secondary.cooldown.Start();
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
        public virtual void Primary(Entity parent, Vector2 target_pos, double delta)
        {
            Vector2 target = Math0.MoveToward(parent.GetPosition(), target_pos, primary.base_speed);
            GenerateParticle(parent, Vector2.Subtract(target, parent.GetPosition()), 0, Math.Atan2(target.Y - parent.GetPosition().Y, target.X - parent.GetPosition().X));
        }

        /// <summary>
        /// the secondary attack of the base grimoire
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="target_pos"></param>
        public virtual void Secondary(Entity parent, Vector2 target_pos, double delta)
        {
            Vector2 position = parent.GetPosition();
            Vector2 target = Math0.MoveToward(parent.GetPosition(), target_pos, secondary.base_speed);
            double rotation = Math.Atan2(target.Y - position.Y, target.X - position.X);
            // central particle
            GenerateParticle(parent, Vector2.Subtract(target, position), 1, rotation);

            // left particle
            double left_rotation = rotation - 0.18;
            Vector2 left_target = Math0.Rotate(position, target, left_rotation);
            GenerateParticle(parent, Vector2.Subtract(left_target, position), 1, left_rotation);

            // right particle
            double right_rotation = rotation + 0.18;
            Vector2 right_target = Math0.Rotate(position, target, right_rotation);
            GenerateParticle(parent, Vector2.Subtract(right_target, position), 1, right_rotation);
        }


        internal virtual bool PrimaryCheck(Player parent)
        {
            return true;
        }

        internal virtual bool SecondaryCheck(Player parent)
        {
            return true;
        }

        internal virtual void OnDeath(Player parent, Particle particle)
        {
            return;
        }

        public void GenerateParticle(Entity parent,Vector2 velocity, byte type, double rotation, float padding = 0)
        {
            if (parent == null) return;
            switch (type)
            {
                case 0:
                    Add(primary, sprite, parent, velocity, rotation, dmg_multiplier_1, padding);
                    break;
                case 1:
                    Add(secondary, sprite_2, parent, velocity, rotation, dmg_multiplier_2, padding);
                    break;
                default: break;
            }
        }



        /**    UPDATE/DRAW methods    */

        /// <summary>
        /// clears the particles
        /// </summary>
        public void Clear(GameState game_state)
        {
            foreach (var p in particles)
                this.OnDeath(game_state.player, p);
            particles.Clear();
        }

        /// <summary>
        /// updates the grimoire
        /// </summary>
        /// <param name="delta"></param>
        public void Update(double delta, GameState game_state)
        {
            foreach (Particle particle in particles) particle.Update(delta);
            // remove all particles that have run out of life
            List<Particle> dead = particles.FindAll(p => p.lifetime <= 0 || p.IsOutside());
            // death not working???
            foreach (var p in dead)
                OnDeath(game_state.player, p);
            particles.RemoveAll(p => dead.Contains(p));

            List<Particle> _particles = new List<Particle>();

            // get adjacent collision tiles
            foreach (Particle particle in particles)
            {
                Vector tile_pos = Math0.CoordsToTileCoords(particle.position, true);
                tile_pos = Math0.ClampToTileMap(tile_pos.To2());
                Tile tile = game_state.GetCollisionLayer().GetTiles()[tile_pos.y, tile_pos.x];
                if (particle.IsColliding(tile) && !tile.NULL && !particle.ignore_collision)
                {
                    Effect.HitEffect(particle.position, particle.rotation, this.elemental_type, game_state);
                    _particles.Add(particle);
                    break;
                }
            }

            // remove any colliding with a tile
            foreach (var p in _particles)
                OnDeath(game_state.player, p);
            particles.RemoveAll(particle => _particles.Contains(particle));

            primary.cooldown.Update(Variables.PARTICLE_SUBTRACTOR * delta);
            secondary.cooldown.Update(Variables.PARTICLE_SUBTRACTOR * delta);
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
                    return new Water();
                case "wind":
                    return new Wind();
                case "earth":
                    return new Earth();
                case "fire":
                    return new Fire();
                case "lightning":
                    return new Lightning();
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
