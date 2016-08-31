using System;
using CatSkald.Roguelike.DungeonGenerator.Maps;

namespace CatSkald.Roguelike.Test.DungeonGenerator.UnitTests
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