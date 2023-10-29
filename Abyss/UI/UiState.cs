using Abyss.Draw;
using Abyss.Globals;
using Abyss.Master;
using Abyss.UI.Menus;
using Abyss.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Text;

namespace Abyss.UI
{
    internal class UiState
    {
        // declare testing placeholders ---------------------------------------------------------------------------------------------
        /*public static Dialogue TestDialogue = new Dialogue
            (
                new Text("Hello World.", Variables.DialoguePosition, 0.3f, Variables.DialogueSize),
                new Text[]
                {
                    new Text("Hi", Variables.DialoguePosition + Variables.OptionOffset[0], 0.3f, Variables.DialogueSize, true),
                    new Text("Why?", Variables.DialoguePosition + Variables.OptionOffset[1], 0.3f, Variables.DialogueSize, true),
                    new Text("Goodbye.", Variables.DialoguePosition + Variables.OptionOffset[2], 0.3f, Variables.DialogueSize, true)
                }
            );*/
        // --------------------------------------------------------------------------------------------------------------------------


        private IGui current_ui;

        public UiState() { }

        /// <summary>
        /// Updates the UI state
        /// </summary>
        /// <param name="Window"></param>
        /// <param name="_graphics"></param>
        /// <param name="kb"></param>
        /// <param name="ms"></param>
        /// <param name="game_state"></param>
        public void Update(GameWindow Window, GraphicsDeviceManager _graphics, KeyboardState kb, MouseState ms, GameState game_state)
        {
            // update the current ui state, close it if it needs to close
            Close(game_state);
            IGui previous_ui = current_ui;

            // open the debug menu
            if (Game._KeyInput == Controls.DebugMenu && !(current_ui is DebugConsole))
            {
                // Hook the text input function to the game window
                Window.TextInput += InputUtility.RegisterInput;
                Open(UiControllers._Debug, game_state); // open the debug menu, but close the previous ui
            }

            // open the options menu
            if (Game._KeyInput == Controls.Options && !(current_ui is DebugConsole))
            {
                if (!(current_ui is Option))
                    Open(UiControllers.Options, game_state, false);
                else
                    CloseCurrent();
            }
            
            // CONSOLE PROCESS              CONSOLE     < --- (debug menu)
            if (current_ui is DebugConsole)
                if (InputUtility.HandleInput(kb))
                {
                    CloseCurrent(); // force close the current ui
                    UiControllers._Debug.ProcessCommand(this, game_state); // process the given command
                    Game._TextInput = new StringBuilder(); // reset the text
                }

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
        public void Setup(IGui ui)
        {
            this.current_ui = ui;
        }

        /// <summary>
        /// Opens the given UI, sets the current UI to the given
        /// </summary>
        /// <param name="ui"></param>
        public void Open(IGui ui, GameState game_state, bool visibility = true) 
        {
            this.current_ui = ui;
            game_state.SetVisible(visibility);
        }

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
        public void Close(GameState game_state)
        {
            if (this.current_ui.IsClosed())
            {
                if (this.current_ui is DebugConsole)
                {
                    Game.GameWindow.TextInput -= InputUtility.RegisterInput;
                }
                this.current_ui.UnClose();
                this.Open(UiControllers.HUD, game_state);
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
        public IGui CurrentUi() { return current_ui; }
    }
}
