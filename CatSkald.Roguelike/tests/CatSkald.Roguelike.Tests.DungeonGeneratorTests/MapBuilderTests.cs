using CatSkald.Roguelike.DungeonGenerator;
using CatSkald.Roguelike.DungeonGenerator.Maps;
using NUnit.Framework;

namespace CatSkald.Roguelike.Tests.DungeonGeneratorTests
{
    [TestFixture]
    public class MapBuilderTests
    {
        private MapBuilder _builder;
        private DungeonParameters _params;

        [SetUp]
        public void SetUp()
        {
            _params = new DungeonParameters
            {
                Width = 10,
                Height = 10
            };
            _builder = new MapBuilder(_params);
        }

        [Test]
        public void Build_ReturnsMapWithAllVisitedCell()
        {
            var map = _builder.Build();

            Assert.That(map, Has.All.With.Property(nameof(Cell.IsVisited)).EqualTo(true));
        }

        [TestCase(2)]
        [TestCase(11)]
        [TestCase(144)]
        public void Build_ReturnsMapWithCorrectHeight(int h)
        {
            _params.Height = h;
            _builder.SetParameters(_params);
            Assert.That(_builder.Build().Height, Is.EqualTo(h));
        }

        [TestCase(2)]
        [TestCase(14)]
        [TestCase(144)]
        public void Build_ReturnsMapWithCorrectWidth(int w)
        {
            _params.Width = w;
            _builder.SetParameters(_params);
            Assert.That(_builder.Build().Width, Is.EqualTo(w));
        }

        [Test]
        public void Build_ReturnsNotNull()
        {
            _builder.SetParameters(_params);
            Assert.That(_builder.Build(), Is.Not.Null);
        }
    }
}
