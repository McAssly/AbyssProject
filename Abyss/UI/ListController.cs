using Abyss.UI.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.UI
{
    internal class ListController
    {
        private List<IController> controllers = new List<IController>();
        private Vector2 origin;
        private Vector2 margin;
        private Slider scroll_bar;
        private Vector2 bounds;
        private float controller_height;
        private int top_item = 0; // index of the top item

        public ListController(float x, float y, float margin_x, float margin_y, float max_width, float max_height, float control_height)
        {
            this.origin = new Vector2(x, y);
            this.margin = new Vector2(margin_x, margin_y);
            this.controller_height = control_height;
            this.bounds = new Vector2(max_width, max_height);

            this.scroll_bar = new Slider(true, "", x - margin_x/2, y + margin_y / 2, margin_x, max_height, 0f, false);
            scroll_bar.Action += () => {
                this.top_item = (int)(value * controllers.Count());
            };
        }

        public void Add(IController controller)
        {
            // set position of controller here (overrides given controller's position and size)
            // controller's action should already be set before adding
            controllers.Add(controller);
        }
    }
}
