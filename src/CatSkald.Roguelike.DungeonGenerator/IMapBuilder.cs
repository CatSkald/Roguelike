using CatSkald.Roguelike.DungeonGenerator.Maps;

namespace CatSkald.Roguelike.DungeonGenerator
{
    public interface IMapBuilder
    {
        void SetParameters(DungeonParameters parameters);
        IMap Build();
    }
}