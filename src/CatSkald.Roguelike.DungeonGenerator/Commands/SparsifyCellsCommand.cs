using System;
using System.Linq;
using CatSkald.Roguelike.DungeonGenerator.Maps;
using CatSkald.Tools;

namespace CatSkald.Roguelike.DungeonGenerator.Commands
{
    public sealed class SparsifyCellsCommand : IMapBuilderCommand
    {
        private int _sparseFactor;

        public SparsifyCellsCommand(int sparseFactor)
        {
            Throw.IfNotInRange(0, 100, sparseFactor, nameof(sparseFactor));

            _sparseFactor = sparseFactor;
        }

        public void Execute(IMap map)
        {
            Throw.IfNull(map, nameof(map));

            Sparsify(map);
        }

        private void Sparsify(IMap map)
        {
            var expectedNumberOfRemovedCells = 
                (int)Math.Ceiling(map.Size * (_sparseFactor / 100m)) - 1;

            var removedCellsCount = 0;
            var nonWalls = map.Where(c => !c.IsWall).ToList();
            if (!nonWalls.Any())
            {
                throw new InvalidOperationException("All cells are walls.");
            }

            while (removedCellsCount < expectedNumberOfRemovedCells)
            {
                foreach (var cell in nonWalls.Where(c => c.IsDeadEnd).ToList())
                {
                    if (!cell.IsDeadEnd)
                        continue;

                    var emptySide = cell.Sides
                        .Single(s => s.Value == Side.Empty)
                        .Key;

                    map.CreateWall(cell, emptySide);
                    nonWalls.Remove(cell);
                    removedCellsCount++;
                }
            }
        }
    }
}
