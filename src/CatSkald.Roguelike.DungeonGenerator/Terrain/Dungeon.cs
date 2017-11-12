using CatSkald.Roguelike.Core.Cells;
using CatSkald.Roguelike.Core.Terrain;

namespace CatSkald.Roguelike.DungeonGenerator.Terrain
{
    internal class Dungeon : CellContainer<Cell>, IDungeon
    {
        public Dungeon(int width, int height) : base(width, height, null)
        {
        }
    }
}
