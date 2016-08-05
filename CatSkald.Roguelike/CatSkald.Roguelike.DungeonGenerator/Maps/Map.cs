using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using CatSkald.Roguelike.DungeonGenerator.Directions;
using CatSkald.Stuff;

namespace CatSkald.Roguelike.DungeonGenerator.Maps
{
    [DebuggerDisplay("[{Width},{Height}](AllVisited:{AllVisited})")]
    public class Map : IMap
    {
        private readonly Cell[,] _map;
        private readonly List<Cell> _visitedCells;

        public Map(int width, int height)
        {
            _map = new Cell[height, width];

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    this[x, y] = new Cell(x, y);
                }

            _visitedCells = new List<Cell>(Size);
        }

        public int Width => _map.GetUpperBound(1) + 1;
        public int Height => _map.GetUpperBound(0) + 1;
        public int Size => Height * Width;
        public bool AllVisited => _visitedCells.Count == Size;

        public Cell this[Cell cell]
        {
            get
            {
                return this[cell.Location];
            }
            set
            {
                this[cell.Location] = value;
            }
        }
        public Cell this[Point p]
        {
            get
            {
                return this[p.X, p.Y];
            }
            set
            {
                this[p.X, p.Y] = value;
            }
        }
        public Cell this[int width, int height]
        {
            get
            {
                return _map[height, width];
            }
            set
            {
                _map[height, width] = value;
            }
        }

        public Cell PickRandomCell()
        {
            var point = new Point(StaticRandom.Next(Width), StaticRandom.Next(Height));
            return this[point];
        }
        
        public Cell PickNextRandomVisitedCell(Cell oldCell)
        {
            if (_visitedCells.Count <= 1)
            {
                throw new InvalidOperationException("No visited cells to choose.");
            }

            if (_visitedCells.Count == 2)
            {
                return _visitedCells.Single(c => c != oldCell);
            }

            Cell next;
            var maxIndex = _visitedCells.Count - 1;
            do
            {
                var index = StaticRandom.Next(maxIndex);
                next = _visitedCells[index];
                if (next != oldCell)
                    break;
            }
            while (true);

            return next;
        }

        public bool HasAdjacentCell(Cell cell, Dir direction)
        {
            ThrowIfOutsideMap(cell.Location);

            var newPoint = DirHelper.MoveInDir(cell.Location, direction);
            return !IsOutsideMap(newPoint);
        }

        public bool TryGetAdjacentUnvisitedCell(Cell cell, Dir direction, out Cell adjacentCell)
        {
            ThrowIfOutsideMap(cell.Location);

            var newPoint = DirHelper.MoveInDir(cell.Location, direction);

            if (IsOutsideMap(newPoint) || this[newPoint].IsVisited)
            {
                adjacentCell = null;
                return false;
            }

            adjacentCell = this[newPoint];
            return true;
        }

        public void CreateCorridor(Cell startCell, Cell endCell, Dir direction)
        {
            ThrowIfOutsideMap(startCell.Location);
            ThrowIfOutsideMap(endCell.Location);

            ValidateIfCellsAreAdjacentInDirection(startCell, endCell, direction);

            startCell.Sides[direction] = Side.Empty;
            endCell.Sides[DirHelper.Opposite(direction)] = Side.Empty;
        }

        public void Visit(Cell cell)
        {
            ThrowIfOutsideMap(cell.Location);

            cell.IsVisited = true;
            _visitedCells.Add(cell);
        }

        #region IEnumerable

        public IEnumerator<Cell> GetEnumerator()
        {
            return MapTraverse().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private IEnumerable<Cell> MapTraverse()
        {
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    yield return this[x, y];
                }
        }

        #endregion

        private static void ValidateIfCellsAreAdjacentInDirection(
            Cell startCell, Cell endCell, Dir direction)
        {
            var endPoint = DirHelper.MoveInDir(startCell.Location, direction);
            if (endPoint != endCell.Location)
            {
                throw new ArgumentException(
                    $"Cells are not adjucent in '{direction}' direction:" +
                    $" {startCell.Location}, {endCell.Location}");
            }
        }

        private bool IsOutsideMap(Point point)
        {
            return point.X < 0 || point.X >= Width || point.Y < 0 || point.Y >= Height;
        }

        private void ThrowIfOutsideMap(Point point)
        {
            if (IsOutsideMap(point))
                throw new ArgumentOutOfRangeException(nameof(point), point, $"Point is outside the map.");
        }
    }
}
