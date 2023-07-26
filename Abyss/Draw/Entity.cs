using Abyss.Entities;
using Abyss.Entities.Magic;
using Abyss.Master;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = Microsoft.Xna.Framework.Color;

namespace Abyss.Draw
{
    internal partial class DrawBatch : SpriteBatch
    {
        public void Draw(Player player) // placeholder
        {
            Draw(Globals.TestBox, player.GetDrawObj(), null, Color.White);
        }

        public void Draw(List<Entity> entities)
        {
            foreach (Entity entity in entities)
                Draw(entity);
        }

        public void Draw(Entity entity)
        {
            Draw(Globals.TestBox, entity.GetDrawObj(), null, Color.Red);
        }

        public void Draw(Grimoire grimoire)
        {
            if (Globals.BaseSpell == null) return;
            foreach (var particle in grimoire.Particles)
                Draw(particle, Globals.BaseSpell);
            if (grimoire.is_connected)
                DrawLinesBetween(grimoire.Particles);
        }

        public void DrawLinesBetween(List<Particle> particles)
        {
            for (int i = 0; i < particles.Count - 1; i++)
                this.DrawLine(particles[i].position, particles[i + 1].position, Color.White);
        }

        public void Draw(Particle particle, Texture2D texture)
        {
            foreach (SubParticle sub_particle in particle.particles)
                this.Draw(sub_particle, particle, texture);
            if (particle.connection != null)
                this.DrawLine(particle.position, particle.connection.position, Color.White);
        }

        public void Draw(SubParticle sub_particle, Particle parent, Texture2D texture)
        {
            Draw(texture, parent.position + sub_particle.displacement, null, sub_particle.color, (float)parent.rotation, new Vector2(texture.Width / 2, texture.Height / 2), (float)sub_particle.scale, SpriteEffects.None, 0);
        }
    }
}
