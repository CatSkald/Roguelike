using System;
using CatSkald.Roguelike.Drawing.Painters;
using CatSkald.Roguelike.DungeonGenerator;
using Microsoft.Extensions.Configuration;

namespace CatSkald.Roguelike.Host
{
    public static class Program
    {
        public static void Main()
        {
            var parameters = GatherParameters();
            var generator = new MapBuilder(parameters);
            var map = generator.Build();
            var mapPainter = new TilesMapPainter();
            mapPainter.PaintMap(map);

            Console.ReadKey();
            Console.ReadLine();
        }

        private static DungeonParameters GatherParameters()
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
