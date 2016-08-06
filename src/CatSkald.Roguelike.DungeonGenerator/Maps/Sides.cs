using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CatSkald.Roguelike.DungeonGenerator.Directions;

namespace CatSkald.Roguelike.DungeonGenerator.Maps
{
    public class Sides : Dictionary<Dir, Side>, IEquatable<Sides>
    {
        public Sides()
        {
            var knownDirections = DirHelper.GetNonEmptyDirs();
            foreach (var dir in knownDirections)
            {
                this.Add(dir, Side.Wall);
            }
        }

        #region IEquatable

        public bool Equals(Sides other)
        {
            return this.Count == other.Count
                && this.All(it => other[it.Key] == it.Value);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != this.GetType())
                return false;
            return this.Equals((Sides)obj);
        }

        #endregion
    }
}
