using Abyss.Entities;
using Abyss.Utility;
using System;

namespace Abyss.Magic
{
    internal struct StatusEffect
    {
        public double value;
        public Timer timer;

        /*
          0 = crit_rate         PLAYER ONLY
          1 = burn              DOT type effect
         */
        public byte application_id;

        public StatusEffect(double value, double duration, byte id)
        {
            this.value = value;
            this.timer = new Timer(duration);
            this.application_id = id;
        }

        public StatusEffect(double value, Timer timer, byte id)
        {
            this.value = value;
            this.timer = timer;
            this.application_id = id;
        }

        public void Action(Entity parent)
        {
            switch (application_id)
            {
                case 0: break;
                case 1: // apply the DOT effect to the entity
                    if (!parent.regen.IsRunning()) { // regen timer is on a 1 second timer
                        // apply DOT based on the 1 second regen timer
                        parent.TakeDamage(value);
                    }
                    break;
            }
        }

        public void SetValue(double new_value)
        {
            this.value = new_value;
        }

        public readonly override string ToString()
        {
            switch (application_id)
            {
                case 0:
                    return "crit_rate: " + value + " : " + timer;
                case 1:
                    return "burn: " + value + " : " + timer;
                default:
                    return "status not implemented : " + value + " : " + timer;
            }
        }

        internal StatusEffect Clone()
        {
            return new StatusEffect(value, timer, application_id);
        }
    }
}
