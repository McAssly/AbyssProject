﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.Entities.Enemies
{
    internal class Rat : Entity
    {
        public Rat() { }
        public Rat(float x, float y) : base(x, y)
        {
            this.draw_obj = new Rectangle(0,0,16,16);
            this.max_health = 5;
            this.health = this.max_health;
            this.damage = 5;
            this.crit_dmg = 0.2;
            this.crit_rate = 0.33;
            this.defense = 1;
            this.resistence = 3;

            this.attack_cooldown = 0;
            this.attack_cooldown_max = 1;

            UpdateDrawObj();
        }
    }
}
