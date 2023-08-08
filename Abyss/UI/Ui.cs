﻿using Abyss.Entities;
using Abyss.Globals;
using Abyss.Master;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.UI
{
    internal class UiControllers
    {
        // Declare every single UI menu in the game
        public static Interaction Dialogue = new Interaction();
        public static Console _Debug = new Console();
        public static Inventory Invenetory = new Inventory();
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
                case "dialogue":
                    ui_state.OpenDialogue(UiState.TestDialogue); break;
                default: break;
            }
        }
    }

    internal interface Ui
    {
        public void Close();
        public bool IsClosed();
        public void UnClose();
        public void Update(KeyboardState KB, MouseState MS);
    }

    internal class Hud : Ui
    {
        public bool close = false;

        public void Close() { close = true; }
        public bool IsClosed() { return close; }
        public void UnClose() { close = false; }
        public Hud() { }

        public void Update(KeyboardState KB, MouseState MS) { }
    }

    internal class Interaction : Ui
    {
        public bool close = false;

        /**
         * 
         * 
         * WHERE I LEFT OFF WORKING ON DIALOGUE MENUS
         * WORK WITH HOVERING TEXT AND MOVING THROUGH A DIALOGUE TREE
         * 
         * 
         * 
         */
        private Dialogue dialogue;

        public Dialogue GetDialogue() { return dialogue; }

        public void SetDialogue(Dialogue dialogue)
        {
            this.dialogue = dialogue;
        }

        public void Close() { close = true; }
        public bool IsClosed() { return close; }
        public void UnClose() { close = false; }
        public void Update(KeyboardState KB, MouseState MS) { }
    }

    internal class Console : Ui
    {
        public bool close = false;
        public void Close() { close = true; }
        public bool IsClosed() { return close; }
        public void UnClose() { close = false; }
        public Console() { }

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
                    break;
                default:
                    break;
            }
        }
    }

    internal class Inventory : Ui
    {
        public bool close = false;
        public void Close() { close = true; }
        public bool IsClosed() { return close; }
        public void UnClose() { close = false; }
        public void Update(KeyboardState KB, MouseState MS) { }
    }

    internal class Menu : Ui
    {
        public bool close = false;
        public void Close() { close = true; }
        public bool IsClosed() { return close; }
        public void UnClose() { close = false; }
        public void Update(KeyboardState KB, MouseState MS) { }
    }
}
