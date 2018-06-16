using System;
using System.Linq;
using CatSkald.Roguelike.DungeonGenerator.Commands;
using CatSkald.Roguelike.DungeonGenerator.Services;
using CatSkald.Roguelike.DungeonGenerator.Terrain;
using CatSkald.Roguelike.Core.Parameters;
using NUnit.Framework;

namespace CatSkald.Roguelike.Test.DungeonGenerator.UnitTests.Commands
{
    [TestFixture]
    public class SparsifyDeadEndsCommandTests
    {
        private Map _map;
        private MapParameters _parameters;
        private SparsifyDeadEndsCommand _command;

        [SetUp]
        public void SetUp()
        {
            _map = new Map(12, 16);
            _parameters = new MapParameters
            {
                TwistFactor = 50,
                DeadEndSparseFactor = 50
            };
            _command = new SparsifyDeadEndsCommand(new DirectionPicker(0));
        }

        [TestCase(-5)]
        [TestCase(101)]
        public void Execute_ShouldThrow_IfSparseFactorIsInvalid(int factor)
        {
            _parameters.DeadEndSparseFactor = factor;

            Assert.That(() => _command.Execute(_map, _parameters),
                Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Execute_RemovesAllDeadEnds_IfSparseFactor100()
        {
            BuildCorridors();

            var deadEnds = _map.Where(c => c.IsDeadEnd).ToList();

            if (!deadEnds.Any())
                Assert.Inconclusive("No dead ends generated.");

            _parameters.DeadEndSparseFactor = 100;
            _command.Execute(_map, _parameters);

            Assert.That(deadEnds.TrueForAll(c => !_map[c].IsDeadEnd));
        }

        [Test]
        public void Execute_RemovesNoDeadEnds_IfSparseFactor0()
        {
            BuildCorridors();

            var deadEnds = _map.Where(c => c.IsDeadEnd).ToList();

            if (!deadEnds.Any())
                Assert.Inconclusive("No dead ends generated.");

            _parameters.DeadEndSparseFactor = 0;
            _command.Execute(_map, _parameters);

            Assert.That(deadEnds.TrueForAll(c => _map[c].IsDeadEnd));
        }

        [Test]
        public void Execute_RemovesSomeDeadEnds_IfSparseFactor50()
        {
            BuildCorridors();

            var deadEnds = _map.Where(c => c.IsDeadEnd).ToList();

            if (!deadEnds.Any())
                Assert.Inconclusive("No dead ends generated.");

            _parameters.DeadEndSparseFactor = 50;
            _command.Execute(_map, _parameters);

            Assert.That(deadEnds, 
                Has.Some.Matches<MapCell>(c => !_map[c].IsDeadEnd)
                .And.Some.Matches<MapCell>(c => _map[c].IsDeadEnd));
        }

        private void BuildCorridors()
        {
            var corridorBuilderCommand = new CorridorBuilderCommand(
                    new DirectionPicker(_parameters.TwistFactor));
            corridorBuilderCommand.Execute(_map, _parameters);
        }
    }
}
