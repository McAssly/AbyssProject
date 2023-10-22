using Abyss.Globals;
using Abyss.Master;
using Abyss.UI.Controllers;
using Abyss.UI.Menus;
using Abyss.Utility;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Abyss.UI
{
    internal class UiControllers
    {
        // Declare every single UI menu in the game
        public static Interaction Dialogue = new Interaction();
        public static DebugConsole _Debug = new DebugConsole();
        public static PlayerMenu Invenetory = new PlayerMenu();
        public static Interaction Shop = new Interaction();
        public static Menu Main = new Menu();
        public static Menu Options = new Menu();

        // in game HUD
        public static Hud HUD = new Hud();

        /// <summary>
        /// Updates whether the given hud element in the command arguments should be enabled or disabled
        /// </summary>
        /// <param name="args"></param>
        /// <param name="enable"></param>
        public static void EnableDebugHUD(List<string> args, bool enable)
        {
            if (args.Count <= 1) { Debug.WriteLine("No arguments found"); return; }
            // This command really only gives a shit about the first argument passed into the primary command
            /**
             * List of HUD elements
             * 
             * position     - the position of the player
             * health       - the health of the player
             * mana         - the mana of the player
             */
            // get which secondary argument was passed
            switch (args[1])
            {
                case "debug":
                    Variables.DebugDraw = enable; break;
                case "collision":
                    Variables.DebugCollision = enable; break;
                case "fullscreen":
                    Config.Fullscreen = enable; break;
                default: break;
            }
        }

        /// <summary>
        /// opens a ui element
        /// </summary>
        /// <param name="args"></param>
        /// <param name="game_state"></param>
        public static void CommandOpen(List<string> args, UiState ui_state)
        {
            if (args.Count <= 1) { Debug.WriteLine("No arguments found"); return; }
            switch (args[1])
            {
                //case "dialogue":
                  //  ui_state.OpenDialogue(UiState.TestDialogue); break;
                default: break;
            }
        }

        public static void CommandSet(List<string> args)
        {
            if (args.Count <= 1) { Debug.WriteLine("No arguments found"); return; }
            switch (args[1])
            {
                case "window.scale":
                    if (args.Count <= 2) { Debug.WriteLine("No value given"); return; }
                    Config.WindowScalar = double.Parse(args[2]);
                    break;
                default: break;
            }
        }
    }
}
