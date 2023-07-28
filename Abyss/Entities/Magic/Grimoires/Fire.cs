using Abyss.Master;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.Entities.Magic.Grimoires
{
    internal class Fire : Grimoire
    {
        public Fire() : base()
        {
            primary = new ParticleController(Element.fire, 0.7, 0.1, 1.5, 0.5, 0.1, 0.05, true);
            secondary = new ParticleController(Element.fire, 0.3, 0.5, 2, 1, 0.3, 0.05, true);

            sprite = Globals.FireSpell;
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
}
