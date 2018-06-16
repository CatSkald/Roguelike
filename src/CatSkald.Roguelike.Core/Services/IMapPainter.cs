using CatSkald.Roguelike.Core.Information;
using CatSkald.Roguelike.Core.Messages;
using CatSkald.Roguelike.Core.Terrain;

namespace CatSkald.Roguelike.Core.Services
{
    public interface IMapPainter
    {
        void DrawMap(MapImage map, CharacterInformation characterInfo, DungeonInformation dungeonInfo);
        void DrawMessage(GameMessage message);
    }
}
