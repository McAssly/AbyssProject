using Abyss.Items;
using Abyss.Magic;
using System.Collections.Generic;

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
}
