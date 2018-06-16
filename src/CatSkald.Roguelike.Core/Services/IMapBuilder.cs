using CatSkald.Roguelike.Core.Parameters;
using CatSkald.Roguelike.Core.Terrain;

namespace CatSkald.Roguelike.Core.Services
{
    public interface IMapBuilder
    {
        IDungeon Build(MapParameters parameters);
    }
}