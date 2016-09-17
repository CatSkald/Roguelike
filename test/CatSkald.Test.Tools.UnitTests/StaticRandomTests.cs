using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatSkald.Tools;
using NUnit.Framework;

namespace CatSkald.Test.Tools.UnitTests
{
    public class StaticRandomTests
    {
        #region Next(max)
        [Test]
        public void Next_ReturnsAboveZero()
        {
            Assert.That(StaticRandom.Next(50), Is.GreaterThanOrEqualTo(0));
        }

        [Test]
        public void Next_ReturnsZero_IfMaxIs1()
        {
            Assert.That(StaticRandom.Next(1), Is.EqualTo(0));
        }

        [TestCase(0)]
        [TestCase(-42)]
        public void Next_Throws_IfMaxIsZeroOrLess(int max)
        {
            Assert.That(() => StaticRandom.Next(max), 
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }
        #endregion

        #region Next(min, max)
        [TestCase(-12, -7)]
        [TestCase(58, 120)]
        [TestCase(-5, 5)]
        public void NextMinMax_Returns_InRange(int min, int max)
        {
            Assert.That(StaticRandom.Next(min, max),
                Is.GreaterThanOrEqualTo(min)
                .And.LessThan(max));
        }

        [TestCase(2)]
        [TestCase(58)]
        public void NextMinMax_ReturnsMin_IfMaxIs1AboveMin(int min)
        {
            Assert.That(StaticRandom.Next(min, min + 1), Is.EqualTo(min));
        }

        [TestCase(0, -1)]
        [TestCase(-42, -58)]
        [TestCase(12, 12)]
        public void NextMinMax_Throws_IfMaxIsLessOrEqualToMin(int min, int max)
        {
            Assert.That(() => StaticRandom.Next(min, max),
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }
        #endregion

        #region NextInclusive
        [TestCase(-12, -7)]
        [TestCase(58, 120)]
        [TestCase(-5, 5)]
        public void NextInclusive_Returns_InRange(int min, int max)
        {
            Assert.That(StaticRandom.NextInclusive(min, max),
                Is.GreaterThanOrEqualTo(min)
                .And.LessThanOrEqualTo(max));
        }

        [TestCase(2)]
        [TestCase(58)]
        public void NextInclusive_ReturnsMin_IfMaxIsEqualToMin(int min)
        {
            Assert.That(StaticRandom.NextInclusive(min, min), Is.EqualTo(min));
        }

        [TestCase(0, -1)]
        [TestCase(-42, -58)]
        public void NextInclusive_Throws_IfMaxIsLessThanMin(int min, int max)
        {
            Assert.That(() => StaticRandom.NextInclusive(min, max),
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }
        #endregion

        #region NextNotEqualToOld
        [TestCase(-12, -7)]
        [TestCase(58, 120)]
        [TestCase(-5, 5)]
        public void NextNotEqualToOld_Returns_InRangeAndNotEqualToExcept(int min, int max)
        {
            var except = min + 1;

            Assert.That(StaticRandom.NextNotEqualToOld(min, max, except),
                Is.GreaterThanOrEqualTo(min)
                .And.LessThan(max)
                .And.Not.EqualTo(except));
        }

        [TestCase(0)]
        [TestCase(-42)]
        public void NextNotEqualToOld_ReturnsMin_IfMaxIs1AboveMinAndMinNotEqualToExcept(int min)
        {
            const int except = -999;

            Assert.That(StaticRandom.NextNotEqualToOld(min, min + 1, except),
                Is.EqualTo(min));
        }

        [TestCase(0, -1)]
        [TestCase(-42, -58)]
        [TestCase(12, 12)]
        public void NextNotEqualToOld_Throws_IfMaxIsLessOrEqualToMin(int min, int max)
        {
            Assert.That(() => StaticRandom.Next(min, max),
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [TestCase(0)]
        [TestCase(-42)]
        public void NextNotEqualToOld_Throws_IfExceptIsTheOnlyPossible(int min)
        {
            Assert.That(() => StaticRandom.NextNotEqualToOld(min, min + 1, min),
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }
        #endregion
    }
}
