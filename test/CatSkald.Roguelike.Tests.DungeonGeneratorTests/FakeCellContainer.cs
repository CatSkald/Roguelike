using CatSkald.Roguelike.DungeonGenerator.Maps;

namespace CatSkald.Roguelike.Tests.DungeonGeneratorTests
{
    public class FakeCellContainer : CellContainer
    {
        public FakeCellContainer(int width, int height) : base(width, height)
        {
        }
    }
}