using System;
using CatSkald.Roguelike.DungeonGenerator.Commands;
using CatSkald.Roguelike.DungeonGenerator.Maps;
using NUnit.Framework;

namespace CatSkald.Roguelike.Tests.DungeonGeneratorTests.Commands
{
    [TestFixture]
    public class CorridorBuilderCommandTests
    {
        [Test]
        public void Execute_ShouldThrow_IfMapNull()
        {
            Map map = null;
            var command = new CorridorBuilderCommand(100);

            Assert.That(() => command.Execute(map),
                Throws.ArgumentNullException);
        }

        [TestCase(-5)]
        [TestCase(101)]
        public void Execute_ShouldThrow_IfSparseFactorIsInvalid(int factor)
        {
            Assert.That(() => new CorridorBuilderCommand(factor),
                Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Execute_SetsAllCellsNonWalls()
        {
            var map = new Map(2, 3);

            var command = new CorridorBuilderCommand(50);
            command.Execute(map);

            Assert.That(map, Has.All.With.Property(nameof(Cell.IsWall)).False);
        }

        [Test]
        public void Execute_SetsAllCellsVisited()
        {
            var map = new Map(4, 3);

            var command = new CorridorBuilderCommand(100);
            command.Execute(map);

            Assert.That(map, Has.All.With.Property(nameof(Cell.IsVisited)).True);
        }
    }
}
