using System.Collections.Generic;

namespace CatSkald.Roguelike.Core.Terrain
{
    public interface IDungeon : IEnumerable<Cell>
    {
        int Width { get; }
        int Height { get; }
        int Size { get; }

        Cell this[Cell point] { get; }
        Cell this[int width, int height] { get; }
    }
}
