using CatSkald.Roguelike.Core.Parameters;
using NUnit.Framework;

namespace CatSkald.Roguelike.Test.Core.UnitTests.Parameters
{
    public class DungeonParametersTests
    {
        [Test]
        public void Constructor_RoomParameters_AreNotNull()
        {
            var parameters = new MapParameters();

            Assert.That(parameters.Room, Is.Not.Null);
        }
    }
}
