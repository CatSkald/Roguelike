using CatSkald.Roguelike.DungeonGenerator.Maps;
using CatSkald.Roguelike.DungeonGenerator.Parameters;

namespace CatSkald.Roguelike.DungeonGenerator
{
    public interface IMapBuilder
    {
        IMap Build(IDungeonParameters parameters);
    }
}