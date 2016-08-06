using CatSkald.Roguelike.DungeonGenerator.Directions;
using CatSkald.Roguelike.DungeonGenerator.Maps;
using NUnit.Framework;

namespace CatSkald.Roguelike.Tests.DungeonGeneratorTests.Maps
{
    [TestFixture]
    public class SidesTests
    {
        ////TODO test Equals

        [Test]
        public void Constructor_HasCountSameAsAllDirections()
        {
            var sides = new Sides();

            Assert.That(sides.Values, Has.All.EqualTo(Side.Wall));
        }

        [Test]
        public void Constructor_ContainsAllDirectionsAfterCreation()
        {
            var sides = new Sides();

            Assert.That(sides.Keys, Is.EquivalentTo(DirHelper.GetNonEmptyDirs()));
        }
    }
}