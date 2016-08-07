using System;
using System.Linq;
using CatSkald.Roguelike.DungeonGenerator;
using CatSkald.Roguelike.DungeonGenerator.Directions;
using CatSkald.Roguelike.DungeonGenerator.Maps;
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

            DrawMap(map);

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

        private static void DrawMap(IMap map)
        {
            var indentedWidth = map.Width + 2;
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.WriteLine(new string('#', indentedWidth));
            for (int y = 0; y < map.Height; y++)
            {
                Console.Write("#");
                for (int x = 0; x < map.Width; x++)
                {
                    var sides = map[x, y].Sides;
                    if (sides.Values.All(s => s == Side.Empty))
                    {
                        Console.Write("╬");
                    }
                    else if (sides.Values.All(s => s == Side.Wall))
                    {
                        Console.Write("#");
                    }

                    // One wall
                    else if (sides[Dir.N] == Side.Wall
                        && sides.Where(s => s.Key != Dir.N).All(s => s.Value == Side.Empty))
                    {
                        Console.Write("╦");
                    }
                    else if (sides[Dir.S] == Side.Wall
                        && sides.Where(s => s.Key != Dir.S).All(s => s.Value == Side.Empty))
                    {
                        Console.Write("╩");
                    }
                    else if (sides[Dir.E] == Side.Wall
                        && sides.Where(s => s.Key != Dir.E).All(s => s.Value == Side.Empty))
                    {
                        Console.Write("╣");
                    }
                    else if (sides[Dir.W] == Side.Wall
                        && sides.Where(s => s.Key != Dir.W).All(s => s.Value == Side.Empty))
                    {
                        Console.Write("╠");
                    }

                    // Dead ends
                    else if (sides[Dir.N] == Side.Empty
                        && sides.Where(s => s.Key != Dir.N).All(s => s.Value == Side.Wall))
                    {
                        Console.Write("^");
                    }
                    else if (sides[Dir.S] == Side.Empty
                        && sides.Where(s => s.Key != Dir.S).All(s => s.Value == Side.Wall))
                    {
                        Console.Write("v");
                    }
                    else if (sides[Dir.E] == Side.Empty
                        && sides.Where(s => s.Key != Dir.E).All(s => s.Value == Side.Wall))
                    {
                        Console.Write(">");
                    }
                    else if (sides[Dir.W] == Side.Empty
                        && sides.Where(s => s.Key != Dir.W).All(s => s.Value == Side.Wall))
                    {
                        Console.Write("<");
                    }

                    // Corridors
                    else if (sides[Dir.N] == Side.Empty && sides[Dir.S] == Side.Empty)
                    {
                        Console.Write("║");
                    }
                    else if (sides[Dir.E] == Side.Empty && sides[Dir.W] == Side.Empty)
                    {
                        Console.Write("═");
                    }
                    else if (sides[Dir.N] == Side.Empty && sides[Dir.W] == Side.Empty)
                    {
                        Console.Write("╝");
                    }
                    else if (sides[Dir.N] == Side.Empty && sides[Dir.E] == Side.Empty)
                    {
                        Console.Write("╚");
                    }
                    else if (sides[Dir.S] == Side.Empty && sides[Dir.W] == Side.Empty)
                    {
                        Console.Write("╗");
                    }
                    else if (sides[Dir.S] == Side.Empty && sides[Dir.E] == Side.Empty)
                    {
                        Console.Write("╔");
                    }
                    else
                    {
                        Console.Write("?");
                    }
                }
                Console.Write("#");
                Console.WriteLine();
            }
            Console.WriteLine(new string('#', indentedWidth));
        }
    }
}
