using System;
using System.Linq;
using CatSkald.Roguelike.DungeonGenerator.Maps;

namespace CatSkald.Roguelike.DungeonGenerator.Commands
{
    public class SparsifyCommand : BaseMapBuilderCommand
    {
        private int _sparseFactor;

        public SparsifyCommand(int sparseFactor)
        {
            if (sparseFactor < 0 || sparseFactor > 100)
                throw new ArgumentOutOfRangeException(
                    nameof(sparseFactor) + 
                    " should be between 0 and 100, but was: " + 
                    sparseFactor);

            _sparseFactor = sparseFactor;
        }

        public override void Execute(IMap map)
        {
            base.Execute(map);

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
