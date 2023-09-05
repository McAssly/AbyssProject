using Microsoft.Xna.Framework.Input;

namespace Abyss.Globals
{
    internal class Controls
    {
        // Player Controls
        public static Keys Up = Keys.W;
        public static Keys Down = Keys.S;
        public static Keys Left = Keys.A;
        public static Keys Right = Keys.D;

        // attack controls
        public static Keys GrimoireSecondary_1 = Keys.Space;
        public static Keys GrimoireSecondary_2 = Keys.Tab;
        public static Keys? AttackKey_1 = null;
        public static Keys? AttackKey_2 = null;
        public static uint AttackMouseFlag_1 = 1; // flags can either be 1 or 2; 1 = LeftClick; 2 = RightClick
        public static uint AttackMouseFlag_2 = 2;


        // Master Controls
        public static Keys DebugMenu = Keys.OemTilde;
        public static Keys Options = Keys.Escape;
    }
}
