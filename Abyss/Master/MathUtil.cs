using Abyss.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
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
using Vector3 = Microsoft.Xna.Framework.Vector3;

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

        public static Vector Round(Vector2 vector)
        {
            return new Vector((int)Math.Round(vector.X), (int)Math.Round(vector.Y));
        }

        public static Vector Convert(Vector2 vector)
        {
            return new Vector((int)Math.Round(vector.X), (int)Math.Round(vector.Y));
        }

        public Vector2 To2()
        {
            return new Vector2(x, y);
        }

        public Vector3 To3()
        {
            return new Vector3(x, y, 0);
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

        public static Vector2 VectorAtAngle(double angle)
        {
            float x = (float)Math.Cos(angle);
            float y = (float)Math.Sin(angle);

            // we must normalize the vector to around r = 1
            float length = (float)Math.Sqrt(x * x + y * y);

            return new Vector2(x / length, y / length);
        }

        public static Vector Clamp(Vector2 vector)
        {
            return Vector.Convert(Vector2.Clamp(vector, new Vector2(0, 0), new Vector2(15, 15)));
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
                return vec1 + direction * (float)delta;
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
            return Clamp(result);
        }


        public static bool WithinRectangle(Vector2 pos1, Vector2 pos2, Vector2 size2)
        {
            return 
                pos1.X >= pos2.X &&
                pos1.X <= pos2.X + size2.X &&
                pos1.Y >= pos2.Y &&
                pos1.Y <= pos2.Y + size2.Y;
        }


        public static bool RectangleCollisionCheck(Vector2 pos1, Vector2 size1, Vector2 pos2, Vector2 size2)
        {
            // determine the max/min values for each rectangle
            Vector2 rect1_min = pos1;
            Vector2 rect1_max = pos1 + size1;
            Vector2 rect2_min = pos2;
            Vector2 rect2_max = pos2 + size2;

            // If there is overlap on both X and Y axes, then the collision check passed, otherwise it didn't; simple
            return rect1_min.X <= rect2_max.X && rect1_max.X >= rect2_min.X && rect1_min.Y <= rect2_max.Y && rect1_max.Y >= rect2_min.Y;
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




        public static Vector CenterWithinRectangle(int outer_width, int outer_height, int inner_width, int inner_height, double scalar = 1)
        {
            return new Vector(
                (int)((outer_width - inner_width * scalar) / (2 * scalar)),
                (int)((outer_height - inner_height * scalar) / (2 * scalar))
            );
        }
    }
}
