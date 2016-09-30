using CatSkald.Roguelike.Core.Parameters;

namespace CatSkald.Roguelike.GameProcessor
{
    public interface IProcessor
    {
        IGameDungeon Dungeon { get; }
        string Message { get; }

        void Initialize(DungeonParameters parameters);
        bool Process();
    }
}
