using CatSkald.Roguelike.DungeonGenerator.Commands;
using CatSkald.Roguelike.DungeonGenerator.Terrain;
using CatSkald.Roguelike.Core.Parameters;

namespace CatSkald.Roguelike.Test.DungeonGenerator.UnitTests.TestHelpers
{
    internal class FakeMapBuilderCommand : AbstractMapBuilderCommand
    {
        public int ExecuteCommandCalls { get; set; }
        public int ValidateParametersCalls { get; set; }

        protected override void ExecuteCommand(
            IMap map, DungeonParameters parameters)
        {
            ExecuteCommandCalls++;
        }

        protected override void ValidateParameters(DungeonParameters parameters)
        {
            ValidateParametersCalls++;
        }
    }
}