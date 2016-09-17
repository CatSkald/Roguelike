using CatSkald.Roguelike.DungeonGenerator.Maps;
using CatSkald.Roguelike.DungeonGenerator.Parameters;

namespace CatSkald.Roguelike.GameProcessor
{
    public interface IProcessor
    {
        IMap Dungeon { get; }
        string Message { get; }

        void Initialize(IDungeonParameters parameters);
        bool Process();
    }
}
