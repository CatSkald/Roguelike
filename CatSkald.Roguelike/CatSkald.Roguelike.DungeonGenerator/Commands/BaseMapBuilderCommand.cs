using System;
using CatSkald.Roguelike.DungeonGenerator.Maps;

namespace CatSkald.Roguelike.DungeonGenerator.Commands
{
    public abstract class BaseMapBuilderCommand : IMapBuilderCommand
    {
        public virtual void Execute(IMap map)
        {
            if (map == null)
                throw new ArgumentNullException(nameof(map));
        }
    }
}
