using System;
using System.Linq;
using CatSkald.Roguelike.DungeonGenerator.Commands;
using CatSkald.Roguelike.DungeonGenerator.Maps;
using NUnit.Framework;

namespace CatSkald.Roguelike.Tests.DungeonGeneratorTests.Commands
{
    [TestFixture]
    public class SparsifyCellsCommandTests
    {
        [Test]
        public void Execute_ShouldThrow_IfAllCellsAreWalls()
        {
            var map = new Map(2, 5);
            var command = new SparsifyCellsCommand(50);

            Assert.That(() => command.Execute(map),
                Throws.InvalidOperationException);
        }
        
        [Test]
        public void Execute_ShouldThrow_IfMapNull()
        {
            Map map = null;
            var command = new SparsifyCellsCommand(100);

            Assert.That(() => command.Execute(map),
                Throws.ArgumentNullException);
        }
        
        [TestCase(-5)]
        [TestCase(101)]
        public void Execute_ShouldThrow_IfSparseFactorIsInvalid(int factor)
        {
            Assert.That(() => new SparsifyCellsCommand(factor),
                Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Execute_LeaveNonWalls_IfSparseFactor50()
        {
            var map = new Map(4, 5);
            new CorridorBuilderCommand(50).Execute(map);

            var command = new SparsifyCellsCommand(50);
            command.Execute(map);

            Assert.That(map, Has.Some.With.Property(nameof(MapCell.IsWall)).False);
        }

        [Test]
        public void Execute_MakeAllCellsWalls_IfSparseFactor100()
        {
            var map = new Map(2, 3);
            new CorridorBuilderCommand(50).Execute(map);

            var command = new SparsifyCellsCommand(100);
            command.Execute(map);

            Assert.That(map, Has.All.With.Property(nameof(MapCell.IsWall)).True);
        }
        
        [Test]
        public void Execute_Sets_IsCorridor_ToFalse()
        {
            var map = new Map(2, 3);
            new CorridorBuilderCommand(50).Execute(map);
            var deadEnds = map.Where(c => c.IsDeadEnd).ToList();

            Assert.That(deadEnds, Has.All.With.Property(nameof(MapCell.IsCorridor)).True);

            var command = new SparsifyCellsCommand(100);
            command.Execute(map);

            Assert.That(deadEnds, Has.All.With.Property(nameof(MapCell.IsCorridor)).False);
        }

        [Test]
        public void Execute_NoCellsMadeWalls_IfSparseFactor0()
        {
            var map = new Map(2, 3);
            new CorridorBuilderCommand(50).Execute(map);

            var command = new SparsifyCellsCommand(0);
            command.Execute(map);

            Assert.That(map, Has.None.With.Property(nameof(MapCell.IsWall)).True);
        }
    }
}
