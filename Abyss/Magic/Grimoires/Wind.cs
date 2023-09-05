
using Abyss.Entities;
using Abyss.Globals;
using Abyss.Utility;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Diagnostics;

namespace Abyss.Magic.Grimoires
{
    internal class Wind : Grimoire
    {
        public Wind() : base()
        {
            primary = new ParticleController(Element.wind, 0.1, 2, 0.5, 7, 0, 0.2, true);
            secondary = new ParticleController(Element.wind, 0.1, 1, 800, 5, 0, 0.3, true, true, true);

            sprite = _Sprites.WindSpell;
            sprite_2 = _Sprites.WindDashSpell;
        }


        public override void Primary(Entity parent, Vector2 target_pos, double delta)
        {
            Vector2 target = Math0.MoveToward(parent.GetPosition(), target_pos, primary.base_speed);
            GenerateParticle(parent, Vector2.Subtract(target, parent.GetPosition()), 0, Math.Atan2(target.Y - parent.GetPosition().Y, target.X - parent.GetPosition().X));
        }

        public override void Secondary(Entity parent, Vector2 target_pos, double delta)
        {
            // override the target position
            if (parent.GetTargetVector() == Vector2.Zero) return;
            target_pos = parent.GetPosition() + parent.GetTargetVector() * (float)secondary.base_speed / 100;
            Vector2 target = Math0.MoveToward(parent.GetPosition(), target_pos, secondary.base_speed / 100);
            Vector2 velocity = Math0.MoveToward(parent.GetPosition(), target_pos, secondary.base_speed * delta);
            GenerateParticle(parent, Vector2.Subtract(target, parent.GetPosition()), 1, Math.Atan2(target.Y - parent.GetPosition().Y, target.X - parent.GetPosition().X));
            parent.AddVelocity(Vector2.Subtract(velocity, parent.GetPosition()));
            parent.SetMaxSpeed(100, secondary.lifetime*2);
            parent.SetFriction(0, secondary.lifetime*2);
        }

        public override string ToString()
        {
            return "wind";
        }
    }
}
