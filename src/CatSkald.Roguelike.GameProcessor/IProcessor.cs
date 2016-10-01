using CatSkald.Roguelike.Core.Parameters;
using CatSkald.Roguelike.GameProcessor.Procession;

namespace CatSkald.Roguelike.GameProcessor
{
    public interface IProcessor
    {
        IGameDungeon Dungeon { get; }
        string Message { get; }

        void Initialize(DungeonParameters parameters);

        ProcessResult Process(GameAction action);
        ProcessResult ProcessSubAction(GameAction action);
    }
}
