using System;
using System.Drawing;
using CatSkald.Roguelike.DungeonGenerator.Directions;
using CatSkald.Roguelike.DungeonGenerator.Maps;

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
                throw new ArgumentOutOfRangeException(
                    name, point, name + " is outside the map.");
            }
        }
        
        public static void IfOutsideMap(IMap map, Cell cell, string name = "cell")
        {
            IfOutsideMap(map, cell.Location, name);
        }

        public static void IfNoCorridor(Cell cell, Dir direction, string name = "cell")
        {
            if (cell.Sides[direction] != Side.Empty)
            {
                throw new InvalidOperationException(
                    $"{name} has no corridor in direction '{direction}'.");
            }
        }

        public static void IfNotAdjacent(Cell startCell, Cell endCell, Dir direction)
        {
            var endPoint = DirHelper.MoveInDir(startCell.Location, direction);
            if (endPoint != endCell.Location)
            {
                throw new ArgumentException(
                    $"{startCell.Location} and {endCell.Location} are not adjacent in direction '{direction}'.");
            }
        }
#pragma warning restore CC0021 // Use nameof
    }
}
