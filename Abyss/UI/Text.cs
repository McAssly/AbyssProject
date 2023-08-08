using Abyss.Master;
using Abyss.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.UI
{
    internal class Text
    {
        private protected Vector2 position;
        private protected Vector2 size;
        private protected float scale;
        private protected string text;

        private protected bool selectable;
        private protected bool selected = false;
        public Text(string text, int x, int y, float scale, Vector2? size = null, bool selectable = false)
        {
            this.text = text;
            this.position = new Vector2(x, y);
            this.scale = scale;
            this.selectable = selectable;
            if (size.HasValue)
                this.size = size.Value;
        }

        public Text(string text, Vector2 position, float scale, Vector2? size = null, bool selectable = false)
        {
            this.text = text;
            this.position = position;
            this.scale = scale;
            this.selectable = selectable;
            if (size.HasValue)
                this.size = size.Value;
        }

        public void Append(string text)
        {
            this.text += text;
        }

        public void Delete()
        {
            if (!string.IsNullOrEmpty(text))
            {
                text = text.Remove(text.Length - 1);
            }
        }

        public void Update(string new_text)
        {
            if (text != new_text)
                text = new_text;
        }

        public void UpdateSelection(MouseState MS)
        {
            // if this text box is not selectable then who cares move on
            if (!this.selectable) return;

            // otherwise lets try and make it selectable
        }

        public bool Hovering()
        {
            Vector2 m = InputUtility.MousePosition();
            return m.X >= position.X && m.X <= position.X + size.X
                && m.Y >= position.Y && m.Y <= position.Y + size.Y;
        }

        public bool IsSelectable()
        {
            return this.selectable;
        }

        public float GetX()
        {
            return position.X;
        }
        public float GetY()
        {
            return position.Y;
        }
        public Vector2 GetPosition()
        {
            return position;
        }
        public float GetWidth()
        {
            return size.X;
        }
        public float GetHeight()
        {
            return size.Y;
        }
        public float GetScale()
        {
            return scale;
        }
        public string GetText()
        {
            return text;
        }
    }
}
