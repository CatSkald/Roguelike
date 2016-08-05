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
                _picker.GetRandomDirection();
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
                _picker.GetRandomDirection();
                counter--;
            }

            _picker.Reset();

            Assert.That(_picker.HasDirections, Is.EqualTo(true));
        }

        [Test]
        public void ContainsAllDirections()
        {
            var counter = expectedCount;
            var dirs = new List<Dir>();

            while (counter > 0)
            {
                dirs.Add(_picker.GetRandomDirection());
                counter--;
            }

            Assert.That(dirs, Is.EquivalentTo(DirHelper.GetNonEmptyDirs()));
        }

        [Test]
        public void ContainsAllDirections_AfterReset()
        {
            var counter = expectedCount;

            while (counter > 0)
            {
                _picker.GetRandomDirection();
                counter--;
            }

            _picker.Reset();

            counter = expectedCount;
            var dirs = new List<Dir>();

            while (counter > 0)
            {
                dirs.Add(_picker.GetRandomDirection());
                counter--;
            }

            Assert.That(dirs, Is.EquivalentTo(DirHelper.GetNonEmptyDirs()));
        }
    }
}
