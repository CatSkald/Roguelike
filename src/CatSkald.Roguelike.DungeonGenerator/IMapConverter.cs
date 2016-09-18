using CatSkald.Roguelike.Core.Terrain;
using CatSkald.Roguelike.DungeonGenerator.Maps;

namespace CatSkald.Roguelike.DungeonGenerator
{
    public interface IMapConverter
    {
        IDungeon ConvertToDungeon(IMap map);
    }
}
