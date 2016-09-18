using System.Collections.Generic;
using System.Drawing;
using CatSkald.Roguelike.Core.Terrain;

namespace CatSkald.Roguelike.DungeonGenerator.Terrain
{
    public interface IMap : IEnumerable<MapCell>
    {
        int Width { get; }
        int Height { get; }
        int Size { get; }
        Rectangle Bounds { get; }

        bool AllVisited { get; }
        IReadOnlyCollection<Room> Rooms { get; }

        MapCell this[MapCell point] { get; }
        MapCell this[Point point] { get; }
        MapCell this[int width, int height] { get; }

        bool HasAdjacentCell(MapCell cell, Dir direction);
        bool TryGetAdjacentCell(MapCell cell, Dir direction, out MapCell adjacentCell);

        void Visit(MapCell currentCell);

        void CreateCorridorSide(MapCell currentCell, MapCell nextCell, Dir direction, Side side);
        void CreateWall(MapCell cell, Dir dir);

        void InsertRoom(Room room, Point position);

        MapCell PickRandomCell();
        MapCell PickNextRandomVisitedCell(MapCell currentCell);
    }
}