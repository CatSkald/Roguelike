﻿using System;
using System.Drawing;
using CatSkald.Roguelike.Core.Cells;

namespace CatSkald.Roguelike.Core.Terrain
{
    public abstract class CellContainer<T> : BaseContainer<T> 
        where T : ICell, new()
    {
        private readonly T[,] cells;
        private Rectangle bounds;

        protected CellContainer(
            int width, int height, Action<T> cellInitializer = null) 
            : base(width, height)
        {
            bounds = new Rectangle(0, 0, width, height);

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

        public Rectangle Bounds
        {
            get { return bounds; }
            protected set { bounds = value; }
        }

        public T this[T cell] => this[cell.Location];
    }
}