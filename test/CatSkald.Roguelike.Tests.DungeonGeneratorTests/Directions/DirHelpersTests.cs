using System.Drawing;
using CatSkald.Roguelike.DungeonGenerator.Directions;
using NUnit.Framework;

namespace CatSkald.Roguelike.Tests.DungeonGeneratorTests.Directions
{
    [TestFixture]
    public class DirHelpersTests
    {
        [Test]
        public void GetAllNotEmptyDirs_ReturnsCorrectDirs()
        {
            Assert.That(DirHelper.GetNonEmptyDirs(),
                Is.EquivalentTo(new[] { Dir.N, Dir.E, Dir.S, Dir.W }));
        }
        
        [TestCase(Dir.N, Dir.S)]
        [TestCase(Dir.E, Dir.W)]
        public void Opposite_ReturnsCorrectDir(Dir dir, Dir opposite)
        {
            Assert.That(DirHelper.Opposite(dir), Is.EqualTo(opposite));
            Assert.That(DirHelper.Opposite(opposite), Is.EqualTo(dir));
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(5)]
        public void Opposite_ThrowsForUnknownDir(int dir)
        {
            Assert.That(() => DirHelper.Opposite((Dir)dir), Throws.ArgumentException);
        }
        
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(5)]
        public void MoveInDirection_ThrowsForUnknownDir(int dir)
        {
            Assert.That(() => DirHelper.MoveInDir(Point.Empty, (Dir)dir),
                Throws.ArgumentException);
        }

        [TestCase(5, 4, Dir.N)]
        [TestCase(6, 5, Dir.E)]
        [TestCase(5, 6, Dir.S)]
        [TestCase(4, 5, Dir.W)]
        public void MoveInDirection_MovesInCorrectDirection(int newWidth, int newHeight, Dir dir)
        {
            var point = new Point(5, 5);

            Assert.That(DirHelper.MoveInDir(point, dir),
                Is.EqualTo(new Point(newWidth, newHeight)));
        }
    }
}