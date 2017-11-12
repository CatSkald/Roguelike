using CatSkald.Roguelike.Core.Terrain;
using CatSkald.Roguelike.DungeonGenerator.Terrain;

namespace CatSkald.Roguelike.DungeonGenerator.Services
{
    internal interface IMapConverter
    {
        IDungeon ConvertToDungeon(IMap map);
    }
}
