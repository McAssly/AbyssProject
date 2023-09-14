using Abyss.Magic;
using Abyss.Master;
using Abyss.Sprites;
using Abyss.Utility;
using MonoGame.Extended;
using System;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Abyss.Entities
{
    internal class Enemy : Entity
    {
        // declare the enemy's attack
        public Attack attack;

        // declare the enemy attack cooldown
        public Timer attack_cooldown;

        // declare if the enemy's position is locked to the tile
        public bool tile_locked;

        // declare if the enemy is alerted of the player
        public bool alerted;

        // declare the range in which the enemy can see
        public double sight_range;

        //declare movement cooldowns
        public Timer move_cooldown;
        public Timer wander_cooldown;

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
        public void Update(double delta, GameState game_state)
        {
            this.attack.Update(delta, game_state);
            this.Clamp();
            this.invulnerability.Update(delta);
            attack_cooldown.Update(delta);
            if (this.target_vector != Vector2.Zero)
                this.angle = Math0.AngleAtVector(target_vector);

            if (this.alerted)
                this.Alert(delta, game_state);
            else
                this.UnAlert(delta, game_state);

            if (IsInRange(game_state.player.GetPosition()) && !attack_cooldown.IsRunning())
                this.alerted = true;
        }

        public virtual void Alert(double delta, GameState game_state)
        {
            this.Move(game_state.GetCollisionLayer(), delta);
            this.target_vector = (game_state.player.GetPosition() - this.position).NormalizedCopy();
            // reached target
            if ((game_state.player.GetPosition() - this.position).LengthSquared() <= 2 * 2)
            {
                target = Vector2.Zero;
                this.Attack(game_state, delta);
                this.alerted = false;
            }
            if (!IsInRange(game_state.player.GetPosition())) this.alerted = false;
        }

        public virtual void UnAlert(double delta, GameState game_state)
        {
            this.Move(game_state.GetCollisionLayer(), delta);
            if (!this.wander_cooldown.IsRunning())
            {
                target_vector = Math0.VectorAtAngle(Math0.RandomAngle());
                this.wander_cooldown.Start();
            }
            this.wander_cooldown.Update(delta);
        }

        public virtual void Attack(GameState game_state, double delta)
        {
            this.attack.AddParticle(this, Vector2.Zero, this.angle);
            this.attack_cooldown.Start();
        }



        public bool IsInRange(Vector2 position)
        {
            return ((position - this.position).LengthSquared() <= range * range) 
                && Math0.AngleInRange(Math0.AngleBetweenVectors(this.position, position), FacingAngle() - sight_range, FacingAngle() + sight_range);
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
