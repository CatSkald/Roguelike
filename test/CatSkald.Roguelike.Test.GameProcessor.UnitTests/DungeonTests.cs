using System.Linq;
using CatSkald.Roguelike.Test.GameProcessor.UnitTests.TestHelpers;
using CatSkald.Roguelike.Core.Cells;
using CatSkald.Roguelike.GameProcessor;
using NUnit.Framework;
using System.Drawing;

namespace CatSkald.Roguelike.Test.GameProcessor.UnitTests
{
    public class DungeonTests
    {
        [Test]
        public void Ctor_CreatesCorrectCells()
        {
            var fake = new FakeDungeon(3, 3);
            fake[0, 1].Type = XType.Character;
            fake[0, 2].Type = XType.Empty;
            fake[1, 1].Type = XType.DoorClosed;
            fake[1, 2].Type = XType.Wall;

            var dungeon = new Dungeon(fake);

            Assert.That(dungeon.Select(c => c.Type).ToList(),
                Is.EquivalentTo(fake.Select(c => c.Type)));
            Assert.That(dungeon,
                Has.Property(nameof(dungeon.Width)).EqualTo(fake.Width)
                .And.Property(nameof(dungeon.Height)).EqualTo(fake.Height));
        }

        [Test]
        public void PlaceCharacter_GivenCharacter_ThenPutsItOnCorrectCell()
        {
            var dungeon = new Dungeon(1, 1);
            var expected = new Character();

            dungeon.PlaceCharacter(expected);

            Assert.That(dungeon.Character, Is.SameAs(expected));
        }

        [TestCase(0, 0)]
        [TestCase(0, 1)]
        public void CanMove_GivenLocationInside_ThenReturnsTrue(int x, int y)
        {
            var dungeon = new Dungeon(2, 2);
            var newLocation = new Point(x, y);
            dungeon[newLocation].Type = XType.Empty;

            var result = dungeon.CanMove(newLocation);

            Assert.IsTrue(result);
        }

        [TestCase(XType.Empty)]
        [TestCase(XType.StairsDown)]
        [TestCase(XType.StairsUp)]
        [TestCase(XType.DoorOpened)]
        [TestCase(XType.DoorClosed)]
        public void CanMove_GivenCellAvailableForMove_ThenReturnsTrue(XType cellType)
        {
            var dungeon = new Dungeon(1, 1);
            var newLocation = new Point(0, 0);
            dungeon[newLocation].Type = cellType;

            var result = dungeon.CanMove(newLocation);

            Assert.IsTrue(result);
        }

        [TestCase(XType.Wall)]
        public void CanMove_GivenNonEmptyCell_ThenReturnsFalse(XType cellType)
        {
            var dungeon = new Dungeon(1, 1);
            var newLocation = new Point(0, 0);
            dungeon[newLocation].Type = cellType;

            var result = dungeon.CanMove(newLocation);

            Assert.IsFalse(result);
        }

        [TestCase(-1, 0)]
        [TestCase(0, -1)]
        [TestCase(0, 1)]
        [TestCase(1, 0)]
        [TestCase(1, 1)]
        public void CanMove_GivenLocationOutside_ThenReturnsFalse(int x, int y)
        {
            var dungeon = new Dungeon(1, 1);
            var newLocation = new Point(x, y);

            var result = dungeon.CanMove(newLocation);

            Assert.IsFalse(result);
        }
    }
}
