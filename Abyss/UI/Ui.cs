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
        public void Close();
        public bool IsClosed();
        public void UnClose();
        public void Update(KeyboardState KB, MouseState MS);
        public void Draw(SpriteBatch spriteBatch);
    }

    internal class Hud : Ui
    {
        public bool close = false;
        public void Close() { close = true; }
        public bool IsClosed() {  return close; }
        public void UnClose() { close = false; }
        public Hud() { }
        public void Update(KeyboardState KB, MouseState MS) { }
        public void Draw(SpriteBatch spriteBatch) { }
    }

    internal class Interaction : Ui
    {
        public bool close = false;
        public void Close() { close = true; }
        public bool IsClosed() { return close; }
        public void UnClose() { close = false; }
        public void Update(KeyboardState KB, MouseState MS) { }
        public void Draw(SpriteBatch spriteBatch) { }
    }

    internal class Console : Ui
    {
        public bool close = false;
        public void Close() { close = true; }
        public bool IsClosed() { return close; }
        public void UnClose() { close = false; }
        public Console() { }

        public void Update(KeyboardState KB, MouseState MS) { }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.FillRectangle(new RectangleF(0, 0, Globals.WindowW, Globals.TILE_SIZE * 2), Globals.Black);
            Text.Draw(spriteBatch, Game._TextInput, new Vector2(8, 8), (float)0.2);
        }
    }

    internal class Inventory : Ui
    {
        public bool close = false;
        public void Close() { close = true; }
        public bool IsClosed() { return close; }
        public void UnClose() { close = false; }
        public void Update(KeyboardState KB, MouseState MS) { }
        public void Draw(SpriteBatch spriteBatch) { }
    }

    internal class Menu : Ui
    {
        public bool close = false;
        public void Close() { close = true; }
        public bool IsClosed() { return close; }
        public void UnClose() { close = false; }
        public void Update(KeyboardState KB, MouseState MS) { }
        public void Draw(SpriteBatch spriteBatch) { }
    }
}
