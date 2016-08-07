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
