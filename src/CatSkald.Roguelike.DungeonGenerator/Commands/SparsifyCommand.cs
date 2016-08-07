using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CatSkald.Roguelike.DungeonGenerator.Maps;
using CatSkald.Tools;

namespace CatSkald.Roguelike.DungeonGenerator.Commands
{
    public sealed class SparsifyCommand : IMapBuilderCommand
    {
        private int _sparseFactor;

        public SparsifyCommand(int sparseFactor)
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
            var nonRemovedCells = map.Where(c => !c.IsWall).ToList();
            while (removedCellsCount < expectedNumberOfRemovedCells)
            {
                foreach (var cell in nonRemovedCells.Where(c => c.IsDeadEnd).ToList())
                {
                    if (!cell.IsDeadEnd)
                        continue;

                    var emptySide = cell.Sides
                        .Single(s => s.Value == Side.Empty)
                        .Key;

                    map.RemoveCorridor(cell, emptySide);
                    nonRemovedCells.Remove(cell);
                    removedCellsCount++;
                }
            }
        }
    }
}
