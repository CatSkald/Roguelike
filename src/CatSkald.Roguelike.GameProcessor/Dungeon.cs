using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CatSkald.Roguelike.Core.Cells;
using CatSkald.Roguelike.Core.Cells.Creatures;
using CatSkald.Roguelike.Core.Terrain;

namespace CatSkald.Roguelike.GameProcessor
{
    public class Dungeon : CellContainer<Cell>, IGameDungeon
    {
        private readonly XType[] availableForMove = new[]
            {
                XType.Empty,
                XType.StairsDown,
                XType.StairsUp,
                XType.DoorOpened,
                XType.DoorClosed
            };

        private readonly XType[] availableForStandingOn = new[]
            {
                XType.StairsDown,
                XType.StairsUp
            };

        public Dungeon(int width, int height) : base(width, height, null)
        {
        }

        public Dungeon(IDungeon map) 
            : base(map.Width, map.Height, c => InitializeCell(map, c))
        {
        }

        public Character Character { get; private set; }

        public void PlaceCharacter(Character character)
        {
            Character = character;
        }

        private static Cell InitializeCell(IDungeon map, Cell cell)
        {
            var type = map[cell.Location.X, cell.Location.Y].Type;
            if (type == XType.DoorClosed)
            {
                cell = new Door
                {
                    Location = cell.Location
                };
            }
            else
            {
                cell.Type = type;
            }
            return cell;
        }

        public bool CanMove(Point newLocation)
        {
            return Bounds.Contains(newLocation) && IsCellAvailableForMove();

            bool IsCellAvailableForMove()
            {
                return availableForMove.Contains(this[newLocation].Type);
            }
        }

        public IEnumerable<XType> GetCellContent(Point location)
        {
            var cell = this[location];

            if (availableForStandingOn.Contains(cell.Type))
            {
                yield return cell.Type;
            }
        }
    }
}
