using System.Drawing;
using CatSkald.Roguelike.Core.Terrain;
using NUnit.Framework;

namespace CatSkald.Roguelike.Test.Core.UnitTests
{
    [TestFixture]
    public class DirHelperTests
    {
        [Test]
        public void GetMainDirs_ReturnsCorrectDirs()
        {
            Assert.That(DirHelper.GetMainDirs(),
                Is.EquivalentTo(new[] { Dir.N, Dir.E, Dir.S, Dir.W }));
        }

        [TestCase(Dir.N, Dir.S)]
        [TestCase(Dir.S, Dir.N)]
        [TestCase(Dir.E, Dir.W)]
        [TestCase(Dir.W, Dir.E)]
        [TestCase(Dir.SE, Dir.NW)]
        [TestCase(Dir.SW, Dir.NE)]
        [TestCase(Dir.NE, Dir.SW)]
        [TestCase(Dir.NW, Dir.SE)]
        public void Opposite_ReturnsCorrectDir(Dir dir, Dir opposite)
        {
            Assert.That(DirHelper.Opposite(dir), Is.EqualTo(opposite));
            Assert.That(DirHelper.Opposite(opposite), Is.EqualTo(dir));
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(9)]
        public void Opposite_ThrowsForUnknownDir(int dir)
        {
            Assert.That(() => DirHelper.Opposite((Dir)dir), Throws.ArgumentException);
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(9)]
        public void MoveInDirection_ThrowsForUnknownDir(int dir)
        {
            Assert.That(() => DirHelper.MoveInDir(Point.Empty, (Dir)dir),
                Throws.ArgumentException);
        }

        [TestCase(5, 4, Dir.N)]
        [TestCase(6, 4, Dir.NE)]
        [TestCase(4, 4, Dir.NW)]
        [TestCase(6, 5, Dir.E)]
        [TestCase(5, 6, Dir.S)]
        [TestCase(6, 6, Dir.SE)]
        [TestCase(4, 6, Dir.SW)]
        [TestCase(4, 5, Dir.W)]
        public void MoveInDirection_MovesInCorrectDirection(int newWidth, int newHeight, Dir dir)
        {
            var point = new Point(5, 5);

            Assert.That(DirHelper.MoveInDir(point, dir),
                Is.EqualTo(new Point(newWidth, newHeight)));
        }
    }
}