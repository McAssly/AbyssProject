using Abyss.Entities;
using Abyss.Sprites;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Abyss.Magic
{
    internal class Magic
    {
        public List<Particle> particles = new List<Particle>();
        private protected byte elemental_type = 0;

        public Particle Hits(Entity entity)
        {
            Particle dealer = particles.Find(x => x.IsColliding(entity));
            if (dealer != null)
            {
                if (!dealer.pierce) particles.Remove(dealer);
                if (dealer.pierce) dealer.ReduceDamage();
                return dealer;
            }
            return null;
        }

        public void Add(ParticleController controller, AnimatedSprite sprite, Entity parent, Vector2 velocity, double rotation, double dmg_mult, float padding = 0)
        {
            particles.Add(
                new Particle(
                    parent, 
                    parent.GetPosition() + new Vector2((parent.sprite.width + 1) / 2, 
                    (parent.sprite.height + 1) / 2) + padding * velocity, 
                    velocity,
                    controller, 
                    parent.CalculateDamage(controller.base_damage), 
                    rotation, 
                    (sprite != null) ? sprite.Clone() : null, 
                    dmg_mult));
        }

        public void Clear()
        {
            particles.Clear();
        }

        public byte GetElement()
        {
            return this.elemental_type;
        }
    }
}
