using UnityEngine;

namespace Assets.Scripts.Extensions
{
    public static class VectorExtensions
    {
        /// <summary>
        /// Returns a new vector in which the Y axis is inverted.
        /// </summary>
        /// <param name="vect">Vector to invert on the Y axis.</param>
        /// <returns>A new vector in which the Y axis is inverted.</returns>
        public static Vector2 InvertY(this Vector2 vect)
        {
            return new Vector2(vect.x, -vect.y);
        }

        /// <summary>
        /// Returns a new vector in which the Y axis is inverted.
        /// </summary>
        /// <param name="vect">Vector to invert on the Y axis.</param>
        /// <returns>A new vector in which the Y axis is inverted.</returns>
        public static Vector3 InvertY(this Vector3 vect)
        {
            return new Vector3(vect.x, -vect.y, 0);
        }
    }
}
