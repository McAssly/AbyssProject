using Abyss.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Diagnostics;

namespace Abyss.UI.Controllers
{
    internal class Button : IController
    {
        private protected Text label;
        private protected Rectangle bounds;
        public bool hovered;
        public bool enabled;
        public bool visible;

        internal Action action;

        /// <summary>
        /// Makes a button with a given action
        /// </summary>
        /// <param name="label"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Button(string label, int x, int y, int width, int height)
        {
            bounds = new Rectangle(x, y, width, height);
            this.label = MakeCenteredLabel(label, bounds);
            enabled = false;
            hovered = false;
            visible = true;
        }

        public Button(string label)
        {
            this.bounds = new Rectangle(0, 0, 0, 0);
            this.label = new Text(label, new Vector2(0, 0), 1);
            this.enabled = false;
            this.hovered = false;
            this.visible = true;
        }


        /// <summary>
        /// updates the button's state
        /// </summary>
        /// <param name="ms"></param>
        public void Update(MouseState ms)
        {
            Press(ms);
            if (this.enabled)
            {
                action?.Invoke();
                this.enabled = false;
                return;
            }
        }


        /// <summary>
        /// presses the button if passing a hovering/click condition
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="mouse_position"></param>
        /// <param name="output"></param>
        public void Press(MouseState ms, bool hoverstate = true)
        {
            if (Math0.WithinRectangle(InputUtility.MousePosition(), bounds)) // hovering over the checkbox
            {
                hovered = hoverstate;
                if (InputUtility.IsClicked(ms, 1, true))
                { // if the user clicked within bounds
                    this.enabled = true;
                    return;
                }
            }
            else hovered = !hoverstate;
        }

        public bool IsHovered()
        {
            return hovered;
        }

        public RectangleF GetDrawBox(float y_offset = 0)
        {
            return new RectangleF(bounds.X, bounds.Y, bounds.Width, bounds.Height);
        }

        public Text GetLabel(float y_offset = 0)
        {
            if (y_offset > 0) {
                return label.CreateAtOffset(0, y_offset);
            }
            return label;
        }

        bool IController.IsVisible()
        {
            return visible;
        }

        bool IController.IsPressed()
        {
            throw new System.NotImplementedException();
        }

        public void SetHovered(bool hovered)
        {
            this.hovered = hovered;
        }

        public void Set(float x, float y, float w, float h)
        {
            this.bounds = new Rectangle((int)x, (int)y, (int)w, (int)h);
            this.label = MakeCenteredLabel(label.GetText(), this.bounds);
        }

        public void SetAction(Action action)
        {
            this.action += action;
        }

        /// <summary>
        /// makes a label positioned within the given bounds (formatted)
        /// </summary>
        /// <param name="text"></param>
        /// <param name="bounds"></param>
        /// <param name="padding"></param>
        /// <returns></returns>
        public static Text MakeCenteredLabel(string text, Rectangle bounds, int padding = 10)
        {
            // determine the inner bounds for the label based on the given padding
            int inner_width = bounds.Width - padding;
            Vector center_point = Math0.CenterWithinRectangle(bounds.Width, bounds.Height, (int)Text.CalculatePixelWidth(text, 0.5f), bounds.Height + 4);

            text = Text.FormatInWidth(text, inner_width, 0.5f);
            return new Text(text, bounds.X + center_point.x, bounds.Y + center_point.y, 0.5f);
        }
    }
}
