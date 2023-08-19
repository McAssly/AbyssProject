using Abyss.Globals;
using Abyss.Levels;
using Abyss.Master;
using Abyss.Sprites;
using Abyss.Utility;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Abyss.Entities
{
    internal class Enemy : Entity
    {
        // declare the enemy attack cooldown
        public double attack_cooldown = 0;
        public double attack_cooldown_max;


        //declare movement cooldowns
        public double move_cooldown = 0;
        public double move_cooldown_max = 0;

        // declare the enemy's range
        public double range;


        public Vector2 target = new Vector2();

        /// <summary>
        /// initialize the base stats of the enemy
        /// </summary>
        public virtual void Initialize()
        {

        }

        /// <summary>
        /// the default enemy AI simply moves and accelerates towards the player
        /// </summary>
        /// <param name="delta"></param>
        /// <param name="game_state"></param>
        public virtual void Update(double delta, GameState game_state)
        {
            this.ClampPosition();
            this.Move(game_state.GetCollisionLayer(), delta);
            if (IsInRange(game_state.player.GetPosition()))
                this.target_vector = (game_state.player.GetPosition() - this.position).NormalizedCopy();
            else
                this.target_vector = Math0.VectorAtAngle(Math0.RandomAngle());

            this.Attack(game_state);
        }


        public virtual void Attack(GameState game_state)
        {
            // deal damage
            if (this.DealDamage(game_state.player) > 0 && this.attack_cooldown <= 0)
            {
                game_state.player.ReduceHealth(this.DealDamage(game_state.player));
                this.attack_cooldown = this.attack_cooldown_max;
            }
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
