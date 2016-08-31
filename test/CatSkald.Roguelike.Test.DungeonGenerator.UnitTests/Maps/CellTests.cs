using System.Drawing;
using System.Linq;
using CatSkald.Roguelike.Core.Terrain;
using CatSkald.Roguelike.DungeonGenerator.Maps;
using NUnit.Framework;

namespace CatSkald.Roguelike.Test.DungeonGenerator.UnitTests.Maps
{
    [TestFixture]
    public class CellTests
    {
        private MapCell _cell;

        [SetUp]
        public void SetUp()
        {
            _cell = new MapCell();
        }

        #region Constructor
        [TestCase(0, 0)]
        [TestCase(-2, -150)]
        [TestCase(5, 7)]
        [TestCase(15, 1)]
        [TestCase(150, 150)]
        public void Constructor_SetsCorrectLocationForXY(int x, int y)
        {
            var cell = new MapCell(x, y);

            Assert.That(cell.Location, Is.EqualTo(new Point(x, y)));
        }

        [TestCase(0, 0)]
        [TestCase(-2, -150)]
        [TestCase(5, 7)]
        [TestCase(15, 1)]
        [TestCase(150, 150)]
        public void Constructor_SetsCorrectLocationForPoint(int x, int y)
        {
            var cell = new MapCell(new Point(x, y));

            Assert.That(cell.Location, Is.EqualTo(new Point(x, y)));
        }

        [Test]
        public void Constructor_IsVisited_IsFalse()
        {
            var cell = new MapCell();

            Assert.That(cell.IsVisited, Is.False);
        }

        [Test]
        public void Constructor_IsCorridor_IsFalse()
        {
            var cell = new MapCell();

            Assert.That(cell.IsCorridor, Is.False);
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
            var cell = new MapCell
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
        
        [TestCase(true)]
        [TestCase(false)]
        public void IsCorridor_GetReturnsSameValue_AsWasSet(bool value)
        {
            _cell.IsCorridor = value;

            Assert.That(_cell.IsCorridor, Is.EqualTo(value));
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
        
        [TestCase(Dir.N)]
        [TestCase(Dir.E)]
        [TestCase(Dir.S)]
        [TestCase(Dir.W)]
        public void IsDeadEnd_IsTrue_IfOneSideIsEmpty(Dir empty)
        {
            _cell.Sides[empty] = Side.Empty;

            Assert.That(_cell.IsDeadEnd, Is.True);
        }
        
        [TestCase(Dir.N)]
        [TestCase(Dir.E)]
        [TestCase(Dir.S)]
        [TestCase(Dir.W)]
        public void IsDeadEnd_IsTrue_IfOneSideIsDoor(Dir empty)
        {
            _cell.Sides[empty] = Side.Door;

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
            var other = new MapCell();

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

            var other = new MapCell
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
            var other = new MapCell
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

            var other = new MapCell
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
            var other = new MapCell();

            foreach (var dir in emptyDirs)
            {
                _cell.Sides[dir] = Side.Empty;
            }

            CustomAssert.IEquatableMembersWorkForDifferentObjects(_cell, other);
        }
        #endregion
    }
}
