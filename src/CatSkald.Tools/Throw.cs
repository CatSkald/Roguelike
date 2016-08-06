using System;

namespace CatSkald.Tools
{
    /// <summary>
    /// Class with different validation methods.
    /// </summary>
    public static class Throw
    {
        /// <summary>
        /// Validates that <paramref name="value"/> is not null.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="value"/>. Must be reference type.</typeparam>
        /// <param name="value">The value to check.</param>
        /// <param name="name">The name of object which is validated.</param>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        public static void IfNull<T>(T value, string name) where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        /// <summary>
        /// Validates that <paramref name="value"/> is in specified range.
        /// </summary>
        /// <typeparam name="T">Type of <paramref name="value"/>. Must be IComparable.</typeparam>
        /// <param name="min">The inclusive lower bound of range.</param>
        /// <param name="max">The inclusive upper bound of range.</param>
        /// <param name="value">The value to check.</param>
        /// <param name="name">The name of object which is validated.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is less than <paramref name="min"/> or greater than <paramref name="max"/>.</exception>
        public static void IfNotInRange<T>(T min, T max, T value, string name)
            where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0 || value.CompareTo(max) > 0)
            {
                throw new ArgumentOutOfRangeException(
                    name, value, $"Should be in range {min} <= {name} <= {max}.");
            }
        }
    }
}
