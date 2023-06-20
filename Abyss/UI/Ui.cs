using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.UI
{
    internal interface Ui
    {
        public bool IsClosed();
        public void UnClose();
        public void Update(KeyboardState KB, MouseState MS);
        public void Draw(SpriteBatch spriteBatch);
    }

    internal class HUD : Ui
    {
        public bool close = false;
        public bool IsClosed() {  return close; }
        public void UnClose() { close = false; }
        public HUD() { }
        public void Update(KeyboardState KB, MouseState MS) { }
        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }

    internal class Interaction : Ui
    {
        public bool close = false;
        public bool IsClosed() { return close; }
        public void UnClose() { close = false; }
        public void Update(KeyboardState KB, MouseState MS) { }
        public void Draw(SpriteBatch spriteBatch)
        {
           
        }
    }

    internal class Console : Ui
    {
        public bool close = false;
        public bool IsClosed() { return close; }
        public void UnClose() { close = false; }
        private Text input = new Text("", new Vector2(8, 8), (float)0.2, false);
        public Console() { }

        /// <summary>
        /// Updates the console process, and takes in keyboard inputs primarily
        /// </summary>
        /// <param name="KB"></param>
        /// <param name="MS"></param>
        public void Update(KeyboardState KB, MouseState MS)
        {
            if (KB.IsKeyDown(Keys.Enter))
                close = true;
            if (KB.IsKeyDown(Keys.Back))
                input.Delete();
            else
            {
                if (KB.GetPressedKeys().Length > 0)
                {
                    input.Append(KB.GetPressedKeys()[0].ToString());
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.FillRectangle(new RectangleF(0, 0, Globals.WindowW, Globals.TILE_SIZE * 2), Globals.Black);
            input.Draw(spriteBatch);
        }
    }

    internal class Inventory : Ui
    {
        public bool close = false;
        public bool IsClosed() { return close; }
        public void UnClose() { close = false; }
        public void Update(KeyboardState KB, MouseState MS) { }
        public void Draw(SpriteBatch spriteBatch)
        {
            
        }
    }

    internal class Menu : Ui
    {
        public bool close = false;
        public bool IsClosed() { return close; }
        public void UnClose() { close = false; }
        public void Update(KeyboardState KB, MouseState MS) { }
        public void Draw(SpriteBatch spriteBatch)
        {
           
        }
    }
}
