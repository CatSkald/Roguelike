using System.Collections.Generic;
using System.Drawing;
using CatSkald.Roguelike.Core.Cells;
using CatSkald.Roguelike.Core.Cells.Creatures;
using CatSkald.Roguelike.Core.Information;
using CatSkald.Roguelike.Core.Terrain;

namespace CatSkald.Roguelike.GameProcessor
{
    public interface IGameDungeon : IDungeon
    {
        Character Character { get; }

        void PlaceCharacter(Character character);

        bool CanMove(Point newLocation);

        IEnumerable<Appearance> GetCellContent(Point location);
        DungeonInformation GetInfo();
    }
}
