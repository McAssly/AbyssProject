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

        public Rat(Texture2D texture, float x, float y) : base(texture, x, y)
        {
            this.texture = texture;
            this.width = this.texture.Width;
            this.height = this.texture.Height;
            this.Initialize();
        }

        public Rat(float x, float y) : base(x,y)
        {
            this.Initialize();
        }

        public override void Load()
        {
            this.texture = Sprites.TestBox;
            this.width = texture.Width;
            this.height = texture.Height;
        }

        public override Entity Clone()
        {
            return new Rat(this.texture, this.pos.X * 16, this.pos.Y * 16);
        }
    }
}
