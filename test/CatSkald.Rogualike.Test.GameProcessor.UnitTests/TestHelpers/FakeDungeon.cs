using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using CatSkald.Roguelike.Core.Objects;
using CatSkald.Roguelike.Core.Terrain;

namespace CatSkald.Rogualike.Test.GameProcessor.UnitTests.TestHelpers
{
    public class FakeDungeon : IDungeon
    {
        private readonly Cell[,] cells;

        public FakeDungeon() : this(0, 0)
        {
        }

        public FakeDungeon(int width, int height)
        {
            Width = width;
            Height = height;
            Size = width * height;

            cells = new Cell[height, width];

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    var cell = new Cell
                    {
                        Location = new Point(x, y),
                        Type = XType.Wall
                    };
                    this[x, y] = cell;
                }
        }

        public int Width { get; }
        public int Height { get; }
        public int Size { get; }

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
