using CatSkald.Roguelike.Core.Parameters;

namespace CatSkald.Roguelike.GameProcessor.Initialization
{
    public interface IDungeonPopulator
    {
        void Fill(IGameDungeon dungeon, DungeonParameters parameters);
    }
}
