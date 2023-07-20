using Abyss.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.Draw
{
    internal partial class DrawBatch : SpriteBatch
    {
        public void Draw(Entity entity) // placeholder
        {
            Draw(Globals.TestBox, entity.GetDrawObj(), null, Color.White);
        }

        public void Draw(Grimoire grimoire)
        {
            if (Globals.BaseSpell == null) return;
            foreach (var particle in grimoire.Particles)
            {
                Draw(particle, Globals.BaseSpell);
            }
        }

        public void Draw(Particle particle, Texture2D texture)
        {
            foreach (SubParticle sub_particle in particle.particles)
            {
                this.Draw(sub_particle, particle, texture);
            }
        }

        public void Draw(SubParticle sub_particle, Particle parent, Texture2D texture)
        {
            Draw(texture, parent.position + sub_particle.displacement, null, Color.White, (float)parent.rotation, new Vector2(texture.Width / 2, texture.Height / 2), 1, SpriteEffects.None, 0);
        }

        public void Draw(SubParticle particle, Texture2D texture)
        {

        }
    }
}
