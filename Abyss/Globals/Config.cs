using Abyss.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Abyss.Globals
{
    internal class Config
    {
        public static double MaxFrameRate = 144;
        public static double WindowScalar = 1.5;
        public static bool Fullscreen = false;


        private static void SetWindowScale(double scale, GraphicsDeviceManager _graphics)
        {
            WindowScalar = scale;
            Variables.UpdateGameScaling(_graphics);
        }

        private static void SetFullscreen(bool enable, GraphicsDeviceManager _graphics)
        {
            if (enable)
            {
                Variables.GameScale = 4;
                Variables.WindowW = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                Variables.WindowH = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                Variables.DrawPosition = Math0.CenterWithinRectangle(Variables.WindowW, Variables.WindowH, 256, 256, 4).To3();
                Variables.UpdateWindowSize(_graphics);
                if (!_graphics.IsFullScreen) _graphics.ToggleFullScreen();
            }
            else
            {
                if (_graphics.IsFullScreen) _graphics.ToggleFullScreen();
                Variables.UpdateGameScaling(_graphics);
            }
        }

        public static void ToggleFullscreen()
        {
            Fullscreen = !Fullscreen;
        }


        public static void Update(GraphicsDeviceManager _graphics)
        {
            SetWindowScale(WindowScalar, _graphics);
            SetFullscreen(Fullscreen, _graphics);
        }
    }
}
