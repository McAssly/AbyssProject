using Abyss.Globals;
using Abyss.Master;
using Abyss.UI.Controllers;
using Abyss.Utility;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.UI.Menus
{
    internal class Menu : IGui
    {
        public bool close = false;
        public byte previous;

        public ListController controls;

        public Menu() {

        }

        public void Initialize(GameState game_state, UiState ui_state)
        {
        }

        public void UpdateOrigin()
        {
            //this.origin = Math0.CenterWithinRectangle(Variables.WindowW, Variables.WindowH, ui_width, Variables.WindowH - padding, Variables.GameScale, 1);
        }

        public void Close() { close = true; }
        public bool IsClosed() { return close; }
        public void UnClose() { close = false; }
        public void Update(KeyboardState KB, MouseState MS)
        {
            UpdateOrigin();

            //fullscreen.Update(MS);
            //close_button.Update(MS);
        }
    }
}
