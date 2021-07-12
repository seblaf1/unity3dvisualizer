using System;

namespace Assets.Scripts.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Truncates the N last characters of the string.
        /// </summary>
        /// <param name="str">String to truncate the last N characters.</param>
        /// <param name="skipAmount">N</param>
        /// <returns>Substring which doesn't contain the last N characters.</returns>
        public static string Truncate(this string str, int skipAmount)
        {
            if (str.Length < skipAmount)
                throw new ArgumentException("Can't skip an amount of characters longer than the string itself.", nameof(skipAmount));

            return str.Substring(0, str.Length - skipAmount);
        }
    }
}
