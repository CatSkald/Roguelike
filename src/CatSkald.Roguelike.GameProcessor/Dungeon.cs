using CatSkald.Roguelike.Core.Cells;
using CatSkald.Roguelike.Core.Terrain;

namespace CatSkald.Roguelike.GameProcessor
{
    public class Dungeon : CellContainer<Cell>
    {
        public Dungeon(int width, int height) : base(width, height, null)
        {
        }


    }
}
