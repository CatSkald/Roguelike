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
        
        public void CreateCorridorSide(Cell startCell, Cell endCell, Dir direction, Side side)
        {
            ThrowD.IfOutsideMap(this, startCell, nameof(startCell));
            ThrowD.IfOutsideMap(this, endCell, nameof(endCell));
            ThrowD.IfNotAdjacent(startCell, endCell, direction);
            if (side == Side.Wall)
                throw new ArgumentException("Wall cannot be used as corridor side.");

            startCell.Sides[direction] = side;
            startCell.IsCorridor = true;
            endCell.Sides[DirHelper.Opposite(direction)] = side;
            endCell.IsCorridor = true;
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

                CreateDoorIfNeeded(
                    cell.Location.X == 0, cell, Dir.W, mapCell);
                CreateDoorIfNeeded(
                    cell.Location.X == room.Width - 1, cell, Dir.E, mapCell);
                CreateDoorIfNeeded(
                    cell.Location.Y == 0, cell, Dir.N, mapCell);
                CreateDoorIfNeeded(
                    cell.Location.Y == room.Height - 1, cell, Dir.S, mapCell);

                cell.IsVisited = mapCell.IsVisited;
                foreach (var side in cell.Sides)
                {
                    mapCell.Sides[side.Key] = side.Value;
                }
            }
        }

        private void CreateDoorIfNeeded(
            bool isBorderCell, Cell roomCell, Dir wallDir, Cell mapCell)
        {
            Cell adjacent;
            if (isBorderCell
                && mapCell.Sides[wallDir] != Side.Wall
                && TryGetAdjacentCell(mapCell, wallDir, out adjacent))
            {
                roomCell.Sides[wallDir] = Side.Door;
                CreateCorridorSide(mapCell, adjacent, wallDir, Side.Door);
            }
        }

        private bool IsOutsideMap(Point point)
        {
            return point.X < 0 || point.X >= Width 
                || point.Y < 0 || point.Y >= Height;
        }
    }
}
