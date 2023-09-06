﻿using Abyss.Entities;
using Abyss.Globals;
using Abyss.Magic;
using Abyss.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Abyss.Draw
{
    internal partial class DrawState : SpriteBatch
    {
        /// <summary>
        /// draws the given entity
        /// </summary>
        /// <param name="entity"></param>
        public void Draw(Entity entity)
        {
            Draw(entity.sprite, entity.GetPosition(), Color.White);
        }

        /// <summary>
        /// draws the given enemy (as red)
        /// </summary>
        /// <param name="enemy"></param>
        public void Draw(Enemy enemy)
        {
            Draw(enemy.sprite, enemy.GetPosition(), Color.Red);
        }

        /// <summary>
        /// draws the given player
        /// </summary>
        /// <param name="player"></param>
        public void Draw(Player player)
        {
            if (Variables.DebugCollision)
            {
                Vector[] tile_positions = player.AdjacentTiles();
                foreach (Vector tile_pos in tile_positions)
                {
                    this.DrawRectangle(new Rectangle(tile_pos.x * 16, tile_pos.y * 16, 16, 16), Color.White);
                }
            }
            Draw(player.sprite, player.GetPosition(), Color.White);
        }


        /// <summary>
        /// if given a list of enemies draw them
        /// </summary>
        /// <param name="enemies"></param>
        public void Draw(Enemy[] enemies)
        {
            foreach (Enemy enemy in enemies)
                Draw(enemy);
        }


        /// <summary>
        /// draw every particle within the grimoire
        /// </summary>
        /// <param name="grimoire"></param>
        public void Draw(Grimoire grimoire)
        {
            if (_Sprites.BaseSpell == null) return;
            foreach (var particle in grimoire.Particles)
                Draw(particle);
        }


        /// <summary>
        /// draw the given particle
        /// </summary>
        /// <param name="particle"></param>
        public void Draw(Particle particle)
        {
            if (Variables.DebugCollision)
                this.DrawCircle(new CircleF(new Point2(particle.position.X, particle.position.Y), (float)particle.radius), 16, Color.LightBlue);
            this.Draw(particle.sprite, particle.position, Color.White, particle.rotation, 1, true);
        }
    }
}
