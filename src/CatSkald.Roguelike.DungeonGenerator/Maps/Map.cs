using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using CatSkald.Roguelike.DungeonGenerator.Directions;
using CatSkald.Roguelike.DungeonGenerator.Utils;
using CatSkald.Tools;

namespace CatSkald.Roguelike.DungeonGenerator.Maps
{
    [DebuggerDisplay("[{Width},{Height}](AllVisited:{AllVisited})")]
    public sealed class Map : IMap
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

        public Cell this[Cell cell] => this[cell.Location];
        public Cell this[Point p] => this[p.X, p.Y];
        public Cell this[int width, int height]
        {
            get
            {
                return _map[height, width];
            }
            private set
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
                throw new InvalidOperationException("No visited cells to choose.");

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
            } while (true);

            return next;
        }

        public bool HasAdjacentCell(Cell cell, Dir direction)
        {
            ThrowD.IfOutsideMap(this, cell);

            var newPoint = DirHelper.MoveInDir(cell.Location, direction);
            return !IsOutsideMap(newPoint);
        }

        public bool TryGetAdjacentUnvisitedCell(
            Cell cell, Dir direction, out Cell adjacentCell)
        {
            ThrowD.IfOutsideMap(this, cell);

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
            ThrowD.IfOutsideMap(this, startCell, nameof(startCell));
            ThrowD.IfOutsideMap(this, endCell, nameof(endCell));
            ThrowD.IfNotAdjacent(startCell, endCell, direction);

            startCell.Sides[direction] = Side.Empty;
            endCell.Sides[DirHelper.Opposite(direction)] = Side.Empty;
        }

        public void RemoveCorridor(Cell startCell, Dir direction)
        {
            ThrowD.IfNoCorridor(startCell, direction, nameof(startCell));

            var endCell = this[DirHelper.MoveInDir(startCell.Location, direction)];
            var endDirection = direction.Opposite();

            ThrowD.IfNoCorridor(endCell, endDirection, nameof(endCell));

            startCell.Sides[direction] = Side.Wall;
            endCell.Sides[endDirection] = Side.Wall;
        }

        public void Visit(Cell cell)
        {
            ThrowD.IfOutsideMap(this, cell);

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

        private bool IsOutsideMap(Point point)
        {
            return point.X < 0 || point.X >= Width || point.Y < 0 || point.Y >= Height;
        }
    }
}
