using CatSkald.Roguelike.DungeonGenerator.Terrain;
using CatSkald.Roguelike.Core.Parameters;
using CatSkald.Tools;

namespace CatSkald.Roguelike.DungeonGenerator.Commands
{
    public abstract class AbstractMapBuilderCommand : IMapBuilderCommand
    {
        public void Execute(IMap map, DungeonParameters parameters)
        {
            Throw.IfNull(map, nameof(map));
            Throw.IfNull(parameters, nameof(parameters));
            Throw.IfNull(parameters.RoomParameters, nameof(parameters.RoomParameters));
            ValidateParameters(parameters);

            ExecuteCommand(map, parameters);
        }

        protected abstract void ValidateParameters(DungeonParameters parameters);
        protected abstract void ExecuteCommand(IMap map, DungeonParameters parameters);
    }
}
