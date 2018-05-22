using System;
using CatSkald.Roguelike.Core.Cells;
using CatSkald.Roguelike.Core.Terrain;

namespace CatSkald.Roguelike.Test.Core.UnitTests.TestHelpers
{
    public class FakeCellContainer : CellContainer<Cell>
    {
        public FakeCellContainer(int width, int height) 
            : this(width, height, null)
        {
        }

        public FakeCellContainer(int width, int height, Func<Cell, Cell> cellInitializer)
             : base(width, height, cellInitializer)
        {
        }
    }
}