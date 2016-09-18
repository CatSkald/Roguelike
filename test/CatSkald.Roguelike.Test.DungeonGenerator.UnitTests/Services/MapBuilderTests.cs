using System;
using System.Collections.Generic;
using CatSkald.Roguelike.DungeonGenerator;
using CatSkald.Roguelike.DungeonGenerator.Services;
using CatSkald.Roguelike.Core.Parameters;
using CatSkald.Roguelike.Test.DungeonGenerator.UnitTests.TestHelpers;
using NUnit.Framework;
using NSubstitute;

namespace CatSkald.Roguelike.Test.DungeonGenerator.UnitTests.Services
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
            _builder = new MapBuilder(new List<IMapBuilderCommand>(), 
                Substitute.For<IMapConverter>());
        }

        [Test]
        public void Constructor_Throws_IfCommandsNull()
        {
            Assert.That(() => new MapBuilder(null, Substitute.For<IMapConverter>()),
                Throws.TypeOf<ArgumentNullException>());
        }
        
        [Test]
        public void Constructor_Throws_IfAnyCommand()
        {
            Assert.That(() => 
                    new MapBuilder(new List<IMapBuilderCommand> { null }, 
                    Substitute.For<IMapConverter>()),
                Throws.TypeOf<ArgumentNullException>());
        }
        
        [Test]
        public void Constructor_Throws_IfConverterNull()
        {
            Assert.That(() => new MapBuilder(
                    new List<IMapBuilderCommand>(), 
                    null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Build_ExecutesEveryCommand()
        {
            var commands = new List<IMapBuilderCommand>
            {
                new FakeMapBuilderCommand(),
                new FakeMapBuilderCommand(),
                new FakeMapBuilderCommand(),
                new FakeMapBuilderCommand(),
                new FakeMapBuilderCommand()
            };
            var builder = new MapBuilder(commands, Substitute.For<IMapConverter>());
            builder.Build(_params);

            Assert.That(commands, Has.All
                .With.Property(nameof(FakeMapBuilderCommand.ExecuteCommandCalls))
                .EqualTo(1));
        }

        [Test]
        public void Build_Throws_IfParametersNull()
        {
            DungeonParameters parameters = null;

            Assert.That(() => _builder.Build(parameters),
                Throws.TypeOf<ArgumentNullException>());
        }

        [TestCase(-52)]
        [TestCase(-1)]
        [TestCase(0)]
        public void Build_Throws_IfWidthLess1(int width)
        {
            _params.Width = width;

            Assert.That(() => _builder.Build(_params),
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [TestCase(-25)]
        [TestCase(-1)]
        [TestCase(0)]
        public void Build_Throws_IfHeightLess1(int height)
        {
            _params.Width = height;

            Assert.That(() => _builder.Build(_params),
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }
    }
}
