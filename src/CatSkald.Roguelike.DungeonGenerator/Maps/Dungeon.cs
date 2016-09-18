using System.Collections;
using System.Collections.Generic;
using CatSkald.Roguelike.Core.Terrain;
using CatSkald.Tools;

namespace CatSkald.Roguelike.DungeonGenerator
{
    public class Dungeon : IDungeon
    {
        private Cell[,] _dungeon;

        public Dungeon(int width, int height)
        {
            Throw.IfLess(0, width, nameof(width));
            Throw.IfLess(0, height, nameof(height));

            Width = width;
            Height = height;
            Size = width * height;

            _dungeon = new Cell[height, width];

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    _dungeon[y, x] = new Cell(x, y);
                }
        }

        public int Width { get; }
        public int Height { get; }
        public int Size { get; }

        public Cell this[Cell cell] => this[cell.X, cell.Y];
        public Cell this[int width, int height]
        {
            get
            {
                return _dungeon[height, width];
            }
            protected set
            {
                _dungeon[height, width] = value;
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
