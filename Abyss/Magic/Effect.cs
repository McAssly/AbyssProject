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


        public Effect(Vector2 position, AnimatedSprite sprite, double lifetime)
        {
            this.position = position;
            this.sprite = sprite;
            this.lifetime = lifetime;
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

        public static void BurstEffect(Vector2 position, GameState game_state)
        {
            game_state.particle_fx.Add(new Effect(position, _Sprites.BurstEffect.Clone(), 0.28));
        }
    }
}
