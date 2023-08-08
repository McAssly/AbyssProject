using Abyss.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.Entities
{
    internal class Enemy : Entity
    {
        // declare the enemy attack cooldown
        public double attack_cooldown;
        public double attack_cooldown_max;

        /// <summary>
        /// initialize the base stats of the enemy
        /// </summary>
        public virtual void Initialize()
        {

        }

        public Enemy(SpriteSheet sprite, float x, float y) : base(sprite, x, y)
        {
            this.Initialize();
        }

        public Enemy(float x, float y) : base(x, y)
        {
            this.Initialize();
        }

        public override Enemy Clone()
        {
            return new Enemy(this.sprite, this.position.X * 16, this.position.Y * 16);
        }
    }
}
