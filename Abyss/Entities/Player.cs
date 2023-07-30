using Abyss.Entities.Magic;
using Abyss.Map;
using Abyss.Master;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Timers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using NullableVector = Abyss.Master.NullableVector;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Abyss.Entities
{
    internal struct Inventory
    {
        public Grimoire[] grimoires; // max = 2, can only hold 2 grimoires
        public Card[] cards; // max total = 8
        public int[] regen_pots; // array max = 2, combined value max = 16  (both are counters), [0] = health, [1] = mana
        public int[] instant_pots; // array max = 2, combiend value max = 4 (see line above)
        public List<Item> extras; // unlimited number of extra items, these are mainly for questlines or auxillary stuff so not entirely important

        public Inventory(Grimoire[] grimoires, Card[] cards = null, int[] regen_pots = null, int[] instant_pots = null, List<Item> extras = null)
        {
            this.grimoires = grimoires;

            // set the player's cards
            if (cards == null)
                this.cards = new Card[0];
            else 
                this.cards = cards;

            // set the player's number of regen pots
            if (regen_pots == null)
                this.regen_pots = new int[2] { 0, 0 };
            else
                this.regen_pots = regen_pots;

            // set the player's number of instant pots
            if (instant_pots == null)
                this.instant_pots = new int[2] { 0, 0 };
            else
                this.instant_pots = instant_pots;

            // set the player's key items
            if (extras == null)
                this.extras = new List<Item>();
            else
                this.extras = extras;
        }

        public static Grimoire[] ParseGrimoires(string[] grim_data)
        {
            return new Grimoire[]
            {
                Grimoire.Which(grim_data[0]),
                Grimoire.Which(grim_data[1])
            };
        }

        public static Card[] ParseCards(int[] card_data)
        {
            List<Card> cards = new List<Card>();
            for (int id = 0; id < card_data.Length; id++)
            {
                if (Item.WhichCard(id) != null)
                    cards.Add(Item.WhichCard(id));
            }
            return cards.ToArray();
        }
    }

    internal class Player : Entity
    {
        // declare the player's inventory
        public Inventory Inventory;

        public Player(Texture2D texture) :base(texture)
        {
            this.pos = new Vector2();
            this.speed = 1.5;
            this.crit_dmg = 0.7; // 70%
            this.crit_rate = 0.05; // 5%
            this.damage = 1; // 1 base
            this.defense = 1; // 1 divisor
        }

        /// <summary>
        /// loads the given save data into the player state
        /// </summary>
        /// <param name="data"></param>
        public void LoadSave(PlayerData data)
        {
            this.pos = data.position * new System.Numerics.Vector2(16,16);
            this.health = data.current_hp;
            this.max_health = data.max_hp;
            this.mana = data.current_mana;
            this.max_mana = data.max_mana;
            // inventory
            this.Inventory = data.inventory;
        }

        /// <summary>
        /// Handles player attack sequence
        /// </summary>
        /// <param name="KB"></param>
        /// <param name="MS"></param>
        public void Attack(KeyboardState KB, MouseState MS)
        {
            // get the keyboard controls (off by default)
            bool kb_atk_1 = false;
            bool kb_atk_2 = false;
            if (Controls.AttackKey_1.HasValue) kb_atk_1 = KB.IsKeyDown(Controls.AttackKey_1.Value);
            if (Controls.AttackKey_2.HasValue) kb_atk_2 = KB.IsKeyDown(Controls.AttackKey_2.Value);

            // detect keyboard/mouse buttons, if the corresponding ones are pressed then active the corresponding grimoire and spell
            if (kb_atk_1 || MathUtil.IsClicked(MS, Controls.AttackMouseFlag_1))
            {
                // there are secondary spells and to activate them the control sequence has to be pressed (default: l-shift)
                if (KB.IsKeyDown(Controls.GrimoireSecondary_1))
                    this.Cast(0, 2); // secondary spell
                else this.Cast(0, 1); // primary spell
            }
            if (kb_atk_2 || MathUtil.IsClicked(MS, Controls.AttackMouseFlag_2))
            {
                if (KB.IsKeyDown(Controls.GrimoireSecondary_2))
                    this.Cast(1, 2); // secondary spell
                else this.Cast(1, 1); // primary spell
            }
        }

        
        /// <summary>
        /// updates the player instance
        /// </summary>
        /// <param name="delta"></param>
        /// <param name="KB"></param>
        /// <param name="MS"></param>
        /// <param name="GM"></param>
        public void Update(double delta, KeyboardState KB, MouseState MS, GameMaster GM)
        {
            CalcInputVector(KB);
            Move(GM.GetCurrentTileMap(), delta);
            ClampPosition();

            /* ATTACK DETECTION AND CASTING */
            GM.player.Attack(KB, MS);
            if (time_elapsed >= 1 && GM.player.GetMana() < GM.player.GetMaxMana())
            {
                GM.player.AddMana(1);
                time_elapsed = 0;
            }

            // if the player has any status effects update their statuses
            if (statuses.Count > 0)
            {
                // iterate through each status and apply each of them
                for (int i = 0; i < statuses.Count; i++)
                {
                    // create a copy of the status
                    StatusEffect status_copy = statuses[i];
                    status_copy.timer -= delta;
                    statuses[i] = status_copy;
                }
                statuses.RemoveAll(effect => effect.timer <= 0);
            }
            time_elapsed += delta;
            regen_timer += delta;
        }


        /// <summary>
        /// attacks using the primary grimoire
        /// </summary>
        public void Cast(int index, int type)
        {
            if (mana > 0) Inventory.grimoires[index].Attack(this, MathUtil.MousePositionInGame(), type);
        }

        /// <summary>
        /// Determines the input vector of the player and normalizes it to a radius of 1
        /// </summary>
        /// <param name="KB"></param>
        public void CalcInputVector(KeyboardState KB)
        {
            // If a key is being pressed it is = 1, if it is not then it is = 0
            // Therefore if the player is pressing the right key their input.x should be (+), if left then (-), if both then (0)
            this.movement_vec.X = MathUtil.KeyStrength(KB, Controls.Right) - MathUtil.KeyStrength(KB, Controls.Left);
            this.movement_vec.Y = MathUtil.KeyStrength(KB, Controls.Down) - MathUtil.KeyStrength(KB, Controls.Up);

            if (this.movement_vec != Vector2.Zero)
                this.movement_vec.Normalize();
        }


        /// <summary>
        /// Returns which side the player is leaving on the current level's map
        /// </summary>
        /// <returns>which side the player is leaving, returns nullable if they aren't leaving</returns>
        public Side? ExittingSide()
        {
            // Check on the x-axis, LEFT / RIGHT
            switch (pos.X)
            {
                case -1: // if they are just left of the game window
                    return Side.LEFT; // left
                case 16 * 16 - 16 + 1: // if they are just right of the game window
                    return Side.RIGHT; // right
                default: break;
            }
            // Check on the y-axis, TOP / BOTTOM
            switch (pos.Y)
            {
                case -1: // if they are just above the game window (map)
                    return Side.TOP; // then its the top
                case 16 * 16 - 16 + 1: // if they are just below it
                    return Side.BOTTOM; // then its the bottom
                default: break;
            }
            // if not a single check passed then they aren't leaving the map
            return null;
        }


        public NullableVector GetNextPosition(Side side)
        {
            switch (side)
            {
                case Side.LEFT: return new NullableVector(16 * 16 - 16, null);
                case Side.RIGHT: return new NullableVector(0, null);
                case Side.TOP: return new NullableVector(null, 16 * 16 - 16);
                case Side.BOTTOM: return new NullableVector(null, 0);
                default: return new NullableVector();
            }
        }


        public Vector2 GetPosition(NullableVector? modifier)
        {
            if (!modifier.HasValue) return this.pos;
            if (modifier.Value.x.HasValue && modifier.Value.y.HasValue)
                return new Vector2(modifier.Value.x.Value, modifier.Value.y.Value);
            else if (modifier.Value.x.HasValue)
                return new Vector2(modifier.Value.x.Value, this.pos.Y);
            else if (modifier.Value.y.HasValue)
                return new Vector2(this.pos.X, modifier.Value.y.Value);
            return this.pos;
        }

        public void SetPosition(Vector2 _new)
        {
            this.pos = _new;
        }
    }
}
