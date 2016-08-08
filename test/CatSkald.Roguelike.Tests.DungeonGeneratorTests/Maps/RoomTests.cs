using CatSkald.Roguelike.DungeonGenerator.Directions;
using CatSkald.Roguelike.DungeonGenerator.Maps;
using NUnit.Framework;

namespace CatSkald.Roguelike.Tests.DungeonGeneratorTests.Maps
{
    [TestFixture]
    public class RoomTests
    {
        [TestCase(2, 2)]
        [TestCase(11, 15)]
        [TestCase(15, 4)]
        public void Constructor_CellsHasCorrectBounds(int w, int h)
        {
            var room = new Room(w, h);

            Assert.That(room, Has.All.Matches<Cell>(cell =>
            {
                return (cell.Sides[Dir.W] == Side.Empty || cell.Location.X == 0)
                && (cell.Sides[Dir.E] == Side.Empty || cell.Location.X == room.Width - 1)
                && (cell.Sides[Dir.N] == Side.Empty || cell.Location.Y == 0)
                && (cell.Sides[Dir.S] == Side.Empty || cell.Location.Y == room.Height - 1);
            }));
        }
    }
}
