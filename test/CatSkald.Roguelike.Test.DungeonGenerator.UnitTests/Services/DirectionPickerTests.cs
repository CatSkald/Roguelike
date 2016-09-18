using System;
using System.Collections.Generic;
using CatSkald.Roguelike.Core.Terrain;
using CatSkald.Roguelike.DungeonGenerator.Services;
using NUnit.Framework;

namespace CatSkald.Roguelike.Test.DungeonGenerator.UnitTests.Services
{
    [TestFixture]
    public class DirectionPickerTests
    {
        private readonly int expectedCount = DirHelper.GetNonEmptyDirs().Count;
        private DirectionPicker _picker;

        [SetUp]
        public void SetUp()
        {
            _picker = new DirectionPicker(50);
        }

        #region NextDirection

        [Test]
        public void NextDirection_ThrowsIfNoDirection()
        {
            var expectedDirs = DirHelper.GetNonEmptyDirs();
            RetrieveDirections(_picker, expectedDirs.Count);

            Assert.That(() => _picker.NextDirection(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [TestCase(Dir.N)]
        [TestCase(Dir.S)]
        [TestCase(Dir.W)]
        [TestCase(Dir.E)]
        public void NextDirection_DoesNotChooseLastIfTwistFactor100(Dir lastDir)
        {
            _picker.SetTwistFactor(100);
            _picker.LastDirection = lastDir;

            Assert.That(_picker.NextDirection(), Is.Not.EqualTo(lastDir));
        }

        [TestCase(Dir.N)]
        [TestCase(Dir.S)]
        [TestCase(Dir.W)]
        [TestCase(Dir.E)]
        public void NextDirection_ChoosesLastIfTwistFactor0(Dir lastDir)
        {
            _picker.SetTwistFactor(0);
            _picker.LastDirection = lastDir;

            Assert.That(_picker.NextDirection(), Is.EqualTo(lastDir));
        }

        #endregion

        #region HasDirections

        [Test]
        public void HasDirections_TrueAfterCreation()
        {
            Assert.That(_picker.HasDirections, Is.True);
        }

        [Test]
        public void HasDirections_FalseAfterAllPicked()
        {
            var counter = expectedCount;

            while (counter > 0)
            {
                _picker.NextDirection();
                counter--;
            }

            Assert.That(_picker.HasDirections, Is.False);
        }

        [Test]
        public void HasDirections_TrueAfterReset()
        {
            var counter = expectedCount;

            while (counter > 0)
            {
                _picker.NextDirection();
                counter--;
            }

            _picker.ResetDirections();

            Assert.That(_picker.HasDirections, Is.True);
        }

        #endregion

        #region Constructor

        [Test]
        public void Constructor_ContainsAllDirections()
        {
            var expectedDirs = DirHelper.GetNonEmptyDirs();
            var dirs = new List<Dir>();
            for (int i = 0; i < expectedDirs.Count; i++)
            {
                dirs.Add(_picker.NextDirection());
            }

            Assert.That(dirs, Is.EquivalentTo(expectedDirs));
        }

        [TestCase(66)]
        [TestCase(99)]
        public void Constructor_SetsCorrectTwistFactor(int value)
        {
            _picker = new DirectionPicker(value);

            Assert.That(_picker.TwistFactor, Is.EqualTo(value));
        }

        [TestCase(-1)]
        [TestCase(101)]
        [TestCase(10000)]
        public void Constructor_ThrowsCorrectErrorIfTwistFactorIsIncorrect(int value)
        {
            Assert.That(() => new DirectionPicker(value),
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        #endregion

        #region SetTwistFactor
        [TestCase(-1)]
        [TestCase(101)]
        [TestCase(10000)]
        public void SetTwistFactor_ThrowsCorrectErrorIfTwistFactorIsIncorrect(
    int value)
        {
            Assert.That(() => new DirectionPicker(value),
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [TestCase(0)]
        [TestCase(66)]
        [TestCase(100)]
        public void SetTwistFactor_SetsCorrectTwistFactor(int value)
        {
            _picker.SetTwistFactor(value);

            Assert.That(_picker.TwistFactor, Is.EqualTo(value));
        }
        #endregion

        #region ResetDirections

        [Test]
        public void ResetDirections_RestoresAllDirections()
        {
            var expectedDirs = DirHelper.GetNonEmptyDirs();
            RetrieveDirections(_picker, expectedDirs.Count);

            _picker.ResetDirections();

            var dirs = new List<Dir>();
            for (int i = 0; i < expectedDirs.Count; i++)
            {
                dirs.Add(_picker.NextDirection());
            }

            Assert.That(dirs, Is.EquivalentTo(expectedDirs));
        }

        [TestCase(42)]
        [TestCase(99)]
        public void ResetDirections_DoesNotChangeTwistFactor(int value)
        {
            _picker.SetTwistFactor(value);
            _picker.ResetDirections();

            Assert.That(_picker.TwistFactor, Is.EqualTo(value));
        }

        [TestCase(Dir.Zero)]
        [TestCase(Dir.N)]
        [TestCase(Dir.W)]
        public void ResetDirections_DoesNotChangeLastDirection(Dir dir)
        {
            _picker.LastDirection = dir;
            _picker.ResetDirections();

            Assert.That(_picker.LastDirection, Is.EqualTo(dir));
        }

        #endregion

        #region ShouldChangeDirection

        [Test]
        public void ShouldChangeDirection_TrueForTwistFactor100()
        {
            _picker.SetTwistFactor(100);

            Assert.That(_picker.ShouldChangeDirection(), Is.True);
        }

        [Test]
        public void ShouldChangeDirection_FalseForTwistFactor0()
        {
            _picker.SetTwistFactor(0);

            Assert.That(_picker.ShouldChangeDirection(), Is.False);
        }

        #endregion

        private static void RetrieveDirections(DirectionPicker _picker, int count)
        {
            for (int i = 0; i < count; i++)
            {
                _picker.NextDirection();
            }
        }
    }
}
