using CatSkald.Roguelike.Core.Terrain;
using CatSkald.Roguelike.DungeonGenerator.Parameters;

namespace CatSkald.Roguelike.GameProcessor
{
    public interface IProcessor
    {
        IDungeon Dungeon { get; }
        string Message { get; }

        void Initialize(IDungeonParameters parameters);
        bool Process();
    }
}
