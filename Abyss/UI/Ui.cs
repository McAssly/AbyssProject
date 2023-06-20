using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.UI
{
    internal class UiControllers
    {
        // Declare every single UI menu in the game
        public static Interaction Dialogue = new Interaction();
        public static Console Debug = new Console();
        public static Inventory Invenetory = new Inventory();
        public static Interaction Shop = new Interaction();
        public static Menu Main = new Menu();
        public static Menu Options = new Menu();

        // in game HUD
        public static Hud HUD = new Hud();
    }

    internal interface Ui
    {
        public bool IsClosed();
        public void UnClose();
        public void Update(KeyboardState KB, MouseState MS);
        public void Draw(SpriteBatch spriteBatch);
    }

    internal class Hud : Ui
    {
        public bool close = false;
        public bool IsClosed() {  return close; }
        public void UnClose() { close = false; }
        public Hud() { }
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
        private TextBuilder input = new TextBuilder("", new Vector2(8, 8), (float)0.2, false);
        public Console() { }

        /// <summary>
        /// Does nothing, refer to 
        /// </summary>
        /// <param name="KB"></param>
        /// <param name="MS"></param>
        public void Update(KeyboardState KB, MouseState MS)
        {
            if (KB.GetPressedKeyCount() == 1)
                RegisterInput(OnInput);
            else
                UnRegisterInput(OnInput);
        }
        public static void RegisterInput(EventHandler<TextInputEventArgs> method)
        {
            Game.gw.TextInput += method;
        }
        public static void UnRegisterInput(EventHandler<TextInputEventArgs> method)
        {
            Game.gw.TextInput -= method;
        }

        public void OnInput(object sender, TextInputEventArgs e)
        {
            Keys? k = e.Key;
            char? c = e.Character;
            Debug.WriteLine(k.ToString());
            if (k.Equals(Keys.Enter))
                close = true;
            else if (k.Equals(Keys.Back))
                input.Delete();
            else
            {
                input.Append(c);
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
