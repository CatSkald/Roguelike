using CatSkald.Roguelike.Core.Objects;

namespace CatSkald.Roguelike.Core.Terrain
{
    public sealed class Cell
    {
        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public XType Type { get; set; }
    }
}