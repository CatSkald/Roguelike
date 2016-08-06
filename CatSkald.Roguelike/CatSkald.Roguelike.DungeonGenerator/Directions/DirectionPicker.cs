using System;
using System.Collections.Generic;
using System.Linq;
using CatSkald.Stuff;

namespace CatSkald.Roguelike.DungeonGenerator.Directions
{
    public class DirectionPicker
    {
        public static readonly int TwistFactorMin = 0;
        public static readonly int TwistFactorMax = TwistFactorUpperBound;

        // This is needed for optional parameter in constructor
        private const int TwistFactorUpperBound = 100;
        private List<Dir> _directions;

        public DirectionPicker(int twistFactor = TwistFactorUpperBound)
        {
            if (twistFactor < TwistFactorMin || twistFactor > TwistFactorMax)
                throw new ArgumentOutOfRangeException(
                    $"twistFactor should be between {TwistFactorMin} and {TwistFactorMax}, but was: {twistFactor}");

            TwistFactor = twistFactor;
            ResetDirections();
        }

        public bool HasDirections => _directions.Count > 0;
        public int TwistFactor { get; set; }
        public Dir LastDirection { get; set; }

        public bool ShouldChangeDirection()
        {
            return TwistFactor > StaticRandom.Next(TwistFactorMin, TwistFactorMax);
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
                    } while (changeDirection && result == LastDirection);
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
