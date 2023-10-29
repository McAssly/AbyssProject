using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;

namespace Abyss.UI
{
    internal interface IController
    {
        public void Update(MouseState ms);
        public void Press(MouseState ms, bool output=true);



        public RectangleF GetDrawBox(float y_offset);
        public Text GetLabel(float y_offset);
        public bool IsVisible();
        public bool IsHovered();
        public bool IsPressed();
        public void Set(float x, float y, float w, float h);
        public void SetAction(Action action);
    }
}
