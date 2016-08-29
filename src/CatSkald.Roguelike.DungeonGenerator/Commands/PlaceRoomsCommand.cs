using System;
using System.Drawing;
using System.Linq;
using CatSkald.Roguelike.DungeonGenerator.Maps;
using CatSkald.Tools;

namespace CatSkald.Roguelike.DungeonGenerator.Commands
{
    public sealed class PlaceRoomsCommand : IMapBuilderCommand
    {
        private const int adjacentCorridorBonus = 1;
        private const int overlappedCorridorBonus = 3;
        private const int overlappedRoomBonus = 100;

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
            int bestRoomScore = short.MaxValue;
            Room roomToPlace = null;
            Cell cellToPlace = null;
            var success = false;
            foreach (var room in rooms)
            {
                foreach (var cell in map)
                {
                    var newBounds = new Rectangle(
                        cell.Location.X,
                        cell.Location.Y,
                        room.Width + cell.Location.X,
                        room.Height + cell.Location.Y);
                    if (!map.Bounds.Contains(newBounds))
                        continue;

                    var roomScore = CalculateRoomScore(room, map, cell);
                    success = bestRoomScore > roomScore;
                    if (success)
                    {
                        roomToPlace = room;
                        cellToPlace = cell;
                        bestRoomScore = roomScore;
                    }
                }

                if (roomToPlace != null && cellToPlace != null)
                {
                    map.InsertRoom(roomToPlace, cellToPlace.Location);

                    success = false;
                    roomToPlace = null;
                    cellToPlace = null;
                    bestRoomScore = short.MaxValue;
                }
            }
        }

        private static int CalculateRoomScore(Room room, IMap map, Cell cell)
        {
            int bestScore = short.MaxValue;

            var currentScore = 0;
            foreach (var roomCell in room)
            {
                var currentCell = map[
                    roomCell.Location.X + cell.Location.X,
                    roomCell.Location.Y + cell.Location.Y];

                currentScore += adjacentCorridorBonus
                    * roomCell.Sides
                        .Where(s => s.Value == Side.Wall)
                        .Count(s => currentCell.Sides[s.Key] == Side.Empty);

                currentScore += overlappedCorridorBonus
                    * currentCell.Sides.Count(it => it.Value == Side.Empty);

                currentScore += overlappedRoomBonus
                    * map.Rooms.Count(r => r.Bounds.Contains(currentCell.Location));
            }

            return Math.Min(currentScore, bestScore);
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
