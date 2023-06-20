using Abyss.Map;
using Abyss.UI;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.Master
{   
    internal class GameMaster
    {
        // declare testing placeholders
        public static Text test_text;
        public static Level TestLevel;

        // Declare the game managers (UI/Level)
        private Level currentLevel;
        private Ui currentUi;

        public GameMaster() { }

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
        /// Opens the given UI, sets the current UI to the given
        /// </summary>
        /// <param name="ui"></param>
        public void Open(Ui ui)
        {
            this.currentUi = ui;
        }

        /// <summary>
        /// If the current UI is subject to close then close it and reset back to the HUD
        /// . Otherwise simply do nothing
        /// </summary>
        public void Close()
        {
            if (this.currentUi.IsClosed())
            {
                this.currentUi.UnClose();
                this.currentUi = UiControllers.HUD;
            }
        }


        public bool IsHud()
        {
            return currentUi is Hud;
        }

        // GETTERS / SETTERS


        public Level CurrentLevel() { return currentLevel; }
        public Ui CurrentUi() { return currentUi; }
        public TileMap GetCurrentTileMap() { return currentLevel.GetCurrent(); }


        public static void LoadLevels(ContentManager Content, int start_x, int start_y)
        {
            TestLevel.LoadLevel(Content, start_x, start_y);
        }

        // UPDATE SECTION -----------------------------------------
        public void UpdateUi(KeyboardState KB, MouseState MS)
        {
            this.currentUi.Update(KB, MS);
        }


        // DRAW SECTION -------------------------------------------

        public void DrawLevel(SpriteBatch spriteBatch)
        {
            GetCurrentTileMap().Draw(spriteBatch);
        }

        public void DrawUi(SpriteBatch spriteBatch)
        {
            if (currentUi == null) return;
            currentUi.Draw(spriteBatch);
            //test_text.Draw(spriteBatch);
        }
    }
}
