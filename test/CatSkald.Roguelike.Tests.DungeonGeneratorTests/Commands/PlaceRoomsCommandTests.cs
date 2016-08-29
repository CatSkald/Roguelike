using System;
using CatSkald.Roguelike.DungeonGenerator;
using CatSkald.Roguelike.DungeonGenerator.Commands;
using CatSkald.Roguelike.DungeonGenerator.Maps;
using NUnit.Framework;

namespace CatSkald.Roguelike.Tests.DungeonGeneratorTests.Commands
{
    [TestFixture]
    public class PlaceRoomsCommandTests
    {
        [Test]
        public void Execute_ShouldThrow_IfMapNull()
        {
            Map map = null;
            var parameters = new DungeonParameters
            {
                RoomParameters = new RoomParameters()
            };
            var command = new PlaceRoomsCommand(parameters);

            Assert.That(() => command.Execute(map),
                Throws.ArgumentNullException);
        }

        #region Constructor
        [Test]
        public void Constructor_ShouldThrow_IfParametersNull()
        {
            var map = new Map(5, 5);
            DungeonParameters parameters = null;

            Assert.That(() => new PlaceRoomsCommand(parameters),
                Throws.ArgumentNullException);
        }

        [Test]
        public void Constructor_ShouldThrow_IfRoomParametersNull()
        {
            var map = new Map(5, 5);
            var parameters = new DungeonParameters
            {
                RoomParameters = null
            };

            Assert.That(() => new PlaceRoomsCommand(parameters),
                Throws.ArgumentNullException);
        }
        #endregion

        ////TODO Validation
        ////TODO RoomCountIsCorrect
        ////TODO RoomsAreNotIntersect
        ////TODO RoomSizeIsCorrect (including equal w/h)
    }
}
