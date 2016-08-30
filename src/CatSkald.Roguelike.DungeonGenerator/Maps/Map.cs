using System;
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
    public sealed class Map : CellContainer, IMap
    {
        private readonly List<Cell> _visitedCells;
        private readonly List<Room> _rooms;

        public Map(int width, int height) : base(width, height)
        {
            _visitedCells = new List<Cell>(Size);
            _rooms = new List<Room>();
        }

        public bool AllVisited => _visitedCells.Count == Size;
        public IReadOnlyCollection<Room> Rooms
        {
            get { return _rooms.AsReadOnly(); }
        }

        public Cell PickRandomCell()
        {
            var point = new Point(
                StaticRandom.Next(Width), 
                StaticRandom.Next(Height));
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

        public bool TryGetAdjacentCell(
            Cell cell, Dir direction, out Cell adjacentCell)
        {
            ThrowD.IfOutsideMap(this, cell);

            var newPoint = DirHelper.MoveInDir(cell.Location, direction);

            if (IsOutsideMap(newPoint))
            {
                adjacentCell = null;
                return false;
            }

            adjacentCell = this[newPoint];
            return true;
        }
        
        public void CreateSide(Cell startCell, Cell endCell, Dir direction, Side side)
        {
            ThrowD.IfOutsideMap(this, startCell, nameof(startCell));
            ThrowD.IfOutsideMap(this, endCell, nameof(endCell));
            ThrowD.IfNotAdjacent(startCell, endCell, direction);

            startCell.Sides[direction] = side;
            endCell.Sides[DirHelper.Opposite(direction)] = side;
        }

        public void CreateWall(Cell startCell, Dir direction)
        {
            ThrowD.IfOutsideMap(this, startCell, nameof(startCell));
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

        public void InsertRoom(Room room, Point position)
        {
            ThrowD.IfOutsideMap(this, position);

            room.Offset(position);
            ThrowD.IfOutsideMap(this, room);

            _rooms.Add(room);

            foreach (var cell in room)
            {
                var newLocation = new Point(
                    cell.Location.X + position.X,
                    cell.Location.Y + position.Y);
                var mapCell = this[newLocation.X, newLocation.Y];

                CreateBorderIfNeeded(cell.Location.X == 0, Dir.W, mapCell);
                CreateBorderIfNeeded(cell.Location.X == room.Width - 1, Dir.E, mapCell);
                CreateBorderIfNeeded(cell.Location.Y == 0, Dir.N, mapCell);
                CreateBorderIfNeeded(cell.Location.Y == room.Height - 1, Dir.S, mapCell);

                cell.IsVisited = mapCell.IsVisited;
                foreach (var side in cell.Sides)
                {
                    mapCell.Sides[side.Key] = side.Value;
                }
            }
        }

        private void CreateBorderIfNeeded(bool isBorderCell, Dir dir, Cell mapCell)
        {
            if (isBorderCell
                && mapCell.Sides[dir] == Side.Empty
                && HasAdjacentCell(mapCell, dir))
            {
                CreateWall(mapCell, dir);
            }
        }

        private bool IsOutsideMap(Point point)
        {
            return point.X < 0 || point.X >= Width 
                || point.Y < 0 || point.Y >= Height;
        }
    }
}
