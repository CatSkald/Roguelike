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
            var dungeon = new FakeDungeon();
            var mapBuilder = Substitute.For<IMapBuilder>();
            mapBuilder.Build(Arg.Any<DungeonParameters>())
                .Returns(dungeon);

            var processor = new Processor(
                mapBuilder,
                Substitute.For<IDungeonPopulator>(),
                Substitute.For<IMapPainter>());
            processor.Initialize(new DungeonParameters());

            Assert.That(processor.Dungeon, Is.SameAs(dungeon));
        }
        
        [Test]
        public void Initialize_BuilderIsCalled()
        {
            var parameters = new DungeonParameters();
            var mapBuilder = Substitute.For<IMapBuilder>();

            var processor = new Processor(
                mapBuilder,
                Substitute.For<IDungeonPopulator>(),
                Substitute.For<IMapPainter>());
            processor.Initialize(parameters);

            mapBuilder.Received(1).Build(parameters);
        }

        [Test]
        public void Initialize_PopulatorIsCalled()
        {
            var parameters = new DungeonParameters();
            var dungeon = new FakeDungeon();
            var mapBuilder = Substitute.For<IMapBuilder>();
            mapBuilder.Build(parameters).Returns(dungeon);
            var populator = Substitute.For<IDungeonPopulator>();

            var processor = new Processor(
                mapBuilder,
                populator,
                Substitute.For<IMapPainter>());
            processor.Initialize(parameters);

            populator.Received(1).Fill(dungeon);
        }

        [Test]
        public void Process_PainterIsCalled()
        {
            var parameters = new DungeonParameters();
            var dungeon = new FakeDungeon();
            var mapBuilder = Substitute.For<IMapBuilder>();
            mapBuilder.Build(parameters).Returns(dungeon);
            var painter = Substitute.For<IMapPainter>();

            var processor = new Processor(
                mapBuilder,
                Substitute.For<IDungeonPopulator>(),
                Substitute.For<IMapPainter>());
            processor.Initialize(parameters);
            processor.Process();

            painter.Received(1).DrawMap(dungeon);
        }
    }
}
