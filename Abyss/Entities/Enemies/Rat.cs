using Abyss.Sprite;
using Microsoft.Xna.Framework;
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

        public void Initialize()
        {
            this.max_health = 5;
            this.health = this.max_health;
            this.damage = 25;
            this.crit_dmg = 0.0;
            this.crit_rate = 0.0;
            this.defense = 1;

            this.attack_cooldown = 0;
            this.attack_cooldown_max = 1;
        }

        public Rat(SpriteSheet sprite, float x, float y) : base(sprite, x, y)
        {
            this.sprite = sprite;
            this.width = this.sprite.width - 1;
            this.height = this.sprite.height - 1;
            this.Initialize();
        }

        public Rat(float x, float y) : base(x,y)
        {
            this.Initialize();
        }

        public override void Load()
        {
            this.sprite = Sprites.TestBox;
            this.width = sprite.width - 1;
            this.height = sprite.height - 1;
        }

        public override Entity Clone()
        {
            return new Rat(this.sprite, this.pos.X * 16, this.pos.Y * 16);
        }
    }
}
