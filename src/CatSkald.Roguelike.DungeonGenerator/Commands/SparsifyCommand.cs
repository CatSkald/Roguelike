using System;
using System.Linq;
using CatSkald.Roguelike.DungeonGenerator.Maps;

namespace CatSkald.Roguelike.DungeonGenerator.Commands
{
    public sealed class SparsifyCommand : IMapBuilderCommand
    {
        private int _sparseFactor;

        public SparsifyCommand(int sparseFactor)
        {
            if (sparseFactor < 0 || sparseFactor > 100)
            {
                throw new ArgumentOutOfRangeException(
                      nameof(sparseFactor), sparseFactor, "Should be between 0 and 100");
            }

            _sparseFactor = sparseFactor;
        }

        public void Execute(IMap map)
        {
            if (map == null)
                throw new ArgumentNullException(nameof(map));

            Sparsify(map);
        }

        private static void Sparsify(IMap map)
        {
            foreach (var cell in map)
            {
                if (cell.IsDeadEnd)
                {
                    var emptySide = cell.Sides.Single(s => s.Value == Side.Empty).Key;
                    map.RemoveCorridor(cell, emptySide);
                }
            }
        }
    }
}
