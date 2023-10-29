using Abyss.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;

namespace Abyss.UI.Controllers
{
    internal delegate void SliderAction(float value);
    internal class Slider : IController
    {
        private Text label;
        private Vector2 position;
        private bool vertical;
        private float length;
        private float width;
        private float value;
        private bool hovered;
        private bool pressed;
        internal Action action;
        private bool visible;

        public Slider(bool vertical, string label, float x, float y, float length, float width, float value, bool visible)
        {
            this.vertical = vertical;
            this.position = new Vector2(x, y);
            this.label = new Text(label, position, 1);
            this.length = length;
            this.width = width;
            this.value = value;
            this.visible = visible;
        }

        RectangleF IController.GetDrawBox(float y_offset)
        {
            throw new NotImplementedException();
        }

        internal float GetValue() { return this.value; }

        Text IController.GetLabel(float y_offset) { return label; }
        bool IController.IsHovered() { return hovered; }
        bool IController.IsPressed() { return pressed; }
        bool IController.IsVisible() { return visible; }

        void IController.Press(MouseState ms, bool output)
        {
            throw new NotImplementedException();
        }

        public void Set(float x, float y, float w, float l)
        {
            this.position = new Vector2(x, y);
            this.width = w;
            this.length = l;
        }

        void IController.SetAction(Action action)
        {
            throw new NotImplementedException("Slider does not contain a basic action.");
        }

        void IController.Update(MouseState ms)
        {
            throw new NotImplementedException();
        }
    }
}
