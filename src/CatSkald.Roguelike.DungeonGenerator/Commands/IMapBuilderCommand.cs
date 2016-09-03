using CatSkald.Roguelike.DungeonGenerator.Maps;
using CatSkald.Roguelike.DungeonGenerator.Parameters;

namespace CatSkald.Roguelike.DungeonGenerator
{
    public interface IMapBuilderCommand
    {
        void Execute(IMap map, IDungeonParameters parameters);
    }
}