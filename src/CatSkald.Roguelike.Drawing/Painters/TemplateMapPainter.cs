using System;
using System.Linq;
using CatSkald.Roguelike.Core.Terrain;
using CatSkald.Roguelike.DungeonGenerator.Maps;

namespace CatSkald.Roguelike.Drawing.Painters
{
    public sealed class TemplateMapPainter : IMapPainter
    {
        public void PaintMap(IMap map)
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
                    if (sides.Values.All(s => s != Side.Wall))
                    {
                        Console.Write(".");
                    }
                    else if (sides.Values.All(s => s == Side.Wall))
                    {
                        Console.Write("#");
                    }

                    // One wall
                    else if (sides[Dir.N] == Side.Wall
                        && sides.Where(s => s.Key != Dir.N).All(s => s.Value != Side.Wall))
                    {
                        Console.Write("╦");
                    }
                    else if (sides[Dir.S] == Side.Wall
                        && sides.Where(s => s.Key != Dir.S).All(s => s.Value != Side.Wall))
                    {
                        Console.Write("╩");
                    }
                    else if (sides[Dir.E] == Side.Wall
                        && sides.Where(s => s.Key != Dir.E).All(s => s.Value != Side.Wall))
                    {
                        Console.Write("╣");
                    }
                    else if (sides[Dir.W] == Side.Wall
                        && sides.Where(s => s.Key != Dir.W).All(s => s.Value != Side.Wall))
                    {
                        Console.Write("╠");
                    }

                    // Dead ends
                    else if (sides[Dir.N] != Side.Wall
                        && sides.Where(s => s.Key != Dir.N).All(s => s.Value == Side.Wall))
                    {
                        Console.Write("^");
                    }
                    else if (sides[Dir.S] != Side.Wall
                        && sides.Where(s => s.Key != Dir.S).All(s => s.Value == Side.Wall))
                    {
                        Console.Write("v");
                    }
                    else if (sides[Dir.E] != Side.Wall
                        && sides.Where(s => s.Key != Dir.E).All(s => s.Value == Side.Wall))
                    {
                        Console.Write(">");
                    }
                    else if (sides[Dir.W] != Side.Wall
                        && sides.Where(s => s.Key != Dir.W).All(s => s.Value == Side.Wall))
                    {
                        Console.Write("<");
                    }

                    // Corridors
                    else if (sides[Dir.N] != Side.Wall && sides[Dir.S] != Side.Wall)
                    {
                        Console.Write("║");
                    }
                    else if (sides[Dir.E] != Side.Wall && sides[Dir.W] != Side.Wall)
                    {
                        Console.Write("═");
                    }
                    else if (sides[Dir.N] != Side.Wall && sides[Dir.W] != Side.Wall)
                    {
                        Console.Write("╝");
                    }
                    else if (sides[Dir.N] != Side.Wall && sides[Dir.E] != Side.Wall)
                    {
                        Console.Write("╚");
                    }
                    else if (sides[Dir.S] != Side.Wall && sides[Dir.W] != Side.Wall)
                    {
                        Console.Write("╗");
                    }
                    else if (sides[Dir.S] != Side.Wall && sides[Dir.E] != Side.Wall)
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
