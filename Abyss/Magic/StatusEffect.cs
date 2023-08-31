using Abyss.Entities;

namespace Abyss.Magic
{
    internal struct StatusEffect
    {
        public double value;
        public double timer;

        /*
          0 = crit_rate                         ONLY EFFECTS PLAYER
          1 = crit_dmg                          ONLY EFFECTS PLAYER
          2 = base_dmg
          3 = base_spd
          4 = regen_hp      // action based
          5 = regen_mana    // action based     ONLY EFFECTS PLAYER
          6 = friction
         */
        public byte application_id;

        public StatusEffect(double value, double timer, byte id)
        {
            this.value = value;
            this.timer = timer;
            this.application_id = id;
        }

        public void Action(Entity parent)
        {
            if (parent.regen_timer >= 1)
            {
                switch (application_id)
                {
                    case 4:
                        parent.ReduceHealth(value);
                        break;
                    case 5:
                        if (parent is Player) (parent as Player).ReduceMana(value);
                        break;
                    default: break;
                }
                parent.regen_timer = 0;
            }
        }

        public void LowerTimer(double delta)
        {
            this.timer -= delta;
        }

        public readonly override string ToString()
        {
            switch (application_id)
            {
                case 0:
                    return "crit_rate: " + value + " : " + timer;
                case 1:
                    return "crit_dmg: " + value + " : " + timer;
                case 2:
                    return "damage: " + value + " : " + timer;
                case 3:
                    return "speed: " + value + " : " + timer;
                case 4:
                    return "regen_hp: " + value + " : " + timer;
                case 5:
                    return "regen_mn: " + value + " : " + timer;
                case 6:
                    return "friction: " + value + " : " + timer;
                default: return "";
            }
        }
    }
}
