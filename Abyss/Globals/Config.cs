using Abyss.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Abyss.Globals
{
    internal class Config
    {
        public static double MaxFrameRate = 144;
        public static double WindowScalar = 1.5;
        public static bool Fullscreen = false;
        private static bool oldFullscreen = Fullscreen;
        public static bool ExitStatus = false;


        private static void SetWindowScale(double scale, GraphicsDeviceManager _graphics)
        {
            WindowScalar = scale;
            Variables.UpdateGameScaling(_graphics);
        }

        private static void EnableFullscreen(GraphicsDeviceManager _graphics)
        {
            Variables.GameScale = 4;
            Variables.WindowW = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            Variables.WindowH = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            Variables.DrawPosition = Math0.CenterWithinRectangle(Variables.WindowW, Variables.WindowH, 256, 256, 4).To3();
            Variables.UpdateWindowSize(_graphics);
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();
        }

        public static void DisableFullscreen(GraphicsDeviceManager _graphics)
        {
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();
            Variables.UpdateGameScaling(_graphics);
        }

        public static void ToggleFullscreen()
        {
            Fullscreen = !Fullscreen;
        }

        public static void Exit()
        {
            ExitStatus = true;
        }


        public static void Update(GraphicsDeviceManager _graphics)
        {
            if (!Fullscreen) SetWindowScale(WindowScalar, _graphics);
            if (Fullscreen && !oldFullscreen)
            {
                Debug.WriteLine("Enabling Fullscreen");
                EnableFullscreen(_graphics);
                oldFullscreen = Fullscreen;
            } else if (!Fullscreen && oldFullscreen)
            {
                Debug.WriteLine("Disabling Fullscreen");
                DisableFullscreen(_graphics);
                oldFullscreen = Fullscreen;
            }
        }
    }
}
