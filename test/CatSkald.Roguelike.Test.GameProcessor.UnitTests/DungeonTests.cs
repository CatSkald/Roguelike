using System.Linq;
using CatSkald.Roguelike.Test.GameProcessor.UnitTests.TestHelpers;
using CatSkald.Roguelike.Core.Cells;
using CatSkald.Roguelike.GameProcessor;
using NUnit.Framework;
namespace CatSkald.Roguelike.Test.GameProcessor.UnitTests
{
    public class DungeonTests
    {
        [Test]
        public void Constructor_CreatesCorrectCells()
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
        public void Character_GetReturnsSameValue_AsWasSet()
        {
            var dungeon = new Dungeon(1, 1);
            var expected = new Character();

            dungeon.Character = expected;

            Assert.That(dungeon.Character, Is.SameAs(expected));
        }
    }
}
