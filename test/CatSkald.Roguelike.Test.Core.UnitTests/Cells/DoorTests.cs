using CatSkald.Roguelike.Core.Cells;
using NUnit.Framework;

namespace CatSkald.Roguelike.Test.Core.UnitTests.Cells
{
    internal sealed class DoorTests
    {
        [Test]
        public void Ctor_SetsTypeToDoorClosed()
        {
            var door = new Door();

            Assert.That(door.Type, Is.EqualTo(XType.DoorClosed));
        }

        [Test]
        public void IsOpened_GivenClosedDoor_ThenReturnsFalse()
        {
            var door = new Door
            {
                Type = XType.DoorClosed
            };

            Assert.That(door.IsOpened, Is.False);
        }

        [Test]
        public void IsOpened_GivenOpenedDoor_ThenReturnsTrue()
        {
            var door = new Door
            {
                Type = XType.DoorOpened
            };

            Assert.That(door.IsOpened, Is.True);
        }

        [Test]
        public void Open_GivenClosedDoor_ThenChangesTypeToOpened()
        {
            var door = new Door
            {
                Type = XType.DoorClosed
            };

            door.Open();

            Assert.That(door.Type, Is.EqualTo(XType.DoorOpened));
        }
    }
}
