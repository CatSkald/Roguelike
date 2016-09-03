using System;
using System.Linq;
using CatSkald.Roguelike.DungeonGenerator.Commands;
using CatSkald.Roguelike.DungeonGenerator.Maps;
using CatSkald.Roguelike.DungeonGenerator.Parameters;
using NUnit.Framework;

namespace CatSkald.Roguelike.Test.DungeonGenerator.UnitTests.Commands
{
    [TestFixture]
    public class SparsifyCellsCommandTests
    {
        private Map _map;
        private DungeonParameters _parameters;
        private SparsifyCellsCommand _command;

        [SetUp]
        public void SetUp()
        {
            _map = new Map(8, 6);
            _parameters = new DungeonParameters
            {
                TwistFactor = 50,
                CellSparseFactor = 50
            };
            _command = new SparsifyCellsCommand();
        }

        [Test]
        public void Execute_ShouldThrow_IfAllCellsAreWalls()
        {
            Assert.That(() => _command.Execute(_map, _parameters),
                Throws.InvalidOperationException);
        }

        [TestCase(-5)]
        [TestCase(101)]
        public void Execute_ShouldThrow_IfSparseFactorIsInvalid(int factor)
        {
            _parameters.CellSparseFactor = factor;

            Assert.That(() => _command.Execute(_map, _parameters),
                Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Execute_LeaveNonWalls_IfSparseFactor50()
        {
            BuildCorridors();

            _command.Execute(_map, _parameters);

            Assert.That(_map,
                Has.Some.With.Property(nameof(MapCell.IsWall)).False
                .And.Some.With.Property(nameof(MapCell.IsWall)).True);
        }

        [Test]
        public void Execute_MakeAllCellsWalls_IfSparseFactor100()
        {
            BuildCorridors();

            _parameters.CellSparseFactor = 100;
            _command.Execute(_map, _parameters);

            Assert.That(_map,
                Has.All.With.Property(nameof(MapCell.IsWall)).True);
        }

        [Test]
        public void Execute_Sets_IsCorridor_ToFalse()
        {
            BuildCorridors();

            var deadEnds = _map.Where(c => c.IsDeadEnd).ToList();

            _parameters.CellSparseFactor = 100;
            _command.Execute(_map, _parameters);

            Assert.That(deadEnds,
                Has.All.With.Property(nameof(MapCell.IsCorridor)).False);
        }

        [Test]
        public void Execute_NoCellsMadeWalls_IfSparseFactor0()
        {
            BuildCorridors();

            _parameters.CellSparseFactor = 0;
            _command.Execute(_map, _parameters);

            Assert.That(_map,
                Has.None.With.Property(nameof(MapCell.IsWall)).True);
        }

        private void BuildCorridors()
        {
            var corridorBuilderCommand = new CorridorBuilderCommand(
                    new DirectionPicker(_parameters.TwistFactor));
            corridorBuilderCommand.Execute(_map, _parameters);
        }
    }
}
