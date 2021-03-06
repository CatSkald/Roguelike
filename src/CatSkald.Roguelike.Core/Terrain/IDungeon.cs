﻿using System.Collections.Generic;
using System.Drawing;
using CatSkald.Roguelike.Core.Cells;

namespace CatSkald.Roguelike.Core.Terrain
{
    public interface IDungeon : IEnumerable<Cell>
    {
        int Width { get; }
        int Height { get; }
        int Size { get; }

        Cell this[Cell cell] { get; }
        Cell this[Point point] { get; }
        Cell this[int width, int height] { get; }
    }
}
