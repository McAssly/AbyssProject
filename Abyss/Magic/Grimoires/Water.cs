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
            primary = new ParticleController(Element.water, 1.0, 3, 3, 3, 0.4, 0.7);
            secondary = new ParticleController(Element.water, 0, 0, 0, 10, 0, 1.0);

            sprite = _Sprites.WaterSpell;
            sprite_2 = sprite;
        }


        public override void Primary(Entity parent, Vector2 target_pos, double delta)
        {
            Vector2 target = Math0.MoveToward(parent.GetPosition(), target_pos, primary.base_speed);
            GenerateParticle(parent, Vector2.Subtract(target, parent.GetPosition()), 0, Math.Atan2(target.Y - parent.GetPosition().Y, target.X - parent.GetPosition().X), 3);
        }


        public override void Secondary(Entity parent, Vector2 target_pos, double delta)
        {
            parent.statuses.Add(secondary_status);
        }

        public override void Attack(Player parent, Vector2 targetPos, int type, double delta)
        {
            switch (type)
            {
                case 1: // primary
                    if (primary.cooldown <= 0 && parent.GetMana() >= primary.mana_cost)
                    {
                        Primary(parent, targetPos, delta);
                        parent.ReduceMana(primary.mana_cost);
                        primary.cooldown = primary.cooldown_max;
                    }
                    break;
                case 2: // secondary
                    if (secondary.cooldown <= 0 && parent.GetMana() >= secondary.mana_cost && !parent.statuses.Exists(status => status.application_id == 0))
                    {
                        Secondary(parent, targetPos, delta);
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
}
