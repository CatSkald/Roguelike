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
        #region HasAdjacentCell

        [TestCase(1, 1)]
        [TestCase(2, 3)]
        [TestCase(5, 2)]
        public void HasAdjacentCell_AlwaysReturnsTrueForCellInTheMiddle(int w, int h)
        {
            var map = new Map(10, 10);
            var p = new Cell(w, h);
            Assert.That(map.HasAdjacentCell(p, Dir.W));
            Assert.That(map.HasAdjacentCell(p, Dir.S));
            Assert.That(map.HasAdjacentCell(p, Dir.E));
            Assert.That(map.HasAdjacentCell(p, Dir.N));
        }

        [Test]
        public void HasAdjacentCell_ReturnsCorrectForTopLeftCell()
        {
            var map = new Map(10, 10);
            var p = new Cell();
            Assert.That(!map.HasAdjacentCell(p, Dir.W));
            Assert.That(map.HasAdjacentCell(p, Dir.S));
            Assert.That(map.HasAdjacentCell(p, Dir.E));
            Assert.That(!map.HasAdjacentCell(p, Dir.N));
        }

        [Test]
        public void HasAdjacentCell_ReturnsCorrectForTopRightCell()
        {
            var map = new Map(10, 10);
            var p = new Cell(0, 9);
            Assert.That(!map.HasAdjacentCell(p, Dir.W));
            Assert.That(!map.HasAdjacentCell(p, Dir.S));
            Assert.That(map.HasAdjacentCell(p, Dir.E));
            Assert.That(map.HasAdjacentCell(p, Dir.N));
        }

        [Test]
        public void HasAdjacentCell_ReturnsCorrectForBottomLeftCell()
        {
            var map = new Map(10, 10);
            var p = new Cell(9, 0);
            Assert.That(map.HasAdjacentCell(p, Dir.W));
            Assert.That(map.HasAdjacentCell(p, Dir.S));
            Assert.That(!map.HasAdjacentCell(p, Dir.E));
            Assert.That(!map.HasAdjacentCell(p, Dir.N));
        }

        [Test]
        public void HasAdjacentCell_ReturnsCorrectForBottoRightCell()
        {
            var map = new Map(10, 10);
            var p = new Cell(9, 9);
            Assert.That(map.HasAdjacentCell(p, Dir.W));
            Assert.That(!map.HasAdjacentCell(p, Dir.S));
            Assert.That(!map.HasAdjacentCell(p, Dir.E));
            Assert.That(map.HasAdjacentCell(p, Dir.N));
        }

        [TestCase(0, 4, Dir.W)]
        [TestCase(0, 7, Dir.W)]
        [TestCase(9, 2, Dir.E)]
        [TestCase(9, 5, Dir.E)]
        [TestCase(3, 0, Dir.N)]
        [TestCase(8, 0, Dir.N)]
        [TestCase(2, 9, Dir.S)]
        [TestCase(1, 9, Dir.S)]
        public void HasAdjacentCell_ReturnsCorrectForNonCornerBorderCells(int w, int h, Dir dir)
        {
            var map = new Map(10, 10);
            var p = new Cell(w, h);
            var dirs = DirHelper.GetNonEmptyDirs();
            Assert.That(!map.HasAdjacentCell(p, dir));
            Assert.That(dirs.Where(d => d != dir).Select(d => map.HasAdjacentCell(p, d)),
                Has.All.EqualTo(true));
        }

        [TestCase(0)]
        [TestCase(55)]
        [TestCase(11)]
        public void HasAdjacentCell_ThrowsForInvalidDirection(int dir)
        {
            var map = new Map(10, 10);
            var p = new Cell(4, 5);
            Assert.That(() => map.HasAdjacentCell(p, (Dir)dir), Throws.ArgumentException);
        }

        #endregion

        [Test]
        public void Visit_SetIsVisitedTrue()
        {
            var map = new Map(3, 3);
            var cell = new Cell();

            map.Visit(cell);

            Assert.That(cell.IsVisited, Is.EqualTo(true));
        }

        #region Indexers

        [TestCase(10, 10)]
        [TestCase(1, 2)]
        [TestCase(4, 4)]
        [TestCase(100, 25)]
        public void MapIndexerSetsAndGetsSameValue(int w, int h)
        {
            var map = new Map(200, 200);

            map[w, h].IsVisited = true;

            Assert.That(map[w, h].IsVisited, Is.EqualTo(true));
            Assert.That(map.Count(it => !it.IsVisited), Is.EqualTo(map.Size - 1));
        }

        [TestCase(10, 10)]
        [TestCase(1, 2)]
        [TestCase(4, 4)]
        [TestCase(100, 25)]
        public void MapPointIndexerSetsAndGetsSameValue(int w, int h)
        {
            var map = new Map(200, 200);

            var p = new Cell(w, h);
            map[p].IsVisited = true;

            Assert.That(map[p].IsVisited, Is.EqualTo(true));
            Assert.That(map.Count(it => !it.IsVisited), Is.EqualTo(map.Size - 1));
        }

        [TestCase(0, 2, 3)]
        [TestCase(1, 1, 5)]
        [TestCase(2, 1, 8)]
        public void MapIndexerSetsCorrectValue(int w, int h, int count)
        {
            var map = new Map(3, 3);

            map[w, h].IsVisited = true;

            Assert.That(map.Take(count).Last().IsVisited, Is.EqualTo(true));
            Assert.That(map.Count(it => !it.IsVisited), Is.EqualTo(map.Size - 1));
        }

        [TestCase(0, 2, 3)]
        [TestCase(1, 1, 5)]
        [TestCase(2, 1, 8)]
        public void MapPointIndexerSetsCorrectValue(int x, int y, int count)
        {
            var map = new Map(3, 3);

            var p = new Cell(x, y);
            map[p].IsVisited = true;

            Assert.That(map.Take(count).Last().IsVisited, Is.EqualTo(true));
            Assert.That(map.Count(it => !it.IsVisited), Is.EqualTo(map.Size - 1));
        }

        #endregion

        #region Properties

        [Test]
        public void AllVisited_FalseIfOnlyOneVisited()
        {
            var map = new Map(5, 5);

            map.Visit(map[3, 1]);

            Assert.That(map.AllVisited, Is.EqualTo(false));
        }
        
        [Test]
        public void AllVisited_FalseForNewMap()
        {
            var map = new Map(5, 5);

            Assert.That(map.AllVisited, Is.EqualTo(false));
        }

        [TestCase(10, 10)]
        [TestCase(1, 2)]
        [TestCase(4, 4)]
        [TestCase(30, 7)]
        public void AllVisited_TrueIfAllVisited(int w, int h)
        {
            var map = new Map(w, h);

            foreach (var cell in map)
            {
                map.Visit(cell);
            }

            Assert.That(map.AllVisited, Is.EqualTo(true));
        }

        [TestCase(7)]
        [TestCase(15)]
        public void AllVisited_FalseIfOnlyFewVisited(int numberOfVisited)
        {
            var map = new Map(5, 5);

            // Visit some cells in the middle of the map
            foreach (var cell in map.Skip(5).Take(numberOfVisited))
            {
                map.Visit(cell);
            }

            Assert.That(map.AllVisited, Is.EqualTo(false));
        }
        
        [Test]
        public void AllVisited_FalseIfOneNotVisited()
        {
            var map = new Map(5, 5);

            foreach (var cell in map.Skip(1))
            {
                map.Visit(cell);
            }

            Assert.That(map.AllVisited, Is.EqualTo(false));
        }

        #endregion

        #region CreateCorridor

        [TestCase(0, 0, 0, 1, Dir.S, Dir.N)]
        [TestCase(1, 1, 1, 0, Dir.N, Dir.S)]
        [TestCase(0, 0, 1, 0, Dir.E, Dir.W)]
        [TestCase(1, 1, 0, 1, Dir.W, Dir.E)]
        public void CreateCorridor_CorridorCreatedCorrectly(
            int x1, int y1, int x2, int y2, Dir corridorDirection, Dir d2)
        {
            var map = new Map(5, 5);
            var startCell = new Cell(x1, y1);
            var endCell = new Cell(x2, y2);

            map.CreateCorridor(startCell, endCell, corridorDirection);

            Assert.That(startCell.Sides[corridorDirection], Is.EqualTo(Side.Empty));
            Assert.That(startCell.Sides.Where(s => s.Key != corridorDirection),
                Has.All.With.Property("Value").EqualTo(Side.Wall));

            Assert.That(endCell.Sides[d2], Is.EqualTo(Side.Empty));
            Assert.That(endCell.Sides.Where(s => s.Key != d2),
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

            var startCell = new Cell(x1, y1);
            var endCell = new Cell(x2, y2);

            Assert.That(() => map.CreateCorridor(startCell, endCell, corridorDirection),
                Throws.ArgumentException);
        }

        [TestCase(0, 0, 0, -1, Dir.N)]
        [TestCase(0, 0, -1, 0, Dir.E)]
        [TestCase(0, -1, 0, 0, Dir.S)]
        [TestCase(-1, 0, 0, 0, Dir.W)]
        public void CreateCorridor_ThrowsForCellsOutsideMap(
            int x1, int y1, int x2, int y2, Dir corridorDirection)
        {
            var map = new Map(5, 5);

            var startCell = new Cell(x1, y1);
            var endCell = new Cell(x2, y2);

            Assert.That(() => map.CreateCorridor(startCell, endCell, corridorDirection),
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        #endregion

        ////TODO RemoveCorridor

        #region Constructor

        [TestCase(10, 10)]
        [TestCase(1, 2)]
        public void Constructor_MapIsEmptyWhenCreated(int w, int h)
        {
            var map = new Map(w, h);

            Assert.That(map, Has.All.Property(nameof(Cell.IsVisited)).EqualTo(false));
        }

        #endregion

        #region OutsideMapException

        [TestCase(-2, 5)]
        [TestCase(2, -2)]
        [TestCase(10, 3)]
        [TestCase(1, 30)]
        [TestCase(-1, -30)]
        [TestCase(100, 300)]
        public void MapActionsThrowCorrectExceptionWhenPointIsOutsideMap(int w, int h)
        {
            var actions = new Dictionary<string, Action<IMap, Cell>>
            {
                { "Visit", (m, c) => m.Visit(c) },
                { "HasAdjacentCell", (m, c) => m.HasAdjacentCell(c, Dir.E) }
            };

            var map = new Map(5, 5);
            var point = new Cell(w, h);

            foreach (var action in actions.Values)
            {
                Assert.That(() => action(map, point),
                    Throws.TypeOf<ArgumentOutOfRangeException>(),
                    action + " should throw correct error if point is outside the map.");
            }
        }

        #endregion
    }
}
