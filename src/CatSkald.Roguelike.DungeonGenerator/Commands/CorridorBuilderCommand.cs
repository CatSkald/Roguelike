using System;
using System.Linq;
using CatSkald.Roguelike.DungeonGenerator.Directions;
using CatSkald.Roguelike.DungeonGenerator.Maps;
using CatSkald.Tools;

namespace CatSkald.Roguelike.DungeonGenerator.Commands
{
    public sealed class CorridorBuilderCommand : IMapBuilderCommand
    {
        private DirectionPicker _directionsPicker;

        public CorridorBuilderCommand(int twistFactor)
        {
            Throw.IfNotInRange(0, 100, twistFactor, nameof(twistFactor));

            _directionsPicker = new DirectionPicker(twistFactor);
        }

        public void Execute(IMap map)
        {
            Throw.IfNull(map, nameof(map));

            BuildCorridors(map);
        }

        private void BuildCorridors(IMap map)
        {
            if (map.Any(c => c.IsVisited))
            {
                throw new InvalidOperationException(
                    "Map should not contain visited cells.");
            }

            Cell nextCell;
            var direction = Dir.Zero;

            var currentCell = map.PickRandomCell();
            map.Visit(currentCell);
            bool success;
            do
            {
                _directionsPicker.LastDirection = direction;
                _directionsPicker.ResetDirections();
                success = TryPickRandomUnvisitedAdjacentCell(
                    map, currentCell, out nextCell, out direction);
                if (success)
                {
                    map.CreateCorridor(currentCell, nextCell, direction);
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
            IMap map, Cell currentCell, out Cell nextCell, out Dir direction)
        {
            var success = false;
            do
            {
                direction = _directionsPicker.NextDirection();
                success = map.TryGetAdjacentCell(currentCell, direction, out nextCell)
                    && !nextCell.IsVisited;
            }
            while (_directionsPicker.HasDirections && !success);

            return success;
        }
    }
}
