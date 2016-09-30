using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CatSkald.Roguelike.Core.Terrain;
using CatSkald.Roguelike.Test.Core.UnitTests.TestHelpers;
using NUnit.Framework;

namespace CatSkald.Roguelike.Test.Core.UnitTests.Terrain
{
    [TestFixture]
    public class BaseContainerTests
    {
        private BaseContainer<object> _container;

        [SetUp]
        public void SetUp()
        {
            _container = new FakeBaseContainer(11, 15);
        }

        [TestCase(0, 0, 0)]
        [TestCase(1, 2, 5)]
        [TestCase(2, 2, 8)]
        public void Indexers_ReturnCorrectCell(int x, int y, int skip)
        {
            _container = new FakeBaseContainer(3, 3);
            var point = new Point(x, y);
            var expected = _container.Skip(skip).Take(1).Single();

            Assert.That(_container[x, y], Is.SameAs(expected), 
                "XYIndexer not working");
            Assert.That(_container[point], Is.SameAs(expected), 
                "PointIndexer not working");
        }

        #region Properties
        [TestCase(1, 2)]
        [TestCase(4, 4)]
        [TestCase(100, 25)]
        public void Width_ReturnsCorrectValue(int w, int h)
        {
            _container = new FakeBaseContainer(w, h);

            Assert.That(_container.Width, Is.EqualTo(w));
        }

        [TestCase(1, 2)]
        [TestCase(4, 4)]
        [TestCase(100, 25)]
        public void Height_ReturnsCorrectValue(int w, int h)
        {
            _container = new FakeBaseContainer(w, h);

            Assert.That(_container.Height, Is.EqualTo(h));
        }

        [TestCase(1, 2)]
        [TestCase(3, 3)]
        [TestCase(100, 25)]
        public void Size_EqualTo_WidthMultiplyHeight(int w, int h)
        {
            _container = new FakeBaseContainer(w, h);

            Assert.That(_container.Size, Is.EqualTo(w * h));
        }

        [TestCase(2, 3)]
        [TestCase(4, 4)]
        [TestCase(101, 15)]
        public void Size_EqualTo_Count(int w, int h)
        {
            _container = new FakeBaseContainer(w, h);

            Assert.That(_container.Size, Is.EqualTo(_container.Count()));
        }
        #endregion

        #region IEnumerable
        [Test]
        public void Enumerator_TraversesInCorrectOrder()
        {
            _container = new FakeBaseContainer(3, 3);

            var queue = new Queue<object>(_container.Size);
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

        [TestCase(1, 2)]
        [TestCase(4, 4)]
        [TestCase(100, 25)]
        public void ToList_IsCorrect(int w, int h)
        {
            _container = new FakeBaseContainer(w, h);

            Assert.That(_container.ToList(), Has.Count.EqualTo(w * h));
        }
        #endregion
    }
}
