using Abyss.Sprite;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.Entities.Magic.Grimoires
{
    internal class Water : Grimoire
    {
        private protected StatusEffect secondary_status = new StatusEffect(0.6, 5, 0);

        public Water() : base()
        {
            primary = new ParticleController(Element.water, 1.0, 3, 3, 3, 0.4, 0.7);
            secondary = new ParticleController(Element.water, 0, 0, 0, 10, 0, 1.0);

            sprite = Sprites.WaterSpell;
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
}
