using System;
using System.Drawing;
using CatSkald.Roguelike.DungeonGenerator.Directions;
using CatSkald.Roguelike.DungeonGenerator.Maps;
using CatSkald.Tools;

namespace CatSkald.Roguelike.DungeonGenerator.Utils
{
    /// <summary>
    /// DungeonGenerator specific validations
    /// </summary>
    public static class ThrowD
    {
#pragma warning disable CC0021 // Use nameof
        public static void IfOutsideMap(IMap map, Point point, string name = "point")
        {
            if (point.X < 0 || point.X >= map.Width
                || point.Y < 0 || point.Y >= map.Height)
            {
                throw new ArgumentOutOfRangeException(name, point, 
                    name + " is outside the map. Map bounds: " + map.Bounds);
            }
        }
        
        public static void IfOutsideMap(IMap map, Cell cell, string name = "cell")
        {
            Throw.IfNull(cell, name);

            IfOutsideMap(map, cell.Location, name);
        }
        
        public static void IfOutsideMap(IMap map, Room room, string name = "room")
        {
            Throw.IfNull(room, name);

            if (!map.Bounds.Contains(room.Bounds))
            {
                throw new ArgumentOutOfRangeException(name, room.Bounds, 
                    name + " is outside the map. Map bounds: " + map.Bounds);
            }
        }

        public static void IfNoCorridor(Cell cell, Dir direction, string name = "cell")
        {
            Throw.IfNull(cell, name);

            if (cell.Sides[direction] == Side.Wall)
            {
                throw new InvalidOperationException(
                    $"{name} has no corridor in direction '{direction}'.");
            }
        }

        public static void IfNotAdjacent(Cell startCell, Cell endCell, Dir direction)
        {
            Throw.IfNull(startCell, nameof(startCell));
            Throw.IfNull(endCell, nameof(endCell));

            var endPoint = DirHelper.MoveInDir(startCell.Location, direction);
            if (endPoint != endCell.Location)
            {
                throw new InvalidOperationException(
                    $"{startCell.Location} and {endCell.Location} are not adjacent in direction '{direction}'.");
            }
        }
#pragma warning restore CC0021 // Use nameof
    }
}
