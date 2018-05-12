using System.Linq;
using CatSkald.Roguelike.Core.Cells;
using CatSkald.Tools;

namespace CatSkald.Roguelike.GameProcessor.Initialization
{
    public class DungeonPopulator : IDungeonPopulator
    {
        public void Fill(IGameDungeon dungeon)
        {
            //TODO create all the stuff and place into dungeon

            CreateStairs(dungeon);
            CreateCharacter(dungeon);
        }

        private static void CreateStairs(IGameDungeon dungeon)
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

        private static void CreateCharacter(IGameDungeon dungeon)
        {
            var startingCell = GetCharacterStartingCell();
            var character = new Character
            {
                Location = startingCell.Location
            };
            dungeon.PlaceCharacter(character);

            Cell GetCharacterStartingCell()
            {
                return dungeon.Single(c => c.Type == XType.StairsUp);
            }
        }
    }
}
