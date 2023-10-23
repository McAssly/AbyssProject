using Abyss.Entities;
using Abyss.Globals;
using Abyss.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Abyss.Magic.Grimoires
{
    internal class Fire : Grimoire
    {
        private protected StatusEffect burn_effect = new StatusEffect(1.3, 4, 1);
        /// <summary>
        /// links to the enemies burned and determines the actual DOT damage, for updating
        /// should be optimized later ;p
        /// </summary>
        private protected Dictionary<Entity, StatusEffect> burned = new Dictionary<Entity, StatusEffect>();

        private protected ParticleController burn_extension = new ParticleController(0.25, 0, 0, 0, 0, 0, 5, true);

        public Fire() : base()
        {
            primary = new ParticleController(1, 0, 2, 5, 0.25, 0.5, 2, true);
            
            // used for burst radius
            secondary = new ParticleController(0.25, 5, 0, 10, 0, 4, 8, true);

            this.elemental_type = 1; // set to fire type

            sprite = _Sprites.FireSpell; // need to be updated (but for now I will leave them the same)
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
