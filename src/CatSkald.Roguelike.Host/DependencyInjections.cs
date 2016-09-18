using System;
using System.Collections.Generic;
using CatSkald.Roguelike.Core.Services;
using CatSkald.Roguelike.Drawing;
using CatSkald.Roguelike.DungeonGenerator;
using CatSkald.Roguelike.DungeonGenerator.Commands;
using CatSkald.Roguelike.DungeonGenerator.Services;
using CatSkald.Roguelike.Core.Parameters;
using CatSkald.Roguelike.GameProcessor;
using CatSkald.Roguelike.GameProcessor.Initialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace CatSkald.Roguelike.Host
{
    internal static class DependencyInjections
    {
        public static IServiceCollection AddLogging(IServiceCollection services)
        {
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddNLog();
              
            services.AddSingleton<ILoggerFactory>(loggerFactory);

            return services;
        }

        public static IServiceCollection AddMapBuilding(
            this IServiceCollection services)
        {
            services.AddTransient<IDirectionPicker>(
                s => new DirectionPicker(
                    s.GetService<DungeonParameters>().TwistFactor));

            services.AddTransient<IList<IMapBuilderCommand>>(s => GenerateCommands(s));
            services.AddScoped<IMapConverter, MapConverter>();
            services.AddScoped<IMapBuilder, MapBuilder>();

            return services;
        }
        
        public static IServiceCollection AddMapProcessing(
            this IServiceCollection services)
        {
            services.AddScoped<IDungeonPopulator, DungeonPopulator>();
            services.AddScoped<IProcessor, Processor>();

            return services;
        }

        public static IServiceCollection AddMapPainting(
            this IServiceCollection services)
        {
            services.AddTransient<IMapPainter, ConsolePainter>();

            return services;
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