using System;
using System.Collections.Generic;
using CatSkald.Roguelike.Drawing.Converters;
using CatSkald.Roguelike.Drawing.Painters;
using CatSkald.Roguelike.DungeonGenerator;
using CatSkald.Roguelike.DungeonGenerator.Commands;
using CatSkald.Roguelike.DungeonGenerator.Maps;
using CatSkald.Roguelike.DungeonGenerator.Parameters;
using Microsoft.Extensions.DependencyInjection;

namespace CatSkald.Roguelike.Host
{
    internal static class DependencyInjections
    {
        public static IServiceCollection AddLogging(IServiceCollection services)
        {
            // ILoggerFactory loggerFactory = new Logging.LoggerFactory();
            // serviceCollection.AddInstance<ILoggerFactory>(loggerFactory);

            return services;
        }

        public static IServiceCollection AddMapBuilding(
            this IServiceCollection services)
        {
            services.AddTransient<IDirectionPicker>(
                s => new DirectionPicker(
                    s.GetService<IDungeonParameters>().TwistFactor));

            services.AddTransient(s => GenerateCommands(s));
            services.AddScoped<IMapBuilder, MapBuilder>();

            return services;
        }

        public static IServiceCollection AddMapPainting(
            this IServiceCollection services)
        {
            services.AddTransient<ITilesConverter<IMap>, TilesConverter>();
            services.AddTransient<IMapPainter, TilesMapPainter>();

            return services;
        }

        private static IList<IMapBuilderCommand> GenerateCommands(IServiceProvider s)
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