using System;
using System.Linq;
using CatSkald.Roguelike.DungeonGenerator.Maps;
using CatSkald.Tools;

namespace CatSkald.Roguelike.DungeonGenerator.Commands
{
    public sealed class PlaceRoomsCommand : IMapBuilderCommand
    {
        private RoomParameters _params;

        public PlaceRoomsCommand(DungeonParameters parameters)
        {
            ValidateParameters(parameters);

            _params = parameters.RoomParameters;
        }

        public void Execute(IMap map)
        {
            Throw.IfNull(map, nameof(map));

            var rooms = CreateRooms();
            map.SetRooms(rooms);
        }

        private Room[] CreateRooms()
        {
            var rooms = Enumerable
                .Repeat<Func<Room>>(CreateRoom, _params.Count)
                #pragma warning disable CC0031 // Check for null before calling a delegate
                .Select(f => f())
                #pragma warning restore CC0031 // Check for null before calling a delegate
                .ToArray();

            return rooms;
        }

        private Room CreateRoom()
        {
            return new Room(
                StaticRandom.Next(_params.MinWidth, _params.MaxWidth),
                StaticRandom.Next(_params.MinHeight, _params.MaxHeight));
        }

        private static void ValidateParameters(DungeonParameters parameters)
        {
            var roomParameters = parameters.RoomParameters;

            Throw.IfNull(roomParameters, nameof(roomParameters));

            Throw.IfLess(0, roomParameters.Count, nameof(roomParameters.Count));

            Throw.IfLess(0, roomParameters.MinWidth, nameof(roomParameters.MinWidth));
            Throw.IfLess(0, roomParameters.MinHeight, nameof(roomParameters.MinHeight));

            Throw.IfLess(roomParameters.MinWidth, roomParameters.MaxWidth,
                nameof(roomParameters.MaxWidth));
            Throw.IfLess(roomParameters.MinHeight, roomParameters.MaxHeight,
                nameof(roomParameters.MaxHeight));

            Throw.IfGreater(parameters.Width, roomParameters.MaxWidth,
                nameof(roomParameters.MaxWidth));
            Throw.IfGreater(parameters.Height, roomParameters.MaxHeight,
                nameof(roomParameters.MaxHeight));
        }
    }
}
