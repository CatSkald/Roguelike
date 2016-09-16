using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CatSkald.Roguelike.Core.Terrain;
using CatSkald.Roguelike.DungeonGenerator.Maps;
using NUnit.Framework;

namespace CatSkald.Roguelike.Test.DungeonGenerator.UnitTests.Maps
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
            Assert.That(_map, Has.All.Property(nameof(MapCell.IsVisited)).False);
        }

        #region AllVisited
        [Test]
        public void AllVisited_FalseIfOnlyOneVisited()
        {
            _map.Visit(_map[3, 1]);

            Assert.That(_map.AllVisited, Is.False);
        }

        [Test]
        public void AllVisited_FalseForNewMap()
        {
            Assert.That(_map.AllVisited, Is.False);
        }

        [Test]
        public void AllVisited_TrueIfAllVisited()
        {
            foreach (var cell in _map)
            {
                _map.Visit(cell);
            }

            Assert.That(_map.AllVisited, Is.True);
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

            Assert.That(_map.AllVisited, Is.False);
        }

        [Test]
        public void AllVisited_FalseIfOneNotVisited()
        {
            foreach (var cell in _map.Skip(1))
            {
                _map.Visit(cell);
            }

            Assert.That(_map.AllVisited, Is.False);
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
                .And.With.Property(nameof(MapCell.IsVisited)).True);
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
                Has.All.True);
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
            MapCell adjacent;

            var result = _map.TryGetAdjacentCell(_map[x, y], dir, out adjacent);

            Assert.That(result, Is.False);
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
            MapCell adjacent;

            var result = _map.TryGetAdjacentCell(_map[x, y], dir, out adjacent);

            Assert.That(result, Is.True);
            Assert.That(adjacent, Is.SameAs(_map[expectedX, expectedY]));
        }
        #endregion

        #region CreateCorridorSide
        [TestCase(0, 0, 0, 1, Dir.S, Dir.N)]
        [TestCase(1, 1, 1, 0, Dir.N, Dir.S)]
        [TestCase(0, 0, 1, 0, Dir.E, Dir.W)]
        [TestCase(1, 1, 0, 1, Dir.W, Dir.E)]
        public void CreateCorridorSide_CorridorCreatedCorrectly(
            int x1, int y1, int x2, int y2, Dir corridorDir, Dir oppositeDir)
        {
            var map = new Map(5, 5);
            var startCell = map[x1, y1];
            var endCell = map[x2, y2];
            var emptySide = Side.Empty;

            map.CreateCorridorSide(startCell, endCell, corridorDir, emptySide);

            Assert.That(startCell.Sides[corridorDir], Is.EqualTo(emptySide));
            Assert.That(startCell.Sides.Where(s => s.Key != corridorDir),
                Has.All.With.Property("Value").EqualTo(Side.Wall));

            Assert.That(endCell.Sides[oppositeDir], Is.EqualTo(emptySide));
            Assert.That(endCell.Sides.Where(s => s.Key != oppositeDir),
                Has.All.With.Property("Value").EqualTo(Side.Wall));
        }

        [Test]
        public void CreateCorridorSide_CorridorCreatedCorrectly()
        {
            var map = new Map(5, 5);

            Assert.That(() => map.CreateCorridorSide(
                map[0, 0], map[0, 1], Dir.S, Side.Wall),
                Throws.ArgumentException);
        }

        [TestCase(1, 1, 1, 0, Dir.S)]
        [TestCase(0, 0, 0, 1, Dir.N)]
        [TestCase(0, 0, 0, 1, Dir.E)]
        [TestCase(0, 0, 0, 1, Dir.W)]
        [TestCase(1, 1, 1, 1, Dir.E)]
        [TestCase(1, 1, 0, 0, Dir.E)]
        [TestCase(2, 1, 3, 4, Dir.E)]
        public void CreateCorridorSide_ThrowsForNonAjacentCells(
            int x1, int y1, int x2, int y2, Dir corridorDirection)
        {
            var map = new Map(5, 5);

            var startCell = map[x1, y1];
            var endCell = map[x2, y2];

            Assert.That(() => 
                map.CreateCorridorSide(startCell, endCell, corridorDirection, Side.Empty),
                Throws.InvalidOperationException);
        }
        #endregion

        #region CreateWall
        [TestCase(0, 0, 0, 1, Dir.S, Dir.N)]
        [TestCase(1, 1, 1, 0, Dir.N, Dir.S)]
        [TestCase(0, 0, 1, 0, Dir.E, Dir.W)]
        [TestCase(1, 1, 0, 1, Dir.W, Dir.E)]
        public void CreateWall_CorridorRemovedCorrectly(
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

            map.CreateWall(startCell, corridorDir);

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
        public void CreateWall_Throws_IfStartCellHasNoCorridor(
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

            Assert.That(() => map.CreateWall(startCell, corridorDir),
                Throws.InvalidOperationException);
        }

        [TestCase(0, 0, 0, 1, Dir.S)]
        [TestCase(1, 1, 1, 0, Dir.N)]
        [TestCase(0, 0, 1, 0, Dir.E)]
        [TestCase(1, 1, 0, 1, Dir.W)]
        public void CreateWall_Throws_IfEndCellHasNoCorridor(
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

            Assert.That(() => map.CreateWall(startCell, corridorDir),
                Throws.InvalidOperationException);
        }
        #endregion

        [Test]
        public void Visit_SetsIsVisitedTrue()
        {
            var cell = _map[2, 5];

            _map.Visit(cell);

            Assert.That(cell.IsVisited, Is.True);
        }

        #region InsertRoom
        [TestCase(0, 0, 10, 3)]
        [TestCase(0, 0, 1, 15)]
        [TestCase(4, 3, 5, 1)]
        [TestCase(3, 3, 1, 3)]
        public void InsertRoom_ThrowsIfRoomOutsideMap(int x, int y, int w, int h)
        {
            _map = new Map(5, 5);

            Assert.That(() => _map.InsertRoom(new Room(w, h), new Point(x, y)),
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void InsertRoom_RoomAppearsInRooms()
        {
            _map.InsertRoom(new Room(4, 4), new Point(0, 0));

            Assert.That(_map.Rooms, Has.Count.EqualTo(1));
        }

        [TestCase(1, 0)]
        [TestCase(2, 2)]
        [TestCase(3, 1)]
        public void InsertRoom_OffsetIsAppliedToRoom(int x, int y)
        {
            _map.InsertRoom(new Room(2, 3), new Point(x, y));

            Assert.That(_map.Rooms.Single(), 
                Has.Property(nameof(Room.Bounds))
                .EqualTo(new Rectangle(x, y, 2, 3)));
        }

        [TestCase(1, 0)]
        [TestCase(5, 5)]
        public void InsertRoom_MapCellSidesAreUpdated(int x, int y)
        {
            _map.InsertRoom(new Room(4, 3), new Point(x, y));

            var room = _map.Rooms.Single();

            Assert.That(room, Has.All.Matches<MapCell>(c => c.Sides
                .Equals(_map[new Point(c.Location.X + x, c.Location.Y + y)].Sides)));
        }
        
        [Test]
        public void InsertRoom_AdjacentCellSidesHaveCorrectSides()
        {
            var map = new Map(3, 3);
            foreach (var cell in map)
            {
                foreach (var side in cell.Sides.Keys.ToList())
                {
                    cell.Sides[side] = Side.Empty;
                }
            }
            var room = new Room(2, 2);

            map.InsertRoom(room, new Point(0, 0));

            Assert.That(map[2, 0].Sides[Dir.W], Is.EqualTo(Side.Door), "[2, 0] W");
            Assert.That(map[2, 0].Sides.Where(s => s.Key != Dir.W).Select(s => s.Value),
                Has.All.EqualTo(Side.Empty), "[2, 0] *");

            Assert.That(map[2, 1].Sides[Dir.W], Is.EqualTo(Side.Door), "[2, 1] W");
            Assert.That(map[2, 1].Sides.Where(s => s.Key != Dir.W).Select(s => s.Value),
                Has.All.EqualTo(Side.Empty), "[2, 1] *");

            Assert.That(map[1, 2].Sides[Dir.N], Is.EqualTo(Side.Door), "[1, 2] N");
            Assert.That(map[1, 2].Sides.Where(s => s.Key != Dir.N).Select(s => s.Value),
                Has.All.EqualTo(Side.Empty), "[1, 2] *");

            Assert.That(map[0, 2].Sides[Dir.N], Is.EqualTo(Side.Door), "[0, 2] N");
            Assert.That(map[0, 2].Sides.Where(s => s.Key != Dir.N).Select(s => s.Value), 
                Has.All.EqualTo(Side.Empty), "[0, 2] *");

            var newRoom = new Room(2, 2);
            newRoom[0, 1].Sides[Dir.S] = Side.Door;
            newRoom[1, 0].Sides[Dir.E] = Side.Door;
            newRoom[1, 1].Sides[Dir.S] = Side.Door;
            newRoom[1, 1].Sides[Dir.E] = Side.Door;
            foreach (var cell in newRoom)
            {
                Assert.That(cell.Sides, Is.EquivalentTo(map[cell].Sides),
                    "Failed cell: " + cell.Location);
            }
        }
        
        [Test]
        public void InsertRoom_WithSize1Cell_CellIsCorridorWithDoors()
        {
            var map = new Map(2, 2);
            foreach (var cell in map)
            {
                foreach (var side in cell.Sides.Keys.ToList())
                {
                    cell.Sides[side] = Side.Empty;
                }
            }
            var room = new Room(1, 1);

            map.InsertRoom(room, new Point(0, 0));
            var mapCellSides = map[0, 0].Sides;

            Assert.That(mapCellSides[Dir.W], Is.EqualTo(Side.Wall), "W");
            Assert.That(mapCellSides[Dir.E], Is.EqualTo(Side.Door), "E");
            Assert.That(mapCellSides[Dir.S], Is.EqualTo(Side.Door), "S");
            Assert.That(mapCellSides[Dir.N], Is.EqualTo(Side.Wall), "N");
        }
        
        [TestCase(true)]
        [TestCase(false)]
        public void InsertRoom_RoomCellsIsVisitedIsUpdated(bool isVisited)
        {
            foreach (var cell in _map)
            {
                cell.IsVisited = isVisited;
            }

            _map.InsertRoom(new Room(4, 3), new Point(3, 4));

            var room = _map.Rooms.Single();

            Assert.That(room, 
                Has.All.With.Property(nameof(MapCell.IsVisited)).EqualTo(isVisited));
        }

        [TestCase(1, 4)]
        [TestCase(5, 5)]
        public void InsertRoom_RoomCellLocationsAreNotChanged(int x, int y)
        {
            _map.InsertRoom(new Room(4, 3), new Point(x, y));

            var actualLocations = _map.Rooms.Single().Select(c => c.Location);
            var expected = new Room(4, 3).Select(c => c.Location);
            Assert.That(actualLocations, Is.EquivalentTo(expected));
        }
        #endregion

        [TestCase(-2, 5)]
        [TestCase(2, -2)]
        [TestCase(25, 3)]
        [TestCase(1, 30)]
        [TestCase(-1, -30)]
        [TestCase(100, 300)]
        public void MapActions_ThrowCorrectException_WhenPointIsOutsideMap(
            int x, int y)
        {
            var actions = new Dictionary<string, Action<IMap, MapCell>>
            {
                { "Visit", (m, c) => m.Visit(c) },
                { "HasAdjacentCell", (m, c) => m.HasAdjacentCell(c, Dir.E) },
                { "CreateCorridorSide_StartCell", (m, c) => m.CreateCorridorSide(c, _map[0, 0], Dir.E, Side.Empty) },
                { "CreateCorridorSide_EndCell", (m, c) => m.CreateCorridorSide(_map[0, 0], c, Dir.E, Side.Empty) },
                { "CreateWall", (m, c) => m.CreateWall(c, Dir.E) },
                { "InsertRoom", (m, c) => m.InsertRoom(new Room(2, 2), c.Location) },
                { "TryGetAdjacentCell", (m, c) => {
                        MapCell outCell;
                        m.TryGetAdjacentCell(c, Dir.E, out outCell);
                    }
                }
            };

            foreach (var action in actions.Values)
            {
                Assert.That(() => action(_map, new MapCell(x, y)),
                    Throws.TypeOf<ArgumentOutOfRangeException>(),
                    action + " should throw correct error if point is outside the map.");
            }
        }
    }
}
