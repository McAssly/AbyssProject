using Abyss.Globals;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.Utility
{
    internal class InputUtility
    {
        /// <summary>
        /// gets the mouse's position within the user's monitor
        /// </summary>
        /// <returns></returns>
        public static Vector2 MousePosition()
        {
            return new Vector2(Game._MouseState.X / (float)Variables.GameScale, Game._MouseState.Y / (float)Variables.GameScale);
        }

        /// <summary>
        /// gets the mouse position with in the game's draw screen
        /// </summary>
        /// <returns></returns>
        public static Vector2 MousePositionInGame()
        {
            return MousePosition() - new Vector2(Variables.DrawPosition.X, Variables.DrawPosition.Y);
        }


        /// <summary>
        /// returns the numerical value of 1 or 0 depending on whether the key is being pressed or not
        /// </summary>
        /// <param name="kb"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int KeyStrength(KeyboardState kb, Keys key)
        {
            if (kb.IsKeyDown(key))
                return 1;
            else
                return 0;
        }


        /// <summary>
        /// Checks if the current mouse button flag is being pressed (clicked)
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static bool IsClicked(MouseState ms, uint flag)
        {
            switch (flag)
            {
                case 1:
                    return ms.LeftButton == ButtonState.Pressed;
                case 2:
                    return ms.RightButton == ButtonState.Pressed;
                default: return false;
            }
        }


        // STATIC
        /// <summary>
        /// Handle's keyboard input for the game, when using the debug console, or entering in the player name, etc.
        /// </summary>
        /// <param name="kb"></param>
        /// <returns></returns>
        public static bool HandleInput(KeyboardState kb)
        {
            Keys[] keys = kb.GetPressedKeys();

            // Handle control keys
            foreach (Keys key in keys)
            {
                if (Game._prevKeyboardState.IsKeyUp(key)) // ensure repetitions are not caused
                {
                    if (key == Keys.Enter) // close key
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// The text input hook for the game window to register keyboard text input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void RegisterInput(object sender, TextInputEventArgs e)
        {
            Keys? k = e.Key;
            char c = e.Character;
            if (!char.IsControl(c) && c != '`')
                Game._TextInput.Append(c);
            else if (k == Keys.Back && Game._TextInput.Length > 0)
                Game._TextInput.Length--;
        }


        /// <summary>
        /// registers key inputs on a seperate stream from the update sequence
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void RegisterKey(object sender, TextInputEventArgs e)
        {
            Game._KeyInput = e.Key;
        }
    }
}
