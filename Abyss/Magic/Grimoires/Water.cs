using Abyss.Entities;
using Abyss.Globals;
using Abyss.Utility;
using Microsoft.Xna.Framework;
using System;

namespace Abyss.Magic.Grimoires
{
    internal class Water : Grimoire
    {
        private protected StatusEffect secondary_status = new StatusEffect(0.6, 5, 0);

        public Water() : base()
        {
            primary = new ParticleController(1.0, 3, 3, 3, 0.4, 0.25, 4);
            secondary = new ParticleController(0, 0, 0, 10, 0, 1.0);

            sprite = _Sprites.WaterSpell;
            sprite_2 = sprite;
        }


        public override void Primary(Entity parent, Vector2 target_pos, double delta)
        {
            (parent as Player).damage_mult = dmg_multiplier_1;
            Vector2 target = Math0.MoveToward(parent.GetPosition(), target_pos, primary.base_speed);
            GenerateParticle(parent, Vector2.Subtract(target, parent.GetPosition()), 0, Math.Atan2(target.Y - parent.GetPosition().Y, target.X - parent.GetPosition().X), 3);
        }


        public override void Secondary(Entity parent, Vector2 target_pos, double delta)
        {
            parent.AddStatus(secondary_status);
        }


        internal override bool SecondaryCheck(Player parent)
        {
            return !parent.statuses.Exists(status => status.application_id == 0);
        }



        public override string ToString()
        {
            return "water";
        }
    }
}
