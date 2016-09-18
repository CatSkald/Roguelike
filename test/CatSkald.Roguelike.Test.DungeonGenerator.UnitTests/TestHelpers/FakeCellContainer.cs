using System;
using CatSkald.Roguelike.DungeonGenerator.Terrain;

namespace CatSkald.Roguelike.Test.DungeonGenerator.UnitTests.TestHelpers
{
    public class FakeCellContainer : CellContainer
    {
        public FakeCellContainer(int width, int height) : this(width, height, null)
        {
        }

        public FakeCellContainer(int width, int height, Action<MapCell> cellInitializer)
             : base(width, height, cellInitializer)
        {
        }
    }
}