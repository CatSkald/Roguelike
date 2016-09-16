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
        
        //TODO add tests for failures

        //TODO extract range into parameters
        #region IfNotInRange
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(42)]
        [TestCase(99)]
        [TestCase(100)]
        public void IfNotInRange_ThrowsNothing_IfValueInRange(int value)
        {
            Assert.That(() => Throw.IfNotInRange(0, 100, value, "p"),
                Throws.Nothing);
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
