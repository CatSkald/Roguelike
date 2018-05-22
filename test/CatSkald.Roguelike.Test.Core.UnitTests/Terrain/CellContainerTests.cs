using System.Drawing;
using System.Linq;
using CatSkald.Roguelike.Core.Cells;
using CatSkald.Roguelike.Core.Terrain;
using CatSkald.Roguelike.Test.Core.UnitTests.TestHelpers;
using NUnit.Framework;

namespace CatSkald.Roguelike.Test.Core.UnitTests.Terrain
{
    [TestFixture]
    public class CellContainerTests
    {
        private CellContainer<Cell> _container;

        [SetUp]
        public void SetUp()
        {
            _container = new FakeCellContainer(11, 15);
        }

        [Test]
        public void Constructor_CellsAreNotNull_WhenCellContainerCreated()
        {
            Assert.That(_container, Has.All.Not.Null);
        }

        [Test]
        public void Constructor_InitializeCellsMethodIsCalled_WhenCellContainerCreated()
        {
            _container = new FakeCellContainer(32, 57,
                cell => {
                    cell.Type = XType.StairsDown;
                    return cell;
                });

            Assert.That(_container, Has.All
                .With.Property(nameof(Cell.Type))
                .EqualTo(XType.StairsDown));
        }

        [Test]
        public void Constructor_InitializeCellsMethodOverridesCell_WhenCellContainerCreated()
        {
            var singletonCell = new Cell();
            _container = new FakeCellContainer(32, 57, cell => singletonCell);

            Assert.That(_container, Has.All.SameAs(singletonCell));
        }

        [Test]
        public void Constructor_CellsHasUniqueLocations_WhenCellContainerCreated()
        {
            Assert.That(_container.Select(cell => cell.Location), Is.Unique);
        }

        [TestCase(0, 0, 0)]
        [TestCase(1, 2, 5)]
        [TestCase(2, 2, 8)]
        public void Indexer_ReturnCorrectCell(int x, int y, int skip)
        {
            _container = new FakeCellContainer(3, 3);
            var point = new Point(x, y);
            var cell = new Cell { Location = new Point(x, y) };
            var expected = _container.Skip(skip).Take(1).Single();

            Assert.That(_container[cell], Is.SameAs(expected));
        }

        [TestCase(1, 2)]
        [TestCase(4, 4)]
        [TestCase(99, 12)]
        public void Bounds_AreCorrect(int w, int h)
        {
            _container = new FakeCellContainer(w, h);

            Assert.That(_container.Bounds, Is.EqualTo(new Rectangle(0, 0, w, h)));
        }
    }
}
