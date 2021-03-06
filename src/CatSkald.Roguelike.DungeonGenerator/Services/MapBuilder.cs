﻿using System.Collections.Generic;
using CatSkald.Roguelike.DungeonGenerator.Terrain;
using CatSkald.Roguelike.Core.Parameters;
using CatSkald.Tools;
using CatSkald.Roguelike.Core.Terrain;
using CatSkald.Roguelike.Core.Services;

namespace CatSkald.Roguelike.DungeonGenerator.Services
{
    internal sealed class MapBuilder : IMapBuilder
    {
        private readonly IMapConverter converter;
        private readonly IList<IMapBuilderCommand> commands;

        public MapBuilder(IList<IMapBuilderCommand> commands, IMapConverter converter)
        {
            Throw.IfNullOrHasNull(commands, nameof(commands));
            Throw.IfNull(converter, nameof(converter));

            this.commands = commands;
            this.converter = converter;
        }

        public IDungeon Build(MapParameters parameters)
        {
            Throw.IfNull(parameters, nameof(parameters));
            Throw.IfLess(1, parameters.Width, nameof(parameters.Width));
            Throw.IfLess(1, parameters.Height, nameof(parameters.Height));

            var map = new Map(parameters.Width, parameters.Height);

            foreach (var command in commands)
            {
                command.Execute(map, parameters);
            }

            return converter.ConvertToDungeon(map);
        }
    }
}
