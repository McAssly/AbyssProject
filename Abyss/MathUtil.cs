using Abyss.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Abyss
{
    public enum SIDES {
        LEFT = 0, RIGHT = 1, TOP = 2, BOTTOM = 3
    }
    /**
     * This class simply operates as a place to store functions that simply do calculations
     * that is all
     */
    internal class MathUtil
    {
        // any useful math variables
        public static Vector2[] offsets = new Vector2[4]
        {
            Vector2.Zero,
            new Vector2(Globals.TILE_SIZE),
            new Vector2(Globals.TILE_SIZE, 0),
            new Vector2(0, Globals.TILE_SIZE)
        };

        /**
         * Moves the first input vector to the second\
         *  (does not account for frame rate)
         * 
         * @param   Vector2     the first vector
         * @param   Vector2     the second vector to move towards
         * @param   int         the speed to move by
         */
        public static Vector2 MoveToward(Vector2 vec1, Vector2 vec2, double delta)
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
                return vec1 + direction * (int)delta;
        }

        /**
         * Moves the first input value to the second\
         *  (does not account for frame rate)
         * 
         * @param   float       the first value
         * @param   float       the second value to move towards
         * @param   int         the speed to move by
         */
        public static float MoveTowardI(float val1, float val2, double delta)
        {
            // get the direction to move in
            float direction = val2 - val1;
            // get the distance they need to move in
            float distance = Math.Abs(direction);

            // if our speed is too fast then return the target position
            if (delta >= distance)
                return val2;
            else // otherwise move in that direciton
                return val1 + direction * (int)delta;
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

        /**
         * Converts coordinates to tile map coordinates
         * 
         * @param   Vector2     the current coordinates
         */
        public static Vector2 CoordsToTileCoords(Vector2 coords)
        {
            Vector2 result = coords / Globals.TILE_SIZE;
            result.Floor();
            return result;
        }

        /**
         * Returns true if the given position is within the bounds of the given four points
         * 
         * @param   Vector2     the given position in question
         * @param   float       left bound
         * @param   float       right bound
         * @param   float       top bound
         * @param   float       bottom bound
         */
        public static bool IsWithin(Vector2 pos, float left, float right, float top, float bottom)
        {
            return pos.X >= left && pos.X <= right && pos.Y >= top && pos.Y <= bottom;
        }
    }
}
