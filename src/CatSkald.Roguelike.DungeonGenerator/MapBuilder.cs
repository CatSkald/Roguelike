using System.Collections.Generic;
using CatSkald.Roguelike.DungeonGenerator.Commands;
using CatSkald.Roguelike.DungeonGenerator.Maps;
using CatSkald.Tools;

namespace CatSkald.Roguelike.DungeonGenerator
{
    public sealed class MapBuilder : IMapBuilder
    {
        private readonly List<IMapBuilderCommand> _commands = 
            new List<IMapBuilderCommand>();
        private DungeonParameters _params;

        public MapBuilder(DungeonParameters parameters)
        {
            SetParameters(parameters);
        }

        public void SetParameters(DungeonParameters parameters)
        {
            Throw.IfNull(parameters, nameof(parameters));

            _params = parameters;
            _commands.Clear();
            // order matters
            _commands.AddRange(new IMapBuilderCommand[] {
                new CorridorBuilderCommand(parameters.TwistFactor),
                new SparsifyCellsCommand(parameters.CellSparseFactor),
                new SparsifyDeadEndsCommand(parameters.DeadEndSparseFactor),
                new PlaceRoomsCommand(parameters)
            });
        }

        public IMap Build()
        {
            var map = new Map(_params.Width, _params.Height);

            foreach (var command in _commands)
            {
                command.Execute(map);
            }

            return map;
        }
    }
}
