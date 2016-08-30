using System;
using System.Collections.Generic;
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
            PlaceRooms(map, rooms);
        }

        private static void PlaceRooms(IMap map, List<Room> rooms)
        {
            foreach (var room in rooms)
            {
                int bestRoomScore = short.MaxValue;
                Cell cellToPlace = null;

                foreach (var cell in map)
                {
                    // place rooms only in corridors to ensure it has exit
                    if (!cell.IsCorridor)
                        continue;

                    var newBounds = new Rectangle(
                        cell.Location.X,
                        cell.Location.Y,
                        room.Width,
                        room.Height);
                    if (!map.Bounds.Contains(newBounds))
                        continue;

                    var roomScore = CalculateRoomScore(room, map, cell);
                    if (bestRoomScore > roomScore)
                    {
                        cellToPlace = cell;
                        bestRoomScore = roomScore;
                    }
                }

                if (cellToPlace != null)
                {
                    map.InsertRoom(room, cellToPlace.Location);
                }
            }
        }

        private static int CalculateRoomScore(Room room, IMap map, Cell cell)
        {
            var currentScore = 0;

            foreach (var roomCell in room)
            {
                var currentCell = map[
                    roomCell.Location.X + cell.Location.X,
                    roomCell.Location.Y + cell.Location.Y];

                currentScore += adjacentCorridorBonus
                    * roomCell.Sides
                        .Count(s =>
                        {
                            Cell adjacentCell;
                            return map.TryGetAdjacentCell(
                                currentCell, s.Key, out adjacentCell)
                                && adjacentCell.Sides[s.Key] == Side.Empty;
                        });

                if (currentCell.IsCorridor)
                {
                    currentScore += overlappedCorridorBonus;
                }

                currentScore += overlappedRoomBonus
                    * map.Rooms.Count(r => r.Bounds.Contains(currentCell.Location));
            }

            return currentScore;
        }

        private List<Room> CreateRooms()
        {
            var rooms = Enumerable
                .Repeat<Func<Room>>(CreateRoom, _params.Count)
                #pragma warning disable CC0031 // Check for null before calling a delegate
                .Select(f => f())
                #pragma warning restore CC0031 // Check for null before calling a delegate
                .ToList();

            return rooms;
        }

        private Room CreateRoom()
        {
            return new Room(
                StaticRandom.NextInclusive(_params.MinWidth, _params.MaxWidth),
                StaticRandom.NextInclusive(_params.MinHeight, _params.MaxHeight));
        }

        private static void ValidateParameters(DungeonParameters parameters)
        {
            Throw.IfNull(parameters, nameof(parameters));

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
