using CatSkald.Roguelike.DungeonGenerator.Maps;

namespace CatSkald.Roguelike.DungeonGenerator
{
    public interface IMapBuilderCommand
    {
        void Execute(IMap map);
    }
}