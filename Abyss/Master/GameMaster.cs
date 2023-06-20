using Abyss.Map;
using Abyss.UI;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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

        // Declare the game managers (UI/Level)
        private LevelManager M_Level;
        private UiManager M_UI;

        public GameMaster() { }



        // GETTERS / SETTERS


        public UiManager GetUiManager() { return M_UI; }
        public LevelManager GetLevelManager() { return M_Level; }
        public void SetUIManager(UiManager uiManager) { M_UI = uiManager; }
        public void SetLevelManager(LevelManager levelManager) { M_Level = levelManager; }
        public TileMap GetCurrentTileMap() { return M_Level.GetCurrentTileMap(); }




        public static void LoadLevels(ContentManager Content, int start_x, int start_y)
        {
            LevelManager.Load(Content, start_x, start_y);
        }


        // DRAW SECTION -------------------------------------------

        public void DrawLevel(SpriteBatch spriteBatch)
        {
            M_Level.GetCurrentTileMap().Draw(spriteBatch);
        }

        public void DrawUi(SpriteBatch spriteBatch)
        {
            M_UI.Draw(spriteBatch);
            //test_text.Draw(spriteBatch);
        }
    }
}
