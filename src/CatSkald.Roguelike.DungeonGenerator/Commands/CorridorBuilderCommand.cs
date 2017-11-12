using System;
using System.Linq;
using CatSkald.Roguelike.Core.Terrain;
using CatSkald.Roguelike.DungeonGenerator.Terrain;
using CatSkald.Roguelike.Core.Parameters;
using CatSkald.Tools;
using CatSkald.Roguelike.DungeonGenerator.Services;

namespace CatSkald.Roguelike.DungeonGenerator.Commands
{
    internal sealed class CorridorBuilderCommand : AbstractMapBuilderCommand
    {
        private readonly IDirectionPicker _directionPicker;

        public CorridorBuilderCommand(IDirectionPicker directionPicker)
        {
            _directionPicker = directionPicker;
        }

        protected override void ExecuteCommand(IMap map, DungeonParameters parameters)
        {
            BuildCorridors(map);
        }

        protected override void ValidateParameters(DungeonParameters parameters)
        {
            Throw.IfNotInRange(0, 100, parameters.TwistFactor, nameof(parameters.TwistFactor));
        }

        private void BuildCorridors(IMap map)
        {
            if (map.Any(c => c.IsVisited))
            {
                throw new InvalidOperationException(
                    "Map should not contain visited cells.");
            }

            MapCell nextCell;
            var direction = Dir.Zero;

            var currentCell = map.PickRandomCell();
            map.Visit(currentCell);
            bool success;
            do
            {
                _directionPicker.LastDirection = direction;
                _directionPicker.ResetDirections();
                success = TryPickRandomUnvisitedAdjacentCell(
                    map, currentCell, out nextCell, out direction);
                if (success)
                {
                    map.CreateCorridorSide(currentCell, nextCell, direction, Side.Empty);
                    map.Visit(nextCell);
                    currentCell = nextCell;
                    nextCell = null;
                }
                else
                {
                    currentCell = map.PickNextRandomVisitedCell(currentCell);
                }
            }
            while (!map.AllVisited);
        }

        private bool TryPickRandomUnvisitedAdjacentCell(
            IMap map, MapCell currentCell, out MapCell nextCell, out Dir direction)
        {
            var success = false;
            do
            {
                direction = _directionPicker.NextDirection();
                success = map.TryGetAdjacentCell(currentCell, direction, out nextCell)
                    && !nextCell.IsVisited;
            }
            while (_directionPicker.HasDirections && !success);

            return success;
        }
    }
}
