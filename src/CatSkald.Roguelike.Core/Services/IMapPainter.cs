using CatSkald.Roguelike.Core.Terrain;

namespace CatSkald.Roguelike.Core.Services
{
    public interface IMapPainter
    {
        void DrawMap(MapImage map);
        void DrawMessage(string message);
        void DrawEndGameScreen();
    }
}
