using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.Entities.Magic
{
    internal class WaterGrimoire : Grimoire
    {
        public WaterGrimoire() : base()
        {
            primary = new ParticleController(Element.water, 1.0, 2, 1, 1, 0.4, 0.05);
            secondary = new ParticleController(Element.water, 0, 0, 0, 5, 0, 1.0);

            sub_particles = new SubParticle[2]
            {
                new SubParticle(0, 0, 0, 0, 0, 0),
                new SubParticle(-2,-2,0,0,0,1.0)
            };
        }


        public override void Secondary(Entity parent, Vector2 target_pos)
        {
            StatusEffect effect = new StatusEffect(0.6, 5, 0);
            if (!parent.statuses.Exists(status => status.application_id == 0))
                parent.statuses.Add(effect);
        }



        public override string ToString()
        {
            return "water";
        }
    }

    internal class WindGrimoire : Grimoire
    {
        public override string ToString()
        {
            return "wind";
        }
    }

    internal class EarthGrimoire : Grimoire
    {
        public override string ToString()
        {
            return "earth";
        }
    }

    internal class FireGrimoire : Grimoire
    {
        public override string ToString()
        {
            return "fire";
        }
    }

    internal class LightningGrimoire : Grimoire
    {
        public override string ToString()
        {
            return "lightning";
        }
    }
}
