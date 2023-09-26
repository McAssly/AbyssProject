using Abyss.Levels;
using Abyss.Magic;
using Abyss.Master;
using Abyss.Sprites;
using Abyss.Utility;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;

namespace Abyss.Entities.Enemies
{
    internal class KnightStatue : Enemy
    {
        private protected Timer state_timer;
        private protected Timer asleep_delay;

        public override void Initialize()
        {
            this.attack = new Attack(
                new ParticleController(Element.NULL, 1, 1, 0, 0, 0, 0, 12)
                );

            this.max_health = 5;
            this.health = max_health;
            this.damage = 25;
            this.defense = 1;
            this.alerted = false;
            this.asleep = true;
            this.speed = 1;

            this.attack_cooldown = new Timer(1);
            this.state_timer = new Timer(5);
            this.asleep_delay = new Timer(1.5);
            this.asleep_delay.Start();

            this.move_cooldown = new Timer(1);
            this.wander_cooldown = new Timer(0.2);

            this.range = 64;
            this.sight_range = 0; // ignore
        }


        public override void Update(double delta, GameState game_state)
        {
            this.attack.Update(delta, game_state);
            this.Clamp();
            this.invulnerability.Update(delta);
            this.state_timer.Update(delta);
            this.move_cooldown.Update(delta);
            this.asleep_delay.Update(delta);
            attack_cooldown.Update(delta);
            if (this.target_vector != Vector2.Zero)
                this.angle = Math0.AngleAtVector(target_vector);

            if (this.alerted)
                this.Alert(delta, game_state);
            else
                this.UnAlert(delta, game_state);

            if (IsInRange(game_state.player.GetPosition()) && !asleep_delay.IsRunning())
                this.asleep = false;
        }

        public override void Alert(double delta, GameState game_state)
        {
            this.target_vector = game_state.player.GetPosition();
            if (Math0.RectangleCollisionCheck(this.position, new Vector2(16, 16), game_state.player.GetPosition(), game_state.player.GetSize()))
            {
                this.Attack(game_state, delta);
                this.alerted = false;
                return;
            }
            if (this.state_timer.IsRunning())
            {
                if (!move_cooldown.IsRunning())
                {
                    this.position = GetNextPosition(game_state.GetCollisionLayer(), !this.alerted);
                    move_cooldown.Start(0.5);
                }
            } else
            {
                // swap states
                this.alerted = false;
                this.state_timer.Start();
            }
        }

        public override void UnAlert(double delta, GameState game_state)
        { 
            if (this.asleep) return;
            this.target_vector = game_state.player.GetPosition();
            // if hitting the player
            if (Math0.RectangleCollisionCheck(this.position, new Vector2(16, 16), game_state.player.GetPosition(), game_state.player.GetSize()))
            {
                Vector dir = GetMovementDirection();
                // get distances on both axis
                float xdist = Math.Abs(target_vector.X - position.X);
                float ydist = Math.Abs(target_vector.Y - position.Y);

                // determine which direction takes priority

                // straights
                if (dir.x != 0 && dir.y != 0)
                {
                    if (xdist > ydist) dir.y = 0;
                    else dir.x = 0;
                }

                game_state.player.AddVelocity(new Vector2(dir.x * 0.2f, dir.y * 0.2f));
            }
            if (this.state_timer.IsRunning())
            {
                if (!move_cooldown.IsRunning())
                {
                    this.position = GetNextPosition(game_state.GetCollisionLayer(), !this.alerted);
                    move_cooldown.Start();
                }
            }
            else
            {
                this.alerted = true;
                this.state_timer.Start();
            }
        }

        private Vector GetMovementDirection()
        {
            Vector dir = new Vector(0,0);
            if (target_vector.X > position.X) dir.x = 1;
            if (target_vector.X < position.X) dir.x = -1;
            if (target_vector.Y > position.Y) dir.y = 1;
            if (target_vector.Y < position.Y) dir.y = -1;
            return dir;
        }


        private Vector2 GetNextPosition(Layer collision_layer, bool straights)
        {
            Vector dir = GetMovementDirection();
            // get distances on both axis
            float xdist = Math.Abs(target_vector.X - position.X);
            float ydist = Math.Abs(target_vector.Y - position.Y);

            // determine which direction takes priority

            // straights
            if (dir.x != 0 && dir.y != 0 && straights)
            {
                if (xdist > ydist) dir.y = 0;
                else dir.x = 0;
            }

            // diagonals (make sure it moves diagonally ONLY)
            if (!straights)
            {
                if (ydist == 0) dir.y = 1;
                if (xdist == 0) dir.x = 1;
            }

            Vector2 result = this.position + new Vector2(16 * dir.x, 16 * dir.y);
            Vector result_tile_pos = Math0.CoordsToTileCoords(result);
            // hit a collision tile
            if (!collision_layer.GetTiles()[result_tile_pos.y, result_tile_pos.x].NULL)
            {
                result = this.position; // don't move (TEMPORARY)
            }

            return result;
        }


        public override void TakeDamage(double amount)
        {
            if (this.alerted)
            {
                base.TakeDamage(amount);
            }
            this.alerted = true;
        }


        public KnightStatue(SpriteSheet sprite, float x, float y) : base(sprite, x, y)
        {
            this.Initialize();
        }

        public KnightStatue(float x, float y) : base(x, y)
        {
            this.Initialize();
        }


        public override void Load()
        {
            this.sprite = Abyss.Globals._Sprites.TestBox;
            this.width = sprite.width - 1;
            this.height = sprite.height - 1;
        }


        public override KnightStatue Clone()
        {
            return new KnightStatue(this.sprite, this.position.X * 16, this.position.Y * 16);
        }
    }
}
