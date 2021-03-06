﻿using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CatSkald.Roguelike.Core.Cells;
using CatSkald.Roguelike.Core.Cells.Creatures;
using CatSkald.Roguelike.Core.Information;
using CatSkald.Roguelike.Core.Terrain;
using CatSkald.Roguelike.GameProcessor;

namespace CatSkald.Roguelike.Test.GameProcessor.UnitTests.TestHelpers
{
    public class FakeDungeon : CellContainer<Cell>, IGameDungeon
    {
        public FakeDungeon() : this(1, 1)
        {
        }

        public FakeDungeon(int width, int height) 
            : base(width, height, InitializeCell)
        {
            Character = new Character(new MainStats(), new Point());
        }
        
        public Character Character { get; set; }
        public List<Monster> Monsters { get; } = new List<Monster>();

        public void PlaceCharacter(Character character)
        {
            Character = character;
        }

        private static Cell InitializeCell(Cell cell)
        {
            cell.Type = XType.Wall;
            return cell;
        }

        public bool CanMove(Point newLocation) => true;

        public IEnumerable<Appearance> GetCellContent(Point location) =>
            Enumerable.Empty<Appearance>();

        public DungeonInformation GetInfo() => new DungeonInformation();
    }
}
