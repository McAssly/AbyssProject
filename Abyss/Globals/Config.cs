using Abyss.Master;
using Abyss.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.Globals
{
    internal class Config
    {
        public static double MaxFrameRate = 144;
        public static double WindowScalar = 1.5;


        public static void SetWindowScale(double scale, GraphicsDeviceManager _graphics)
        {
            WindowScalar = scale;
            Variables.UpdateGameScaling(_graphics);
        }

        public static void Fullscreen(bool enable, GraphicsDeviceManager _graphics)
        {
            if (enable)
            {
                Variables.GameScale = 4;
                Variables.WindowW = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                Variables.WindowH = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                Variables.DrawPosition = Math0.CenterWithinRectangle(Variables.WindowW, Variables.WindowH, 256, 256, 4).To3();
                Variables.UpdateWindowSize(_graphics);
                _graphics.ToggleFullScreen();
            }
            else
            {
                _graphics.ToggleFullScreen();
                Variables.UpdateGameScaling(_graphics);
            }
        }
    }
}
