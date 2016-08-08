using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using CatSkald.Tools;

namespace CatSkald.Roguelike.DungeonGenerator.Maps
{
    public abstract class CellContainer : IEnumerable<Cell>
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
        public Cell this[Point point] => this[point.X, point.Y];
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

        #region IEnumerable
        public IEnumerator<Cell> GetEnumerator()
        {
            return Traverse().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private IEnumerable<Cell> Traverse()
        {
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    yield return this[x, y];
                }
        }
        #endregion
    }
}