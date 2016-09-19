using CatSkald.Roguelike.DungeonGenerator.Services;
using CatSkald.Roguelike.DungeonGenerator.Terrain;
using NUnit.Framework;

namespace CatSkald.Roguelike.Test.DungeonGenerator.UnitTests.Services
{
    public class MapConverterTests
    {
        [TestCase(1, 6)]
        [TestCase(100, 3)]
        public void ConvertToDungeon_ReturnsDungeon_WithCorrectSize(int w, int h)
        {
            var converter = new MapConverter();
            var map = new Map(w, h);
            var expectedWidth = w * 2 + 1;
            var expectedHeight = h * 2 + 1;

            var dungeon = converter.ConvertToDungeon(new Map(w, h));

            Assert.That(dungeon, 
                Has.Property(nameof(Dungeon.Width)).EqualTo(expectedWidth)
                .And.Property(nameof(Dungeon.Height)).EqualTo(expectedHeight));
        } 
    }
}
