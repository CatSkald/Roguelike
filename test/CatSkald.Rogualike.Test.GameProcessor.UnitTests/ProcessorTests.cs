using CatSkald.Rogualike.Test.GameProcessor.UnitTests.TestHelpers;
using CatSkald.Roguelike.Core.Parameters;
using CatSkald.Roguelike.Core.Services;
using CatSkald.Roguelike.GameProcessor.Initialization;
using NSubstitute;
using NUnit.Framework;

namespace CatSkald.Rogualike.Test.GameProcessor.UnitTests
{
    public class ProcessorTests
    {
        [Test]
        public void Initialize_CorrectDungeonIsSet()
        {
            var parameters = new DungeonParameters();
            var dungeon = new FakeDungeon();
            var mapBuilder = Substitute.For<IMapBuilder>();
            mapBuilder.Build(parameters).Returns(dungeon);

            var processor = new Processor(
                mapBuilder,
                Substitute.For<IDungeonPopulator>(),
                Substitute.For<IMapPainter>());
            processor.Initialize(parameters);

            Assert.That(processor.Dungeon, Is.SameAs(dungeon));
        }
    }
}
