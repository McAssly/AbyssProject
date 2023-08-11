using Abyss.Globals;
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

        public float GetPixelWidth()
        {
            return CalculatePixelWidth(text, scale);
        }

        public static float CalculatePixelWidth(string text, float scale)
        {
            int perfect = PerfectWidth(text);
            int imperfect = ImperfectWidth(text);
            return (perfect + imperfect) * scale;
        }

        private static int ImperfectWidth(string text)
        {
            StringBuilder imperfect_width = new StringBuilder();
            foreach (char c in text)
                if (!char.IsLetter(c) || !AcceptableWidth(c) || !char.IsDigit(c))
                    imperfect_width.Append(c);
            string imperfect = imperfect_width.ToString();
            int width = 0;
            foreach (char c in imperfect)
                width += PxWidth(c);
            return width;
        }

        private static int PerfectWidth(string text)
        {
            StringBuilder perfect_width = new StringBuilder();
            foreach (char c in text)
                if (char.IsLetter(c) || AcceptableWidth(c) || char.IsDigit(c))
                    perfect_width.Append(c);
            return perfect_width.Length * (14 + Variables.TextSpacing);
        }

        private static int PxWidth(char c)
        {
            switch (c)
            {
                case ' ': return 1 + Variables.TextSpacing;
                case '!': case '|': case '\'': return 3 + Variables.TextSpacing;
                case '*': case '-': case '"': return 9 + Variables.TextSpacing;
                case '(': case ')': case '[': case ']': case '{': case '}': return 11 + Variables.TextSpacing;
                case '.': case ',': case ';': case ':': return 6 + Variables.TextSpacing;
                default: return 0;
            }
        }

        private static bool AcceptableWidth(char c)
        {
            switch (c)
            {
                case '@': case '~': case '#': case '$': case '%': case '/': case '\\': case '^': case '&':
                case '=': case '_': case '+': case '?': case '<': case '>':
                    return true;
                default: return false;
            }
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
