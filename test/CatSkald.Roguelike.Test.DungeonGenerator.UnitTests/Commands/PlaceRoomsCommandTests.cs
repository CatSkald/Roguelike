using System;
using System.Drawing;
using System.Linq;
using CatSkald.Roguelike.DungeonGenerator.Commands;
using CatSkald.Roguelike.DungeonGenerator.Services;
using CatSkald.Roguelike.DungeonGenerator.Terrain;
using CatSkald.Roguelike.Core.Parameters;
using NUnit.Framework;

namespace CatSkald.Roguelike.Test.DungeonGenerator.UnitTests.Commands
{
    [TestFixture]
    public class PlaceRoomsCommandTests
    {
        private DungeonParameters _parameters;
        private Map _map;
        private PlaceRoomsCommand _command;

        [SetUp]
        public void SetUp()
        {
            _map = new Map(15, 20);
            _parameters = new DungeonParameters
            {
                CellSparseFactor = 50,
                DeadEndSparseFactor = 60,
                TwistFactor = 45,
                Height = 20,
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
            _command = new PlaceRoomsCommand();

            var corridorBuilderCommand = new CorridorBuilderCommand(
                new DirectionPicker(_parameters.TwistFactor));
            corridorBuilderCommand.Execute(_map, _parameters);
        }
        
        #region Execute
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(3)]
        public void Execute_ShouldGenerateCorrectNumberOfRooms(int count)
        {
            _parameters.RoomParameters.Count = count;

            _command.Execute(_map, _parameters);

            Assert.That(_map.Rooms, Has.Count.EqualTo(count));
        }
        
        [Test]
        public void Execute_GeneratedRooms_DoNotIntersect()
        {
            _map = new Map(75, 80);

            _parameters.RoomParameters.Count = 8;

            _command.Execute(_map, _parameters);

            var bounds = _map.Rooms.Select(r => r.Bounds).ToList();

            Assert.That(bounds, Is.Unique
                .And.All.Matches<Rectangle>(current => bounds
                .Where(b => b != current)
                .All(b => !b.IntersectsWith(current))));
        }

        [Test]
        public void Execute_GeneratedRooms_HasDoors()
        {
            _map = new Map(75, 80);
            _parameters.RoomParameters.Count = 8;

            _command.Execute(_map, _parameters);

            var rooms = _map.Rooms.ToList();

            Assert.That(rooms, Has.All.With.Some
                .Matches<MapCell>(c => c.Sides.Any(s => s.Value == Side.Door)));
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

            _command.Execute(_map, _parameters);

            Assert.That(_map.Rooms,
                Has.All.Matches<Room>(r =>
                r.Width <= maxW
                && r.Width >= minW
                && r.Height <= maxH
                && r.Height >= minH));
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
            Assert.That(() => _command.Execute(map, parameters), Throws.Nothing);
        }

        [TestCase(-10)]
        [TestCase(-1)]
        public void Constructor_ValidateParameters_Throws_IfMinWidthNegative(
            int value)
        {
            var map = new Map(5, 5);
            var parameters = new DungeonParameters
            {
                RoomParameters =
                {
                    MinWidth = value
                }
            };

            Assert.That(() => _command.Execute(map, parameters),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                    .With.Property(nameof(ArgumentOutOfRangeException.ParamName))
                    .EqualTo(nameof(RoomParameters.MinWidth)));
        }
        
        [TestCase(-10)]
        [TestCase(-1)]
        public void Constructor_ValidateParameters_Throws_IfMinHeightNegative(int value)
        {
            var map = new Map(5, 5);
            var parameters = new DungeonParameters
            {
                RoomParameters =
                {
                    MinHeight = value
                }
            };

            Assert.That(() => _command.Execute(map, parameters),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                    .With.Property(nameof(ArgumentOutOfRangeException.ParamName))
                    .EqualTo(nameof(RoomParameters.MinHeight)));
        }
        
        [TestCase(10, 6)]
        [TestCase(4, 1)]
        public void Constructor_ValidateParameters_Throws_IfMaxHeightLessMin(
            int min, int max)
        {
            var map = new Map(5, 5);
            var parameters = new DungeonParameters
            {
                RoomParameters =
                {
                    MinHeight = min,
                    MaxHeight = max
                }
            };

            Assert.That(() => _command.Execute(map, parameters),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                    .With.Property(nameof(ArgumentOutOfRangeException.ParamName))
                    .EqualTo(nameof(RoomParameters.MaxHeight)));
        }
        
        [TestCase(10, 6)]
        [TestCase(4, 1)]
        public void Constructor_ValidateParameters_Throws_IfMaxWidthLessMin(
            int min, int max)
        {
            var map = new Map(5, 5);
            var parameters = new DungeonParameters
            {
                RoomParameters =
                {
                    MinWidth = min,
                    MaxWidth = max
                }
            };

            Assert.That(() => _command.Execute(map, parameters),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                    .With.Property(nameof(ArgumentOutOfRangeException.ParamName))
                    .EqualTo(nameof(RoomParameters.MaxWidth)));
        }
        
        [TestCase(4, 3)]
        [TestCase(44, 23)]
        public void Constructor_ValidateParameters_Throws_IfMaxWidthGreaterThanWidth(
            int maxW, int w)
        {
            var map = new Map(5, 5);
            var parameters = new DungeonParameters
            {
                Width = w,
                RoomParameters =
                {
                    MaxWidth = maxW
                }
            };

            Assert.That(() => _command.Execute(map, parameters),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                    .With.Property(nameof(ArgumentOutOfRangeException.ParamName))
                    .EqualTo(nameof(RoomParameters.MaxWidth)));
        }
        
        [TestCase(13, 12)]
        [TestCase(31, 21)]
        public void Constructor_ValidateParameters_Throws_IfMaxHeightGreaterThanHeight(
            int maxH, int h)
        {
            var map = new Map(5, 5);
            var parameters = new DungeonParameters
            {
                Width = h,
                RoomParameters =
                {
                    MaxHeight = maxH
                }
            };

            Assert.That(() => _command.Execute(map, parameters),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                    .With.Property(nameof(ArgumentOutOfRangeException.ParamName))
                    .EqualTo(nameof(RoomParameters.MaxHeight)));
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

            Assert.That(() => _command.Execute(map, parameters),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                    .With.Property(nameof(ArgumentOutOfRangeException.ParamName))
                    .EqualTo(nameof(RoomParameters.Count)));
        }
        #endregion
    }
}
