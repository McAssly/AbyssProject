
namespace Abyss.Items
{
    internal class Card : Item
    {
        /// <summary>
        /// Enables the player to place a temporary respawn point (time dilation) that is 1 time use
        /// </summary>
        public static Card Time; // ID = 0

        /// <summary>
        /// Increases the player's max speed by 50% of their current health for 30 seconds
        /// </summary>
        public static Card Wind; // ID = 1

        /// <summary>
        /// Increases the player's defense by 50% of their current resistence for 30 seconds
        /// </summary>
        public static Card Earth; // ID = 2

        /// <summary>
        /// Sets the player on fire, while on fire the player's damage is increased by 25% of their max HP - current HP for 30s
        /// </summary>
        public static Card Fire; // ID = 3

        /// <summary>
        /// TBD
        /// </summary>
        public static Card Lightning; // ID = 4

        /// <summary>
        /// Makes the player wet, and increases their crit rate by 70% of their current crit damage.
        /// </summary>
        public static Card Water; // ID = 5
    }
}
