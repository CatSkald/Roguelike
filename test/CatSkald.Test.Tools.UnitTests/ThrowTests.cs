using System;
using System.Collections.Generic;
using CatSkald.Tools;
using NUnit.Framework;

namespace CatSkald.Test.Tools.UnitTests
{
    public class ThrowTests
    {
        [Test]
        public void IfNull_ThrowsNothing_ForNotNull()
        {
            Assert.That(() => Throw.IfNull(new object(), "myParameter"),
                Throws.Nothing);
        }
        
        #region IfNotInRange
        [TestCase(0, 10, 0)]
        [TestCase(5, 144, 144)]
        [TestCase(-1, 1, 0)]
        [TestCase(-44, -44, -44)]
        public void IfNotInRange_ThrowsNothing_IfValueInRange(
            int min, int max, int value)
        {
            Assert.That(() => Throw.IfNotInRange(min, max, value, "p"),
                Throws.Nothing);
        }

        [TestCase(0, 10, -5)]
        [TestCase(5, 144, 145)]
        [TestCase(-1, 1, -2)]
        [TestCase(-44, -44, -43)]
        public void IfNotInRange_Throws_IfValueNotInRange(
            int min, int max, int value)
        {
            Assert.That(() => Throw.IfNotInRange(min, max, value, "p"),
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }
        #endregion

        #region IfGreater
        [TestCase(-34, -35)]
        [TestCase(1, 0)]
        [TestCase(5097, 907)]
        public void IfGreater_ThrowsNothing_IfValueIsLess(int max, int value)
        {
            Assert.That(() => Throw.IfGreater(max, value, "p"),
                Throws.Nothing);
        }

        [TestCase(-36, -35)]
        [TestCase(-1, 0)]
        [TestCase(-5097, 907)]
        public void IfGreater_ThrowsNothing_IfValueIsGreater(int max, int value)
        {
            Assert.That(() => Throw.IfGreater(max, value, "p"),
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }
        #endregion

        #region IfLess
        [TestCase(-345, -344)]
        [TestCase(-1, 0)]
        [TestCase(5097, 9047)]
        public void IfLess_ThrowsNothing_IfValueIsGreater(int min, int value)
        {
            Assert.That(() => Throw.IfLess(min, value, "p"),
                Throws.Nothing);
        }

        [TestCase(-343, -344)]
        [TestCase(1, 0)]
        [TestCase(97, -500)]
        public void IfLess_Throws_IfValueIsLess(int min, int value)
        {
            Assert.That(() => Throw.IfLess(min, value, "p"),
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }
        #endregion

        [TestCase("someArgument")]
        [TestCase("null")]
        public void ThrowMethods_ThrowException_WithCorrectParamName(
            string paramName)
        {
            var actions = new Dictionary<string, Action<string>>
            {
                { "IfNull", (p) => Throw.IfNull<object>(null, p) },
                { "IfNotInRange", (p) => Throw.IfNotInRange(0, 0, 100, p) },
                { "IfGreater", (p) => Throw.IfGreater(0, 100, p) },
                { "IfLess", (p) => Throw.IfLess(100, 0, p) },
            };

            foreach (var action in actions)
            {
                Assert.That(() => action.Value(paramName),
                    Throws.Exception
                    .With.Property(nameof(ArgumentException.ParamName)),
                    action.Key);
            }
        }
    }
}
