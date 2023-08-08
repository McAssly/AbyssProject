using Abyss.Draw;
using Abyss.Globals;
using Abyss.Master;
using Abyss.Utility;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.UI
{
    internal class UiState
    {
        // declare testing placeholders ---------------------------------------------------------------------------------------------
        public static Dialogue TestDialogue = new Dialogue
            (
                new Text("Hello World.", Variables.DialoguePosition, 0.3f, Variables.DialogueSize),
                new Text[]
                {
                    new Text("Hi", Variables.DialoguePosition + Variables.OptionOffset[0], 0.3f, Variables.DialogueSize, true),
                    new Text("Why?", Variables.DialoguePosition + Variables.OptionOffset[1], 0.3f, Variables.DialogueSize, true),
                    new Text("Goodbye.", Variables.DialoguePosition + Variables.OptionOffset[2], 0.3f, Variables.DialogueSize, true)
                }
            );
        // --------------------------------------------------------------------------------------------------------------------------


        private Ui current_ui;

        public UiState()
        {

        }

        /// <summary>
        /// Updates the current ui
        /// </summary>
        /// <param name="kb"></param>
        /// <param name="ms"></param>
        public void Update(KeyboardState kb, MouseState ms)
        {
            this.current_ui.Update(kb, ms);
        }


        /// <summary>
        /// draws the current game ui
        /// </summary>
        /// <param name="sprite_batch"></param>
        public void Draw(DrawState sprite_batch, GameState gs)
        {
            if (current_ui == null) return;
            sprite_batch.Draw(current_ui, gs);
        }


        /// <summary>
        /// Setup the current ui of the game 
        /// </summary>
        /// <param name="ui"></param>
        public void Setup(Ui ui)
        {
            this.current_ui = ui;
        }

        /// <summary>
        /// Opens the given UI, sets the current UI to the given
        /// </summary>
        /// <param name="ui"></param>
        public void Open(Ui ui) { this.current_ui = ui; }

        /// <summary>
        /// opens the given dialogue in the dialogue ui window
        /// </summary>
        /// <param name="dialogue"></param>
        public void OpenDialogue(Dialogue dialogue)
        {
            // unhook the input controller
            Game.GameWindow.TextInput -= InputUtility.RegisterInput;

            // open the given dialogue
            UiControllers.Dialogue.SetDialogue(dialogue);
            this.current_ui = UiControllers.Dialogue;
        }

        /// <summary>
        /// If the current UI is subject to close then close it and reset back to the HUD
        /// . Otherwise simply do nothing
        /// </summary>
        public void Close()
        {
            if (this.current_ui.IsClosed())
            {
                if (this.current_ui is UI.Console)
                {
                    Game.GameWindow.TextInput -= InputUtility.RegisterInput;
                }
                this.current_ui.UnClose();
                this.current_ui = UiControllers.HUD;
            }
        }

        /// <summary>
        /// Force closes the current ui of the game state
        /// </summary>
        public void CloseCurrent() { this.current_ui.Close(); }

        /// <summary>
        /// Whether or not the game state is running the game or not (Ui is showing the HUD not any other menu)
        /// </summary>
        /// <returns>Whether the current ui is set to the HUD or not</returns>
        public bool IsHud() { return current_ui is Hud; }

        /// <summary>
        /// Grabs the current ui the game state is running
        /// </summary>
        /// <returns>the game state's current ui state</returns>
        public Ui CurrentUi() { return current_ui; }
    }
}
