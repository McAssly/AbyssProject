
using Abyss.Entities;
using Abyss.Globals;
using Abyss.Utility;
using Microsoft.Xna.Framework;
using System;

namespace Abyss.Magic.Grimoires
{
    internal class Wind : Grimoire
    {
        public Wind() : base()
        {
            primary = new ParticleController(Element.wind, 0.1, 2, 1, 7, 0, 0.2, true);
            secondary = new ParticleController(Element.wind, 0.3, 0, 1, 5, 0, 0.8, false);

            sprite = _Sprites.WindSpell;
            sprite_2 = _Sprites.WindDashSpell;
        }


        public override void Primary(Entity parent, Vector2 target_pos)
        {
            Vector2 target = Math0.MoveToward(parent.GetPosition(), target_pos, primary.base_speed);
            GenerateParticle(parent, Vector2.Subtract(target, parent.GetPosition()), 0, Math.Atan2(target.Y - parent.GetPosition().Y, target.X - parent.GetPosition().X));
        }

        public override void Secondary(Entity parent, Vector2 target_pos)
        {
            base.Secondary(parent, target_pos);
        }

        public override string ToString()
        {
            return "wind";
        }
    }
}
