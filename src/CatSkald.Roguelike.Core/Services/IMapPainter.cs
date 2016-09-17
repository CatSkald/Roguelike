using CatSkald.Roguelike.Core.Terrain;

namespace CatSkald.Roguelike.Core.Services
{
    public interface IMapPainter
    {
        void DrawMap(IDungeon map);
        void DrawMessage(string message);
    }
}
