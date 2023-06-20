using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.UI
{

    internal class UiManager
    {
        // Declare every single UI menu in the game
        public static Interaction Dialogue = new Interaction();
        public static Console Debug = new Console();
        public static Inventory Invenetory = new Inventory();
        public static Interaction Shop = new Interaction();
        public static Menu Main = new Menu();
        public static Menu Options = new Menu();

        // in game HUD
        public static HUD HUD = new HUD();

        // the current UI interface to interact with
        private Ui current;

        // The constructor for the UI Manager
        public UiManager(Ui current) 
        {
            this.current = current;
        }

        // GETTERS / SETTERS
        public Ui GetCurrent() { return current; }
        public void SetCurrent(Ui current) { this.current = current;}

        public bool IsHUD()
        {
            if (current is HUD) return true;
            return false;
        }

        /// <summary>
        /// Opens the given UI, sets the current UI to the given
        /// </summary>
        /// <param name="ui"></param>
        public void Open(Ui ui)
        {
            this.current = ui;
        }

        /// <summary>
        /// If the current UI is subject to close then close it and reset back to the HUD
        /// . Otherwise simply do nothing
        /// </summary>
        public void Close()
        {
            if (this.current.IsClosed())
            {
                this.current.UnClose();
                this.current = HUD;
            }
        }

        /// <summary>
        /// Updates the ui manager and all its necessary processes
        /// </summary>
        public void Update(KeyboardState KB, MouseState MS)
        {
            this.current.Update(KB, MS);
        }


        // Draws the current UI
        public void Draw(SpriteBatch spriteBatch)
        {
            if (current == null) return;

            current.Draw(spriteBatch);
        }
    }
}
