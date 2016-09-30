using CatSkald.Roguelike.Core.Cells;
using NUnit.Framework;
namespace CatSkald.Roguelike.Test.Core.UnitTests.Cells
{
    public class TileTests
    {
        [TestCase(XType.Character)]
        [TestCase(XType.StairsDown)]
        public void Constructor_SetsCorrectType(XType type)
        {
            var tile = new Tile(type);

            Assert.That(tile.Type, Is.EqualTo(type));
        }
    }
}
