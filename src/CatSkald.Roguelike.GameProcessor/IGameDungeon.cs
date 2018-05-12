using System.Drawing;
using CatSkald.Roguelike.Core.Cells;
using CatSkald.Roguelike.Core.Terrain;

namespace CatSkald.Roguelike.GameProcessor
{
    public interface IGameDungeon : IDungeon
    {
        Character Character { get; }

        void PlaceCharacter(Character character);

        bool CanMove(Point newLocation);
    }
}
