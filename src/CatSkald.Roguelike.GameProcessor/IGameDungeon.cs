using CatSkald.Roguelike.Core.Cells;
using CatSkald.Roguelike.Core.Terrain;

namespace CatSkald.Roguelike.GameProcessor
{
    public interface IGameDungeon : IDungeon
    {
        Character Character { get; set; }
    }
}
