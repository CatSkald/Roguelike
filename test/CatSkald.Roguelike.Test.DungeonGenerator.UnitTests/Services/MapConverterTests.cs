using System.Drawing;
using System.Linq;
using CatSkald.Roguelike.Core.Cells;
using CatSkald.Roguelike.Core.Terrain;
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

            var dungeon = converter.ConvertToDungeon(map);

            Assert.That(dungeon, 
                Has.Property(nameof(Dungeon.Width)).EqualTo(expectedWidth)
                .And.Property(nameof(Dungeon.Height)).EqualTo(expectedHeight));
        }

        [Test]
        public void ConvertToDungeon_DungeonHasOnlyWalls_IfMapContainsAllWalls()
        {
            var converter = new MapConverter();
            var map = new Map(5, 5);

            var dungeon = converter.ConvertToDungeon(map);

            Assert.That(dungeon,
                Has.All.With.Property(nameof(Cell.Type)).EqualTo(XType.Wall));
        }

        [Test]
        public void ConvertToDungeon_DungeonIsRoom_IfMapIsSingleRoom()
        {
            var converter = new MapConverter();
            var map = new Map(5, 5);
            map.InsertRoom(new Room(5, 5), new Point(0, 0));

            var dungeon = converter.ConvertToDungeon(map);

            Assert.That(
                dungeon.Where(c => c.Location.X != 0
                    && c.Location.X != dungeon.Width - 1
                    && c.Location.Y != 0
                    && c.Location.Y != dungeon.Height - 1),
                Has.All.With.Property(nameof(Cell.Type)).EqualTo(XType.Empty));
        }

        [Test]
        public void ConvertToDungeon_Dungeon_HasSomeEmptyCells_IfMapContainsCorridor()
        {
            var converter = new MapConverter();
            var map = new Map(5, 5);
            map.CreateCorridorSide(map[1, 2], map[2, 2], Dir.E, Side.Empty);

            var dungeon = converter.ConvertToDungeon(map);

            Assert.That(dungeon,
                Has.Some.With.Property(nameof(Cell.Type)).EqualTo(XType.Empty), 
                "Non borders should be empty.");

            Assert.That(
                dungeon.Where(c => c.Location.X == 0
                    || c.Location.X == dungeon.Width - 1
                    || c.Location.Y == 0
                    || c.Location.Y == dungeon.Height - 1),
                Has.All.With.Property(nameof(Cell.Type)).EqualTo(XType.Wall),
                "Borders should be walls.");
        }
        
        [Test]
        public void ConvertToDungeon_Dungeon_HasDoor_IfMapContainsDoor()
        {
            var converter = new MapConverter();
            var map = new Map(5, 5);
            var cell1 = map[1, 2];
            var cell2 = map[1, 2];
            map.CreateCorridorSide(map[1, 2], map[2, 2], Dir.E, Side.Door);

            var dungeon = converter.ConvertToDungeon(map);

            Assert.That(dungeon,
                Has.Some.With.Property(nameof(Cell.Type)).EqualTo(XType.DoorClosed));
        }
    }
}
