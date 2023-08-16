using Abyss.Levels;
using Abyss.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.UI
{
    public delegate void ButtonAction();

    internal class Button
    {
        private protected Text label;
        private protected Rectangle bounds;
        public bool hovered;
        public bool enabled;
        private protected readonly bool is_checkbox;

        public event ButtonAction Action;

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
            this.bounds = new Rectangle(x, y, width, height);
            this.label = Button.MakeCenteredLabel(label, bounds);
            this.enabled = false;
            this.hovered = false;
            this.is_checkbox = false;
        }


        /// <summary>
        /// Makes a checkbox type button
        /// </summary>
        /// <param name="label_x"></param>
        /// <param name="label_y"></param>
        /// <param name="label"></param>
        /// <param name="box_x"></param>
        /// <param name="box_y"></param>
        /// <param name="size"></param>
        public Button(int label_x, int label_y, string label, int box_x, int box_y, int size)
        {
            this.bounds =  new Rectangle(box_x, box_y, size, size);
            this.label = new Text(label, label_x, label_y, 1);
            this.enabled = false;
            this.hovered = false;
            this.is_checkbox = true;
        }


        /// <summary>
        /// updates the button's state
        /// </summary>
        /// <param name="ms"></param>
        public void Update(MouseState ms)
        {
            Vector2 mouse_position = InputUtility.MousePosition();
            if (is_checkbox)
            {
                bool prev_state = enabled;
                Press(ms, mouse_position, !enabled);
                if (prev_state != enabled) Action?.Invoke();
            }
            else
            {
                Press(ms, mouse_position);
                if (enabled)
                {
                    Action?.Invoke();
                    enabled = false;
                }
            }
        }


        /// <summary>
        /// presses the button if passing a hovering/click condition
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="mouse_position"></param>
        /// <param name="output"></param>
        public void Press(MouseState ms, Vector2 mouse_position, bool output = true)
        {
            if (Math0.WithinRectangle(mouse_position, bounds)) // hovering over the checkbox
            {
                hovered = output;
                if (InputUtility.IsClicked(ms, 1)) enabled = output;
            }
            else
                hovered = !output;
        }


        public bool IsCheckBox()
        {
            return is_checkbox;
        }

        public bool IsHovered()
        {
            return hovered;
        }

        public RectangleF GetDrawBackground()
        {
            return new RectangleF(bounds.X, bounds.Y, bounds.Width, bounds.Height);
        }

        public Text GetLabel()
        {
            return label;
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
            int inner_x = bounds.X + padding;
            int inner_y = bounds.Y + padding;

            text = Text.FormatInWidth(text, inner_width, 1);
            return new Text(text, inner_x, inner_y, 1);
        }
    }
}
