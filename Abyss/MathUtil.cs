using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss
{
    /**
     * This class simply operates as a place to store functions that simply do calculations
     * that is all
     */
    internal class MathUtil
    {
        /**
         * Moves the first input vector to the second\
         *  (does not account for frame rate)
         * 
         * @param   Vector2     the first vector
         * @param   Vector2     the second vector to move towards
         * @param   int         the speed to move by
         */
        public static Vector2 moveToward(Vector2 vec1, Vector2 vec2, int delta)
        {
            // get the direction to move in
            Vector2 direction = vec2 - vec1;
            // get the distance they need to move in
            double distance = direction.Length();
            // and normalize the direction so we don't overshoot the target position
            if (direction != Vector2.Zero)
                direction.Normalize();

            // if our speed is too fast then return the target position
            if (delta >= distance)
                return vec2;
            else // otherwise move in that direciton
                return vec1 + direction * delta;
        }

        /**
         * Returns a numerical value of 1 or 0 depending on whether the key is being pressed or not
         * 
         * @param   KeyboardState   the current game keyboard state
         * @param   Keys             the key in question
         */
        public static int KeyStrength(KeyboardState KB, Keys Key)
        {
            if (KB.IsKeyDown(Key))
                return 1;
            else
                return 0;
        }
    }
}
