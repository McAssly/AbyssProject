using Abyss.Globals;
using Abyss.Magic;
using Abyss.Master;
using Abyss.Sprites;
using Abyss.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Abyss.Entities
{
    internal class Player : Entity
    {
        internal int current_grimoire = 0;
        private Timer swap_cooldown = new Timer(0.2);
        public Inventory inventory;

        // stats
        private protected double max_mana;
        private protected double mana;
        private protected double crit_dmg; // percentage to increase damage by
        private protected double crit_rate; // between 0 and 1, a percentage value

        internal double damage_mult = 1;

        // for crits
        private protected static readonly Random random = new Random();



        public Player(SpriteSheet sprite) : base(sprite)
        {
            this.position = new Vector2();
            this.speed = 1;
            this.crit_dmg = 0.7;
            this.crit_rate = 0.05;
            this.damage = 1;
            this.defense = 1;
        }


        /// <summary>
        /// loads the given player data (save) into the player
        /// </summary>
        /// <param name="data"></param>
        public void Load(PlayerData data)
        {
            this.position = data.position * new Vector2(16, 16);
            this.health = data.current_hp;
            this.max_health = data.max_hp;
            this.mana = data.current_mana;
            this.max_mana = data.max_mana;
            this.inventory = data.inventory;
        }


        /// <summary>
        /// Handles player attack sequence
        /// </summary>
        /// <param name="kb"></param>
        /// <param name="ms"></param>
        public void Attack(KeyboardState kb, MouseState ms, double delta)
        {
            swap_cooldown.Update(delta);
            // swap the current grimoire if swap key is pressed
            if (kb.IsKeyDown(Controls.SwapGrimoire) && !swap_cooldown.IsRunning())
            {
                switch (current_grimoire)
                {
                    case 0: current_grimoire = 1; swap_cooldown.Start(); break;
                    case 1: current_grimoire = 0; swap_cooldown.Start(); break;
                }
            }

            // get the keyboard controls (off by default)
            bool kb_atk_1 = false;
            bool kb_atk_2 = false;
            if (Controls.PrimaryKey.HasValue) kb_atk_1 = kb.IsKeyDown(Controls.PrimaryKey.Value);
            if (Controls.SecondaryKey.HasValue) kb_atk_2 = kb.IsKeyDown(Controls.SecondaryKey.Value);

            // detect keyboard/mouse buttons, if the corresponding ones are pressed then active the corresponding grimoire and spell
            if (kb_atk_1 || InputUtility.IsClicked(ms, Controls.Primary, false))
            {
                this.CastSpell(current_grimoire, 1, delta); // primary spell
            }
            else if (kb_atk_2 || InputUtility.IsClicked(ms, Controls.Secondary))
            {
                this.CastSpell(current_grimoire, 2, delta); // secondary spell
            }
        }


        /// <summary>
        /// updates the player instance
        /// </summary>
        /// <param name="delta"></param>
        /// <param name="kb"></param>
        /// <param name="ms"></param>
        /// <param name="game_state"></param>
        public void Update(double delta, KeyboardState kb, MouseState ms, GameState game_state)
        {
            invulnerability.Update(delta);
            CalculateInputVector(kb);
            Move(game_state.GetCollisionLayer(), delta);
            this.Clamp();
            if (this.target_vector != Vector2.Zero)
                this.angle = Math0.AngleAtVector(target_vector);

            /* ATTACK DETECTION AND CASTING SPELLS */
            game_state.player.Attack(kb, ms, delta);
            if (!regen.IsRunning() && this.mana < this.max_mana) {
                this.RegenerateMana(1);
                this.regen.Start();
            }

            // update the status effects on the player
            this.ApplyStatuses(delta);

            // handle animations
            switch (sprite.GetSection())
            {
                // RUNNING animation
                case 0:
                    if (target_vector != Vector2.Zero) sprite.Play(); else sprite.Stop(true); sprite.Loop(); break;
                // ATTACKING animation
                case 2:
                    sprite.Play(); sprite.UnLoop(); break;
                default: sprite.Play(); break;
            }
            sprite.Update(delta, target_vector);
        }


        /// <summary>
        /// attacks using the primary grimoire
        /// </summary>
        public void CastSpell(int index, int type, double delta)
        {
            if (mana > 0) inventory.grimoires[index].Attack(this, InputUtility.MousePositionInGame(), type, delta);
        }


        /// <summary>
        /// Determines the input vector of the player and normalizes it to a radius of 1
        /// </summary>
        /// <param name="KB"></param>
        public void CalculateInputVector(KeyboardState KB)
        {
            // If a key is being pressed it is = 1, if it is not then it is = 0
            // Therefore if the player is pressing the right key their input.x should be (+), if left then (-), if both then (0)
            this.target_vector.X = InputUtility.KeyStrength(KB, Controls.Right) - InputUtility.KeyStrength(KB, Controls.Left);
            this.target_vector.Y = InputUtility.KeyStrength(KB, Controls.Down) - InputUtility.KeyStrength(KB, Controls.Up);

            if (this.target_vector != Vector2.Zero)
                this.target_vector.Normalize();
        }



        /// <summary>
        /// Returns which side the player is leaving on the current level's map
        /// </summary>
        /// <returns>which side the player is leaving, returns nullable if they aren't leaving</returns>
        public Side? ExittingSide()
        {
            // Check on the x-axis, LEFT / RIGHT
            switch ((int)position.X)
            {
                case 0: // if they are just left of the game window
                    return Side.LEFT; // left
                case 15 * 16: // if they are just right of the game window
                    return Side.RIGHT; // right
                default: break;
            }
            // Check on the y-axis, TOP / BOTTOM
            switch ((int)position.Y)
            {
                case 0: // if they are just above the game window (map)
                    return Side.TOP; // then its the top
                case 15 * 16: // if they are just below it
                    return Side.BOTTOM; // then its the bottom
                default: break;
            }
            // if not a single check passed then they aren't leaving the map
            return null;
        }


        /// <summary>
        /// gets the next possible position for the player across map areas
        /// </summary>
        /// <param name="side"></param>
        /// <returns></returns>
        public NullableVector GetNextPosition(Side side)
        {
            switch (side)
            {
                case Side.LEFT: return new NullableVector(15 * 16 - 1, null);
                case Side.RIGHT: return new NullableVector(1, null);
                case Side.TOP: return new NullableVector(null, 15 * 16 - 1);
                case Side.BOTTOM: return new NullableVector(null, 1);
                default: return new NullableVector();
            }
        }


        /// <summary>
        /// gets the position of the player, but also adds on a modifier
        /// </summary>
        /// <param name="modifier"></param>
        /// <returns></returns>
        public Vector2 GetPosition(NullableVector? modifier)
        {
            if (!modifier.HasValue) return this.position;
            if (modifier.Value.x.HasValue && modifier.Value.y.HasValue)
                return new Vector2(modifier.Value.x.Value, modifier.Value.y.Value);
            else if (modifier.Value.x.HasValue)
                return new Vector2(modifier.Value.x.Value, this.position.Y);
            else if (modifier.Value.y.HasValue)
                return new Vector2(this.position.X, modifier.Value.y.Value);
            return this.position;
        }




        /// <summary>
        /// reduces the player's mana by a given amount
        /// </summary>
        /// <param name="amount"></param>
        public void ReduceMana(double amount)
        {
            mana -= amount;
            if (mana < 0) mana = 0;
        }

        /// <summary>
        /// regenerates a given amount of mana instantly
        /// </summary>
        /// <param name="amount"></param>
        public void RegenerateMana(double amount)
        {
            mana += amount;
        }


        /// <summary>
        /// calculates the player's damage
        /// </summary>
        /// <param name="base_damage"></param>
        /// <returns></returns>
        public override double CalculateDamage(double base_damage = 1)
        {
            // do the temp variables
            double crit_dmg = this.crit_dmg;
            double crit_rate = this.crit_rate;
            double damage = base_damage * this.damage;

            // find any required statuses and apply the status effects
            if (statuses != null)
            {
                StatusEffect? crit_rate_effect = statuses.Find(effect => effect.application_id == 0);
                StatusEffect? crit_dmg_effect = statuses.Find(effect => effect.application_id == 1);
                StatusEffect? damage_effect = statuses.Find(effect => effect.application_id == 2);

                // if the efffects existed then apply the effect
                if (crit_rate_effect.HasValue) crit_rate += crit_rate_effect.Value.value;
                if (crit_dmg_effect.HasValue) crit_dmg += crit_dmg_effect.Value.value;
                if (damage_effect.HasValue) damage += damage_effect.Value.value;
            }

            // calculate the actual damage values
            int iterations = 0;
            while (random.NextDouble() < crit_rate && iterations < 3)
            {
                damage = damage + (damage * crit_dmg / (iterations + 1));
                iterations++;
            }
            this.last_damage = damage * damage_mult;
            return damage;
        }


        public double GetMaxMana() { return max_mana; }

        public double GetMana() { return mana; }

        internal void SetMana(double amount)
        {
            this.mana = amount;
        }

        internal Grimoire GetCurrentGrimoire()
        {
            return this.inventory.grimoires[current_grimoire];
        }
    }
}
