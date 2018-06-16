using System.Drawing;
using CatSkald.Roguelike.Core.Cells;
using NUnit.Framework;

namespace CatSkald.Roguelike.Test.Core.UnitTests.Cells
{
    internal sealed class DoorTests
    {
        [Test]
        public void Ctor_SetsTypeToDoor()
        {
            var door = new Door(new Point());

            Assert.That(door.Type, Is.EqualTo(XType.Door));
        }

        [Test]
        public void Ctor_SetsIsOpenedToFalse()
        {
            var door = new Door(new Point());

            Assert.False(door.IsOpened);
        }

        [Test]
        public void Open_GivenClosedDoor_ThenChangesTypeToOpened()
        {
            var door = new Door(new Point());

            door.Open();

            Assert.True(door.IsOpened);
        }
    }
}
