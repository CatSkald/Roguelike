﻿using CatSkald.Roguelike.DungeonGenerator.Terrain;
using CatSkald.Roguelike.Core.Parameters;

namespace CatSkald.Roguelike.DungeonGenerator
{
    internal interface IMapBuilderCommand
    {
        void Execute(IMap map, MapParameters parameters);
    }
}