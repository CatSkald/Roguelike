using CatSkald.Roguelike.DungeonGenerator.Maps;
using CatSkald.Roguelike.Core.Parameters;
using CatSkald.Roguelike.Test.DungeonGenerator.UnitTests.TestHelpers;
using NUnit.Framework;

namespace CatSkald.Roguelike.Test.DungeonGenerator.UnitTests.Commands
{
    [TestFixture]
    public class AbstractMapBuilderCommandTests
    {
        [Test]
        public void Execute_ShouldThrow_IfMapNull()
        {
            Map map = null;
            DungeonParameters parameters = new DungeonParameters();
            var command = new FakeMapBuilderCommand();

            Assert.That(() => command.Execute(map, parameters),
                Throws.ArgumentNullException);
        }

        [Test]
        public void Execute_ShouldThrow_IfRoomParametersNull()
        {
            var map = new Map(3, 4);
            DungeonParameters parameters = new DungeonParameters
            {
                RoomParameters = null
            };
            var command = new FakeMapBuilderCommand();

            Assert.That(() => command.Execute(map, parameters),
                Throws.ArgumentNullException);
        }

        [Test]
        public void Execute_ShouldThrow_IfParametersNull()
        {
            var map = new Map(3, 4);
            DungeonParameters parameters = null;
            var command = new FakeMapBuilderCommand();

            Assert.That(() => command.Execute(map, parameters),
                Throws.ArgumentNullException);
        }

        [Test]
        public void Execute_ShouldCallValidateParameters()
        {
            var map = new Map(3, 4);
            DungeonParameters parameters = new DungeonParameters();
            var command = new FakeMapBuilderCommand();

            command.Execute(map, parameters);

            Assert.That(command.ValidateParametersCalls, Is.EqualTo(1));
        }

        [Test]
        public void Execute_ShouldCallExecuteCommand()
        {
            var map = new Map(3, 4);
            DungeonParameters parameters = new DungeonParameters();
            var command = new FakeMapBuilderCommand();

            command.Execute(map, parameters);

            Assert.That(command.ExecuteCommandCalls, Is.EqualTo(1));
        }
    }
}
