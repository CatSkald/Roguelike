using CatSkald.Roguelike.DungeonGenerator.Terrain;
using CatSkald.Roguelike.Core.Parameters;

namespace CatSkald.Roguelike.DungeonGenerator
{
    public interface IMapBuilderCommand
    {
        void Execute(IMap map, DungeonParameters parameters);
    }
}