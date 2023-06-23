using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        private protected float scale;
        private protected string text;

        private protected bool selectable;
        public Text(string text, Vector2 position, float scale, bool selectable = false) 
        { 
            this.text = text;
            this.pos = position;
            this.scale = scale;
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

        public void Draw(SpriteBatch spriteBatch)
        {
            if (spriteBatch == null) return;
            spriteBatch.DrawString(Globals.FONT, text, pos, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }

        public static void Draw(SpriteBatch spriteBatch, StringBuilder input, Vector2 pos, float scale)
        {
            if (spriteBatch == null) return;
            spriteBatch.DrawString(Globals.FONT, input.ToString(), pos, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }
    }
}
