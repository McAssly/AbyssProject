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
using System.Reflection.Metadata.Ecma335;
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

            this.move_cooldown_max = 10;

            this.range = 70;
        }


        public override void Update(double delta, GameState game_state)
        {
            this.ClampPosition();
            // move towards the player if they are in range
            if (IsInRange(game_state.player.GetPosition()))
            {
                // get the target position to lunge towards
                if (target == Vector2.Zero) target = game_state.player.GetPosition();

                // get the movement vector
                target_vector = target - this.position;
                if (Math.Floor(target_vector.Length()) != 0) target_vector.Normalize();
                
                // charge towards the target
                this.Charge(game_state.GetCollisionLayer(), delta);

                // reached target
                if (target_vector == Vector2.Zero)
                {
                    target = Vector2.Zero;
                    this.Attack(game_state);
                }
            }
            else
            {
                this.Move(game_state.GetCollisionLayer(), delta);
                target_vector = Math0.VectorAtAngle(Math0.RandomAngle());
            }
        }


        private void Charge(Layer collision_layer, double delta)
        {
            velocity = target_vector * (float)max_speed * (float)speed;
            if (velocity.X != 0 && this.CollisionCheck(collision_layer, new Vector2(velocity.X, 0)))
                velocity.X = 0;
            if (velocity.Y != 0 && this.CollisionCheck(collision_layer, new Vector2(0, velocity.Y)))
                velocity.Y = 0;
            position += velocity * new Vector2((float)(delta * Variables.FRAME_FACTOR));
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
