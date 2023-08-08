using Abyss.Globals;
using Abyss.Magic;
using Abyss.Master;
using Abyss.UI;
using Abyss.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Text = Abyss.UI.Text;

namespace Abyss.Draw
{
    internal partial class DrawState : SpriteBatch
    {
        public void Draw(Text text)
        {
            Color bg, fg;
            fg = Color.White;
            bg = Color.Black;
            // the text is selectable change the colors when hovering
            if (text.IsSelectable())
            {
                if (text.Hovering()) // the user is hovering over the textbox
                { // invert the background and foreground colors
                    bg = Color.White;
                    fg = Color.Black;
                }
                this.FillRectangle(new RectangleF(text.GetX(), text.GetY(), text.GetWidth(), text.GetHeight()), bg);
            }
            else
            {
                this.FillRectangle(new RectangleF(text.GetX(), text.GetY(), text.GetWidth(), text.GetHeight()), bg);
            }
            this.DrawString(_Sprites.Font, text.GetText(), text.GetPosition(), fg, 0, Vector2.Zero, text.GetScale(), SpriteEffects.None, 0);
        }


        public void Draw(Hud hud, GameState gs)
        {
            // draw the debug HUD on screen
            if (Variables.DebugDraw)
            {
                Draw(new Text("HP: " + gs.player.GetHealth() + "/" + gs.player.GetMaxHealth(), 16, 16, 0.5f));
                Draw(new Text("MN: " + gs.player.GetMana() + "/" + gs.player.GetMaxMana(), 16, 32, 0.5f));
                Draw(new Text("POS: " + gs.player.GetPosition().X + ", " + gs.player.GetPosition().Y + "\nTPOS: " +
                    Math0.CoordsToTileCoords(gs.player.GetPosition()).x + ", " + Math0.CoordsToTileCoords(gs.player.GetPosition()).y
                    , 16, 48, 0.3f));
                Draw(new Text("Ms-W: " + (int)InputUtility.MousePosition().X + ", " + (int)InputUtility.MousePosition().Y, 16, 64, 0.4f));
                Draw(new Text("Ms-G: " + (int)InputUtility.MousePositionInGame().X + ", " + (int)InputUtility.MousePositionInGame().Y, 16, 80, 0.4f));
                Draw(new Text("Grim: " + gs.player.inventory.grimoires[0].ToString() + ", " + gs.player.inventory.grimoires[1].ToString(), 16, 96, 0.3f));
                Draw(new Text("DMG: " + gs.player.last_damage, 16, 112, 0.3f));
                Draw(new Text((int)gs.fps + " fps", (int)Variables.DrawPosition.X + 16 * 17, 16, 0.5f));
                Draw(new Text(1 / gs.fps + "", (int)Variables.DrawPosition.X + 16 * 17, 32, 0.5f));
                // draw the status effects on the player
                for (int i = 0; i < gs.player.statuses.Count; i++)
                {
                    StatusEffect effect = gs.player.statuses[i];
                    Draw(new Text(effect.ToString(), (int)Variables.DrawPosition.X + 16 * 17, 48 + 8 * i, 0.3f));
                }
            }
            else // draw the regular HUD
            {

            }
        }

        public void Draw(UI.Console console)
        {
            this.FillRectangle(new RectangleF(0, 0, Variables.WindowW, 16 * 2), Color.Black);
            DrawString(_Sprites.Font, Game._TextInput.ToString(), new Vector2(8, 8), Color.White, 0, Vector2.Zero, (float)0.3, SpriteEffects.None, 0);
        }

        public void Draw(UI.Dialogue dialogue)
        {
            this.Draw(dialogue.master);
            if (dialogue.options != null)
                foreach (Text option in dialogue.options)
                    this.Draw(option);
        }

        public void Draw(UI.Interaction interaction)
        {
            this.Draw(interaction.GetDialogue());
        }

        public void Draw(Ui ui, GameState gs)
        {
            if (ui is Hud)
            {
                Draw(ui as Hud, gs);
            }
            else if (ui is UI.Console)
            {
                Draw(ui as UI.Console);
            }
            else if (ui is UI.Interaction)
            {
                Draw(ui as UI.Interaction);
            }
        }
    }
}
