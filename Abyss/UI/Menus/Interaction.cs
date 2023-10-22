using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.UI.Menus
{
    internal class Interaction : IGui
    {
        public bool close = false;

        /**
         * 
         * 
         * WHERE I LEFT OFF WORKING ON DIALOGUE MENUS
         * WORK WITH HOVERING TEXT AND MOVING THROUGH A DIALOGUE TREE
         * 
         * 
         * 
         */
        private Dialogue dialogue;

        public Dialogue GetDialogue() { return dialogue; }

        public void SetDialogue(Dialogue dialogue)
        {
            this.dialogue = dialogue;
        }

        public void Close() { close = true; }
        public bool IsClosed() { return close; }
        public void UnClose() { close = false; }
        public void Update(KeyboardState KB, MouseState MS) { }
    }
}
