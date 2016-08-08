using System.Diagnostics;
using CatSkald.Roguelike.DungeonGenerator.Directions;

namespace CatSkald.Roguelike.DungeonGenerator.Maps
{
    [DebuggerDisplay("[{Width},{Height}]")]
    public sealed class Room : CellContainer
    {
        public Room(int width, int height) : base(width, height)
        {
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    var cell = new Cell(x, y);
                    cell.Sides[Dir.W] = x == 0 ? Side.Wall : Side.Empty;
                    cell.Sides[Dir.E] = x == Width - 1 ? Side.Wall : Side.Empty;
                    cell.Sides[Dir.N] = y == 0 ? Side.Wall : Side.Empty;
                    cell.Sides[Dir.S] = y == Height - 1 ? Side.Wall : Side.Empty;

                    this[x, y] = cell;
                }
        }
    }
}
