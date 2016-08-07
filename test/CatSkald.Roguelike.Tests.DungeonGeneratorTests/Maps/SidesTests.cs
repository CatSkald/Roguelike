using System.Linq;
using CatSkald.Roguelike.DungeonGenerator.Directions;
using CatSkald.Roguelike.DungeonGenerator.Maps;
using NUnit.Framework;

namespace CatSkald.Roguelike.Tests.DungeonGeneratorTests.Maps
{
    [TestFixture]
    public class SidesTests
    {
        private Sides _sides;

        [SetUp]
        public void SetUp()
        {
            _sides = new Sides();
        }

        #region Constructor
        [Test]
        public void Constructor_HasCountSameAsAllDirections()
        {
            Assert.That(_sides.Values, Has.All.EqualTo(Side.Wall));
        }

        [Test]
        public void Constructor_ContainsAllDirections()
        {
            Assert.That(_sides.Keys, Is.EquivalentTo(DirHelper.GetNonEmptyDirs()));
        }

        [Test]
        public void Constructor_AllSidesAreWalls()
        {
            Assert.That(_sides.Values, Has.All.EqualTo(Side.Wall));
        }
        #endregion

        #region IEquatable
        [Test]
        public void IEquatableMembers_WorksCorrect_ForDefaultSides()
        {
            var other = new Sides();

            CustomAssert.IEqualityMembersWorkForEqualObjects(_sides, other);
        }

        [TestCase(new[] { Dir.N, Dir.E, Dir.S, Dir.W })]
        [TestCase(new[] { Dir.E, Dir.N, Dir.W })]
        [TestCase(new[] { Dir.E, Dir.S })]
        [TestCase(new[] { Dir.N })]
        public void IEquatableMembers_WorksCorrect_IfSidesEqual(params Dir[] emptyDirs)
        {
            var other = new Sides();

            foreach (var dir in emptyDirs)
            {
                _sides[dir] = Side.Empty;
                other[dir] = Side.Empty;
            }

            CustomAssert.IEqualityMembersWorkForEqualObjects(_sides, other);
        }
        
        [TestCase(new[] { Dir.N, Dir.E, Dir.S, Dir.W })]
        [TestCase(new[] { Dir.E, Dir.N, Dir.W })]
        [TestCase(new[] { Dir.E, Dir.S })]
        [TestCase(new[] { Dir.N })]
        public void IEquatableMembers_WorksCorrect_IfOneHasSomeDifferentSides(params Dir[] emptyDirs)
        {
            var other = new Sides();

            foreach (var dir in emptyDirs)
            {
                _sides[dir] = Side.Empty;
            }

            CustomAssert.IEquatableMembersWorkForDifferentObjects(_sides, other);
        }

        [TestCase(new[] { Dir.N })]
        [TestCase(new[] { Dir.E })]
        [TestCase(new[] { Dir.W })]
        [TestCase(new[] { Dir.S })]
        [TestCase(new[] { Dir.E, Dir.S })]
        [TestCase(new[] { Dir.W, Dir.N })]
        public void IEquatableMembers_WorksCorrect_IfOppositeSidesDiffers(params Dir[] emptyDirs)
        {
            var other = new Sides();

            foreach (var dir in emptyDirs)
            {
                _sides[dir] = Side.Empty;
                other[dir.Opposite()] = Side.Empty;
            }

            CustomAssert.IEquatableMembersWorkForDifferentObjects(_sides, other);
        }
        #endregion
    }
}