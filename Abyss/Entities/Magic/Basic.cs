using Abyss.Master;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.Entities.Magic
{
    internal class WaterGrimoire : Grimoire
    {
        private protected StatusEffect secondary_status = new StatusEffect(0.6, 5, 0);

        public WaterGrimoire() : base()
        {
            primary = new ParticleController(Element.water, 1.0, 3, 1, 3, 0.4, 0.7);
            secondary = new ParticleController(Element.water, 0, 0, 0, 10, 0, 1.0);

            sub_particles = new SubParticle[1]
            {
                new SubParticle(0, 0, 0, 0, 0, 0.3, Color.White, 1.5, true), // CENTER
            };

            is_connected = false;
        }


        public override void Secondary(Entity parent, Vector2 target_pos)
        {
            parent.statuses.Add(secondary_status);
        }

        public override void Attack(Entity parent, Vector2 targetPos, int type)
        {
            switch (type)
            {
                case 1: // primary
                    if (primary.cooldown <= 0 && parent.GetMana() >= primary.mana_cost)
                    {
                        Primary(parent, targetPos);
                        parent.ReduceMana(primary.mana_cost);
                        primary.cooldown = primary.cooldown_max;
                    }
                    break;
                case 2: // secondary
                    if (secondary.cooldown <= 0 && parent.GetMana() >= secondary.mana_cost && !parent.statuses.Exists(status => status.application_id == 0))
                    {
                        Secondary(parent, targetPos);
                        parent.ReduceMana(secondary.mana_cost);
                        secondary.cooldown = secondary.cooldown_max;
                    }
                    break;
                default: return;
            }
        }



        public override string ToString()
        {
            return "water";
        }
    }

    internal class FireGrimoire : Grimoire
    {
        public FireGrimoire() : base()
        {
            primary = new ParticleController(Element.water, 0.7, 0.1, 1.5, 0.5, 0.1, 0.05, true);
            secondary = new ParticleController(Element.water, 0.3, 0.5, 2, 1, 0.3, 0.05, true);

            sub_particles = new SubParticle[1]
                {
                    new SubParticle(0, 0, 0, 0, 0, 0, Color.White, 2)
                };

            is_connected = false;
        }

        public override void Primary(Entity parent, Vector2 target_pos)
        {
            Vector2 target = MathUtil.MoveToward(parent.GetPosition(), target_pos, primary.base_speed);
            GenerateParticle(parent, Vector2.Subtract(target, parent.GetPosition()), 0, Math.Atan2(target.Y - parent.GetPosition().Y, target.X - parent.GetPosition().X));
        }

        public override void Secondary(Entity parent, Vector2 target_pos)
        {
            double[] rotations = new double[8] { 0.0, 0.785398163397, 1.57079632679, 2.35619449019, 3.14159265359, 3.92699081699, 4.71238898038, 5.49778714378 };
            foreach (double rot in rotations)
                GenerateParticle(parent, MathUtil.VectorAtAngle(rot), 1, rot);
        }

        public override string ToString()
        {
            return "fire";
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

    internal class LightningGrimoire : Grimoire
    {
        public override string ToString()
        {
            return "lightning";
        }
    }
}
