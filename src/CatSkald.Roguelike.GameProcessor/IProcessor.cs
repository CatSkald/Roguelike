using System.Collections.Generic;
using CatSkald.Roguelike.Core.Messages;
using CatSkald.Roguelike.Core.Parameters;
using CatSkald.Roguelike.GameProcessor.Procession;

namespace CatSkald.Roguelike.GameProcessor
{
    public interface IProcessor
    {
        IGameDungeon Dungeon { get; }
        IList<GameMessage> Messages { get; }

        void Initialize(MapParameters parameters);

        ProcessResult Process(GameAction action);
        ProcessResult ProcessSubAction(GameAction action);
    }
}
