using System;
using System.Linq;
using CatSkald.Roguelike.DungeonGenerator.Directions;
using CatSkald.Roguelike.DungeonGenerator.Maps;
using CatSkald.Tools;

namespace CatSkald.Roguelike.DungeonGenerator.Commands
{
    public sealed class SparsifyDeadEndsCommand : IMapBuilderCommand
    {
        private int _sparseFactor;
        private DirectionPicker _directionsPicker;

        public SparsifyDeadEndsCommand(int sparseFactor)
        {
            Throw.IfNotInRange(0, 100, sparseFactor, nameof(sparseFactor));

            _sparseFactor = sparseFactor;
            _directionsPicker = new DirectionPicker(0);
        }

        public void Execute(IMap map)
        {
            Throw.IfNull(map, nameof(map));

            foreach (var cell in map)
            {
                if (cell.IsDeadEnd && _sparseFactor > StaticRandom.Next(0, 100))
                {
                    BuildCorridor(map, cell);
                }
            }
        }

        private void BuildCorridor(IMap map, Cell currentCell)
        {
            Cell nextCell;
            var direction = Dir.Zero;

            bool success;
            do
            {
                _directionsPicker.LastDirection = direction;
                _directionsPicker.ResetDirections();
                var emptySide = currentCell.Sides
                    .Single(s => s.Value != Side.Wall)
                    .Key;
                success = false;
                do
                {
                    direction = _directionsPicker.NextDirectionExcept(emptySide);
                    success = map.TryGetAdjacentCell(currentCell, direction, out nextCell);

                    if (success)
                    {
                        map.CreateCorridorSide(currentCell, nextCell, direction, Side.Empty);
                    }
                } while (_directionsPicker.HasDirections && !success);

                if (!success)
                {
                    break;
                }
            } while (currentCell.IsDeadEnd);
        }
    }
}
