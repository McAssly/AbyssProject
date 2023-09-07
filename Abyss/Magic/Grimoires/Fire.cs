using Abyss.Entities;
using Abyss.Globals;
using Abyss.Utility;
using Microsoft.Xna.Framework;
using System;

namespace Abyss.Magic.Grimoires
{
    internal class Fire : Grimoire
    {
        public Fire() : base()
        {
            primary = new ParticleController(Element.fire, 0.7, 0.1, 1.5, 0.5, 0.1, 0.05, 3, true);
            secondary = new ParticleController(Element.fire, 0.3, 5, 2, 10, 0.3, 0.25, 9, true);

            sprite = _Sprites.FireSpell;
            sprite_2 = _Sprites.FireBurstSpell;
        }

        public override void Primary(Entity parent, Vector2 target_pos, double delta)
        {
            (parent as Player).damage_mult = dmg_multiplier_1;
            Vector2 target = Math0.MoveToward(parent.GetPosition(), target_pos, primary.base_speed);
            GenerateParticle(parent, Math0.OffsetDirection(Vector2.Subtract(target, parent.GetPosition()), 0.174), 0, Math.Atan2(target.Y - parent.GetPosition().Y, target.X - parent.GetPosition().X), 8);
        }

        public override void Secondary(Entity parent, Vector2 target_pos, double delta)
        {
            (parent as Player).damage_mult = dmg_multiplier_2;
            double[] rotations = new double[8] { 0.0, 0.785398163397, 1.57079632679, 2.35619449019, 3.14159265359, 3.92699081699, 4.71238898038, 5.49778714378 };
            foreach (double rot in rotations)
                GenerateParticle(parent, Math0.VectorAtAngle(rot), 1, rot);
        }

        public override string ToString()
        {
            return "fire";
        }
    }
}
