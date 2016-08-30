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
        private DungeonParameters _parameters;
        private Map _map;

        [SetUp]
        public void SetUp()
        {
            _map = new Map(15, 20);
            _parameters = new DungeonParameters
            {
                CellSparseFactor = 50,
                DeadEndSparseFactor = 60,
                TwistFactor = 45,
                Height = 11,
                Width = 15,
                RoomParameters =
                {
                    Count = 2,
                    MaxHeight = 5,
                    MaxWidth = 5,
                    MinHeight = 5,
                    MinWidth = 4
                }
            };
        }

        #region Execute
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

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(3)]
        public void Execute_ShouldGenerateCorrectNumberOfRooms(int count)
        {
            _parameters.RoomParameters.Count = count;

            var command = new PlaceRoomsCommand(_parameters);
            command.Execute(_map);

            Assert.That(_map.Rooms, Has.Count.EqualTo(count));
        }

        [TestCase(3, 3, 3, 3)]
        [TestCase(2, 4, 2, 4)]
        [TestCase(5, 6, 2, 2)]
        public void Execute_ShouldGenerateRoomsWithCorrectSize(
            int minW, int maxW, int minH, int maxH)
        {
            _parameters.RoomParameters.MinHeight = minH;
            _parameters.RoomParameters.MaxHeight = maxH;
            _parameters.RoomParameters.MinWidth = minW;
            _parameters.RoomParameters.MaxWidth = maxW;

            var command = new PlaceRoomsCommand(_parameters);
            command.Execute(_map);

            Assert.That(_map.Rooms,
                Has.All.Matches<Room>(r => r.Width <= maxW && r.Width >= minW
                && r.Height <= maxH && r.Height >= minH));
        } 
        #endregion

        #region Constructor_ValidateParameters
        [Test]
        public void Constructor_ValidateParameters_Succeeds_IfParametersOK()
        {
            var map = new Map(5, 5);
            var parameters = new DungeonParameters
            {
                CellSparseFactor = 50,
                DeadEndSparseFactor = 60,
                TwistFactor = 45,
                Height = 11,
                Width = 15,
                RoomParameters =
                {
                    Count = 2,
                    MaxHeight = 5,
                    MaxWidth = 5,
                    MinHeight = 5,
                    MinWidth = 4
                }
            };

            Assert.That(() => new PlaceRoomsCommand(parameters), Throws.Nothing);
        }

        [Test]
        public void Constructor_ValidateParameters_Throws_IfParametersNull()
        {
            var map = new Map(5, 5);
            DungeonParameters parameters = null;

            Assert.That(() => new PlaceRoomsCommand(parameters),
                Throws.ArgumentNullException);
        }

        [Test]
        public void Constructor_ValidateParameters_Throws_IfRoomParametersNull()
        {
            var map = new Map(5, 5);
            var parameters = new DungeonParameters
            {
                RoomParameters = null
            };

            Assert.That(() => new PlaceRoomsCommand(parameters),
                Throws.ArgumentNullException);
        }

        [TestCase(-10)]
        [TestCase(-1)]
        public void Constructor_ValidateParameters_Throws_IfRoomCount(int count)
        {
            var map = new Map(5, 5);
            var parameters = new DungeonParameters
            {
                RoomParameters =
                {
                    Count = count
                }
            };

            Assert.That(() => new PlaceRoomsCommand(parameters),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                    .With.Property(nameof(ArgumentOutOfRangeException.ParamName))
                    .EqualTo(nameof(RoomParameters.Count)));
        }

        [TestCase(-10)]
        [TestCase(-1)]
        public void Constructor_ValidateParameters_Throws_IfRoomCountNegative(int count)
        {
            var map = new Map(5, 5);
            var parameters = new DungeonParameters
            {
                RoomParameters =
                {
                    Count = count
                }
            };

            Assert.That(() => new PlaceRoomsCommand(parameters),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                    .With.Property(nameof(ArgumentOutOfRangeException.ParamName))
                    .EqualTo(nameof(RoomParameters.Count)));
        }
        #endregion

        ////TODO Validation
        ////TODO RoomsAreNotIntersect
    }
}
