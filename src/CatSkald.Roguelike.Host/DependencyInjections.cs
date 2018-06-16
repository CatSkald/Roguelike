using CatSkald.Roguelike.Drawing;
using CatSkald.Roguelike.DungeonGenerator;
using CatSkald.Roguelike.GameProcessor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CatSkald.Roguelike.Core.Parameters;
using Microsoft.Extensions.Configuration;
using System;

namespace CatSkald.Roguelike.Host
{
    internal static class DependencyInjections
    {
        public static IServiceCollection SetUp(this IServiceCollection services)
        {
            return services
                .AddLogging()
                .AddDependencyInjection();
        }

        public static IServiceProvider BuildServiceProvider(this IServiceCollection services)
        {
            var builder = new ContainerBuilder();
            builder.Populate(services);

            return new AutofacServiceProvider(builder.Build());
        }

        private static IServiceCollection AddLogging(this IServiceCollection services)
        {
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddNLog();
              
            services.AddSingleton<ILoggerFactory>(loggerFactory);

            return services;
        }

        private static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            services.AddTransient(_ => GetParameters<MapParameters>("Map"));
            services.AddTransient(_ => GetParameters<DungeonParameters>("Dungeon"));

            new DungeonGenerationModule().Register(services);
            new DrawingModule().Register(services);
            new GameProcessingModule().Register(services);

            return services;
        }

        private static T GetParameters<T>(string sectionName) where T : new()
        {
            var builder = new ConfigurationBuilder()
                    .AddJsonFile("AppSettings.json")
                    .AddEnvironmentVariables();
            var configuration = builder.Build();

            var parameters = new T();
            configuration.GetSection(sectionName).Bind(parameters);

            return parameters;
        }
    }
}