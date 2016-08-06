using CatSkald.Roguelike.DungeonGenerator.Directions;
using CatSkald.Roguelike.DungeonGenerator.Maps;

namespace CatSkald.Roguelike.DungeonGenerator
{
    public class MapBuilder
    {
        private readonly DirectionPicker _directionsPicker;

        public MapBuilder()
        {
            _directionsPicker = new DirectionPicker();
        }

        public IMap Build(DungeonParameters parameters)
        {
            _directionsPicker.TwistFactor = parameters.TwistFactor;
            var map = new Map(parameters.Width, parameters.Height);

            BuildCorridors(map);

            return map;
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
