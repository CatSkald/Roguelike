using System.Drawing;
using System.Linq;
using CatSkald.Roguelike.Core.Cells;
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

        private static void InitializeCell(IDungeon map, Cell cell)
        {
            cell.Type = map[cell.Location.X, cell.Location.Y].Type;
        }

        public bool CanMove(Point newLocation)
        {
            return Bounds.Contains(newLocation) && IsCellAvailableForMove();

            bool IsCellAvailableForMove()
            {
                return availableForMove.Contains(this[newLocation].Type);
            }
        }
    }
}
