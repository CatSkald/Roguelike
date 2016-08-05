using System.Collections.Generic;
using CatSkald.Roguelike.DungeonGenerator.Directions;
using NUnit.Framework;

namespace CatSkald.Roguelike.Tests.DungeonGeneratorTests.Directions
{
    [TestFixture]
    public class DirectionPickerTests
    {
        private readonly int expectedCount = DirHelper.GetNonEmptyDirs().Count;
        private DirectionPicker _picker;

        [SetUp]
        public void SetUp()
        {
            _picker = new DirectionPicker();
        }

        //TODO test NextDirection

        #region HasDirections

        [Test]
        public void HasDirections_TrueAfterCreation()
        {
            Assert.That(_picker.HasDirections, Is.EqualTo(true));
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

            Assert.That(_picker.HasDirections, Is.EqualTo(false));
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

            _picker.Reset();

            Assert.That(_picker.HasDirections, Is.EqualTo(true));
        }

        #endregion

        [Test]
        public void Constructor_ContainsAllDirections()
        {
            var counter = expectedCount;
            var dirs = new List<Dir>();

            while (counter > 0)
            {
                dirs.Add(_picker.NextDirection());
                counter--;
            }

            Assert.That(dirs, Is.EquivalentTo(DirHelper.GetNonEmptyDirs()));
        }

        [TestCase(Dir.Zero)]
        [TestCase(Dir.W)]
        [TestCase(Dir.N)]
        public void Reset_RestoresAllDirections_RegardlessOfDirectionPassed(Dir dir)
        {
            var counter = expectedCount;

            while (counter > 0)
            {
                _picker.NextDirection();
                counter--;
            }

            _picker.Reset(dir);

            counter = expectedCount;
            var dirs = new List<Dir>();

            while (counter > 0)
            {
                dirs.Add(_picker.NextDirection());
                counter--;
            }

            Assert.That(dirs, Is.EquivalentTo(DirHelper.GetNonEmptyDirs()));
        }

        #region ChangeDirection

        [Test]
        public void ChangeDirection_TrueForMaxTwistFactor()
        {
            _picker.TwistFactor = DirectionPicker.TwistFactorMax;
            Assert.That(_picker.ChangeDirection, Is.EqualTo(false));
        }

        [Test]
        public void ChangeDirection_FalseForMinTwistFactor()
        {
            _picker.TwistFactor = DirectionPicker.TwistFactorMin;
            Assert.That(_picker.ChangeDirection, Is.EqualTo(false));
        } 

        #endregion
    }
}
