using System;
using System.Linq;
using CatSkald.Roguelike.Core.Cells;
using CatSkald.Roguelike.Core.Cells.Creatures;
using CatSkald.Roguelike.Core.Parameters;
using CatSkald.Tools;

namespace CatSkald.Roguelike.GameProcessor.Initialization
{
    public class DungeonPopulator : IDungeonPopulator
    {
        public void Fill(IGameDungeon dungeon, DungeonParameters parameters)
        {
            //TODO create all the stuff and place into dungeon

            var emptyCells = dungeon.Where(cell => cell.Type == XType.Empty).ToList();

            var stairs = CreateStairs();
            CreateCharacter(stairs.upstairsCell);
            CreateMonsters();

            void CreateMonsters()
            {
                var factory = new MonsterFactory(parameters.Population);
                var count = Convert.ToInt32(emptyCells.Count * parameters.Population.Density / 100);
                for (int i = count; i > 0; i--)
                {
                    var den = emptyCells[StaticRandom.Next(emptyCells.Count)];
                    var monster = factory.CreateIn(den.Location);
                    dungeon.Monsters.Add(monster);
                    emptyCells.Remove(den);
                }
            }

            (Cell upstairsCell, Cell downstairsCell) CreateStairs()
            {
                var upstairsPosition = StaticRandom.Next(emptyCells.Count);
                var downstairsPosition = StaticRandom.NextNotEqualToOld(
                    0, emptyCells.Count, upstairsPosition);

                var upstairsCell = emptyCells[upstairsPosition];
                dungeon[upstairsCell].Type = XType.StairsUp;
                emptyCells.Remove(upstairsCell);

                var downstairsCell = emptyCells[downstairsPosition];
                dungeon[downstairsCell].Type = XType.StairsDown;
                emptyCells.Remove(downstairsCell);

                return (upstairsCell, downstairsCell);
            }

            void CreateCharacter(Cell startingCell)
            {
                var stats = new MainStats(5, 5, 1, 0);
                var character = new Character(stats, startingCell.Location);
                dungeon.PlaceCharacter(character);
            }
        }
    }
}
