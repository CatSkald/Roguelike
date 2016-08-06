using System;
using CatSkald.Roguelike.DungeonGenerator.Directions;
using CatSkald.Roguelike.DungeonGenerator.Maps;

namespace CatSkald.Roguelike.DungeonGenerator.Commands
{
    public sealed class CorridorBuilderCommand : IMapBuilderCommand
    {
        private DirectionPicker _directionsPicker;

        public CorridorBuilderCommand(int twistFactor)
        {
            if (twistFactor < 0 || twistFactor > 100)
            {
                throw new ArgumentOutOfRangeException(
                      nameof(twistFactor), twistFactor, "Should be between 0 and 100");
            }

            _directionsPicker = new DirectionPicker(twistFactor);
        }

        public void Execute(IMap map)
        {
            if (map == null)
                throw new ArgumentNullException(nameof(map));

            BuildCorridors(map);
        }

        private void BuildCorridors(IMap map)
        {
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
                success = map.TryGetAdjacentUnvisitedCell(currentCell, direction, out nextCell);
            }
            while (_directionsPicker.HasDirections && !success);

            return success;
        }
    }
}
