using Abyss.Entities;
using Abyss.Globals;
using Abyss.Levels;
using Abyss.Master;
using Abyss.Sprites;
using Abyss.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.Magic
{
    internal class Attack : Magic
    {
        private protected ParticleController controller;
        private protected double dmg_multiplier = 1;
        private protected AnimatedSprite sprite;
        internal bool draw_sprite;

        public Attack(ParticleController controller, AnimatedSprite sprite = null)
        {
            this.controller = controller;

            if (sprite == null) draw_sprite = false;
            else
            {
                this.sprite = sprite;
                this.draw_sprite = true;
            }
        }


        public void Update(double delta, GameState game_state)
        {
            // update the particles
            foreach (Particle p in particles) p.Update(delta);

            // remove all dead particles
            particles.RemoveAll(p => p.lifetime <= 0 || p.IsOutside());

            // remove all particles that collide with a collision tile
            List<Particle> dead = new List<Particle>();

            // get adjacent collision tiles
            foreach (Particle particle in particles)
            {
                Vector tile_pos = Math0.CoordsToTileCoords(particle.position, true);
                tile_pos = Math0.ClampToTileMap(tile_pos.To2());
                Tile tile = game_state.GetCollisionLayer().GetTiles()[tile_pos.y, tile_pos.x];
                if (particle.IsColliding(tile) && !tile.NULL && !particle.ignore_collision)
                {
                    Effect.HitEffect(particle.position, particle.rotation, particle.element, game_state);
                    dead.Add(particle);
                    break;
                }
            }

            // remove them
            particles.RemoveAll(p => dead.Contains(p));

            controller.cooldown.Update(Variables.PARTICLE_SUBTRACTOR * delta);
        }


        public void AddParticle(Entity parent, Vector2 velocity, double rotation, float padding = 0)
        {
            Add(controller, sprite, parent, velocity, rotation, dmg_multiplier, padding);
        }
    }
}
