using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CatSkald.Roguelike.Core.Cells;
using CatSkald.Roguelike.Core.Cells.Creatures;
using CatSkald.Roguelike.Core.Information;
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

        public Character Character { get; private set; }

        public List<Monster> Monsters { get; } = new List<Monster>();

        public void PlaceCharacter(Character character)
        {
            Character = character;
        }

        private static Cell InitializeCell(IDungeon map, Cell cell)
        {
            var type = map[cell.Location.X, cell.Location.Y].Type;
            if (type == XType.Door)
            {
                cell = new Door(cell.Location);
            }
            else
            {
                cell.Type = type;
            }
            return cell;
        }

        public bool CanMove(Point newLocation)
        {
            if (!Bounds.Contains(newLocation))
                return false;

            var targetCell = this[newLocation];
            return !targetCell.GetAppearance().IsObstacle
                && targetCell.Content.All(c => !c.GetAppearance().IsObstacle);
        }

        public IEnumerable<Appearance> GetCellContent(Point location)
        {
            var cell = this[location];
            var appearance = cell.GetAppearance();

            if (appearance.IsVisible)
            {
                yield return appearance;
            }

            foreach (var contentAppearance in cell.Content.Select(c => c.GetAppearance())
                                                  .Where(a => a.IsVisible))
            {
                yield return contentAppearance;
            }
        }

        public DungeonInformation GetInfo()
        {
            return new DungeonInformation();
        }
    }
}
