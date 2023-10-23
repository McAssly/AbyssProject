using Abyss.Globals;
using Abyss.Master;
using Abyss.Sprites;
using Microsoft.Xna.Framework;
namespace Abyss.Magic
{
    internal class Effect
    {
        public Vector2 position;
        public AnimatedSprite sprite;
        private protected double lifetime;
        public double rotation;


        public Effect(Vector2 position, AnimatedSprite sprite, double lifetime, double rotation = 0)
        {
            this.position = position;
            this.sprite = sprite;
            this.lifetime = lifetime;
            this.rotation = rotation;
        }

        public void Update(double delta)
        {
            sprite.Update(delta);
            lifetime -= delta;
        }

        public bool IsDead()
        {
            return lifetime <= 0;
        }

        public static void HitEffect(Vector2 position, double rotation, byte element, GameState game_state)
        {
            switch (element)
            {
                case 1: // fire effect
                    {
                        game_state.particle_fx.Add(new Effect(position, _Sprites.ExplosionEffect.Clone(), 0.2, rotation));
                        break;
                    }
                default:
                    {
                        game_state.particle_fx.Add(new Effect(position, _Sprites.BurstEffect.Clone(), 0.28));
                        break;
                    }
            }
        }
    }
}
