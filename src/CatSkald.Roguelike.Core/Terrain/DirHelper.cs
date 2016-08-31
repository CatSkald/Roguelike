using System;
using System.Collections.Generic;
using System.Drawing;

namespace CatSkald.Roguelike.Core.Terrain
{
    public static class DirHelper
    {
        public static IList<Dir> GetNonEmptyDirs()
        {
            return new[] { Dir.N, Dir.E, Dir.S, Dir.W };
        }

        public static Point MoveInDir(Point p, Dir dir)
        {
            switch (dir)
            {
                case Dir.W:
                    p.X -= 1;
                    break;
                case Dir.S:
                    p.Y += 1;
                    break;
                case Dir.E:
                    p.X += 1;
                    break;
                case Dir.N:
                    p.Y -= 1;
                    break;
                default:
                    throw new ArgumentException($"Unknown direction: {dir}");
            }

            return p;
        }

        public static Dir Opposite(this Dir direction)
        {
            Dir result;
            switch (direction)
            {
                case Dir.N:
                    result = Dir.S;
                    break;
                case Dir.E:
                    result = Dir.W;
                    break;
                case Dir.S:
                    result = Dir.N;
                    break;
                case Dir.W:
                    result = Dir.E;
                    break;
                default:
                    throw new ArgumentException($"Invalid direction: {direction}");
            }

            return result;
        }
    }
}
