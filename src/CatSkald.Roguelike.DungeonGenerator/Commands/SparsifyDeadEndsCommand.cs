using System.Linq;
using CatSkald.Roguelike.Core.Terrain;
using CatSkald.Roguelike.DungeonGenerator.Terrain;
using CatSkald.Roguelike.Core.Parameters;
using CatSkald.Tools;
using CatSkald.Roguelike.DungeonGenerator.Services;

namespace CatSkald.Roguelike.DungeonGenerator.Commands
{
    internal sealed class SparsifyDeadEndsCommand : AbstractMapBuilderCommand
    {
        private IDirectionPicker _directionPicker;

        public SparsifyDeadEndsCommand(IDirectionPicker directionPicker)
        {
            _directionPicker = directionPicker;
            // this allows to create nice loops for sparsified corridors
            _directionPicker.SetTwistFactor(0);
        }

        protected override void ExecuteCommand(IMap map, DungeonParameters parameters)
        {
            var _sparseFactor = parameters.DeadEndSparseFactor;
            foreach (var cell in map)
            {
                if (cell.IsDeadEnd && _sparseFactor > StaticRandom.Next(0, 100))
                {
                    BuildCorridor(map, cell);
                }
            }
        }

        protected override void ValidateParameters(DungeonParameters parameters)
        {
            Throw.IfNotInRange(0, 100, parameters.DeadEndSparseFactor,
                nameof(parameters.DeadEndSparseFactor));
        }

        private void BuildCorridor(IMap map, MapCell currentCell)
        {
            MapCell nextCell;
            var direction = Dir.Zero;

            bool success;
            do
            {
                _directionPicker.LastDirection = direction;
                _directionPicker.ResetDirections();
                var emptySide = currentCell.Sides
                    .Single(s => s.Value != Side.Wall)
                    .Key;
                success = false;
                do
                {
                    direction = _directionPicker.NextDirectionExcept(emptySide);
                    success = map.TryGetAdjacentCell(currentCell, direction, out nextCell);

                    if (success)
                    {
                        map.CreateCorridorSide(currentCell, nextCell, direction, Side.Empty);
                    }
                } while (_directionPicker.HasDirections && !success);

                if (!success)
                {
                    return;
                }
            } while (currentCell.IsDeadEnd);
        }
    }
}
