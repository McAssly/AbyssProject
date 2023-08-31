using Abyss.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
namespace Abyss.UI
{
    public delegate void ButtonAction();

    internal class Button
    {
        private protected Text label;
        private protected Rectangle bounds;
        public bool hovered;
        public bool enabled;

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
        }


        /// <summary>
        /// updates the button's state
        /// </summary>
        /// <param name="ms"></param>
        public void Update(MouseState ms)
        {
            Press(ms, InputUtility.MousePosition());
            if (enabled)
            {
                Action?.Invoke();
                enabled = false;
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
            Vector center_point = Math0.CenterWithinRectangle(bounds.Width, bounds.Height, (int)Text.CalculatePixelWidth(text, 0.5f), bounds.Height + 4);

            text = Text.FormatInWidth(text, inner_width, 0.5f);
            return new Text(text, bounds.X + center_point.x, bounds.Y + center_point.y, 0.5f);
        }
    }
}
