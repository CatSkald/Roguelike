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
            var generator = new MapBuilder();
            var width = 30;
            var height = 30;
            var map = generator.Build(new MapParameters(width, height));

            Console.WriteLine(new string('#', width + 2));
            for (int i = 0; i < map.Width; i++)
            {
                Console.Write("#");
                for (int j = 0; j < map.Height; j++)
                {
                    var sides = map[j, i].Sides;
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
            Console.WriteLine(new string('#', width + 2));

            Console.ReadKey();
            Console.ReadLine();
        }
    }
}
