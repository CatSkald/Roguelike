﻿using CatSkald.Roguelike.Core.Terrain;

namespace CatSkald.Roguelike.DungeonGenerator.Maps
{
    public interface IDirectionPicker
    {
        int TwistFactor { get; }
        bool HasDirections { get; }

        Dir LastDirection { get; set; }

        void SetTwistFactor(int value);

        bool ShouldChangeDirection();

        Dir NextDirection();
        Dir NextDirectionExcept(Dir direction);

        void ResetDirections();
    }
}
