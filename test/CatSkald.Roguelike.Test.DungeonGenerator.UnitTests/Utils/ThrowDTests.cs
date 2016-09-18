using System;
using System.Drawing;
using CatSkald.Roguelike.Core.Terrain;
using CatSkald.Roguelike.DungeonGenerator.Terrain;
using CatSkald.Roguelike.DungeonGenerator.Utils;
using NUnit.Framework;

namespace CatSkald.Roguelike.Test.DungeonGenerator.UnitTests.Utils
{
    [TestFixture]
    public class ThrowDTests
    {
        #region IfOutsideMap_Cell
        [TestCase(3, 2)]
        [TestCase(0, 1)]
        [TestCase(1, 1)]
        public void IfOutsideMap_Cell_ShouldSucceed_IfInsideMap(int x, int y)
        {
            var map = new Map(5, 5);
            var cell = new MapCell(x, y);

            Assert.That(() => ThrowD.IfOutsideMap(map, cell), Throws.Nothing);
        }

        [TestCase(0, -1)]
        [TestCase(-3, -1)]
        [TestCase(-5, 0)]
        [TestCase(11, 0)]
        [TestCase(10, 24)]
        [TestCase(0, 23)]
        public void IfOutsideMap_Cell_ShouldThrow_IfOutsideMap(int x, int y)
        {
            var map = new Map(5, 5);
            var cell = new MapCell(x, y);

            Assert.That(() => ThrowD.IfOutsideMap(map, cell),
                Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(5, 4, "myPoint")]
        [TestCase(2, 24, "errorName")]
        public void IfOutsideMap_Cell_ExceptionShouldContainMapBoundsPropertyNameAndValue(
            int width, int height, string name)
        {
            var map = new Map(width, height);
            var cell = new MapCell(-1, -2);

            Assert.That(() => ThrowD.IfOutsideMap(map, cell, name),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                .With.Message.Contains(map.Bounds.ToString())
                .And.Property(nameof(ArgumentOutOfRangeException.ParamName))
                .EqualTo(name).And
                .Property(nameof(ArgumentOutOfRangeException.ActualValue))
                .EqualTo(cell.Location));
        }

        [TestCase("nullCell")]
        [TestCase("cell")]
        public void IfOutsideMap_Cell_ShouldThrow_IfCellNull(string name)
        {
            var map = new Map(5, 5);
            MapCell cell = null;

            Assert.That(
                () => ThrowD.IfOutsideMap(map, cell, name),
                Throws.ArgumentNullException
                .With.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo(name));
        }
        #endregion

        #region IfOutsideMap_Point
        [TestCase(3, 2)]
        [TestCase(0, 1)]
        [TestCase(1, 1)]
        public void IfOutsideMap_Point_ShouldSucceed_IfInsideMap(int x, int y)
        {
            var map = new Map(5, 5);
            var point = new Point(x, y);

            Assert.That(() => ThrowD.IfOutsideMap(map, point), Throws.Nothing);
        }

        [TestCase(0, -1)]
        [TestCase(-3, -1)]
        [TestCase(-5, 0)]
        [TestCase(11, 0)]
        [TestCase(10, 24)]
        [TestCase(0, 23)]
        public void IfOutsideMap_Point_ShouldThrow_IfOutsideMap(int x, int y)
        {
            var map = new Map(5, 5);
            var point = new Point(x, y);

            Assert.That(() => ThrowD.IfOutsideMap(map, point),
                Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(5, 4, "myPoint")]
        [TestCase(2, 24, "errorName")]
        public void IfOutsideMap_Point_ExceptionShouldContainMapBoundsPropertyNameAndValue(
            int width, int height, string name)
        {
            var map = new Map(width, height);
            var point = new Point(-1, -2);

            Assert.That(() => ThrowD.IfOutsideMap(map, point, name),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                .With.Message.Contains(map.Bounds.ToString())
                .And.Property(nameof(ArgumentOutOfRangeException.ParamName))
                .EqualTo(name)
                .And.Property(nameof(ArgumentOutOfRangeException.ActualValue))
                .EqualTo(point));
        }
        #endregion

        #region IfOutsideMap_Room
        [TestCase(3, 2)]
        [TestCase(0, 1)]
        [TestCase(1, 1)]
        public void IfOutsideMap_Room_ShouldSucceed_IfInsideMap(int w, int h)
        {
            var map = new Map(5, 5);
            var room = new Room(w, h);

            Assert.That(() => ThrowD.IfOutsideMap(map, room), Throws.Nothing);
        }

        [TestCase(1, 6)]
        [TestCase(7, 4)]
        [TestCase(10, 10)]
        public void IfOutsideMap_Room_ShouldThrow_IfOutsideMap(int w, int h)
        {
            var map = new Map(5, 5);
            var room = new Room(w, h);

            Assert.That(() => ThrowD.IfOutsideMap(map, room),
                Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(5, 4, "myRoom")]
        [TestCase(2, 7, "errorName")]
        public void IfOutsideMap_Room_ExceptionShouldContainMapBoundsPropertyNameAndValue(
            int width, int height, string name)
        {
            var map = new Map(width, height);
            var room = new Room(10, 11);

            Assert.That(() => ThrowD.IfOutsideMap(map, room, name),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                .With.Message.Contains(map.Bounds.ToString())
                .And.Property(nameof(ArgumentOutOfRangeException.ParamName))
                .EqualTo(name)
                .And.Property(nameof(ArgumentOutOfRangeException.ActualValue))
                .EqualTo(room.Bounds));
        }
        #endregion

        #region IfNoCorridor
        [TestCase(Dir.N)]
        [TestCase(Dir.E)]
        [TestCase(Dir.W)]
        [TestCase(Dir.S)]
        public void IfNoCorridor_ShouldSucceed_IfCorridorExists(Dir dir)
        {
            var cell = new MapCell();
            cell.Sides[dir] = Side.Empty;

            Assert.That(() => ThrowD.IfNoCorridor(cell, dir), Throws.Nothing);
        }

        [TestCase(Dir.N)]
        [TestCase(Dir.E)]
        [TestCase(Dir.W)]
        [TestCase(Dir.S)]
        public void IfNoCorridor_ShouldThrow_IfOutsideMap(Dir dir)
        {
            var cell = new MapCell();
            cell.Sides[dir] = Side.Wall;

            Assert.That(() => ThrowD.IfNoCorridor(cell, dir),
                Throws.InstanceOf<InvalidOperationException>());
        }

        [TestCase(Dir.N, "myPoint")]
        [TestCase(Dir.S, "errorName")]
        public void IfNoCorridor_ExceptionShouldContainMapBoundsPropertyNameAndValue(
            Dir dir, string name)
        {
            var cell = new MapCell();
            cell.Sides[dir] = Side.Wall;

            Assert.That(() => ThrowD.IfNoCorridor(cell, dir, name),
                Throws.InvalidOperationException
                .With.Message.Contains(dir.ToString())
                .And.Message.Contains(name));
        }

        [TestCase("nullCell")]
        [TestCase("cell")]
        public void IfNoCorridor_ShouldThrow_IfCellNull(string name)
        {
            MapCell cell = null;

            Assert.That(
                () => ThrowD.IfNoCorridor(cell, Dir.N, name),
                Throws.ArgumentNullException
                .With.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo(name));
        }
        #endregion

        #region IfNotAdjacent
        [TestCase(0, 0, 1, 0, Dir.E)]
        [TestCase(1, 0, 0, 0, Dir.W)]
        [TestCase(1, 0, 1, 1, Dir.S)]
        [TestCase(1, 1, 1, 0, Dir.N)]
        public void IfNotAdjacent_ShouldSucceed_IfAdjacent(
            int x1, int y1, int x2, int y2, Dir dir)
        {
            var startCell = new MapCell(x1, y1);
            var endCell = new MapCell(x2, y2);

            Assert.That(
                () => ThrowD.IfNotAdjacent(startCell, endCell, dir),
                Throws.Nothing);
        }
        
        [TestCase(0, 0, 1, 1, Dir.N)]
        [TestCase(0, 0, 0, 1, Dir.E)]
        [TestCase(1, 1, 0, 1, Dir.N)]
        public void IfNotAdjacent_ShouldThrow_IfNotAdjacent(
            int x1, int y1, int x2, int y2, Dir dir)
        {
            var startCell = new MapCell(x1, y1);
            var endCell = new MapCell(x2, y2);

            Assert.That(
                () => ThrowD.IfNotAdjacent(startCell, endCell, dir),
                Throws.InvalidOperationException);
        }
        
        [Test]
        public void IfNotAdjacent_ExceptionShouldContainCellsAndDirection()
        {
            var startCell = new MapCell(0, 0);
            var endCell = new MapCell(1, 1);

            Assert.That(
                () => ThrowD.IfNotAdjacent(startCell, endCell, Dir.W),
                Throws.InvalidOperationException
                .With.Message.Contains(startCell.Location.ToString())
                .With.Message.Contains(endCell.Location.ToString())
                .And.Message.Contains(Dir.W.ToString()));
        }
        
        [TestCase(Dir.N)]
        [TestCase(Dir.S)]
        public void IfNotAdjacent_ShouldThrow_IfStartCellNull(Dir dir)
        {
            MapCell startCell = null;
            var endCell = new MapCell(0, 0);

            Assert.That(
                () => ThrowD.IfNotAdjacent(startCell, endCell, dir),
                Throws.ArgumentNullException
                .With.Message.Contains(nameof(startCell)));
        }

        [TestCase(Dir.N)]
        [TestCase(Dir.S)]
        public void IfNotAdjacent_ShouldThrow_IfEndCellNull(Dir dir)
        {
            var startCell = new MapCell(0, 0);
            MapCell endCell = null;

            Assert.That(
                () => ThrowD.IfNotAdjacent(startCell, endCell, dir),
                Throws.ArgumentNullException
                .With.Message.Contains(nameof(endCell)));
        }
        #endregion
    }
}
