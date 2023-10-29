using Abyss.Globals;
using Abyss.Magic;
using Abyss.Master;
using Abyss.UI;
using Abyss.UI.Controllers;
using Abyss.UI.Menus;
using Abyss.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using Text = Abyss.UI.Text;

namespace Abyss.Draw
{
    internal partial class DrawState : SpriteBatch
    {
        public void Draw(Text text, bool background = false)
        {
            if (background)
                this.FillRectangle(new RectangleF(text.GetX(), text.GetY(), text.GetWidth(), text.GetHeight()), Color.Black);
            this.DrawString(_Sprites.Font, text.GetText(), text.GetPosition(), Color.White, 0, Vector2.Zero, text.GetScale(), SpriteEffects.None, 0);
        }

        public void Draw(Text text, Color color)
        {
            this.DrawString(_Sprites.Font, text.GetText(), text.GetPosition(), color, 0, Vector2.Zero, text.GetScale(), SpriteEffects.None, 0);
        }


        public void Draw(Hud hud, GameState gs)
        {
            // draw the debug HUD on screen
            if (Variables.DebugDraw)
            {
                Draw(new Text("HP: " + gs.player.GetHealth() + "/" + gs.player.GetMaxHealth(), 16, 16, 0.5f));
                Draw(new Text("MN: " + gs.player.GetMana() + "/" + gs.player.GetMaxMana(), 16, 32, 0.5f));
                Draw(new Text("POS: " + gs.player.GetPosition().X + ", " + gs.player.GetPosition().Y, 16, 48, 0.3f));
                Draw(new Text("TPOS: " + Math0.CoordsToTileCoords(gs.player.GetPosition()).x + ", " + Math0.CoordsToTileCoords(gs.player.GetPosition()).y, 16, 56, 0.3f));
                Draw(new Text("Ms-W: " + (int)InputUtility.MousePosition().X + ", " + (int)InputUtility.MousePosition().Y, 16, 64, 0.4f));
                Draw(new Text("Ms-G: " + (int)InputUtility.MousePositionInGame().X + ", " + (int)InputUtility.MousePositionInGame().Y, 16, 80, 0.4f));
                Draw(new Text("Grim: " + gs.player.inventory.grimoires[0].ToString() + ", " + gs.player.inventory.grimoires[1].ToString(), 16, 96, 0.3f));
                Draw(new Text("DMG: " + gs.player.last_damage, 16, 112, 0.3f));
                Draw(new Text("VEL: " + Math.Round(gs.player.GetVelocity().X, 2) + ", " + Math.Round(gs.player.GetVelocity().Y, 2), 16, 128, 0.3f));
                Draw(new Text((int)gs.fps + " fps", (int)Variables.DrawPosition.X + 16 * 17, 16, 0.5f));
                Draw(new Text(1 / gs.fps + "", (int)Variables.DrawPosition.X + 16 * 17, 32, 0.5f));
                Draw(new Text("CG: " + gs.player.current_grimoire, 16, 104, 0.3f));
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

        public void Draw(DebugConsole console)
        {
            this.FillRectangle(new RectangleF(0, 0, Variables.WindowW, 16 * 2), Color.Black);
            DrawString(_Sprites.Font, Game._TextInput.ToString(), new Vector2(8, 8), Color.White, 0, Vector2.Zero, 0.4f, SpriteEffects.None, 0);
            this.FillRectangle(new RectangleF(8 + Text.CalculatePixelWidth(Game._TextInput.ToString(), 0.4f), 8 + 10 * 0.4f, 14 * 0.4f, 19 * 0.4f), Variables.ShiftingColor);
        }

        public void Draw(UI.Dialogue dialogue)
        {
            this.Draw(dialogue.master);
            if (dialogue.options != null)
                foreach (Text option in dialogue.options)
                    this.Draw(option);
        }

        public void Draw(Interaction interaction)
        {
            this.Draw(interaction.GetDialogue());
        }

        public void Draw(Option options)
        {
            Draw(options.controls);
        }

        public void Draw(IGui ui, GameState gs)
        {
            if (ui is Hud)
            {
                Draw(ui as Hud, gs);
            }
            else if (ui is DebugConsole)
            {
                Draw(ui as DebugConsole);
            }
            else if (ui is Interaction)
            {
                Draw(ui as Interaction);
            }
            else if (ui is Option)
            {
                Draw(ui as Option);
            }
        }

        public void Draw(ListController controls)
        {
            for (int i = controls.TopIndex(); i < controls.Size(); i++)
            {
                // move all y values back by the top item (top should be y: 0)
                // so modify by (i - topindex)
                Draw(controls.Get(i), -(i - controls.TopIndex()) * controls.GetItemHeight());
            }
        }

        public void Draw(IController control, float y_offset)
        {
            if (control is Button)
            {
                Draw(control as Button, y_offset);
            }
        }

        public void Draw(Button button, float y_offset=0)
        {
            if (button.IsHovered())
            {
                this.FillRectangle(button.GetDrawBox(y_offset), Color.White); // draw background
                this.Draw(button.GetLabel(y_offset), Color.Black); // draw label
            }
            else
            {
                this.DrawRectangle(button.GetDrawBox(y_offset), Color.White); // draw outline
                this.Draw(button.GetLabel(y_offset)); // draw label
            }
        }
    }
}
