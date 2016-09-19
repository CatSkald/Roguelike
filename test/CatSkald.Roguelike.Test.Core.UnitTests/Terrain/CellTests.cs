using System.Drawing;
using CatSkald.Roguelike.Core.Objects;
using CatSkald.Roguelike.Core.Terrain;
using NUnit.Framework;

namespace CatSkald.Roguelike.Test.Core.UnitTests.Terrain
{
    public class CellTests
    {
        [TestCase(0, 0)]
        [TestCase(-2, -150)]
        [TestCase(15, 1)]
        public void Location_GetReturnsSameValue_AsWasSet(int x, int y)
        {
            var cell = new Cell
            {
                Location = new Point(x, y)
            };

            Assert.That(cell.Location, Is.EqualTo(new Point(x, y)));
        }

        [TestCase(XType.Character)]
        [TestCase(XType.Empty)]
        public void Type_GetReturnsSameValue_AsWasSet(XType type)
        {
            var cell = new Cell
            {
                Type = type
            };

            Assert.That(cell.Type, Is.EqualTo(type));
        }
    }
}
