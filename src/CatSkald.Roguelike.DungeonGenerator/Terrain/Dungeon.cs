using CatSkald.Roguelike.Core.Terrain;

namespace CatSkald.Roguelike.DungeonGenerator.Terrain
{
    public class Dungeon : CellContainer<Cell>, IDungeon
    {
        public Dungeon(int width, int height) : base(width, height, null)
        {
        }
    }
}
