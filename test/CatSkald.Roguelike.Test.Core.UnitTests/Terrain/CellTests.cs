using System.Drawing;
using CatSkald.Roguelike.Core.Objects;
using CatSkald.Roguelike.Core.Terrain;
using CatSkald.Roguelike.Test.TestHelpers;
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


        #region IEquatable
        [Test]
        public void IEquatableMembers_WorksCorrect_ForDefaultCells()
        {
            var cell = new Cell();
            var other = new Cell();

            CustomAssert.IEqualityMembersWorkForEqualObjects(cell, other);
        }
        
        [Test]
        public void IEquatableMembers_WorksCorrect_ForSameCell()
        {
            var cell = new Cell();

            CustomAssert.IEqualityMembersWorkForEqualObjects(cell, cell);
        }

        [TestCase(XType.Character, 250, 1000)]
        [TestCase(XType.DoorClosed, 123, 12)]
        public void IEquatableMembers_WorksCorrect_IfCellsEqual(
            XType type, int x, int y)
        {
            var cell = new Cell
            {
                Type = type,
                Location = new Point(x, y)
            };

            var other = new Cell
            {
                Type = type,
                Location = new Point(x, y)
            };

            CustomAssert.IEqualityMembersWorkForEqualObjects(cell, other);
        }

        [TestCase(XType.Wall, XType.StairsDown)]
        [TestCase(XType.Empty, XType.Character)]
        public void IEquatableMembers_WorksCorrect_IfIsVisitedDiffers(
            XType type1, XType type2)
        {
            var cell = new Cell
            {
                Type = type1
            };
            var other = new Cell
            {
                Type = type2
            };

            CustomAssert.IEquatableMembersWorkForDifferentObjects(cell, other);
        }

        [TestCase(0, 0, 0, 1)]
        [TestCase(-5, 0, 0, 0)]
        [TestCase(10, 10, -10, 10)]
        [TestCase(250, 999, 250, 998)]
        public void IEquatableMembers_WorksCorrect_IfLocationDiffers(
            int x1, int y1, int x2, int y2)
        {
            var cell = new Cell
            {
                Location = new Point(x1, y1)
            };

            var other = new Cell
            {
                Location = new Point(x2, y2)
            };

            CustomAssert.IEquatableMembersWorkForDifferentObjects(cell, other);
        }
        #endregion
    }
}
