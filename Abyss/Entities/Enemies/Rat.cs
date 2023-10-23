using Abyss.Globals;
using Abyss.Levels;
using Abyss.Magic;
using Abyss.Master;
using Abyss.Sprites;
using Abyss.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Abyss.Entities.Enemies
{
    internal class Rat : Enemy
    {
        private List<Enemy> swarmed;

        public override void Initialize()
        {
            this.attack = new Attack(
                new ParticleController(0.25, 1, 0, 0, 0, 0, 10, false, true, true)
                );

            this.max_health = 3;
            this.health = this.max_health;
            this.damage = 25;
            this.defense = 1;
            this.alerted = false;
            this.asleep = false;
            this.speed = 0.5;

            this.attack_cooldown = new Timer(1);

            this.move_cooldown = new Timer(1);
            this.wander_cooldown = new Timer(0.7);

            this.range = 70;
            this.sight_range = 1.18;

            swarmed = new List<Enemy>();
        }

        public override void Alert(double delta, GameState game_state)
        {
            // get the target position to lunge towards
            if (target == Vector2.Zero) target = game_state.player.GetPosition();

            // get the movement vector
            target_vector = target - this.position;
            if (Math.Floor(target_vector.LengthSquared()) != 0) target_vector.Normalize();

            // charge towards the target
            this.Charge(game_state.GetCollisionLayer(), delta);

            // reached target
            if (target_vector == Vector2.Zero)
            {
                target = Vector2.Zero;
                this.Attack(game_state, delta);
                this.alerted = false;
            }
        }

        public override void UnAlert(double delta, GameState game_state)
        {
            base.UnAlert(delta, game_state);
            game_state.AddEnemies(swarmed);
            swarmed.Clear();
        }


        private void Charge(Layer collision_layer, double delta)
        {
            velocity = target_vector * (float)max_speed * (float)speed * 2;
            this.Collide(collision_layer);
            position += velocity * new Vector2((float)(delta * Variables.FRAME_FACTOR));
        }


        public override void TakeDamage(double amount)
        {
            base.TakeDamage(amount);
            // add extra functionality
            // took damage:
            if (!this.alerted && !attack_cooldown.IsRunning()) // if the rat was unalert at the time of taking damage
            {
                // summon a new rat with the same health and stats
                Rat new_rat = this.Clone();
                new_rat.SetPosition(this.position - Math0.VectorAtAngle(Math0.RandomAngle()) * 3);
                new_rat.health = new_rat.health / 4 - 1;
                swarmed.Add(new_rat);
            }
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
