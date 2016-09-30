using CatSkald.Roguelike.Core.Terrain;

namespace CatSkald.Roguelike.Test.Core.UnitTests.TestHelpers
{
    public class FakeBaseContainer : BaseContainer<object>
    {
        public FakeBaseContainer(int width, int height)
             : base(width, height)
        {
        }
    }
}