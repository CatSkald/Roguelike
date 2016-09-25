﻿using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CatSkald.Roguelike.Core.Objects;
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

        #region Constructor
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
                });

            Assert.That(_container, Has.All
                .With.Property(nameof(Cell.Type))
                .EqualTo(XType.StairsDown));
        }

        [Test]
        public void Constructor_CellsHasUniqueLocations_WhenCellContainerCreated()
        {
            Assert.That(_container.Select(cell => cell.Location), Is.Unique);
        }
        #endregion

        #region Indexers
        [TestCase(5, 5)]
        [TestCase(1, 4)]
        [TestCase(7, 2)]
        public void AllIndexers_ReturnCorrectCell(int x, int y)
        {
            var point = new Point(x, y);
            var cell = new Cell { Location = new Point(x, y) };
            var expected = _container.Single(it => it.Location == point);

            Assert.That(_container[x, y], Is.SameAs(expected), "XYIndexer not working");
            Assert.That(_container[point], Is.SameAs(expected), "PointIndexer not working");
            Assert.That(_container[cell], Is.SameAs(expected), "CellIndexer not working");
        }

        [Test]
        public void PointIndexer_ReturnsCorrectCell_IfCellIsFirst()
        {
            const int X = 0, Y = 0;
            var point = new Point(X, Y);
            var cell = new Cell { Location = new Point(X, Y) };
            var expected = _container.First();

            Assert.That(_container[X, Y], Is.SameAs(expected), "XYIndexer not working");
            Assert.That(_container[point], Is.SameAs(expected), "PointIndexer not working");
            Assert.That(_container[cell], Is.SameAs(expected), "CellIndexer not working");
        }

        [Test]
        public void PointIndexer_ReturnsCorrectCell_IfCellIsLast()
        {
            var x = _container.Width - 1;
            var y = _container.Height - 1;
            var point = new Point(x, y);
            var cell = new Cell { Location = new Point(x, y) };
            var expected = _container.Last();

            Assert.That(_container[x, y], Is.SameAs(expected), "XYIndexer not working");
            Assert.That(_container[point], Is.SameAs(expected), "PointIndexer not working");
            Assert.That(_container[cell], Is.SameAs(expected), "CellIndexer not working");
        }
        #endregion

        #region Properties
        [TestCase(1, 2)]
        [TestCase(4, 4)]
        [TestCase(100, 25)]
        public void Width_ReturnsCorrectValue(int w, int h)
        {
            _container = new FakeCellContainer(w, h);

            Assert.That(_container.Width, Is.EqualTo(w));
        }

        [TestCase(1, 2)]
        [TestCase(4, 4)]
        [TestCase(100, 25)]
        public void Height_ReturnsCorrectValue(int w, int h)
        {
            _container = new FakeCellContainer(w, h);

            Assert.That(_container.Height, Is.EqualTo(h));
        }

        [TestCase(1, 2)]
        [TestCase(3, 3)]
        [TestCase(100, 25)]
        public void Size_EqualTo_WidthMultiplyHeight(int w, int h)
        {
            _container = new FakeCellContainer(w, h);

            Assert.That(_container.Size, Is.EqualTo(w * h));
        }

        [TestCase(2, 3)]
        [TestCase(4, 4)]
        [TestCase(101, 15)]
        public void Size_EqualTo_Count(int w, int h)
        {
            _container = new FakeCellContainer(w, h);

            Assert.That(_container.Size, Is.EqualTo(_container.Count()));
        }

        [TestCase(1, 2)]
        [TestCase(4, 4)]
        [TestCase(99, 12)]
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