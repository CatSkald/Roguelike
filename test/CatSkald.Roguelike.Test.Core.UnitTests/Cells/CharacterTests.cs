using CatSkald.Roguelike.Core.Cells;
using NUnit.Framework;
namespace CatSkald.Roguelike.Test.Core.UnitTests.Cells
{
    public class CharacterTests
    {
        [Test]
        public void Constructor_SetsCorrectType()
        {
            var tile = new Character();

            Assert.That(tile.Type, Is.EqualTo(XType.Character));
        }
    }
}
