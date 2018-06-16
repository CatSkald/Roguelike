using System.Drawing;
using CatSkald.Roguelike.Core.Cells;
using CatSkald.Roguelike.Core.Terrain;
using NUnit.Framework;

namespace CatSkald.Roguelike.Test.Core.UnitTests.Terrain
{
    [TestFixture]
    public class MapImageTests
    {
        [TestCase(32, 47)]
        [TestCase(1, 1)]
        public void Constructor_CorrectSizeIsSet(int width, int height)
        {
            var image = new MapImage(width, height);

            Assert.That(image, 
                Has.Property(nameof(image.Width)).EqualTo(width)
                .And.Property(nameof(image.Height)).EqualTo(height));
        }

        [TestCase(1, 2)]
        [TestCase(99, 12)]
        public void SetTile_CorrectTileIsSet(int x, int y)
        {
            var image = new MapImage(100, 15);
            var expected = new Appearance("X", "Y", '1', Color.AliceBlue, true, true, true);

            image.SetTile(new Point(x, y), expected);

            Assert.That(image[x, y], 
                Has.Property(nameof(Tile.Appearance)).EqualTo(expected));
            Assert.That(image, Has.Exactly(1)
                .With.Property(nameof(Tile.Appearance)).EqualTo(expected));
        }
    }
}
