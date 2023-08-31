using Microsoft.Xna.Framework;
using System;

namespace Abyss.Utility
{
    /// <summary>
    /// Creates an integer vector of two points with the purpose of certain spare calculations
    /// </summary>
    internal struct Vector
    {
        public int x, y;

        public Vector(int x, int y) { this.x = x; this.y = y; }

        /// <summary>
        /// converts the given float vector to an integer vector
        /// </summary>
        /// <param name="vec2"></param>
        /// <param name="round">determines if the conversion should round or not</param>
        /// <returns></returns>
        public static Vector Convert(Vector2 vec2, bool round = false)
        {
            if (round) return new Vector((int)Math.Round(vec2.X), (int)Math.Round(vec2.X));
            return new Vector((int)vec2.X, (int)vec2.Y);
        }

        /// <summary>
        /// converts to a 2D float vector
        /// </summary>
        /// <returns></returns>
        public Vector2 To2()
        {
            return new Vector2(x, y);
        }


        /// <summary>
        /// converts to a 3D float vector, 3 dimension is empty by default
        /// </summary>
        /// <returns></returns>
        public Vector3 To3(float z = 0)
        {
            return new Vector3(x, y, z);
        }
    }


    /// <summary>
    /// a nullable version of the vector that contains null values
    /// </summary>
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
}
