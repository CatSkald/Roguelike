using System.Drawing;
using CatSkald.Roguelike.DungeonGenerator.Maps;
using NUnit.Framework;

namespace CatSkald.Roguelike.Tests.DungeonGeneratorTests.Maps
{
    [TestFixture]
    public class CellTests
    {
        ////TODO test Equals

        [Test]
        public void Constructor_SidesAreNotNull()
        {
            var cell = new Cell();

            Assert.That(cell.Sides, Is.Not.Null);
        }

        [TestCase(0, 0)]
        [TestCase(-2, -150)]
        [TestCase(5, 7)]
        [TestCase(15, 1)]
        [TestCase(150, 150)]
        public void Constructor_SetsCorrectLocation(int x, int y)
        {
            var cell = new Cell(x, y);

            Assert.That(cell.Location, Is.EqualTo(new Point(x, y)));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void IsVisited_ReturnesCorrectValue(bool value)
        {
            var cell = new Cell
            {
                IsVisited = value
            };

            Assert.That(cell.IsVisited, Is.EqualTo(value));
        }

        [TestCase(0, 0)]
        [TestCase(-2, -150)]
        [TestCase(5, 7)]
        [TestCase(15, 1)]
        [TestCase(150, 150)]
        public void Location_ReturnesCorrectValue(int x, int y)
        {
            var cell = new Cell
            {
                Location = new Point(x, y)
            };

            Assert.That(cell.Location, Is.EqualTo(new Point(x, y)));
        }
    }
}
