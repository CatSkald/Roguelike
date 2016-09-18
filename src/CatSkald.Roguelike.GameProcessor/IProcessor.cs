using CatSkald.Roguelike.Core.Parameters;
using CatSkald.Roguelike.Core.Terrain;

namespace CatSkald.Roguelike.GameProcessor
{
    public interface IProcessor
    {
        IDungeon Dungeon { get; }
        string Message { get; }

        void Initialize(DungeonParameters parameters);
        bool Process();
    }
}
