using Abyss.Entities;
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


        // debug
        public static bool SHOW_MOUSEPOSITION = false;
        public static bool SHOW_POSITION = false;
        public static bool SHOW_HEALTH = true;
        public static bool SHOW_MANA = true;

        /// <summary>
        /// Updates whether the given hud element in the command arguments should be enabled or disabled
        /// </summary>
        /// <param name="args"></param>
        /// <param name="enable"></param>
        public static void UpdateHUDElement(List<string> args, bool enable)
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
                case "position":
                    SHOW_POSITION = enable; break;
                case "health":
                    SHOW_HEALTH = enable; break;
                case "mana":
                    SHOW_MANA = enable; break;
                case "mouse":
                    SHOW_MOUSEPOSITION = enable; break;
                default : break;
            }
        }


        /// <summary>
        /// opens a ui element
        /// </summary>
        /// <param name="args"></param>
        /// <param name="GM"></param>
        public static void CommandOpen(List<string> args, GameMaster GM)
        {
            if (args.Count <= 1) { Debug.WriteLine("No arguments found"); return; }
            switch (args[1])
            {
                case "dialogue":
                    GM.OpenDialogue(GameMaster.TestDialogue); break;
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
        public void Draw(SpriteBatch spriteBatch);
    }

    internal class Hud : Ui
    {
        public bool close = false;

        // The player info variables
        private Text health;
        private Text mana;
        private Text position;

        public void Close() { close = true; }
        public bool IsClosed() {  return close; }
        public void UnClose() { close = false; }
        public Hud() { }

        public void UpdatePlayerInfo(Player player)
        {
            this.health = new Text("HP: " + player.Health() + "/" + player.MaxHealth(), new Vector2(16, 16), 0.5f);
            this.mana = new Text("MN: " + player.Mana() + "/" + player.MaxMana(), new Vector2(16, 32), 0.5f);
            this.position = new Text("POS: " + (int)player.Position().X + ", " + (int)player.Position().Y, new Vector2(16, 48), 0.5f);
        }

        public void Update(KeyboardState KB, MouseState MS) { }
        public void Draw(SpriteBatch spriteBatch) 
        {
            if (spriteBatch == null) return;
            // draw each text on screen
            if (UiControllers.SHOW_HEALTH) this.health.Draw(spriteBatch);
            if (UiControllers.SHOW_MANA) this.mana.Draw(spriteBatch);
            if (UiControllers.SHOW_POSITION) this.position.Draw(spriteBatch);
            if (UiControllers.SHOW_MOUSEPOSITION) new Text("Mouse: " + (int)MathUtil.Mouse().X + ", " + (int)MathUtil.Mouse().Y, new Vector2(16, 64), 0.4f).Draw(spriteBatch);
        }
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

        public void SetDialogue(Dialogue dialogue)
        {
            this.dialogue = dialogue;
        }

        public void Close() { close = true; }
        public bool IsClosed() { return close; }
        public void UnClose() { close = false; }
        public void Update(KeyboardState KB, MouseState MS) { }
        public void Draw(SpriteBatch spriteBatch) { dialogue.Draw(spriteBatch); }
    }

    internal class Console : Ui
    {
        public bool close = false;
        public void Close() { close = true; }
        public bool IsClosed() { return close; }
        public void UnClose() { close = false; }
        public Console() { }

        public void Update(KeyboardState KB, MouseState MS) { }

        public void ProcessCommand(GameMaster GM)
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
                } else
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
                    UiControllers.UpdateHUDElement(args, true);  break;
                case "disable":
                    UiControllers.UpdateHUDElement(args, false); break;
                case "open":
                    UiControllers.CommandOpen(args, GM); break;
                case "save":
                    GM.Save(); break;
                case "set":
                    break;
                default:
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.FillRectangle(new RectangleF(0, 0, Globals.WindowW, Globals.TILE_SIZE * 2), Globals.Black);
            Text.Draw(spriteBatch, Game._TextInput, new Vector2(8, 8), (float)0.2);
        }
    }

    internal class Inventory : Ui
    {
        public bool close = false;
        public void Close() { close = true; }
        public bool IsClosed() { return close; }
        public void UnClose() { close = false; }
        public void Update(KeyboardState KB, MouseState MS) { }
        public void Draw(SpriteBatch spriteBatch) { }
    }

    internal class Menu : Ui
    {
        public bool close = false;
        public void Close() { close = true; }
        public bool IsClosed() { return close; }
        public void UnClose() { close = false; }
        public void Update(KeyboardState KB, MouseState MS) { }
        public void Draw(SpriteBatch spriteBatch) { }
    }
}
