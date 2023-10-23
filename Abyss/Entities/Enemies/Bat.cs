using Abyss.Globals;
using Abyss.Levels;
using Abyss.Magic;
using Abyss.Master;
using Abyss.Sprites;
using Abyss.Utility;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Abyss.Entities.Enemies
{
    internal class Bat : Enemy
    {
        public override void Initialize()
        {
            this.attack = new Attack(
                new ParticleController(0.25, 1, 0, 0, 0, 0, 25)
                );

            this.max_health = 1;
            this.health = max_health;
            this.damage = 50;
            this.defense = 1;
            this.alerted = false;
            this.asleep = true;
            this.speed = 1;
            this.friction = 0;
            this.max_speed = 3;

            this.attack_cooldown = new Timer(1);

            this.move_cooldown = new Timer(1);
            this.wander_cooldown = new Timer(0.2);

            this.range = 50;
            this.sight_range = 0; // ignore
        }


        public override void Alert(double delta, GameState game_state)
        {
            if (this.asleep) this.asleep = false;
            this.Move(game_state.GetCollisionLayer(), delta);
            this.target_vector = (game_state.player.GetPosition() - this.position).NormalizedCopy();
            // they run into the player or a wall -> self destruct
            if (this.CollisionCheck(game_state.GetCollisionLayer(), false) || this.CollisionCheck(game_state.GetCollisionLayer(), true)
                || ((game_state.player.GetPosition() - this.position).LengthSquared() <= 2 * 2))
            {
                target = Vector2.Zero;
                this.Attack(game_state, delta);
                this.alerted = false;
            }

            // never go unalert
        }

        public override void UnAlert(double delta, GameState game_state)
        {
            base.UnAlert(delta, game_state);
        }

        public override void Attack(GameState game_state, double delta)
        {
            base.Attack(game_state, delta);
            this.health = 0;
        }


        public override void TakeDamage(double amount)
        {
            base.TakeDamage(amount);
        }


        public Bat(SpriteSheet sprite, float x, float y) : base(sprite, x, y)
        {
            this.Initialize();
        }

        public Bat(float x, float y) : base(x, y)
        {
            this.Initialize();
        }


        public override void Load()
        {
            this.sprite = Abyss.Globals._Sprites.TestBox;
            this.width = sprite.width - 1;
            this.height = sprite.height - 1;
        }


        public override Bat Clone()
        {
            return new Bat(this.sprite, this.position.X * 16, this.position.Y * 16);
        }
    }
}
