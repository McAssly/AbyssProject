using Abyss.Utility;
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
        public static Keys SwapGrimoire = Keys.Tab;
        public static Keys? PrimaryKey = null;
        public static Keys? SecondaryKey = Keys.Space;
        public static uint Primary = 1; // flags can either be 1 or 2; 1 = LeftClick; 2 = RightClick
        public static uint Secondary = 2;


        // Master Controls
        public static Keys DebugMenu = Keys.OemTilde;
        public static Keys Options = Keys.Escape;

        // Delay between click actions
        public static Timer ClickTimer = new Timer(0.1);
    }
}
