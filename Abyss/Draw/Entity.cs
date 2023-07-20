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
            Draw(Globals.TestBox, entity.GetDrawObj(), Color.White);
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
            Draw(texture, particle.position, null, Color.White, (float)particle.rotation, new Vector2(texture.Width / 2, texture.Height / 2), 1, SpriteEffects.None, 0);
        }
    }
}
