using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CatSkald.Roguelike.Core.Terrain;

namespace CatSkald.Roguelike.DungeonGenerator.Terrain
{
    public sealed class Sides : IEnumerable<KeyValuePair<Dir, Side>>, IEquatable<Sides>
    {
        private readonly Dictionary<Dir, Side> _directions;

        public Sides()
        {
            var knownDirections = DirHelper.GetNonEmptyDirs();

            _directions = new Dictionary<Dir, Side>(knownDirections.Count);
            foreach (var dir in knownDirections)
            {
                _directions.Add(dir, Side.Wall);
            }
        }

        public int Count => _directions.Count;
        public ICollection<Dir> Keys => _directions.Keys;
        public ICollection<Side> Values => _directions.Values;

        public Side this[Dir direction]
        {
            get { return _directions[direction]; }
            set { _directions[direction] = value; }
        }

        #region IEquatable
        public bool Equals(Sides other)
        {
            return this.Count == other.Count
                && _directions.All(it => other[it.Key] == it.Value);
        }

        public override int GetHashCode()
        {
            var result = Count.GetHashCode();
            foreach (var item in _directions)
            {
                result = (result * 397) ^ item.GetHashCode();
            }
            return result;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != this.GetType())
                return false;
            return this.Equals((Sides)obj);
        }
        #endregion

        #region IEnumerable
        public IEnumerator<KeyValuePair<Dir, Side>> GetEnumerator()
        {
            return _directions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}
