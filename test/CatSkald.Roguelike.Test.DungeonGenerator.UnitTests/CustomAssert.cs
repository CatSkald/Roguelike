using System;
using NUnit.Framework;

namespace CatSkald.Roguelike.Test.DungeonGenerator.UnitTests
{
    /// <summary>
    /// Custom NUnit asserts
    /// </summary>
    public static class CustomAssert
    {
        /// <summary>
        /// Asserts that Equals(T) and Equals(object) return true for two objects and GetHashCode returns same hash
        /// </summary>
        /// <typeparam name="T">Type of objects. Must be IEquatable.</typeparam>
        /// <param name="actual">First object</param>
        /// <param name="expected">Second object. Must be equal to first one.</param>
        public static void IEqualityMembersWorkForEqualObjects<T>(
            T actual, T expected) where T : IEquatable<T>
        {
            Assert.That(actual.Equals(expected),
                "Equals(T) should return true.");

            Assert.That(actual.Equals((object)expected),
                "Equals(object) should return true.");

            Assert.That(actual.GetHashCode(), Is.EqualTo(expected.GetHashCode()),
                "GetHashCode should return same hashcode.");
        }

        /// <summary>
        /// Asserts that Equals(T) and Equals(object) return false for two objects and GetHashCode returns different hash
        /// </summary>
        /// <typeparam name="T">Type of objects. Must be IEquatable.</typeparam>
        /// <param name="actual">First object</param>
        /// <param name="expected">Second object. Must not be equal to first one.</param>
        public static void IEquatableMembersWorkForDifferentObjects<T>(
            T actual, T expected) where T : IEquatable<T>
        {
            Assert.That(!actual.Equals(expected),
                "Equals(Cell) should return false.");

            Assert.That(!actual.Equals((object)expected),
                "Equals(object) should return false.");

            Assert.That(actual.GetHashCode(), Is.Not.EqualTo(expected.GetHashCode()),
                "GetHashCode should return different hashcode.");
        }
    }
}
