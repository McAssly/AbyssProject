using Abyss.Sprite;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.Entities.Magic.Infusions
{
    internal class WaterFire : Infused
    {
        private protected StatusEffect boilled_effect = new StatusEffect(1.5, 5, 2);

        public WaterFire() : base()
        {
            L2_R2 = new ParticleController(Element.NULL, 0.5, 0, 0.3, 5, 0.1, 1.0);
            L1_R1 = new ParticleController(Element.NULL, 0.2, 1, 2, 3, 1, 0.1);
            L1_R2 = new ParticleController(Element.NULL, 0.5, 10, 2, 15, 0.3, 1.0);

            sprite0 = Sprites.BoilSpell;
            sprite1 = Sprites.SteamSpell;
        }
    }
}
