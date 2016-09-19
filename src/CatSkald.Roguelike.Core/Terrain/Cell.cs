using System.Drawing;
using CatSkald.Roguelike.Core.Objects;

namespace CatSkald.Roguelike.Core.Terrain
{
    public sealed class Cell : ICell
    {
        public Cell()
        {
        }

        public Cell(int x, int y)
        {
            Location = new Point(x, y);
        }

        public Point Location { get; set; }
        public XType Type { get; set; }
    }
}