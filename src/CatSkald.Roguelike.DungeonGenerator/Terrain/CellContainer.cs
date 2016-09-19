using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using CatSkald.Roguelike.Core.Terrain;
using CatSkald.Tools;

namespace CatSkald.Roguelike.DungeonGenerator.Terrain
{
    public abstract class CellContainer<T> : IEnumerable<T> where T : ICell, new()
    {
        private readonly T[,] cells;
        private Rectangle bounds;

        protected CellContainer(
            int width, int height, Action<T> cellInitializer = null)
        {
            Throw.IfLess(0, width, nameof(width));
            Throw.IfLess(0, height, nameof(height));

            Width = width;
            Height = height;
            Size = width * height;
            bounds = new Rectangle(0, 0, width, height);

            cells = new T[height, width];

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    var cell = new T
                    {
                        Location = new Point(x, y)
                    };
                    cellInitializer?.Invoke(cell);
                    this[x, y] = cell;
                }
        }

        public int Width { get; }
        public int Height { get; }
        public int Size { get; }
        public Rectangle Bounds { get { return bounds; } }

        public T this[T cell] => this[cell.Location];
        public T this[Point point] => this[point.X, point.Y];
        public T this[int width, int height]
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

        public void Offset(Point position)
        {
            bounds.Offset(position);
        }

        #region IEnumerable
        public IEnumerator<T> GetEnumerator()
        {
            return Traverse().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private IEnumerable<T> Traverse()
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