using Abyss.Master;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.UI.Menus
{
    internal class DebugConsole : IGui
    {
        public bool close = false;
        public void Close() { close = true; }
        public bool IsClosed() { return close; }
        public void UnClose() { close = false; }

        public DebugConsole() { }

        public void Update(KeyboardState KB, MouseState MS) { }

        public void ProcessCommand(UiState ui_state, GameState game_state)
        {
            // first conver the text input to a readable string
            string input = Game._TextInput.Append("\n").ToString();
            Debug.WriteLine(input);

            // next create a list of arguments that were passed into the console
            List<string> args = new List<string>();
            // need to build this list, by first starting with each argument
            StringBuilder arg = new StringBuilder();
            foreach (char c in input)
            {
                // add the current char in the input to the argument
                if (c == ' ' || c == '\n') // if we are at the next argument as defined by each delimeter (space or newline)
                {
                    // add the finished argument to the arguements list
                    args.Add(arg.ToString());
                    arg = new StringBuilder(); // reset the arguemnt builder to build more
                }
                else
                    arg.Append(c);
            }

            // if no command was passed then return as nothing happend
            if (args.Count <= 0)
            { Debug.WriteLine("No arguments found"); return; }

            // every command has a starting argument, the primary command
            /**
             * List of primary commands
             * 
             * enable       - enables a hud element
             * disable      - disables a hud element
             * set          - sets a value to an entity
             * save         - saves the game in its current state (prototype)
             * open         - opens a ui element
             */
            string primary = args[0];
            switch (primary)
            {
                case "enable":
                    UiControllers.EnableDebugHUD(args, true); break;
                case "disable":
                    UiControllers.EnableDebugHUD(args, false); break;
                case "open":
                    UiControllers.CommandOpen(args, ui_state); break;
                case "save":
                    game_state.Save(); break;
                case "set":
                    UiControllers.CommandSet(args); break;
                default:
                    break;
            }
        }
    }
}
