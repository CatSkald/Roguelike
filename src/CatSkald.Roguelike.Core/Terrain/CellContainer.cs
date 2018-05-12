using System;
using System.Drawing;
using CatSkald.Roguelike.Core.Cells;

namespace CatSkald.Roguelike.Core.Terrain
{
    public abstract class CellContainer<T> : BaseContainer<T> 
        where T : ICell, new()
    {
        protected CellContainer(
            int width, int height, Action<T> cellInitializer = null) 
            : base(width, height)
        {
            Bounds = new Rectangle(0, 0, width, height);

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

        public Rectangle Bounds { get; protected set; }

        public T this[T cell] => this[cell.Location];
    }
}