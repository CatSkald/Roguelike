using System;
using System.Collections.Generic;
using System.Linq;
using CatSkald.Stuff;

namespace CatSkald.Roguelike.DungeonGenerator.Directions
{
    public class DirectionPicker
    {
        private List<Dir> _directions;

        public DirectionPicker()
        {
            Reset();
        }

        public bool HasDirections => _directions.Count > 0;

        public Dir GetRandomDirection()
        {
            if (!HasDirections)
            {
                throw new InvalidOperationException("No directions to choose.");
            }

            Dir result;
            if (_directions.Count == 1)
            {
                result = _directions.Single();
                _directions.Clear();
            }
            else
            {
                var index = StaticRandom.Next(_directions.Count - 1);
                result = _directions[index];
                _directions.RemoveAt(index);
            }

            return result;
        }

        public void Reset()
        {
            _directions = DirHelper.GetNonEmptyDirs().ToList();
        }
    }
}
