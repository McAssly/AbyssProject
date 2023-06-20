using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.UI
{
    internal class Text
    {
        private Vector2 pos;
        private float scale;
        private string text;

        private bool selectable;
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
    }
}
