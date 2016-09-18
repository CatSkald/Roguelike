using System;
using CatSkald.Roguelike.DungeonGenerator.Commands;
using CatSkald.Roguelike.DungeonGenerator.Maps;
using CatSkald.Roguelike.Core.Parameters;
using NUnit.Framework;

namespace CatSkald.Roguelike.Test.DungeonGenerator.UnitTests.Commands
{
    [TestFixture]
    public class CorridorBuilderCommandTests
    {
        [TestCase(0, 0)]
        [TestCase(1, 2)]
        public void Execute_ShouldThrow_IfMapContainsVisitedCell(int x, int y)
        {
            var map = new Map(2, 3);
            var parameters = new DungeonParameters();
            var command = new CorridorBuilderCommand(new DirectionPicker(50));

            map.Visit(map[x, y]);

            Assert.That(() => command.Execute(map, parameters),
                Throws.InvalidOperationException);
        }

        [TestCase(-5)]
        [TestCase(101)]
        public void Execute_ShouldThrow_IfTwistFactorIsInvalid(int factor)
        {
            var map = new Map(2, 3);
            var parameters = new DungeonParameters
            {
                TwistFactor = factor
            };
            var command = new CorridorBuilderCommand(new DirectionPicker(50));

            Assert.That(() => command.Execute(map, parameters),
                Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Execute_SetsAllCells_ToNonWalls()
        {
            var map = new Map(2, 3);
            var parameters = new DungeonParameters();
            var command = new CorridorBuilderCommand(new DirectionPicker(50));

            command.Execute(map, parameters);

            Assert.That(map, Has.All.With.Property(nameof(MapCell.IsWall)).False);
        }

        [Test]
        public void Execute_SetsAllCells_ToVisited()
        {
            var map = new Map(2, 3);
            var parameters = new DungeonParameters();
            var command = new CorridorBuilderCommand(new DirectionPicker(50));

            command.Execute(map, parameters);

            Assert.That(map, Has.All.With.Property(nameof(MapCell.IsVisited)).True);
        }
    }
}
