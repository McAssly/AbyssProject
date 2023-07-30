using Abyss.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Abyss.Master
{
    internal struct NullableVector
    {
        public int? x, y;
        
        public NullableVector(int? x, int? y)
        {
            this.x = x;
            this.y = y;
        }

        public static readonly NullableVector NULL = new NullableVector(null, null);
    }

    internal struct Vector
    {
        public int x, y;
        
        public Vector(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector Convert(Vector2 vector)
        {
            return new Vector((int)vector.X, (int)vector.Y);
        }
    }

    internal enum Side
    {
        LEFT = 0, RIGHT = 1, TOP = 2, BOTTOM = 3
    }
    /**
     * This class simply operates as a place to store functions that simply do calculations
     * that is all
     */
    internal class MathUtil
    {

        public static Vector2 MousePosition()
        {
            return new Vector2(Game._MouseState.X / (float)Globals.GameScale, Game._MouseState.Y / (float)Globals.GameScale);
        }

        public static Vector2 MousePositionInGame()
        {
            return MousePosition() - new Vector2(Globals.DrawPosition.X, Globals.DrawPosition.Y);
        }



        // any useful math variables
        public static Vector2[] offsets = new Vector2[8]
        {
            Vector2.Zero,
            new Vector2(Globals.TILE_SIZE),
            new Vector2(Globals.TILE_SIZE, 0),
            new Vector2(0, Globals.TILE_SIZE),

            new Vector2(0, Globals.TILE_SIZE / 2),
            new Vector2(Globals.TILE_SIZE, Globals.TILE_SIZE / 2),
            new Vector2(Globals.TILE_SIZE / 2, 0),
            new Vector2(Globals.TILE_SIZE / 2, Globals.TILE_SIZE)
        };

        public static Vector2 VectorAtAngle(double angle)
        {
            float x = (float)Math.Cos(angle);
            float y = (float)Math.Sin(angle);

            // we must normalize the vector to around r = 1
            float length = (float)Math.Sqrt(x * x + y * y);

            return new Vector2(x / length, y / length);
        }

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


        public static Vector2 ApplyAcceleration(Vector2 velocity, double accel)
        {
            double magnitude = Math.Sqrt(velocity.X * velocity.X + velocity.Y * velocity.Y);
            Vector2 normalized = new Vector2((float)(velocity.X / magnitude), (float)(velocity.Y / magnitude));
            return normalized * new Vector2((float)(magnitude + accel));
        }

        public static Vector2 Rotate(Vector2 origin, Vector2 vec, double angle)
        {
            double length = Math.Sqrt(Math.Pow(vec.X - origin.X, 2) + Math.Pow(vec.Y - origin.Y, 2));
            return new Vector2((float)(origin.X + length * Math.Cos(angle)), (float)(origin.Y + length * Math.Sin(angle)));
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
        public static Vector CoordsToTileCoords(Vector2 coords)
        {
            Vector2 result = coords / Globals.TILE_SIZE;
            result.Floor();
            return Vector.Convert(Vector2.Clamp(result, Vector2.Zero, new Vector2(16-1)));
        }

        /**
         * Returns true if the given position is within the bounds of the given four points
         * 
         * 
         * THIS FUNCTION MIGHT BE WRONG AND HAVE POOR LOGIC         <------------------------------------------------------ !!!!
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


        /// <summary>
        /// Checks if the current mouse button flag is being pressed (clicked)
        /// </summary>
        /// <param name="MS"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static bool IsClicked(MouseState MS, uint flag)
        {
            switch (flag)
            {
                case 1: 
                    return MS.LeftButton == ButtonState.Pressed;
                case 2:
                    return MS.RightButton == ButtonState.Pressed;
                default: return false;
            }
        }
    }
}
