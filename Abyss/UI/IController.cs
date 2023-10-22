using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Abyss.UI
{
    internal interface IController
    {
        public void Update(MouseState ms);
        public void Press(MouseState ms, bool output=true);



        public RectangleF GetDrawBox();
        public Text GetLabel();
        public bool IsVisible();
        public bool IsHovered();
        public bool IsPressed();
    }
}
