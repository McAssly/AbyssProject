using Abyss.Globals;
using Abyss.Master;
using Abyss.UI.Controllers;
using Abyss.Utility;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.UI.Menus
{
    internal class Option : IGui
    {
        public bool close = false;
        public byte previous;

        public ListController controls;

        public Option() {
            Vector margin = new Vector(16, 16);
            int width = 256;
            Vector origin = Math0.CenterWithinRectangle(Variables.WindowW, Variables.WindowH, width, Variables.WindowH - margin.y, Variables.GameScale, 1);
            this.controls = new ListController(
                origin.x, origin.y, margin.x * Variables.GameScale, margin.y,
                width, 512, 16);
        }

        public void Initialize(GameState game_state, UiState ui_state)
        {
            Button fullscreen = new Button("fullscreen");
            this.controls.Add(fullscreen, () => {
                Config.ToggleFullscreen();
                fullscreen.SetHovered(Config.Fullscreen);
            });
            this.controls.Add(new Button("close"), () => {
                this.close = true;
            });
            this.controls.Add(new Button("force exit"), () => {
                Config.Exit();
            });
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
            //UpdateOrigin();
            controls.Update(KB, MS);
        }
    }
}
