using System;
using CatSkald.Roguelike.Drawing.Painters;
using CatSkald.Roguelike.DungeonGenerator;
using CatSkald.Roguelike.DungeonGenerator.Parameters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace CatSkald.Roguelike.Host
{
    public static class Program
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();

        public static void Main()
        {
            try
            {
                Log.Debug("Roguelike started.");

                var provider = BuildServiceProvider();
                StartApplication(provider);
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Oops, you've just catched a fatal bug.");
                throw;
            }

            Console.ReadLine();
        }

        private static void StartApplication(IServiceProvider provider)
        {
            var mapBuilder = provider.GetService<IMapBuilder>();
            var map = mapBuilder.Build(GatherParameters());
            Log.Debug("Map created.");

            var mapPainter = provider.GetService<IMapPainter>();
            mapPainter.PaintMap(map);
            Log.Debug("Map painted.");
        }

        private static IServiceProvider BuildServiceProvider()
        {
            var services = new ServiceCollection();
            services.AddTransient(_ => GatherParameters())
                .AddLogging()
                .AddMapBuilding()
                .AddMapPainting();

            return services.BuildServiceProvider();
        }

        private static IDungeonParameters GatherParameters()
        {
            var builder = new ConfigurationBuilder()
                    .AddJsonFile("AppSettings.json")
                    .AddEnvironmentVariables();

            var configuration = builder.Build();

            return new DungeonParameters
            {
                Width = configuration.GetValue<int>("Map:Width"),
                Height = configuration.GetValue<int>("Map:Height"),
                CellSparseFactor = configuration.GetValue<int>("Map:CellSparseFactor"),
                DeadEndSparseFactor = configuration.GetValue<int>("Map:DeadEndSparseFactor"),
                TwistFactor = configuration.GetValue<int>("Map:TwistFactor"),
                RoomParameters = new RoomParameters
                {
                    Count = configuration.GetValue<int>("Map:Room:Count"),
                    MinWidth = configuration.GetValue<int>("Map:Room:MinWidth"),
                    MaxWidth = configuration.GetValue<int>("Map:Room:MaxWidth"),
                    MinHeight = configuration.GetValue<int>("Map:Room:MinHeight"),
                    MaxHeight = configuration.GetValue<int>("Map:Room:MaxHeight")
                }
            };
        }
    }
}
