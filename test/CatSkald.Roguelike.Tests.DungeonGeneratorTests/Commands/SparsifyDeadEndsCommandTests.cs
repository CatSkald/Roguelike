using System;
using System.Linq;
using CatSkald.Roguelike.DungeonGenerator.Commands;
using CatSkald.Roguelike.DungeonGenerator.Maps;
using NUnit.Framework;

namespace CatSkald.Roguelike.Tests.DungeonGeneratorTests.Commands
{
    [TestFixture]
    public class SparsifyDeadEndsCommandTests
    {
        [Test]
        public void Execute_ShouldThrow_IfMapNull()
        {
            Map map = null;
            var command = new SparsifyDeadEndsCommand(100);

            Assert.That(() => command.Execute(map),
                Throws.ArgumentNullException);
        }

        [TestCase(-5)]
        [TestCase(101)]
        public void Execute_ShouldThrow_IfSparseFactorIsInvalid(int factor)
        {
            Assert.That(() => new SparsifyDeadEndsCommand(factor),
                Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Execute_RemovesAllDeadEnds_IfSparseFactor100()
        {
            var map = new Map(4, 3);
            new CorridorBuilderCommand(50).Execute(map);
            var deadEnds = map.Where(c => c.IsDeadEnd).ToList();

            var command = new SparsifyDeadEndsCommand(100);
            command.Execute(map);

            Assert.That(deadEnds.TrueForAll(c => !map[c].IsDeadEnd));
        }

        [Test]
        public void Execute_RemovesNoDeadEnds_IfSparseFactor0()
        {
            var map = new Map(4, 3);
            new CorridorBuilderCommand(50).Execute(map);
            var deadEnds = map.Where(c => c.IsDeadEnd).ToList();

            var command = new SparsifyDeadEndsCommand(0);
            command.Execute(map);

            Assert.That(deadEnds.TrueForAll(c => map[c].IsDeadEnd));
        }
    }
}
