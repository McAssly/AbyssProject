using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.Utility
{
    /// <summary>
    /// stores a sided enum
    /// </summary>
    internal enum Side
    {
        LEFT = 0, TOP = 1, RIGHT = 2, BOTTOM = 3
    }

    internal class Math0
    {
        /// <summary>
        /// converts the given coordinates to the tile coordinates, by default this will floor the coordinates
        /// </summary>
        /// <param name="coords"></param>
        /// <param name="floor"></param>
        /// <returns></returns>
        public static Vector CoordsToTileCoords(Vector2 coords, bool floor = true)
        {
            Vector2 result = coords / 16;
            if (floor) result.Floor();
            else result.Round();
            return Clamp(result);
        }


        /// <summary>
        /// clamps the given vector within the tile boundaries of 0 to 15 (length = 16);
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector Clamp(Vector2 vector)
        {
            return Vector.Convert(Vector2.Clamp(vector, new Vector2(0,0), new Vector2(15, 15)));
        }



        /// <summary>
        /// returns the absolute version of a vector
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector2 Absolute(Vector2 vector)
        {
            return new Vector2(Math.Abs(vector.X), Math.Abs(vector.Y));
        }


        /// <summary>
        /// moves the first vector towards the second based on a given delta
        /// </summary>
        /// <param name="vec1"></param>
        /// <param name="vec2"></param>
        /// <param name="delta"></param>
        /// <returns></returns>
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

        /// <summary>
        /// determines if the given two rectangles are colliding with each other (edge detection)
        /// </summary>
        /// <param name="pos1"></param>
        /// <param name="size1"></param>
        /// <param name="pos2"></param>
        /// <param name="size2"></param>
        /// <returns></returns>
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
        /// Determines the position to place a smaller rectangle within a larger rectangle, with a given scalar
        /// </summary>
        /// <param name="outer_width"></param>
        /// <param name="outer_height"></param>
        /// <param name="inner_width"></param>
        /// <param name="inner_height"></param>
        /// <param name="scalar">default = 1</param>
        /// <returns></returns>
        public static Vector CenterWithinRectangle(int outer_width, int outer_height, int inner_width, int inner_height, double scalar = 1)
        {
            return new Vector(
                (int) ((outer_width - inner_width * scalar) / (2 * scalar)),
                (int) ((outer_height - inner_height * scalar) / (2 * scalar))
                );
        }

        /// <summary>
        /// determines if a point is within a rectangle
        /// </summary>
        /// <param name="pos1"></param>
        /// <param name="pos2"></param>
        /// <param name="size2"></param>
        /// <returns></returns>
        public static bool WithinRectangle(Vector2 pos1, Vector2 pos2, Vector2 size2)
        {
            return
                pos1.X >= pos2.X &&
                pos1.X <= pos2.X + size2.X &&
                pos1.Y >= pos2.Y &&
                pos1.Y <= pos2.Y + size2.Y;
        }


        /// <summary>
        /// alternate
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static bool WithinRectangle(Vector2 pos, Rectangle rect)
        {
            return WithinRectangle(pos, new Vector2(rect.X, rect.Y), new Vector2(rect.Width, rect.Height));
        }

        /// <summary>
        /// applies the given acceleration to the given velocity and returns it
        /// </summary>
        /// <param name="velocity"></param>
        /// <param name="accel"></param>
        /// <returns></returns>
        public static Vector2 ApplyAcceleration(Vector2 velocity, double accel)
        {
            double magnitude = Math.Sqrt(velocity.X * velocity.X + velocity.Y * velocity.Y);
            Vector2 normalized = new Vector2((float)(velocity.X / magnitude), (float)(velocity.Y / magnitude));
            return normalized * new Vector2((float)(magnitude + accel));
        }

        /// <summary>
        /// rotates the vector by a given angle
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="vec"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Vector2 Rotate(Vector2 origin, Vector2 vec, double angle)
        {
            double length = Math.Sqrt(Math.Pow(vec.X - origin.X, 2) + Math.Pow(vec.Y - origin.Y, 2));
            return new Vector2((float)(origin.X + length * Math.Cos(angle)), (float)(origin.Y + length * Math.Sin(angle)));
        }

        /// <summary>
        /// determines the pointing vector of the given angle
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Vector2 VectorAtAngle(double angle)
        {
            float x = (float)Math.Cos(angle);
            float y = (float)Math.Sin(angle);

            // we must normalize the vector to around r = 1
            float length = (float)Math.Sqrt(x * x + y * y);

            return new Vector2(x / length, y / length);
        }
    }
}
