using System.Drawing;
using System.Linq;
using CatSkald.Roguelike.Core.Objects;
using CatSkald.Roguelike.Core.Terrain;
using CatSkald.Roguelike.DungeonGenerator.Terrain;

namespace CatSkald.Roguelike.DungeonGenerator.Services
{
    public class MapConverter : IMapConverter
    {
        public IDungeon ConvertToDungeon(IMap map)
        {
            var dungeon = new Dungeon(map.Width * 2 + 1, map.Height * 2 + 1);

            for (var x = 0; x < map.Width * 2 + 1; x++)
            {
                for (var y = 0; y < map.Height * 2 + 1; y++)
                {
                    dungeon[x, y].Type = XType.Wall;
                }
            }

            foreach (var room in map.Rooms)
            {
                var roomMinPoint = new Point(
                    room.Bounds.Location.X * 2 + 1,
                    room.Bounds.Location.Y * 2 + 1);
                var roomMaxPoint = new Point(
                    room.Bounds.Right * 2,
                    room.Bounds.Bottom * 2);

                // Fill tiles with corridor values for each room in dungeon
                for (var i = roomMinPoint.X; i < roomMaxPoint.X; i++)
                    for (var j = roomMinPoint.Y; j < roomMaxPoint.Y; j++)
                        dungeon[i, j].Type = XType.Empty;
            }

            // Expand it each corridor cell
            foreach (var cell in map.Where(c => c.IsCorridor))
            {
                var location = new Point(
                    cell.Location.X * 2 + 1,
                    cell.Location.Y * 2 + 1);

                dungeon[location.X, location.Y].Type = XType.Empty;

                switch (map[cell.Location].Sides[Dir.N])
                {
                    case Side.Empty:
                        dungeon[location.X, location.Y - 1].Type = XType.Empty;
                        break;
                    case Side.Door:
                        dungeon[location.X, location.Y - 1].Type = XType.DoorClosed;
                        break;
                }

                switch (map[cell.Location].Sides[Dir.S])
                {
                    case Side.Empty:
                        dungeon[location.X, location.Y + 1].Type = XType.Empty;
                        break;
                    case Side.Door:
                        dungeon[location.X, location.Y + 1].Type = XType.DoorClosed;
                        break;
                }

                switch (map[cell.Location].Sides[Dir.W])
                {
                    case Side.Empty:
                        dungeon[location.X - 1, location.Y].Type = XType.Empty;
                        break;
                    case Side.Door:
                        dungeon[location.X - 1, location.Y].Type = XType.DoorClosed;
                        break;
                }

                switch (map[cell.Location].Sides[Dir.E])
                {
                    case Side.Empty:
                        dungeon[location.X + 1, location.Y].Type = XType.Empty;
                        break;
                    case Side.Door:
                        dungeon[location.X + 1, location.Y].Type = XType.DoorClosed;
                        break;
                }
            }

            return dungeon;
        }
    }
}
