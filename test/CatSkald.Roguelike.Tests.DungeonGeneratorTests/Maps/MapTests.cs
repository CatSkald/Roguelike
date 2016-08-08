using System;
using System.Collections.Generic;
using System.Linq;
using CatSkald.Roguelike.DungeonGenerator.Directions;
using CatSkald.Roguelike.DungeonGenerator.Maps;
using NUnit.Framework;

namespace CatSkald.Roguelike.Tests.DungeonGeneratorTests.Maps
{
    [TestFixture]
    public class MapTests
    {
        private Map _map;

        [SetUp]
        public void SetUp()
        {
            _map = new Map(15, 10);
        }

        [Test]
        public void Constructor_CellsAreNotVisited_WhenMapCreated()
        {
            Assert.That(_map, Has.All.Property(nameof(Cell.IsVisited)).EqualTo(false));
        }

        #region AllVisited
        [Test]
        public void AllVisited_FalseIfOnlyOneVisited()
        {
            _map.Visit(_map[3, 1]);

            Assert.That(_map.AllVisited, Is.EqualTo(false));
        }

        [Test]
        public void AllVisited_FalseForNewMap()
        {
            Assert.That(_map.AllVisited, Is.EqualTo(false));
        }

        [Test]
        public void AllVisited_TrueIfAllVisited()
        {
            foreach (var cell in _map)
            {
                _map.Visit(cell);
            }

            Assert.That(_map.AllVisited, Is.EqualTo(true));
        }

        [TestCase(7)]
        [TestCase(15)]
        public void AllVisited_FalseIfOnlyFewVisited(int numberOfVisited)
        {
            // Visit some cells in the middle of the map
            foreach (var cell in _map.Skip(5).Take(numberOfVisited))
            {
                _map.Visit(cell);
            }

            Assert.That(_map.AllVisited, Is.EqualTo(false));
        }

        [Test]
        public void AllVisited_FalseIfOneNotVisited()
        {
            foreach (var cell in _map.Skip(1))
            {
                _map.Visit(cell);
            }

            Assert.That(_map.AllVisited, Is.EqualTo(false));
        }
        #endregion

        [Test]
        public void PickRandomCell_ReturnsCellFromMap()
        {
            var cell = _map.PickRandomCell();

            Assert.That(_map[cell.Location], Is.SameAs(cell));
        }

        #region PickNextRandomVisitedCell
        [Test]
        public void PickNextRandomVisitedCell_ThrowIfOnlyOneCellVisited()
        {
            var visitedCell = _map[0, 1];

            _map.Visit(visitedCell);

            Assert.That(() => _map.PickNextRandomVisitedCell(visitedCell),
                Throws.InvalidOperationException);
        }

        [Test]
        public void PickNextRandomVisitedCell_PicksCorrectVisitedCell_IfOnlyTwoCellsVisited()
        {
            var firstVisitedCell = _map[0, 1];
            var secondVisitedCell = _map[1, 3];

            _map.Visit(firstVisitedCell);
            _map.Visit(secondVisitedCell);

            var cell = _map.PickNextRandomVisitedCell(firstVisitedCell);

            Assert.That(cell, Is.SameAs(secondVisitedCell));
        }

        [Test]
        public void PickNextRandomVisitedCell_PicksCell_ThatWasVisitedAndNotEqualToOld()
        {
            var visitedCell = _map[0, 1];

            _map.Visit(visitedCell);
            _map.Visit(_map[2, 5]);
            _map.Visit(_map[7, 5]);
            _map.Visit(_map[3, 3]);

            var cell = _map.PickNextRandomVisitedCell(visitedCell);

            Assert.That(cell, Is.Not.EqualTo(visitedCell)
                .And.With.Property(nameof(Cell.IsVisited)).EqualTo(true));
        } 
        #endregion

        #region HasAdjacentCell

        [TestCase(1, 1)]
        [TestCase(2, 3)]
        [TestCase(5, 2)]
        public void HasAdjacentCell_AlwaysReturnsTrue_ForCellInTheMiddle(int x, int y)
        {
            var cell = _map[x, y];

            Assert.That(_map.HasAdjacentCell(cell, Dir.W));
            Assert.That(_map.HasAdjacentCell(cell, Dir.S));
            Assert.That(_map.HasAdjacentCell(cell, Dir.E));
            Assert.That(_map.HasAdjacentCell(cell, Dir.N));
        }

        [Test]
        public void HasAdjacentCell_ReturnsCorrect_ForTopLeftCell()
        {
            var cell = _map[0, 0];

            Assert.That(!_map.HasAdjacentCell(cell, Dir.W));
            Assert.That(_map.HasAdjacentCell(cell, Dir.S));
            Assert.That(_map.HasAdjacentCell(cell, Dir.E));
            Assert.That(!_map.HasAdjacentCell(cell, Dir.N));
        }

        [Test]
        public void HasAdjacentCell_ReturnsCorrect_ForTopRightCell()
        {
            var cell = _map[0, _map.Height - 1];
            Assert.That(!_map.HasAdjacentCell(cell, Dir.W));
            Assert.That(!_map.HasAdjacentCell(cell, Dir.S));
            Assert.That(_map.HasAdjacentCell(cell, Dir.E));
            Assert.That(_map.HasAdjacentCell(cell, Dir.N));
        }

        [Test]
        public void HasAdjacentCell_ReturnsCorrect_ForBottomLeftCell()
        {
            var cell = _map[_map.Width - 1, 0];

            Assert.That(_map.HasAdjacentCell(cell, Dir.W));
            Assert.That(_map.HasAdjacentCell(cell, Dir.S));
            Assert.That(!_map.HasAdjacentCell(cell, Dir.E));
            Assert.That(!_map.HasAdjacentCell(cell, Dir.N));
        }

        [Test]
        public void HasAdjacentCell_ReturnsCorrect_ForBottoRightCell()
        {
            var cell = _map[_map.Width - 1, _map.Height - 1];

            Assert.That(_map.HasAdjacentCell(cell, Dir.W));
            Assert.That(!_map.HasAdjacentCell(cell, Dir.S));
            Assert.That(!_map.HasAdjacentCell(cell, Dir.E));
            Assert.That(_map.HasAdjacentCell(cell, Dir.N));
        }

        [TestCase(0, 4, Dir.W)]
        [TestCase(0, 7, Dir.W)]
        [TestCase(9, 2, Dir.E)]
        [TestCase(9, 5, Dir.E)]
        [TestCase(3, 0, Dir.N)]
        [TestCase(8, 0, Dir.N)]
        [TestCase(2, 9, Dir.S)]
        [TestCase(1, 9, Dir.S)]
        public void HasAdjacentCell_ReturnsCorrect_ForNonCornerBorderCells(
            int x, int y, Dir dir)
        {
            var map = new Map(10, 10);
            var cell = map[x, y];
            var dirs = DirHelper.GetNonEmptyDirs();

            Assert.That(!map.HasAdjacentCell(cell, dir));
            Assert.That(dirs.Where(d => d != dir)
                .Select(d => map.HasAdjacentCell(cell, d)),
                Has.All.EqualTo(true));
        }

        [TestCase(0)]
        [TestCase(-5)]
        [TestCase(11)]
        public void HasAdjacentCell_Throws_IfDirectionIsInvalid(int dir)
        {
            Assert.That(() => _map.HasAdjacentCell(_map[3, 4], (Dir)dir), 
                Throws.ArgumentException);
        }

        #endregion

        #region TryGetAdjacentCell
        [TestCase(0, 0, Dir.N)]
        [TestCase(0, 0, Dir.W)]
        [TestCase(14, 9, Dir.E)]
        [TestCase(14, 9, Dir.S)]
        public void TryGetAdjacentCell_ReturnsFalse_IfNoAdjacentCell(int x, int y, Dir dir)
        {
            Cell adjacent;

            var result = _map.TryGetAdjacentCell(_map[x, y], dir, out adjacent);

            Assert.That(result, Is.EqualTo(false));
            Assert.That(adjacent, Is.Null);
        }

        [TestCase(0, 0, Dir.S, 0, 1)]
        [TestCase(5, 5, Dir.N, 5, 4)]
        [TestCase(14, 9, Dir.W, 13, 9)]
        [TestCase(1, 1, Dir.E, 2, 1)]
        public void TryGetAdjacentCell_ReturnsTrue_IfAdjacentCellExists(
            int x, int y, Dir dir, int expectedX, int expectedY)
        {
            var map = new Map(1, 1);
            Cell adjacent;

            var result = _map.TryGetAdjacentCell(_map[x, y], dir, out adjacent);

            Assert.That(result, Is.EqualTo(true));
            Assert.That(adjacent, Is.SameAs(_map[expectedX, expectedY]));
        }
        #endregion

        #region CreateCorridor
        [TestCase(0, 0, 0, 1, Dir.S, Dir.N)]
        [TestCase(1, 1, 1, 0, Dir.N, Dir.S)]
        [TestCase(0, 0, 1, 0, Dir.E, Dir.W)]
        [TestCase(1, 1, 0, 1, Dir.W, Dir.E)]
        public void CreateCorridor_CorridorCreatedCorrectly(
            int x1, int y1, int x2, int y2, Dir corridorDir, Dir oppositeDir)
        {
            var map = new Map(5, 5);
            var startCell = map[x1, y1];
            var endCell = map[x2, y2];

            map.CreateCorridor(startCell, endCell, corridorDir);

            Assert.That(startCell.Sides[corridorDir], Is.EqualTo(Side.Empty));
            Assert.That(startCell.Sides.Where(s => s.Key != corridorDir),
                Has.All.With.Property("Value").EqualTo(Side.Wall));

            Assert.That(endCell.Sides[oppositeDir], Is.EqualTo(Side.Empty));
            Assert.That(endCell.Sides.Where(s => s.Key != oppositeDir),
                Has.All.With.Property("Value").EqualTo(Side.Wall));
        }

        [TestCase(1, 1, 1, 0, Dir.S)]
        [TestCase(0, 0, 0, 1, Dir.N)]
        [TestCase(0, 0, 0, 1, Dir.E)]
        [TestCase(0, 0, 0, 1, Dir.W)]
        [TestCase(1, 1, 1, 1, Dir.E)]
        [TestCase(1, 1, 0, 0, Dir.E)]
        [TestCase(2, 1, 3, 4, Dir.E)]
        public void CreateCorridor_ThrowsForNonAjacentCells(
            int x1, int y1, int x2, int y2, Dir corridorDirection)
        {
            var map = new Map(5, 5);

            var startCell = map[x1, y1];
            var endCell = map[x2, y2];

            Assert.That(() => map.CreateCorridor(startCell, endCell, corridorDirection),
                Throws.ArgumentException);
        }
        #endregion

        #region RemoveCorridor
        [TestCase(0, 0, 0, 1, Dir.S, Dir.N)]
        [TestCase(1, 1, 1, 0, Dir.N, Dir.S)]
        [TestCase(0, 0, 1, 0, Dir.E, Dir.W)]
        [TestCase(1, 1, 0, 1, Dir.W, Dir.E)]
        public void RemoveCorridor_CorridorRemovedCorrectly(
            int x1, int y1, int x2, int y2, Dir corridorDir, Dir oppositeDir)
        {
            var map = new Map(5, 5);
            var startCell = map[x1, y1];
            var endCell = map[x2, y2];
            foreach (var dir in DirHelper.GetNonEmptyDirs())
            {
                startCell.Sides[dir] = Side.Empty;
                endCell.Sides[dir] = Side.Empty;
            }

            map.RemoveCorridor(startCell, corridorDir);

            Assert.That(startCell.Sides[corridorDir], Is.EqualTo(Side.Wall));
            Assert.That(startCell.Sides.Where(s => s.Key != corridorDir),
                Has.All.With.Property("Value").EqualTo(Side.Empty));

            Assert.That(endCell.Sides[oppositeDir], Is.EqualTo(Side.Wall));
            Assert.That(endCell.Sides.Where(s => s.Key != oppositeDir),
                Has.All.With.Property("Value").EqualTo(Side.Empty));
        }

        [TestCase(0, 0, 0, 1, Dir.S)]
        [TestCase(1, 1, 1, 0, Dir.N)]
        [TestCase(0, 0, 1, 0, Dir.E)]
        [TestCase(1, 1, 0, 1, Dir.W)]
        public void RemoveCorridor_Throws_IfStartCellHasNoCorridor(
            int x1, int y1, int x2, int y2, Dir corridorDir)
        {
            var map = new Map(5, 5);
            var startCell = map[x1, y1];
            var endCell = map[x2, y2];
            foreach (var dir in DirHelper.GetNonEmptyDirs())
            {
                startCell.Sides[dir] = Side.Empty;
                endCell.Sides[dir] = Side.Wall;
            }

            Assert.That(() => map.RemoveCorridor(startCell, corridorDir),
                Throws.InvalidOperationException);
        }

        [TestCase(0, 0, 0, 1, Dir.S)]
        [TestCase(1, 1, 1, 0, Dir.N)]
        [TestCase(0, 0, 1, 0, Dir.E)]
        [TestCase(1, 1, 0, 1, Dir.W)]
        public void RemoveCorridor_Throws_IfEndCellHasNoCorridor(
            int x1, int y1, int x2, int y2, Dir corridorDir)
        {
            var map = new Map(5, 5);
            var startCell = map[x1, y1];
            var endCell = map[x2, y2];
            foreach (var dir in DirHelper.GetNonEmptyDirs())
            {
                startCell.Sides[dir] = Side.Wall;
                endCell.Sides[dir] = Side.Empty;
            }

            Assert.That(() => map.RemoveCorridor(startCell, corridorDir),
                Throws.InvalidOperationException);
        }
        #endregion

        [Test]
        public void Visit_SetsIsVisitedTrue()
        {
            var cell = _map[2, 5];

            _map.Visit(cell);

            Assert.That(cell.IsVisited, Is.EqualTo(true));
        }

        ////TODO SetRooms

        [TestCase(-2, 5)]
        [TestCase(2, -2)]
        [TestCase(25, 3)]
        [TestCase(1, 30)]
        [TestCase(-1, -30)]
        [TestCase(100, 300)]
        public void MapActionsThrowCorrectExceptionWhenPointIsOutsideMap(int x, int y)
        {
            var actions = new Dictionary<string, Action<IMap, Cell>>
            {
                { "Visit", (m, c) => m.Visit(c) },
                { "HasAdjacentCell", (m, c) => m.HasAdjacentCell(c, Dir.E) },
                { "CreateCorridor_StartCell", (m, c) => m.CreateCorridor(c, _map[0, 0], Dir.E) },
                { "CreateCorridor_EndCell", (m, c) => m.CreateCorridor(_map[0, 0], c, Dir.E) },
                { "RemoveCorridor", (m, c) => m.RemoveCorridor(c, Dir.E) },
                { "TryGetAdjacentCell", (m, c) => {
                        Cell outCell;
                        m.TryGetAdjacentCell(c, Dir.E, out outCell);
                    }
                }
            };

            foreach (var action in actions.Values)
            {
                Assert.That(() => action(_map, new Cell(x, y)),
                    Throws.TypeOf<ArgumentOutOfRangeException>(),
                    action + " should throw correct error if point is outside the map.");
            }
        }
    }
}
