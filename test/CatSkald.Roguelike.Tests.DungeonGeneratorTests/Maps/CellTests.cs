using System.Drawing;
using System.Linq;
using CatSkald.Roguelike.DungeonGenerator.Directions;
using CatSkald.Roguelike.DungeonGenerator.Maps;
using NUnit.Framework;

namespace CatSkald.Roguelike.Tests.DungeonGeneratorTests.Maps
{
    [TestFixture]
    public class CellTests
    {
        ////TODO test Equals

        #region Constructor
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
        #endregion

        #region IsDeadEnd
        [TestCase(Dir.N)]
        [TestCase(Dir.E)]
        [TestCase(Dir.S)]
        [TestCase(Dir.W)]
        public void IsDeadEnd_IsTrueIfAllSidesAreWallsExceptOne(Dir empty)
        {
            var cell = new Cell();
            cell.Sides[empty] = Side.Empty;

            Assert.That(cell.IsDeadEnd, Is.EqualTo(true));
        }

        [TestCase(0)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void IsDeadEnd_IsFalseIfNotOneSideIsEmpty(int countOfEmptySides)
        {
            var cell = new Cell();
            foreach (var dir in cell.Sides.Keys.ToList())
            {
                cell.Sides[dir] = Side.Empty;
                countOfEmptySides--;
                if (countOfEmptySides == 0)
                    break;
            }

            Assert.That(cell.IsDeadEnd, Is.EqualTo(false));
        } 
        #endregion

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
