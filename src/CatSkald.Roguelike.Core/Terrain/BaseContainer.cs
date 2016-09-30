using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using CatSkald.Tools;

namespace CatSkald.Roguelike.Core.Terrain
{
    public abstract class BaseContainer<T> : IEnumerable<T> 
    {
        private readonly T[,] items;

        protected BaseContainer(int width, int height)
        {
            Throw.IfLess(0, width, nameof(width));
            Throw.IfLess(0, height, nameof(height));

            Width = width;
            Height = height;
            Size = width * height;

            items = new T[height, width];
        }

        public int Width { get; }
        public int Height { get; }
        public int Size { get; }

        public T this[Point point] => this[point.X, point.Y];
        public T this[int width, int height]
        {
            get
            {
                return items[height, width];
            }
            protected set
            {
                items[height, width] = value;
            }
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