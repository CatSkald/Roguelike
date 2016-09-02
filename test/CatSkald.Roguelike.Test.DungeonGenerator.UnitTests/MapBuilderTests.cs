using CatSkald.Roguelike.DungeonGenerator;
using NUnit.Framework;

namespace CatSkald.Roguelike.Test.DungeonGenerator.UnitTests
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
                Height = 10,
                RoomParameters = new RoomParameters()
            };
            _builder = new MapBuilder(_params);
        }

        [TestCase(1)]
        [TestCase(3)]
        public void Build_ReturnsMapWithCorrectRoomCount(int count)
        {
            _params.RoomParameters.MaxWidth = 5;
            _params.RoomParameters.MaxHeight = 5;
            _params.RoomParameters.Count = count;
            _builder.SetParameters(_params);

            var map = _builder.Build();

            Assert.That(map.Rooms, Has.Count.EqualTo(count));
        }
        
        [TestCase(1)]
        [TestCase(11)]
        [TestCase(144)]
        public void Build_ReturnsMapWithCorrectHeight(int h)
        {
            _params.Height = h;
            _builder.SetParameters(_params);

            var map = _builder.Build();

            Assert.That(map.Height, Is.EqualTo(h));
        }

        [TestCase(1)]
        [TestCase(14)]
        [TestCase(144)]
        public void Build_ReturnsMapWithCorrectWidth(int w)
        {
            _params.Width = w;
            _builder.SetParameters(_params);

            var map = _builder.Build();

            Assert.That(map.Width, Is.EqualTo(w));
        }

        [Test]
        public void SetParameters_ShouldThrow_IfParametersNull()
        {
            Assert.That(() => _builder.SetParameters(null), Throws.ArgumentNullException);
        }
    }
}
