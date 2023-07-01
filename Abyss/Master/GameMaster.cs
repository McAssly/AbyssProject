﻿using Abyss.Entities;
using Abyss.Map;
using Abyss.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.Master
{   
    internal class GameMaster
    {
        // declare testing placeholders ---------------------------------------------------------------------------------------------
        public static Dialogue TestDialogue = new Dialogue
            (
                new Text("Hello World.", Globals.DialoguePosition, 0.3f, Globals.DialogueSize), 
                new Text[] 
                {
                    new Text("Hi", Globals.DialoguePosition + Globals.OptionOffset[0], 0.3f, Globals.DialogueSize, true),
                    new Text("Why?", Globals.DialoguePosition + Globals.OptionOffset[1], 0.3f, Globals.DialogueSize, true),
                    new Text("Goodbye.", Globals.DialoguePosition + Globals.OptionOffset[2], 0.3f, Globals.DialogueSize, true)
                }
            );
        public static Level TestLevel;
        // --------------------------------------------------------------------------------------------------------------------------




        // Declare the game managers (UI/Level)
        private Level currentLevel;
        private Ui currentUi;

        // Declare game entities
        protected internal Player player;

        // declare basic constructor
        public GameMaster() { }






        // SETUP / LOAD SECTION
        /// <summary>
        /// Sets up both the current level and current ui of the game within the game master
        /// </summary>
        /// <param name="level"></param>
        /// <param name="ui"></param>
        public void Setup(Level level, Ui ui)
        {
            this.currentLevel = level;
            this.currentUi = ui;
        }


        /// <summary>
        /// Loads every level in the game <--- currently a place holder
        /// </summary>
        /// <param name="Content"></param>
        /// <param name="startingMap"></param>
        public static void LoadLevels(ContentManager Content, int startingMap) { TestLevel.LoadLevel(Content, startingMap); }


        /// <summary>
        /// Loads the player entity for the game
        /// </summary>
        /// <param name="texture"></param>
        public void LoadPlayer(Texture2D texture) { player = new Player(texture); }

        /// <summary>
        /// Loads the current save file <--- placeholder
        /// </summary>
        /// <param name="mapIndex"></param>
        public void LoadSave(int mapIndex){ currentLevel.SetCurrent(mapIndex); }







        // COMMANDS
        /// <summary>
        /// Opens the given UI, sets the current UI to the given
        /// </summary>
        /// <param name="ui"></param>
        public void Open(Ui ui){ this.currentUi = ui; }

        /// <summary>
        /// opens the given dialogue in the dialogue ui window
        /// </summary>
        /// <param name="dialogue"></param>
        public void OpenDialogue(Dialogue dialogue)
        {
            UiControllers.Dialogue.SetDialogue(dialogue);
            this.currentUi = UiControllers.Dialogue;
        }

        /// <summary>
        /// If the current UI is subject to close then close it and reset back to the HUD
        /// . Otherwise simply do nothing
        /// </summary>
        public void Close()
        {
            if (this.currentUi.IsClosed())
            {
                if (this.currentUi is UI.Console)
                {
                    Game.GameWindow.TextInput -= RegisterInput;
                }
                this.currentUi.UnClose();
                this.currentUi = UiControllers.HUD;
            }
        }

        /// <summary>
        /// Saves the current save state of the game, currently a placeholder
        /// </summary>
        public void Save()
        {
            Data.Save("save.xml", this);
        }

        /// <summary>
        /// Force closes the current ui of the game state
        /// </summary>
        public void CloseCurrent() { this.currentUi.Close(); }









        // GETTERS / SETTERS
        /// <summary>
        /// Whether or not the game state is running the game or not (Ui is showing the HUD not any other menu)
        /// </summary>
        /// <returns>Whether the current ui is set to the HUD or not</returns>
        public bool IsHud() { return currentUi is Hud; }

        /// <summary>
        /// Grabs the current level the game state is playing in
        /// </summary>
        /// <returns>the game state's current level</returns>
        public Level CurrentLevel() { return currentLevel; }

        /// <summary>
        /// Grabs the current ui the game state is running
        /// </summary>
        /// <returns>the game state's current ui state</returns>
        public Ui CurrentUi() { return currentUi; }

        /// <summary>
        /// Grabs the current tile map that the game state is rendering
        /// </summary>
        /// <returns>the game state's current tile map</returns>
        public TileMap GetCurrentTileMap() { return currentLevel.GetCurrent(); }

        /// <summary>
        /// Grabs which map index the current level is running in
        /// </summary>
        /// <returns>the game state's current map index</returns>
        public int GetMapIndex() { return Array.IndexOf(currentLevel.GetMaps(), currentLevel.GetCurrent()); }







        // UPDATE SECTION
        /// <summary>
        /// Updates the current ui of the game state
        /// </summary>
        /// <param name="KB"></param>
        /// <param name="MS"></param>
        public void UpdateUi(KeyboardState KB, MouseState MS)
        {
            this.currentUi.Update(KB, MS);
        }

        /// <summary>
        /// Update's the level whenever the player moves to the next level
        /// </summary>
        public void UpdateLevel()
        {
            if (player.ExittingSide().HasValue)
            {
                var prevMap = currentLevel.GetCurrent();
                currentLevel.SetCurrent(player.ExittingSide());
                // fix the player's position
                if (currentLevel.GetCurrent() != prevMap)
                    switch (player.ExittingSide().Value)
                    {
                        case Side.LEFT: player.SetPosition(16 * 16 - 16, null); break;
                        case Side.RIGHT: player.SetPosition(0, null); break;
                        case Side.TOP: player.SetPosition(null, 16 * 16 - 16); break;
                        case Side.BOTTOM: player.SetPosition(null, 0); break;
                    }
            }
        }






        // DRAW SECTION
        /// <summary>
        /// Draw's the current level of the game
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void DrawLevel(SpriteBatch spriteBatch)
        {
            GetCurrentTileMap().Draw(spriteBatch);

            // draw the player
            player.Draw(spriteBatch);
        }

        /// <summary>
        /// Draw's the current Ui of the game
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void DrawUi(SpriteBatch spriteBatch)
        {
            if (currentUi == null) return;
            currentUi.Draw(spriteBatch);
            //test_text.Draw(spriteBatch);
        }






        // STATIC
        /// <summary>
        /// Handle's keyboard input for the game, when using the debug console, or entering in the player name, etc.
        /// </summary>
        /// <param name="KB"></param>
        /// <returns></returns>
        public static bool HandleInput(KeyboardState KB)
        {
            Keys[] keys = KB.GetPressedKeys();

            // Handle control keys
            foreach (Keys key in keys)
            {
                if (Game._prevKeyboardState.IsKeyUp(key)) // ensure repetitions are not caused
                {
                    if (key == Keys.Enter) // close key
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// The text input hook for the game window to register keyboard text input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void RegisterInput(object sender, TextInputEventArgs e)
        {
            Keys? k = e.Key;
            char c = e.Character;
            if (!char.IsControl(c) && c != '`')
                Game._TextInput.Append(c);
            else if (k == Keys.Back && Game._TextInput.Length > 0)
                Game._TextInput.Length--;
        }
    }
}
