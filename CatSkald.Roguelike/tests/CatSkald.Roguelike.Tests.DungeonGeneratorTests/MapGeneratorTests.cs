using CatSkald.Roguelike.DungeonGenerator;
using CatSkald.Roguelike.DungeonGenerator.Maps;
using NUnit.Framework;

namespace CatSkald.Roguelike.Tests.DungeonGeneratorTests
{
    [TestFixture]
    public class MapGeneratorTests
    {
        private MapBuilder _mg;
        private MapParameters _p;

        [SetUp]
        public void SetUp()
        {
            _mg = new MapBuilder();
            _p = new MapParameters
            {
                Width = 10,
                Height = 10
            };
        }

        [Test]
        public void GenerateMap_ReturnsMapWithOneVisitedCell()
        {
            var map = _mg.Build(_p);

            Assert.That(map, Has.All.With.Property(nameof(Cell.IsVisited)).EqualTo(true));
        }

        [TestCase(2)]
        [TestCase(11)]
        [TestCase(144)]
        public void GenerateMap_ReturnsMapWithCorrectHeight(int h)
        {
            _p.Height = h;
            Assert.That(_mg.Build(_p).Height, Is.EqualTo(h));
        }

        [TestCase(2)]
        [TestCase(14)]
        [TestCase(144)]
        public void GenerateMap_ReturnsMapWithCorrectWidth(int w)
        {
            _p.Width = w;
            Assert.That(_mg.Build(_p).Width, Is.EqualTo(w));
        }

        [Test]
        public void GenerateMap_ReturnsNotNull()
        {
            Assert.That(_mg.Build(_p), Is.Not.Null);
        }
    }
}
