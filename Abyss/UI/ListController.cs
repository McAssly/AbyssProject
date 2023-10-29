using Abyss.UI.Controllers;
using Microsoft.Xna.Framework.Input;
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

            this.scroll_bar = new Slider(true, "", x - margin_x / 2, y + margin_y / 2, margin_x, max_height, 0f, false);
            scroll_bar.action += () => {
                this.top_item = (int)(scroll_bar.GetValue() * controllers.Count());
            };
        }

        public void Add(IController controller, Action action)
        {
            // get the position of a new controller in the list
            // set position of controller here (overrides given controller's position and size)
            controller.Set(origin.X, origin.Y + controller_height * controllers.Count() + controllers.Count() * 3, bounds.X, controller_height);
            // set the action for the controller
            controller.SetAction(action);
            // add controller
            controllers.Add(controller);
        }

        public void Add(Slider slider, Action action)
        {
            slider.Set(origin.X, origin.Y + controller_height * controllers.Count() + controllers.Count() * 3, controller_height, bounds.X);
            slider.action += action;
            controllers.Add(slider);
        }

        public int TopIndex()
        {
            return top_item;
        }

        public int Size()
        {
            return controllers.Count();
        }

        public IController Get(int index)
        {
            return controllers[index];
        }

        public float GetItemHeight()
        {
            return controller_height;
        }

        internal void Update(KeyboardState kb, MouseState ms)
        {
            for (int i = top_item; i < controllers.Count; i++)
            {
                controllers[i].Update(ms);
            }
        }
    }
}
