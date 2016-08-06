using System;
using System.Linq;
using CatSkald.Roguelike.DungeonGenerator;
using CatSkald.Roguelike.DungeonGenerator.Directions;
using CatSkald.Roguelike.DungeonGenerator.Maps;

namespace CatSkald.Roguelike.Host
{
    public class Program
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
            return new DungeonParameters
            {
                Width = int.Parse(Configuration.Get("Map.Width")),
                Height = int.Parse(Configuration.Get("Map.Height")),
                TwistFactor = int.Parse(Configuration.Get("Corridors.TwistFactor"))
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
