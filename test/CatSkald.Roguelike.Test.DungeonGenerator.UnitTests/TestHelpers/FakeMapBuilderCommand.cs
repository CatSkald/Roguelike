using CatSkald.Roguelike.DungeonGenerator.Commands;
using CatSkald.Roguelike.DungeonGenerator.Maps;
using CatSkald.Roguelike.DungeonGenerator.Parameters;

namespace CatSkald.Roguelike.Test.DungeonGenerator.UnitTests.TestHelpers
{
    public class FakeMapBuilderCommand : AbstractMapBuilderCommand
    {
        public int ExecuteCommandCalls { get; set; }
        public int ValidateParametersCalls { get; set; }

        protected override void ExecuteCommand(
            IMap map, IDungeonParameters parameters)
        {
            ExecuteCommandCalls++;
        }

        protected override void ValidateParameters(IDungeonParameters parameters)
        {
            ValidateParametersCalls++;
        }
    }
}