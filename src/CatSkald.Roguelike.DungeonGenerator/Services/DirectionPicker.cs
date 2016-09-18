using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CatSkald.Roguelike.Core.Terrain;
using CatSkald.Tools;

namespace CatSkald.Roguelike.DungeonGenerator.Services
{
    [DebuggerDisplay("HasDirs:{HasDirections},Last:{LastDirection}")]
    public sealed class DirectionPicker : IDirectionPicker
    {
        private const int TwistFactorMin = 0;
        private const int TwistFactorMax = 100;
        private int _twistFactor;

        private List<Dir> _directions;

        public DirectionPicker(int twistFactor)
        {
            SetTwistFactor(twistFactor);
            ResetDirections();
        }

        public int TwistFactor => _twistFactor;
        public bool HasDirections => _directions.Count > 0;
        public Dir LastDirection { get; set; }

        public bool ShouldChangeDirection() =>
            _twistFactor > StaticRandom.Next(TwistFactorMin, TwistFactorMax);

        public void SetTwistFactor(int twistFactor)
        {
            Throw.IfNotInRange(TwistFactorMin, TwistFactorMax, twistFactor,
                nameof(twistFactor));

            _twistFactor = twistFactor;
        }

        public Dir NextDirectionExcept(Dir direction)
        {
            _directions.Remove(direction);

            return NextDirection();
        }

        public Dir NextDirection()
        {
            if (!HasDirections)
                throw new InvalidOperationException("No directions to choose.");

            Dir result;
            if (_directions.Count == 1)
            {
                result = _directions.Single();
                _directions.Clear();
            }
            else
            {
                var changeDirection = ShouldChangeDirection();
                if (LastDirection != Dir.Zero
                    && !changeDirection
                    && _directions.Contains(LastDirection))
                {
                    result = LastDirection;
                    _directions.Remove(result);
                }
                else
                {
                    int index;
                    do
                    {
                        index = StaticRandom.Next(_directions.Count);
                        result = _directions[index];
                    }
                    while (changeDirection && result == LastDirection);
                    _directions.RemoveAt(index);
                    LastDirection = result;
                }
            }

            return result;
        }

        public void ResetDirections()
        {
            _directions = DirHelper.GetNonEmptyDirs().ToList();
        }
    }
}
