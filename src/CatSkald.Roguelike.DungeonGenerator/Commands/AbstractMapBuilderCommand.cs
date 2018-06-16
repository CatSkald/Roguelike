using CatSkald.Roguelike.DungeonGenerator.Terrain;
using CatSkald.Roguelike.Core.Parameters;
using CatSkald.Tools;

namespace CatSkald.Roguelike.DungeonGenerator.Commands
{
    internal abstract class AbstractMapBuilderCommand : IMapBuilderCommand
    {
        public void Execute(IMap map, MapParameters parameters)
        {
            Throw.IfNull(map, nameof(map));
            Throw.IfNull(parameters, nameof(parameters));
            Throw.IfNull(parameters.Room, nameof(parameters.Room));
            ValidateParameters(parameters);

            ExecuteCommand(map, parameters);
        }

        protected abstract void ValidateParameters(MapParameters parameters);
        protected abstract void ExecuteCommand(IMap map, MapParameters parameters);
    }
}
