using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CatSkald.Roguelike.Core.Terrain;
using CatSkald.Roguelike.DungeonGenerator.Terrain;
using CatSkald.Roguelike.Core.Parameters;
using CatSkald.Tools;

namespace CatSkald.Roguelike.DungeonGenerator.Commands
{
    internal sealed class PlaceRoomsCommand : AbstractMapBuilderCommand
    {
        private const int AdjacentCorridorBonus = 1;
        private const int OverlappedCorridorBonus = 3;
        private const int OverlappedRoomBonus = 100;

        protected override void ExecuteCommand(
            IMap map, MapParameters parameters)
        {
            var rooms = CreateRooms(parameters.Room);
            foreach (var room in rooms)
            {
                PlaceRoom(map, room);
            }
        }

        protected override void ValidateParameters(MapParameters parameters)
        {
            var roomParameters = parameters.Room;

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

        private static void PlaceRoom(IMap map, Room room)
        {
            int bestRoomScore = short.MaxValue;
            MapCell cellToPlace = null;

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

        private static int CalculateRoomScore(Room room, IMap map, MapCell cell)
        {
            var currentScore = 0;

            foreach (var roomCell in room)
            {
                var currentCell = map[
                    roomCell.Location.X + cell.Location.X,
                    roomCell.Location.Y + cell.Location.Y];

                currentScore += AdjacentCorridorBonus
                    * roomCell.Sides
                        .Count(s => HasAdjacentCorridor(map, currentCell, s.Key));

                if (currentCell.IsCorridor)
                {
                    currentScore += OverlappedCorridorBonus;
                }

                currentScore += OverlappedRoomBonus
                    * map.Rooms.Count(r => r.Bounds.Contains(currentCell.Location));
            }

            return currentScore;
        }

        private static bool HasAdjacentCorridor(
            IMap map, MapCell currentCell, Dir dir)
        {
            MapCell adjacentCell;
            return map.TryGetAdjacentCell(currentCell, dir, out adjacentCell)
                && adjacentCell.Sides[dir] != Side.Wall;
        }

        private static List<Room> CreateRooms(RoomParameters parameters)
        {
            var rooms = Enumerable
                .Repeat<Func<Room>>(() => CreateRoom(parameters), parameters.Count)
#pragma warning disable CC0031 // Check for null before calling a delegate
                .Select(f => f())
#pragma warning restore CC0031 // Check for null before calling a delegate
                .ToList();

            return rooms;
        }

        private static Room CreateRoom(RoomParameters parameters)
        {
            return new Room(
                StaticRandom.NextInclusive(parameters.MinWidth, parameters.MaxWidth),
                StaticRandom.NextInclusive(parameters.MinHeight, parameters.MaxHeight));
        }
    }
}
