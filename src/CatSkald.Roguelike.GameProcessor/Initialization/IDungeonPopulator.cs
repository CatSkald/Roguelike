using CatSkald.Roguelike.Core.Terrain;

namespace CatSkald.Roguelike.GameProcessor.Initialization
{
    public interface IDungeonPopulator
    {
        void Fill(IDungeon dungeon);
    }
}
