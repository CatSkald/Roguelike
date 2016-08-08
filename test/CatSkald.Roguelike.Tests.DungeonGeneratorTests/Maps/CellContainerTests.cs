using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CatSkald.Roguelike.DungeonGenerator.Maps;
using NUnit.Framework;

namespace CatSkald.Roguelike.Tests.DungeonGeneratorTests.Maps
{
    [TestFixture]
    public class CellContainerTests
    {
        private CellContainer _container;

        [SetUp]
        public void SetUp()
        {
            _container = new FakeCellContainer(10, 15);
        }

        #region Properties
        [TestCase(10, 10)]
        [TestCase(1, 2)]
        [TestCase(4, 4)]
        [TestCase(100, 25)]
        public void Width_ReturnsCorrectValue(int w, int h)
        {
            _container = new FakeCellContainer(w, h);

            Assert.That(_container.Width, Is.EqualTo(w));
        }

        [TestCase(10, 10)]
        [TestCase(1, 2)]
        [TestCase(4, 4)]
        [TestCase(100, 25)]
        public void Height_ReturnsCorrectValue(int w, int h)
        {
            _container = new FakeCellContainer(w, h);

            Assert.That(_container.Height, Is.EqualTo(h));
        }

        [TestCase(10, 10)]
        [TestCase(1, 2)]
        [TestCase(4, 4)]
        [TestCase(100, 25)]
        public void Size_IsCorrect(int w, int h)
        {
            _container = new FakeCellContainer(w, h);

            Assert.That(_container.Size, Is.EqualTo(w * h));
        }

        [TestCase(10, 10)]
        [TestCase(1, 2)]
        [TestCase(4, 4)]
        [TestCase(100, 25)]
        public void Bounds_AreCorrect(int w, int h)
        {
            _container = new FakeCellContainer(w, h);

            Assert.That(_container.Bounds, Is.EqualTo(new Rectangle(0, 0, w, h)));
        }
        #endregion

        #region IEnumerable
        [Test]
        public void Enumerator_TraversesInCorrectOrder()
        {
            _container = new FakeCellContainer(3, 3);

            var queue = new Queue<Cell>(_container.Size);
            queue.Enqueue(_container[0, 0]);
            queue.Enqueue(_container[0, 1]);
            queue.Enqueue(_container[0, 2]);
            queue.Enqueue(_container[1, 0]);
            queue.Enqueue(_container[1, 1]);
            queue.Enqueue(_container[1, 2]);
            queue.Enqueue(_container[2, 0]);
            queue.Enqueue(_container[2, 1]);
            queue.Enqueue(_container[2, 2]);

            Assert.That(_container.Size, Is.EqualTo(queue.Count));

            foreach (var item in _container)
            {
                Assert.That(item, Is.SameAs(queue.Dequeue()));
            }
        }

        [TestCase(10, 10)]
        [TestCase(1, 2)]
        [TestCase(4, 4)]
        [TestCase(100, 25)]
        public void ToList_IsCorrect(int w, int h)
        {
            _container = new FakeCellContainer(w, h);

            Assert.That(_container.ToList(), Has.Count.EqualTo(w * h));
        }
        #endregion
    }
}
