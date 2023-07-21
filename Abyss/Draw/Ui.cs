using Abyss.Entities;
using Abyss.Master;
using Abyss.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.Draw
{
    internal partial class DrawBatch : SpriteBatch
    {
        public void Draw(Text text)
        {
            Color bg, fg;
            fg = Color.White;
            bg = Globals.Black;
            // the text is selectable change the colors when hovering
            if (text.IsSelectable())
            {
                if (text.Hovering()) // the user is hovering over the textbox
                { // invert the background and foreground colors
                    bg = Color.White;
                    fg = Globals.Black;
                }
                this.FillRectangle(new RectangleF(text.GetX(), text.GetY(), text.GetWidth(), text.GetHeight()), bg);
            }
            else
            {
                this.FillRectangle(new RectangleF(text.GetX(), text.GetY(), text.GetWidth(), text.GetHeight()), bg);
            }
            DrawString(Globals.Font, text.GetText(), text.GetPosition(), fg, 0, Vector2.Zero, text.GetScale(), SpriteEffects.None, 0);
        }


        public void Draw(Hud hud, GameMaster GM)
        {
            // draw the debug HUD on screen
            if (UiControllers.SHOW_DEBUG_HUD)
            {
                Draw(new Text("HP: " + GM.player.GetHealth() + "/" + GM.player.GetMaxHealth(), 16, 16, 0.5f));
                Draw(new Text("MN: " + GM.player.GetMana() + "/" + GM.player.GetMaxMana(), 16, 32, 0.5f));
                Draw(new Text("POS: " + (int)GM.player.GetPosition().X + ", " + (int)GM.player.GetPosition().Y, 16, 48, 0.5f));
                Draw(new Text("Ms-W: " + (int)MathUtil.MousePosition().X + ", " + (int)MathUtil.MousePosition().Y, 16, 64, 0.4f));
                Draw(new Text("Ms-G: " + (int)MathUtil.MousePositionInGame().X + ", " + (int)MathUtil.MousePositionInGame().Y, 16, 80, 0.4f));
                Draw(new Text("Grim: " + GM.player.Inventory.grimoires[0].ToString() + ", " + GM.player.Inventory.grimoires[1].ToString(), 16, 96, 0.3f));
                Draw(new Text("DMG: " + GM.player.last_damage, 16, 112, 0.3f));
                Draw(new Text((int)GM.fps + " fps", (int) Globals.DrawPosition.X + 16 * 17, 16, 0.5f));
                Draw(new Text(1 / GM.fps + "", (int)Globals.DrawPosition.X + 16 * 17, 32, 0.5f));
                // draw the status effects on the player
                for (int i = 0; i < GM.player.statuses.Count; i++)
                {
                    StatusEffect effect = GM.player.statuses[i];
                    Draw(new Text(effect.ToString(), (int)Globals.DrawPosition.X + 16 * 17, 48 + 8 * i, 0.3f));
                }
            }
            else // draw the regular HUD
            {

            }
        }

        public void Draw(UI.Console console)
        {
            this.FillRectangle(new RectangleF(0, 0, Globals.WindowW, Globals.TILE_SIZE * 2), Globals.Black);
            DrawString(Globals.Font, Game._TextInput.ToString(), new Vector2(8, 8), Color.White, 0, Vector2.Zero, (float)0.3, SpriteEffects.None, 0);
        }

        public void Draw(UI.Dialogue dialogue)
        {
            this.Draw(dialogue.master);
            if (dialogue.options != null)
                foreach (Text option in dialogue.options)
                    this.Draw(option);
        }

        public void Draw(Ui ui, GameMaster GM)
        {
            if (ui is Hud)
            {
                Draw(ui as Hud, GM);
            }
            else if (ui is UI.Console)
            {
                Draw(ui as UI.Console);
            }
            else if (ui is UI.Dialogue)
            {
                Draw(ui as UI.Dialogue);
            }
        }
    }
}
