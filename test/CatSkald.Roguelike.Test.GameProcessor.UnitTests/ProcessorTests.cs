using System.Drawing;
using CatSkald.Roguelike.Test.GameProcessor.UnitTests.TestHelpers;
using CatSkald.Roguelike.Core.Cells;
using CatSkald.Roguelike.Core.Cells.Creatures;
using CatSkald.Roguelike.Core.Parameters;
using CatSkald.Roguelike.Core.Services;
using CatSkald.Roguelike.Core.Terrain;
using CatSkald.Roguelike.GameProcessor;
using CatSkald.Roguelike.GameProcessor.Initialization;
using CatSkald.Roguelike.GameProcessor.Procession;
using NSubstitute;
using NUnit.Framework;
using CatSkald.Roguelike.Core.Messages;

namespace CatSkald.Roguelike.Test.GameProcessor.UnitTests
{
    public class ProcessorTests
    {
        [Test]
        public void Initialize_CorrectDungeonIsSet()
        {
            var dungeon = new FakeDungeon();
            var mapBuilder = Substitute.For<IMapBuilder>();
            mapBuilder.Build(Arg.Any<MapParameters>())
                .Returns(dungeon);

            var processor = new Processor(
                mapBuilder,
                Substitute.For<IDungeonPopulator>(),
                Substitute.For<IMapPainter>());
            processor.Initialize(new MapParameters());

            Assert.That(processor.Dungeon,
                Has.Property(nameof(Dungeon.Size)).EqualTo(dungeon.Size));
        }
        
        [Test]
        public void Initialize_BuilderIsCalled()
        {
            var parameters = new MapParameters();
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
            var parameters = new MapParameters();
            var dungeon = new FakeDungeon();
            var mapBuilder = Substitute.For<IMapBuilder>();
            mapBuilder.Build(parameters).Returns(dungeon);
            var populator = Substitute.For<IDungeonPopulator>();

            var processor = new Processor(
                mapBuilder,
                populator,
                Substitute.For<IMapPainter>());
            processor.Initialize(parameters);

            populator.Received(1).Fill(
                Arg.Is<IGameDungeon>(d => d.Size == dungeon.Size));
        }

        [Test]
        public void Process_PainterDrawMapIsCalledWithCorrectMapImage()
        {
            var parameters = new MapParameters();
            var dungeon = new FakeDungeon(5, 4);
            var mapBuilder = Substitute.For<IMapBuilder>();
            mapBuilder.Build(parameters).Returns(dungeon);
            var populator = Substitute.For<IDungeonPopulator>();
            var character = new Character(new MainStats(), new Point(1, 1));
            populator.WhenForAnyArgs(it => it.Fill(Arg.Any<IGameDungeon>()))
                .Do(d =>
                {
                    d.Arg<IGameDungeon>().PlaceCharacter(character);
                });
            var painter = Substitute.For<IMapPainter>();

            var processor = new Processor(
                mapBuilder,
                populator,
                painter);
            processor.Initialize(parameters);
            processor.Process(GameAction.None);

            painter.Received(1).DrawMap(Arg.Is<MapImage>(d => 
                d.Width == dungeon.Width
                && d.Height == dungeon.Height
                && d[1, 1].Appearance == character.Appearance));
        }

        [Test]
        public void Process_PainterDrawMessageIsCalled()
        {
            var parameters = new MapParameters();
            var dungeon = new FakeDungeon(5, 4);
            var mapBuilder = Substitute.For<IMapBuilder>();
            mapBuilder.Build(parameters).Returns(dungeon);
            var populator = Substitute.For<IDungeonPopulator>();
            var character = new Character(new MainStats(), new Point(1, 1));
            populator.WhenForAnyArgs(it => it.Fill(Arg.Any<IGameDungeon>()))
                .Do(d => 
                {
                    d.Arg<IGameDungeon>().PlaceCharacter(character);
                });
            var painter = Substitute.For<IMapPainter>();

            var processor = new Processor(
                mapBuilder,
                populator,
                painter);
            processor.Initialize(parameters);
            processor.Process(GameAction.StartGame);

            ////TODO test precise message
            painter.Received(1).DrawMessage(Arg.Any<GameMessage>());
        }
    }
}
