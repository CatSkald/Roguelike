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
        private Cell _cell;

        [SetUp]
        public void SetUp()
        {
            _cell = new Cell();
        }

        #region Constructor
        [TestCase(0, 0)]
        [TestCase(-2, -150)]
        [TestCase(5, 7)]
        [TestCase(15, 1)]
        [TestCase(150, 150)]
        public void Constructor_SetsCorrectLocationForXY(int x, int y)
        {
            var cell = new Cell(x, y);

            Assert.That(cell.Location, Is.EqualTo(new Point(x, y)));
        }

        [TestCase(0, 0)]
        [TestCase(-2, -150)]
        [TestCase(5, 7)]
        [TestCase(15, 1)]
        [TestCase(150, 150)]
        public void Constructor_SetsCorrectLocationForPoint(int x, int y)
        {
            var cell = new Cell(new Point(x, y));

            Assert.That(cell.Location, Is.EqualTo(new Point(x, y)));
        }
        #endregion

        #region Properties
        [TestCase(0, 0)]
        [TestCase(-2, -150)]
        [TestCase(5, 7)]
        [TestCase(15, 1)]
        [TestCase(150, 150)]
        public void Location_GetReturnsSameValue_AsWasSet(int x, int y)
        {
            var cell = new Cell
            {
                Location = new Point(x, y)
            };

            Assert.That(cell.Location, Is.EqualTo(new Point(x, y)));
        }

        [Test]
        public void Sides_SetToDefaultSides_WhenCellIsJustCreated()
        {
            Assert.That(_cell.Sides, Is.EqualTo(new Sides()));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void IsVisited_GetReturnsSameValue_AsWasSet(bool value)
        {
            _cell.IsVisited = value;

            Assert.That(_cell.IsVisited, Is.EqualTo(value));
        }

        [Test]
        public void IsWall_IsTrue_IfAllSidesAreWalls()
        {
            Assert.That(_cell.IsWall, Is.True);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void IsWall_IsFalse_IfNotAllSidesAreWalls(int countOfEmptySides)
        {
            foreach (var dir in _cell.Sides.Keys.ToList())
            {
                _cell.Sides[dir] = Side.Empty;
                countOfEmptySides--;
                if (countOfEmptySides == 0)
                    break;
            }

            Assert.That(_cell.IsWall, Is.False);
        }
        
        [Test]
        public void IsCorridor_IsFalse_IfAllSidesAreWalls()
        {
            Assert.That(_cell.IsCorridor, Is.False);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void IsCorridor_IsTrue_IfSomeSidesAreEmpty(int countOfEmptySides)
        {
            foreach (var dir in _cell.Sides.Keys.ToList())
            {
                _cell.Sides[dir] = Side.Empty;
                countOfEmptySides--;
                if (countOfEmptySides == 0)
                    break;
            }

            Assert.That(_cell.IsCorridor, Is.True);
        }

        [TestCase(Dir.N)]
        [TestCase(Dir.E)]
        [TestCase(Dir.S)]
        [TestCase(Dir.W)]
        public void IsDeadEnd_IsTrue_IfAllSidesAreWallsExceptOne(Dir empty)
        {
            _cell.Sides[empty] = Side.Empty;

            Assert.That(_cell.IsDeadEnd, Is.True);
        }

        [TestCase(0)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void IsDeadEnd_IsFalse_IfNotOneSideIsEmpty(int countOfEmptySides)
        {
            foreach (var dir in _cell.Sides.Keys.ToList())
            {
                if (countOfEmptySides == 0)
                    break;
                _cell.Sides[dir] = Side.Empty;
                countOfEmptySides--;
            }

            Assert.That(_cell.IsDeadEnd, Is.False);
        }
        #endregion

        #region IEquatable
        [Test]
        public void IEquatableMembers_WorksCorrect_ForDefaultCells()
        {
            var other = new Cell();

            CustomAssert.IEqualityMembersWorkForEqualObjects(_cell, other);
        }

        [TestCase(true, 250, 1000, new[] { Dir.N, Dir.E, Dir.S, Dir.W })]
        [TestCase(true, 0, 1, new[] { Dir.N })]
        [TestCase(false, 5, 1, new[] { Dir.E, Dir.S })]
        [TestCase(true, -15, 1, new Dir[0])]
        public void IEquatableMembers_WorksCorrect_IfCellsEqual(bool isVisited, int x, int y, params Dir[] emptyDirs)
        {
            _cell.IsVisited = isVisited;
            _cell.Location = new Point(x, y);

            var other = new Cell
            {
                IsVisited = isVisited,
                Location = new Point(x, y)
            };

            foreach (var dir in emptyDirs)
            {
                _cell.Sides[dir] = Side.Empty;
                other.Sides[dir] = Side.Empty;
            }

            CustomAssert.IEqualityMembersWorkForEqualObjects(_cell, other);
        }

        [Test]
        public void IEquatableMembers_WorksCorrect_IfIsVisitedDiffers()
        {
            _cell.IsVisited = true;
            var other = new Cell
            {
                IsVisited = false
            };

            CustomAssert.IEquatableMembersWorkForDifferentObjects(_cell, other);
        }

        [TestCase(0, 0, 0, 1)]
        [TestCase(-5, 0, 0, 0)]
        [TestCase(10, 10, -10, 10)]
        [TestCase(250, 999, 250, 998)]
        public void IEquatableMembers_WorksCorrect_IfLocationDiffers(int x1, int y1, int x2, int y2)
        {
            _cell.Location = new Point(x1, y1);

            var other = new Cell
            {
                Location = new Point(x2, y2)
            };

            CustomAssert.IEquatableMembersWorkForDifferentObjects(_cell, other);
        }

        [TestCase(new[] { Dir.N })]
        [TestCase(new[] { Dir.E, Dir.S })]
        [TestCase(new[] { Dir.W, Dir.E, Dir.N })]
        [TestCase(new[] { Dir.N, Dir.E, Dir.S, Dir.W })]
        public void IEquatableMembers_WorksCorrect_IfSidesDiffers(params Dir[] emptyDirs)
        {
            var other = new Cell();

            foreach (var dir in emptyDirs)
            {
                _cell.Sides[dir] = Side.Empty;
            }

            CustomAssert.IEquatableMembersWorkForDifferentObjects(_cell, other);
        }
        #endregion
    }
}
