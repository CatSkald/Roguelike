using System.Linq;
using CatSkald.Roguelike.Test.GameProcessor.UnitTests.TestHelpers;
using CatSkald.Roguelike.Core.Cells;
using CatSkald.Roguelike.Core.Cells.Creatures;
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
            fake[1, 1].Type = XType.Door;
            fake[1, 2].Type = XType.Wall;

            var dungeon = new Dungeon(fake);

            Assert.That(dungeon.Select(c => c.Type).ToList(),
                Is.EquivalentTo(fake.Select(c => c.Type)));
            Assert.That(dungeon,
                Has.Property(nameof(dungeon.Width)).EqualTo(fake.Width)
                .And.Property(nameof(dungeon.Height)).EqualTo(fake.Height));
        }

        [Test]
        public void Ctor_CreatesDoorsAsCorrectType()
        {
            var fake = new FakeDungeon(2, 2);
            fake[0, 1].Type = XType.Character;
            fake[1, 0].Type = XType.Door;
            fake[1, 1].Type = XType.Wall;

            var dungeon = new Dungeon(fake);

            Assert.That(dungeon[0, 0], Is.TypeOf<Cell>(), "Cell 0,0");
            Assert.That(dungeon[0, 1], Is.TypeOf<Cell>(), "Cell 0,1");
            Assert.That(dungeon[1, 0], Is.TypeOf<Door>(), "Cell 1,0");
            Assert.That(dungeon[1, 1], Is.TypeOf<Cell>(), "Cell 1,1");
        }

        [Test]
        public void PlaceCharacter_GivenCharacter_ThenPutsItOnCorrectCell()
        {
            var dungeon = new Dungeon(1, 1);
            var expected = new Character(new MainStats(), new Point());

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
        public void CanMove_GivenCellAvailableForMove_ThenReturnsTrue(XType cellType)
        {
            var dungeon = new Dungeon(1, 1);
            var newLocation = new Point(0, 0);
            dungeon[newLocation].Type = cellType;

            var result = dungeon.CanMove(newLocation);

            Assert.IsTrue(result);
        }

        [Test]
        public void CanMove_GivenUnlockedClosedDoor_ThenReturnsTrue()
        {
            var dungeonWithDoor = new Dungeon(1, 1);
            var newLocation = new Point(0, 0);
            dungeonWithDoor[newLocation].Type = XType.Door;
            var dungeon = new Dungeon(dungeonWithDoor);

            var result = dungeon.CanMove(newLocation);

            Assert.IsTrue(result);
        }

        [Test]
        public void CanMove_GivenLockedClosedDoor_ThenReturnsFalse()
        {
            var dungeonWithDoor = new Dungeon(1, 1);
            var newLocation = new Point(0, 0);
            dungeonWithDoor[newLocation].Type = XType.Door;
            var dungeon = new Dungeon(dungeonWithDoor);
            ((Door)dungeon[newLocation]).Lock();

            var result = dungeon.CanMove(newLocation);

            Assert.IsFalse(result);
        }

        [Test]
        public void CanMove_GivenOpenedDoor_ThenReturnsTrue()
        {
            var dungeonWithDoor = new Dungeon(1, 1);
            var newLocation = new Point(0, 0);
            dungeonWithDoor[newLocation].Type = XType.Door;
            var dungeon = new Dungeon(dungeonWithDoor);
            ((Door)dungeon[newLocation]).Open();

            var result = dungeon.CanMove(newLocation);

            Assert.IsTrue(result);
        }

        [TestCase(XType.Wall)]
        public void CanMove_GivenObstacleCell_ThenReturnsFalse(XType cellType)
        {
            var dungeon = new Dungeon(1, 1);
            var newLocation = new Point(0, 0);
            dungeon[newLocation].Type = cellType;

            var result = dungeon.CanMove(newLocation);

            Assert.IsFalse(result);
        }

        [Test]
        public void CanMove_GivenObstacleMonsterOnCell_ThenReturnsFalse()
        {
            var dungeon = new Dungeon(1, 1);
            var newLocation = new Point(0, 0);
            var cell = dungeon[newLocation];
            cell.Type = XType.Empty;
            cell.Content.Add(new Monster(newLocation, new MainStats(), GetAppearance(true)));

            var result = dungeon.CanMove(newLocation);

            Assert.IsFalse(result);
        }

        [Test]
        public void CanMove_GivenNonObstacleMonsterOnCell_ThenReturnsFalse()
        {
            var dungeon = new Dungeon(1, 1);
            var newLocation = new Point(0, 0);
            var cell = dungeon[newLocation];
            cell.Type = XType.Empty;
            cell.Content.Add(new Monster(newLocation, new MainStats(), GetAppearance(false)));

            var result = dungeon.CanMove(newLocation);

            Assert.IsTrue(result);
        }

        private Appearance GetAppearance(bool isObstacle)
        {
            return new Appearance("1", "2", '3', Color.AliceBlue, 
                isVisible: true, isSolid: true, isObstacle: isObstacle);
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

        [Test]
        public void GetCellContent_GivenCellWithContent_ThenReturnsAppearanceOfCellAndOfEachOfItsContent()
        {
            var dungeon = new Dungeon(1, 1);
            var location = new Point(0, 0);
            var content1 = new Door(location);
            var content2 = new Monster(location, new MainStats(), new Appearance());
            var cellWithContent = dungeon[location];
            cellWithContent.Type = XType.StairsDown;
            cellWithContent.Content.Add(content1);
            cellWithContent.Content.Add(content2);

            var result = dungeon.GetCellContent(location);

            Assert.That(result, Is.EquivalentTo(
                new[]
                {
                    dungeon[location].GetAppearance(),
                    content1.GetAppearance(),
                    content2.GetAppearance()
                }));
        }

        [TestCase(XType.Wall)]
        [TestCase(XType.StairsDown)]
        [TestCase(XType.StairsUp)]
        public void GetCellContent_GivenCell_ThenReturnsCorrectAppearance(XType type)
        {
            var dungeon = new Dungeon(1, 1);
            var location = new Point(0, 0);
            dungeon[location].Type = type;

            var result = dungeon.GetCellContent(location);
            var expectedAppearance = new Cell(new Point(), type).GetAppearance();

            Assert.That(result, Is.EquivalentTo(new[] { expectedAppearance }));
        }

        [Test]
        public void GetCellContent_GivenEmptyCell_ThenReturnsNoAppearance()
        {
            var dungeon = new Dungeon(1, 1);
            var location = new Point(0, 0);
            dungeon[location].Type = XType.Empty;

            var result = dungeon.GetCellContent(location);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void GetCellContent_GivenCharacterInEmptyCell_ThenReturnsEmpty()
        {
            var dungeon = new Dungeon(1, 1);
            var location = new Point(0, 0);
            dungeon[location].Type = XType.Empty;
            dungeon.PlaceCharacter(new Character(new MainStats(), location));

            var result = dungeon.GetCellContent(location);
            var expectedAppearance = new Character(new MainStats(), location).GetAppearance();

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void GetCellContent_GivenDoor_ThenReturnsCorrectAppearance()
        {
            var dungeonWithDoor = new Dungeon(1, 1);
            var location = new Point(0, 0);
            dungeonWithDoor[location].Type = XType.Door;
            var dungeon = new Dungeon(dungeonWithDoor);

            var result = dungeon.GetCellContent(location);
            var expectedAppearance = new Door(location).GetAppearance();

            Assert.That(result, Is.EquivalentTo(new[] { expectedAppearance }));
        }
    }
}
