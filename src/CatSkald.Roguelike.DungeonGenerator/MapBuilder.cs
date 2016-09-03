using System.Collections.Generic;
using CatSkald.Roguelike.DungeonGenerator.Maps;
using CatSkald.Roguelike.DungeonGenerator.Parameters;
using CatSkald.Tools;

namespace CatSkald.Roguelike.DungeonGenerator
{
    public sealed class MapBuilder : IMapBuilder
    {
        private readonly IList<IMapBuilderCommand> _commands;

        public MapBuilder(IList<IMapBuilderCommand> commands)
        {
            Throw.IfNull(commands, nameof(commands));

            _commands = commands;
        }

        public IMap Build(IDungeonParameters parameters)
        {
            Throw.IfNull(parameters, nameof(parameters));
            Throw.IfLess(1, parameters.Width, nameof(parameters.Width));
            Throw.IfLess(1, parameters.Height, nameof(parameters.Height));

            var map = new Map(parameters.Width, parameters.Height);

            foreach (var command in _commands)
            {
                command.Execute(map, parameters);
            }

            return map;
        }
    }
}
