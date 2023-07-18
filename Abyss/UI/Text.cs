using Abyss.Master;
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
        private protected Vector2 pos;
        private protected Vector2 size;
        private protected float scale;
        private protected string text;

        private protected bool selectable;
        private protected bool selected = false;
        public Text(string text, int x, int y, float scale, Vector2? size = null, bool selectable = false) 
        { 
            this.text = text;
            this.pos = new Vector2(x, y);
            this.scale = scale;
            this.selectable = selectable;
            if (size.HasValue)
                this.size = size.Value;
        }

        public Text(string text, Vector2 position, float scale, Vector2? size = null, bool selectable = false)
        {
            this.text = text;
            this.pos = position;
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
            Vector2 m = MathUtil.MousePosition();
            return m.X >= pos.X && m.X <= pos.X + size.X
                && m.Y >= pos.Y && m.Y <= pos.Y + size.Y;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (spriteBatch == null) return;
            Color bg, fg;
            fg = Color.White;
            bg = Globals.Black;
            // the text is selectable change the colors when hovering
            if (selectable)
            {
                if (Hovering()) // the user is hovering over the textbox
                { // invert the background and foreground colors
                    bg = Color.White;
                    fg = Globals.Black;
                }
                spriteBatch.FillRectangle(new RectangleF(pos.X, pos.Y, size.X, size.Y), bg);
            }
            else
            {
                spriteBatch.FillRectangle(new RectangleF(pos.X, pos.Y, size.X, size.Y), bg);
            }
            spriteBatch.DrawString(Globals.FONT, text, pos, fg, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }





        public static void Draw(SpriteBatch spriteBatch, StringBuilder input, Vector2 pos, float scale)
        {
            if (spriteBatch == null) return;
            spriteBatch.DrawString(Globals.FONT, input.ToString(), pos, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }
    }
}
