using CatSkald.Rogualike.Test.GameProcessor.UnitTests.TestHelpers;
using CatSkald.Roguelike.Core.Objects;
using CatSkald.Roguelike.Core.Terrain;
using CatSkald.Roguelike.GameProcessor.Initialization;
using NUnit.Framework;

namespace CatSkald.Rogualike.Test.GameProcessor.UnitTests.Initialization
{
    public class DungeonPopulatorTests
    {
        [Test]
        public void Fill_UpstairsAdded()
        {
            var populator = new DungeonPopulator();
            var dungeon = new FakeDungeon(5, 3);
            dungeon[2, 2].Type = XType.Empty;
            dungeon[3, 3].Type = XType.Empty;

            populator.Fill(dungeon);

            Assert.That(dungeon,
                Has.Exactly(1)
                .With.Property(nameof(Cell.Type))
                .EqualTo(XType.StairsUp));
        }

        [Test]
        public void Fill_DownstairsAdded()
        {
            var populator = new DungeonPopulator();
            var dungeon = new FakeDungeon(5, 3);
            dungeon[2, 2].Type = XType.Empty;
            dungeon[3, 3].Type = XType.Empty;

            populator.Fill(dungeon);

            Assert.That(dungeon,
                Has.Exactly(1)
                .With.Property(nameof(Cell.Type))
                .EqualTo(XType.StairsDown));
        }
    }
}
