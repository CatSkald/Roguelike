using System;
using CatSkald.Roguelike.DungeonGenerator.Maps;

namespace CatSkald.Roguelike.Tests.DungeonGeneratorTests
{
    public class FakeCellContainer : CellContainer
    {
        public FakeCellContainer(int width, int height) : this(width, height, null)
        {
        }

        public FakeCellContainer(int width, int height, Action<Cell> cellInitializer)
             : base(width, height, cellInitializer)
        {
        }
    }
}