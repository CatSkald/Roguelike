using System;
using System.Collections.Generic;
using CatSkald.Roguelike.Core.Parameters;
using CatSkald.Roguelike.Core.Services;
using CatSkald.Roguelike.DungeonGenerator.Commands;
using CatSkald.Roguelike.DungeonGenerator.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CatSkald.Roguelike.DungeonGenerator
{
    public sealed class DungeonGenerationModule
    {
        public void Register(IServiceCollection services)
        {
            services.AddTransient<IDirectionPicker>(
                s => new DirectionPicker(
                    s.GetService<GameParameters>().Map.TwistFactor));

            services.AddTransient<IList<IMapBuilderCommand>>(s => GenerateCommands(s));
            services.AddScoped<IMapConverter, MapConverter>();
            services.AddScoped<IMapBuilder, MapBuilder>();
        }

        private static List<IMapBuilderCommand> GenerateCommands(IServiceProvider s)
        {
            // order matters
            return new List<IMapBuilderCommand> {
                new CorridorBuilderCommand(s.GetService<IDirectionPicker>()),
                new SparsifyCellsCommand(),
                new SparsifyDeadEndsCommand(s.GetService<IDirectionPicker>()),
                new PlaceRoomsCommand()
            };
        }
    }
}
