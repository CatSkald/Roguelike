using System.Collections.Generic;
using System.Drawing;
using CatSkald.Roguelike.DungeonGenerator.Directions;

namespace CatSkald.Roguelike.DungeonGenerator.Maps
{
    public interface IMap : IEnumerable<Cell>
    {
        int Width { get; }
        int Height { get; }
        int Size { get; }
        bool AllVisited { get; }

        Cell this[Cell point] { get; }
        Cell this[Point point] { get; }
        Cell this[int width, int height] { get; }

        bool HasAdjacentCell(Cell cell, Dir direction);
        bool TryGetAdjacentUnvisitedCell(Cell cell, Dir direction, out Cell adjacentCell);

        void Visit(Cell currentCell);

        void CreateCorridor(Cell currentCell, Cell nextCell, Dir direction);
        void RemoveCorridor(Cell cell, Dir dir);

        Cell PickRandomCell();
        Cell PickNextRandomVisitedCell(Cell currentCell);
    }
}