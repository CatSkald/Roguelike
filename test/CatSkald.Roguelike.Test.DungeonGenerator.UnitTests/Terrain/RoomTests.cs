using System.Drawing;
using CatSkald.Roguelike.Core.Terrain;
using CatSkald.Roguelike.DungeonGenerator.Terrain;
using NUnit.Framework;

namespace CatSkald.Roguelike.Test.DungeonGenerator.UnitTests.Terrain
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

            Assert.That(room, Has.All.Matches<MapCell>(cell =>
            {
                return (cell.Sides[Dir.W] == Side.Empty || cell.Location.X == 0)
                && (cell.Sides[Dir.E] == Side.Empty || cell.Location.X == room.Width - 1)
                && (cell.Sides[Dir.N] == Side.Empty || cell.Location.Y == 0)
                && (cell.Sides[Dir.S] == Side.Empty || cell.Location.Y == room.Height - 1);
            }));
        }

        [TestCase(1, 2)]
        [TestCase(4, 4)]
        [TestCase(100, 25)]
        public void Offset_SetsCorrectValue(int x, int y)
        {
            var room = new Room(15, 10);

            room.Offset(new Point(x, y));

            Assert.That(room.Bounds, Is.EqualTo(new Rectangle(x, y, 15, 10)));
        }
    }
}
