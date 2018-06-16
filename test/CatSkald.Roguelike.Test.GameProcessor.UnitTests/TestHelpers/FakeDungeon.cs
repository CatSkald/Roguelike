using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CatSkald.Roguelike.Core.Cells;
using CatSkald.Roguelike.Core.Cells.Creatures;
using CatSkald.Roguelike.Core.Terrain;
using CatSkald.Roguelike.GameProcessor;

namespace CatSkald.Roguelike.Test.GameProcessor.UnitTests.TestHelpers
{
    public class FakeDungeon : CellContainer<Cell>, IGameDungeon
    {
        public FakeDungeon() : this(1, 1)
        {
        }

        public FakeDungeon(int width, int height) 
            : base(width, height, InitializeCell)
        {
            Character = new Character();
        }
        
        public Character Character { get; set; }

        public void PlaceCharacter(Character character)
        {
            Character = character;
        }

        private static Cell InitializeCell(Cell cell)
        {
            cell.Type = XType.Wall;
            return cell;
        }

        public bool CanMove(Point newLocation)
        {
            return true;
        }

        public IEnumerable<XType> GetCellContent(Point location)
        {
            return Enumerable.Empty<XType>();
        }
    }
}
