using System;
using CatSkald.Roguelike.Drawing.Painters;
using CatSkald.Roguelike.DungeonGenerator;
using Microsoft.Extensions.DependencyInjection;

namespace CatSkald.Roguelike.Host
{
    internal static class DependencyInjections
    {
        public static IServiceCollection AddLogging(IServiceCollection services)
        {
            //ILoggerFactory loggerFactory = new Logging.LoggerFactory();
            //serviceCollection.AddInstance<ILoggerFactory>(loggerFactory);

            return services;
        }

        public static IServiceCollection AddMapBuilding(
            this IServiceCollection services)
        {
            services.AddScoped<IMapBuilder, MapBuilder>();

            return services;
        }

        public static IServiceCollection AddMapPainting(
            this IServiceCollection services)
        {
            services.AddScoped<IMapPainter, TilesMapPainter>();

            return services;
        }
    }
}