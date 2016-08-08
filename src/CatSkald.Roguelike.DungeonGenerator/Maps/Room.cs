﻿using System.Diagnostics;
using CatSkald.Roguelike.DungeonGenerator.Directions;

namespace CatSkald.Roguelike.DungeonGenerator.Maps
{
    [DebuggerDisplay("[{Width},{Height}]")]
    public sealed class Room : CellContainer
    {
        public Room(int width, int height) 
            : base(width, height, cell => CellInitializer(cell, width, height))
        {
        }

        private static void CellInitializer(Cell cell, int width, int height)
        {
            cell.Sides[Dir.W] = cell.Location.X == 0 ? Side.Wall : Side.Empty;
            cell.Sides[Dir.E] = cell.Location.X == width - 1 ? Side.Wall : Side.Empty;
            cell.Sides[Dir.N] = cell.Location.Y == 0 ? Side.Wall : Side.Empty;
            cell.Sides[Dir.S] = cell.Location.Y == height - 1 ? Side.Wall : Side.Empty;
        }
    }
}