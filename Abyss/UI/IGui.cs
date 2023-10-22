using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.UI
{
    public delegate void ControllerAction();

    internal interface IGui
    {
        public void Close();
        public bool IsClosed();
        public void UnClose();
        public void Update(KeyboardState KB, MouseState MS);
    }
}
