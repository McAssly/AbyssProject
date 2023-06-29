using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.Entities
{
    internal abstract class Item
    {
    }

    internal sealed class Grimoire : Item
    {
        public static Grimoire Base;

        public static Grimoire Water;
        public static Grimoire Wind;
        public static Grimoire Earth;
        public static Grimoire Fire;
        public static Grimoire Lightning;
    }

    internal sealed class Card : Item
    {
        /// <summary>
        /// Enables the player to place a temporary respawn point (time dilation) that is 1 time use
        /// </summary>
        public static Card Time;

        /// <summary>
        /// Increases the player's max speed by 50% of their current health for 30 seconds
        /// </summary>
        public static Card Wind;

        /// <summary>
        /// Increases the player's defense by 50% of their current resistence for 30 seconds
        /// </summary>
        public static Card Earth;

        /// <summary>
        /// Sets the player on fire, while on fire the player's damage is increased by 25% of their max HP - current HP for 30s
        /// </summary>
        public static Card Fire;

        /// <summary>
        /// TBD
        /// </summary>
        public static Card Lightning;

        /// <summary>
        /// Makes the player wet, and increases their crit rate by 70% of their current crit damage.
        /// </summary>
        public static Card Water;
    }
}
