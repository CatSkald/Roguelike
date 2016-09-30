using CatSkald.Roguelike.Core.Cells;
using CatSkald.Roguelike.Core.Terrain;
using CatSkald.Roguelike.GameProcessor;

namespace CatSkald.Rogualike.Test.GameProcessor.UnitTests.TestHelpers
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

        private static void InitializeCell(Cell cell)
        {
            cell.Type = XType.Wall;
        }
    }
}
