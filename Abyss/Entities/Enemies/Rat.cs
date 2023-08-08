using Abyss.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.Entities.enemies
{
    internal class Rat : Enemy
    {
        public override void Initialize()
        {
            this.max_health = 5;
            this.health = this.max_health;
            this.damage = 25;
            this.defense = 1;

            this.attack_cooldown = 0;
            this.attack_cooldown_max = 1;
        }

        public Rat(SpriteSheet sprite, float x, float y) : base(sprite, x, y)
        {
            this.Initialize();
        }

        public Rat(float x, float y) : base(x, y)
        {
            this.Initialize();
        }

        public override void Load()
        {
            this.sprite = Abyss.Globals._Sprites.TestBox;
            this.width = sprite.width - 1;
            this.height = sprite.height - 1;
        }

        public override Rat Clone()
        {
            return new Rat(this.sprite, this.position.X * 16, this.position.Y * 16);
        }
    }
}
