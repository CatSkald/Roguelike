using CatSkald.Roguelike.Core.Messages;
using CatSkald.Roguelike.Core.Terrain;

namespace CatSkald.Roguelike.Core.Services
{
    public interface IMapPainter
    {
        void DrawMap(MapImage map);
        void DrawMessage(GameMessage message, params string[] args);
        void DrawEndGameScreen();
    }
}
