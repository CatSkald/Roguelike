using CatSkald.Roguelike.DungeonGenerator.Commands;
using CatSkald.Roguelike.DungeonGenerator.Maps;
using NUnit.Framework;

namespace CatSkald.Roguelike.Tests.DungeonGeneratorTests.Commands
{
    [TestFixture]
    public class CorridorBuilderCommandTests
    {
        [Test]
        public void Execute_SetsAllCellsVisited()
        {
            var map = new Map(10, 5);
            var command = new CorridorBuilderCommand(100);

            command.Execute(map);

            Assert.That(map,
                Has.All.With.Property(nameof(Cell.IsVisited)).EqualTo(true));
        }
    }
}
