using System.Drawing;
using CatSkald.Tools;

namespace CatSkald.Roguelike.DungeonGenerator.Maps
{
    public abstract class CellContainer
    {
        private readonly Cell[,] cells;

        protected CellContainer(int width, int height)
        {
            Throw.IfLess(0, width, nameof(width));
            Throw.IfLess(0, height, nameof(height));

            Width = width;
            Height = height;
            Size = width * height;
            Bounds = new Rectangle(0, 0, width, height);

            cells = new Cell[height, width];
        }

        public int Width { get; }
        public int Height { get; }
        public int Size { get; }
        public Rectangle Bounds { get; }

        public Cell this[Cell cell] => this[cell.Location];
        public Cell this[Point p] => this[p.X, p.Y];
        public Cell this[int width, int height]
        {
            get
            {
                return cells[height, width];
            }
            protected set
            {
                cells[height, width] = value;
            }
        }
    }
}