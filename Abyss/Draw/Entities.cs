using Abyss.Entities;
using Abyss.Globals;
using Abyss.Magic;
using Abyss.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System.Diagnostics;

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
            if (Variables.DebugCollision)
                this.DrawRectangle(new Rectangle(
                    (int)(enemy.GetPosition().X + enemy.GetTargetVector().X * 2),
                    (int)(enemy.GetPosition().Y + enemy.GetTargetVector().Y * 2),
                    16, 16), Color.White);
            if (!enemy.alerted && !enemy.attack_cooldown.IsRunning())
            {
                Draw(enemy.sprite, enemy.GetPosition(), (enemy.asleep) ? Color.Blue : Color.Red);
            }
            else if (enemy.attack_cooldown.IsRunning() && !enemy.alerted) Draw(enemy.sprite, enemy.GetPosition(), Color.Yellow);
            else Draw(enemy.sprite, enemy.GetPosition(), Color.Magenta);
        }

        /// <summary>
        /// draws the given player
        /// </summary>
        /// <param name="player"></param>
        public void Draw(Player player)
        {
            if (Variables.DebugCollision)
            {
                for (int i = 0; i < 2; i++)
                {
                    foreach (Vector tile_pos in player.CurrentTilePositions((i == 0) ? true : false))
                        this.DrawRectangle(new Rectangle(tile_pos.x * 16, tile_pos.y * 16, 16, 16), Color.White);
                    foreach (Vector tile_pos in player.GetNextTiles((i == 0) ? true : false))
                        this.DrawRectangle(new Rectangle(tile_pos.x * 16, tile_pos.y * 16, 16, 16), (i == 0) ? Color.Yellow : Color.GreenYellow);
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
            {
                Draw(enemy);
                Draw(enemy.attack);
            }
        }


        /// <summary>
        /// draw every particle within the grimoire
        /// </summary>
        /// <param name="grimoire"></param>
        public void Draw(Grimoire grimoire)
        {
            if (_Sprites.BaseSpell == null) return;
            foreach (var particle in grimoire.particles)
                Draw(particle);
        }


        public void Draw(Attack attack)
        {
            if (attack.draw_sprite)
                foreach (Particle p in attack.particles)
                    Draw(p);
            if (Variables.DebugCollision)
                foreach (Particle p in attack.particles)
                    this.DrawCircle(new CircleF(new Point2(p.position.X, p.position.Y), (float)p.radius), 16, Color.LightPink);
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
