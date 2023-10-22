using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.UI.Menus
{
    internal class PlayerMenu : IGui
    {
        public bool close = false;
        public void Close() { close = true; }
        public bool IsClosed() { return close; }
        public void UnClose() { close = false; }
        public void Update(KeyboardState KB, MouseState MS) { }
    }
}
