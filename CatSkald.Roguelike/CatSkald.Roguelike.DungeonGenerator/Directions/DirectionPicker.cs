using System;
using System.Collections.Generic;
using System.Linq;
using CatSkald.Stuff;

namespace CatSkald.Roguelike.DungeonGenerator.Directions
{
    public class DirectionPicker
    {
        //This is needed for optional parameter in constructor
        private const int twistFactorUpperBound = 100;

        public static readonly int TwistFactorMin = 0;
        public static readonly int TwistFactorMax = twistFactorUpperBound;

        private List<Dir> _directions;
        private Dir _lastDirection;

        public DirectionPicker(int twistFactor = twistFactorUpperBound)
        {
            if (twistFactor < TwistFactorMin || twistFactor > TwistFactorMax)
                throw new ArgumentOutOfRangeException(
                    $"twistFactor should be between {TwistFactorMin} and {TwistFactorMax}, but was: {twistFactor}");

            TwistFactor = twistFactor;
            Reset();
        }

        public bool HasDirections => _directions.Count > 0;
        public int TwistFactor { get; set; }
        public bool ChangeDirection => 
            TwistFactor > StaticRandom.Next(TwistFactorMin, TwistFactorMax);

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
                if (_lastDirection != Dir.Zero 
                    && !ChangeDirection 
                    && _directions.Contains(_lastDirection))
                {
                    result = _lastDirection;
                    _directions.Remove(result);
                }
                else
                {
                    var index = StaticRandom.Next(_directions.Count);
                    result = _directions[index];
                    _directions.RemoveAt(index);
                    _lastDirection = result;
                }
            }

            return result;
        }

        public void Reset(Dir lastDirection = Dir.Zero)
        {
            _lastDirection = lastDirection;
            _directions = DirHelper.GetNonEmptyDirs().ToList();
        }
    }
}
