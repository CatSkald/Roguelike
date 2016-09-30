using CatSkald.Roguelike.Core.Cells;
using CatSkald.Roguelike.Core.Terrain;

namespace CatSkald.Roguelike.GameProcessor
{
    public class Dungeon : CellContainer<Cell>, IGameDungeon
    {
        public Dungeon(int width, int height) : base(width, height, null)
        {
        }

        public Dungeon(IDungeon map) 
            : base(map.Width, map.Height, c => InitializeCell(map, c))
        {
        }

        public Character Character { get; set; }

        private static void InitializeCell(IDungeon map, Cell cell)
        {
            cell.Type = map[cell.Location.X, cell.Location.Y].Type;
        }
    }
}
