using Abyss.Map;
using Abyss.Master;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.Entities
{
    internal struct Inventory
    {
        public Grimoire[] grimoires; // max = 2, can only hold 2 grimoires
        public Card[] cards; // max total = 8
        public int[] regenPots; // array max = 2, combined value max = 16  (both are counters), [0] = health, [1] = mana
        public int[] instantPots; // array max = 2, combiend value max = 4 (see line above)
        public List<Item> extras; // unlimited number of extra items, these are mainly for questlines or auxillary stuff so not entirely important

        public static Grimoire[] ParseGrimoires(string[] grimoireData)
        {
            return new Grimoire[]
            {
                Grimoire.Which(grimoireData[0]),
                Grimoire.Which(grimoireData[1])
            };
        }

        public static Card[] ParseCards(int[] cardData)
        {
            List<Card> cards = new List<Card>();
            for (int id = 0; id < cardData.Length; id++)
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
            // draw object is currently a placeholder as there is no player texture as of now
            this.drawObj = new Rectangle(0, 0, 16, 16);
            this.pos = new Vector2();
            this.speed = 2;
        }

        /// <summary>
        /// loads the given save data into the player state
        /// </summary>
        /// <param name="data"></param>
        public void LoadSave(PlayerData data)
        {
            this.pos = data.position;
            this.health = data.currentHealth;
            this.max_health = data.maxHealth;
            this.mana = data.currentMana;
            this.max_mana = data.maxMana;
            // inventory
            this.Inventory = data.inventory;
        }


        /// <summary>
        /// attacks using the primary grimoire
        /// </summary>
        public void Cast(int index)
        {
            Inventory.grimoires[index].Attack(this, MathUtil.Mouse());
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
    }
}
