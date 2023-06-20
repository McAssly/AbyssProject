using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.Map
{
    internal class LevelManager
    {
        // Levels
        public static Level TestLevel;

        private Level current;
        public LevelManager(Level current) { this.current = current; }

        public TileMap GetCurrentTileMap()
        {
            return current.GetCurrent();
        }

        public static void Load(ContentManager Content, int start_x, int start_y)
        {
            TestLevel.LoadLevel(Content, start_x, start_y);
        }
    }
}
