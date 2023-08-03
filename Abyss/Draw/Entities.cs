using Abyss.Entities;
using Abyss.Entities.Magic;
using Abyss.Map;
using Abyss.Master;
using Abyss.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Vector = Abyss.Master.Vector;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Abyss.Draw
{
    internal partial class DrawBatch : SpriteBatch
    {
        public void Draw(Player player)
        {
            if (Globals.DebugCollision)
            {
                Vector[] tile_positions = player.GenerateTilePositions();
                foreach (Vector tile_pos in tile_positions)
                {
                    this.DrawRectangle(new Rectangle(tile_pos.x * 16, tile_pos.y * 16, 16, 16), Color.White);
                }
            }
            Draw(player.sprite, player.GetPosition(), Color.White);
        }

        public void Draw(List<Entity> entities)
        {
            foreach (Entity entity in entities)
                Draw(entity);
        }

        public void Draw(Entity entity)
        {
            Draw(entity.sprite, entity.GetPosition(), Color.Red);
            this.DrawPoint(entity.GetPosition(), Color.Blue);
            this.DrawPoint(entity.GetPosition() + entity.GetSize(true, false), Color.Blue);
            this.DrawPoint(entity.GetPosition() + entity.GetSize(false, true), Color.Blue);
            this.DrawPoint(entity.GetPosition() + entity.GetSize(), Color.Blue);
        }
        public void Draw(Grimoire grimoire)
        {
            if (Sprites.BaseSpell == null) return;
            foreach (var particle in grimoire.Particles)
                Draw(particle);
        }
        public void DrawLinesBetween(List<Particle> particles)
        {
            for (int i = 0; i < particles.Count - 1; i++)
                this.DrawLine(particles[i].position, particles[i + 1].position, Color.White);
        }

        public void Draw(Particle particle)
        {
            this.Draw(particle.sprite, particle.position, Color.White, particle.rotation, 1, true);
        }
    }
}
