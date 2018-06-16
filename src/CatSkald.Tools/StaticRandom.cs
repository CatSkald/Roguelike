using System;

namespace CatSkald.Tools
{
    /// <summary>
    /// Thread safe singleton pseudo-random number generator
    /// </summary>
    public static class StaticRandom
    {
        private static readonly Random Random = new Random();
        private static readonly object Lock = new object();

        /// <summary>
        /// Returns a non-negative random number less than the specified maximum.
        /// </summary>
        /// <param name="max">The exclusive upper bound of the random number to be generated. Must be greater than 0.</param>
        /// <returns>A number greater than or equal to 0, and less than <paramref name="max"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="max"/> is less than or equal to 0.</exception>
        public static int Next(int max)
        {
            if (max <= 0)
                throw new ArgumentOutOfRangeException($"max should be above zero, but was: {max}");
            if (max == 1)
                return 0;
            lock (Lock)
                return Random.Next(max);
        }

        /// <summary>
        /// Returns a non-negative random number within a specified range.
        /// </summary>
        /// <param name="min">The inclusive lower bound of the random number to be generated.</param>
        /// <param name="max">The exclusive upper bound of the random number to be generated. Must be greater than <paramref name="min"/>.</param>
        /// <returns>A number greater than or equal to <paramref name="min"/>, and less than <paramref name="max"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="max"/> is less than or equal to <paramref name="min"/>.</exception>
        public static int Next(int min, int max)
        {
            if (max <= min)
                throw new ArgumentOutOfRangeException(
                    $"max should be more than min, but was: {max} <= {min}");
            if (max - min == 1)
                return min;
            lock (Lock)
                return Random.Next(min, max);
        }

        /// <summary>
        /// Returns a non-negative random number within a specified range.
        /// </summary>
        /// <param name="min">The inclusive lower bound of the random number to be generated.</param>
        /// <param name="max">The inclusive upper bound of the random number to be generated. Must be greater than <paramref name="min"/>.</param>
        /// <returns>A number greater than or equal to <paramref name="min"/>, and less than or equal to <paramref name="max"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="max"/> is less than or equal to <paramref name="min"/>.</exception>
        public static int NextInclusive(int min, int max)
        {
            if (max < min)
                throw new ArgumentOutOfRangeException(
                    $"max should be more than or equal to min, but was: {max} < {min}");
            if (min == max)
                return min;
            lock (Lock)
                return Random.Next(min, max + 1);
        }

        /// <summary>
        /// Returns a non-negative random number within a specified range that is not equal to specified exception.
        /// </summary>
        /// <param name="min">The inclusive lower bound of the random number to be generated.</param>
        /// <param name="max">The exclusive upper bound of the random number to be generated. Must be greater than <paramref name="min"/>.</param>
        /// <param name="except">The exclusive number which should not be generated.</param>
        /// <returns>A number greater than or equal to <paramref name="min"/>, less than <paramref name="max"/> and not equals to <paramref name="except"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="max"/> is less than or equal to <paramref name="min"/> or <paramref name="except"/> is the only possible number between <paramref name="max"/> and <paramref name="min"/>.</exception>
        public static int NextNotEqualToOld(int min, int max, int except)
        {
            if (max <= min)
                throw new ArgumentOutOfRangeException(
                    $"max should be more than min, but was: {max} <= {min}");
            if (max - min == 1)
            {
                if (min != except)
                {
                    return min;
                }
                throw new ArgumentOutOfRangeException(
                    $"{except} is the only possible number between {min} and {max}");
            }

            int result;
            do
            {
                lock (Lock)
                    result = Random.Next(min, max);
            }
            while (result == except);

            return result;
        }


        /// <summary>
        /// Returns true or false randomly depending on the success chance.
        /// </summary>
        /// <param name="successChanceInPercents">The chance of success result: 100 or more means always return true, 0 or less means always return false.</param>
        /// <returns>Boolean value indicating success.</returns>
        public static bool RollSuccess(int successChanceInPercents)
        {
            if (successChanceInPercents <= 0)
                return false;
            if (successChanceInPercents >= 100)
                return true;
            lock (Lock)
                return successChanceInPercents > Random.Next(100);
        }
    }
}
