using System;
using System.Linq;
using CatSkald.Roguelike.DungeonGenerator.Terrain;
using CatSkald.Roguelike.Core.Parameters;
using CatSkald.Tools;

namespace CatSkald.Roguelike.DungeonGenerator.Commands
{
    public sealed class SparsifyCellsCommand : AbstractMapBuilderCommand
    {
        protected override void ExecuteCommand(IMap map, DungeonParameters parameters)
        {
            var sparseFactor = parameters.CellSparseFactor;
            var expectedNumberOfRemovedCells =
                   (int)Math.Ceiling(map.Size * (sparseFactor / 100m)) - 1;

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
                        .Single(s => s.Value != Side.Wall)
                        .Key;

                    map.CreateWall(cell, emptySide);
                    cell.IsCorridor = false;
                    nonWalls.Remove(cell);
                    removedCellsCount++;
                }
            }
        }

        protected override void ValidateParameters(DungeonParameters parameters)
        {
            Throw.IfNotInRange(0, 100, parameters.CellSparseFactor,
                nameof(parameters.CellSparseFactor));
        }
    }
}
