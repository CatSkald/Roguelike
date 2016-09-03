using CatSkald.Roguelike.DungeonGenerator.Maps;
using CatSkald.Roguelike.DungeonGenerator.Parameters;
using CatSkald.Tools;

namespace CatSkald.Roguelike.DungeonGenerator.Commands
{
    public abstract class AbstractMapBuilderCommand : IMapBuilderCommand
    {
        public void Execute(IMap map, IDungeonParameters parameters)
        {
            Throw.IfNull(map, nameof(map));
            Throw.IfNull(parameters, nameof(parameters));
            Throw.IfNull(parameters.RoomParameters, nameof(parameters.RoomParameters));
            ValidateParameters(parameters);

            ExecuteCommand(map, parameters);
        }

        protected abstract void ValidateParameters(IDungeonParameters parameters);
        protected abstract void ExecuteCommand(IMap map, IDungeonParameters parameters);
    }
}
