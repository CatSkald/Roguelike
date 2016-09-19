using System.Linq;
using CatSkald.Roguelike.Core.Objects;
using CatSkald.Roguelike.Core.Terrain;
using CatSkald.Tools;

namespace CatSkald.Roguelike.GameProcessor.Initialization
{
    public class DungeonPopulator : IDungeonPopulator
    {
        public void Fill(IDungeon dungeon)
        {
            ////TODO create all the stuff and place into dungeon

            CreateStairs(dungeon);
        }

        private static void CreateStairs(IDungeon dungeon)
        {
            var emptyCells = dungeon.Where(cell => cell.Type == XType.Empty).ToList();
            var count = emptyCells.Count;
            var upstairsPosition = StaticRandom.Next(emptyCells.Count);
            var downstairsPosition = StaticRandom.NextNotEqualToOld(
                0, emptyCells.Count, upstairsPosition);
            var upstairsCell = emptyCells[upstairsPosition];
            var downstairsCell = emptyCells[downstairsPosition];
            dungeon[upstairsCell].Type = XType.StairsUp;
            dungeon[downstairsCell].Type = XType.StairsDown;
        }
    }
}
