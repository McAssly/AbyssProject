using Abyss.Master;
using Abyss.Sprites;
using Abyss.Utility;
using MonoGame.Extended;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Abyss.Entities
{
    internal class Enemy : Entity
    {
        // declare the enemy attack cooldown
        public Timer attack_cooldown;


        //declare movement cooldowns
        public Timer move_cooldown;

        // declare the enemy's range
        public double range;


        public Vector2 target = new Vector2();

        /// <summary>
        /// initialize the base stats of the enemy
        /// </summary>
        public virtual void Initialize() { }

        /// <summary>
        /// the default enemy AI simply moves and accelerates towards the player
        /// </summary>
        /// <param name="delta"></param>
        /// <param name="game_state"></param>
        public virtual void Update(double delta, GameState game_state)
        {
            this.UniversalUpdate(delta, game_state);
            this.Move(game_state.GetCollisionLayer(), delta);
            if (IsInRange(game_state.player.GetPosition()))
                this.target_vector = (game_state.player.GetPosition() - this.position).NormalizedCopy();
            else
                this.target_vector = Math0.VectorAtAngle(Math0.RandomAngle());

            this.Attack(game_state, delta);
        }

        public void UniversalUpdate(double delta, GameState game_state)
        {
            this.Clamp();
            this.invulnerability.Update(delta);
            attack_cooldown.Update(delta);
        }

        public virtual void Attack(GameState game_state, double delta)
        {
            if (this.DealDamage(game_state.player) > 0)
                game_state.player.TakeDamage(this.DealDamage(game_state.player));
            this.attack_cooldown.Start();
        }



        public bool IsInRange(Vector2 position)
        {
            return (position - this.position).Length() <= range;
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
