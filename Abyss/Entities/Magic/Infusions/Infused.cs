using Abyss.Draw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.Entities.Magic.Infusions
{
    internal class Infused : Grimoire
    {
        // all infusion based particles
        /*
         1 = the primary spell of the grimoire
         2 = the secondary spell of the grimoire

         L = the grimoire labeled on the left of the infused grimoire
         R = the grimoire labeled on the right of the infused grimoire
         */
        private protected ParticleController L1_R1; // ex: water 1 + fire 1, if the grimoire class is called WaterFire
        private protected ParticleController L1_R2;
        private protected ParticleController L2_R1;
        private protected ParticleController L2_R2;

        // two spell sprites
        private protected AnimatedSprite sprite0;
        private protected AnimatedSprite sprite1;
    }
}
