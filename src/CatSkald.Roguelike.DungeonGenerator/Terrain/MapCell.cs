using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using CatSkald.Roguelike.Core.Cells;

namespace CatSkald.Roguelike.DungeonGenerator.Terrain
{
    [DebuggerDisplay("{Location}(IsWall:{IsWall})")]
    internal sealed class MapCell : IEquatable<MapCell>, ICell
    {
        public MapCell()
        {
            Sides = new Sides();
        }

        public Point Location { get; set; }
        public Sides Sides { get; }
        public bool IsVisited { get; set; }
        public bool IsCorridor { get; set; }
        public bool IsWall =>
            Sides.Values.All(it => it == Side.Wall);
        public bool IsDeadEnd =>
            Sides.Values.Count(it => it != Side.Wall) == 1;

        #region IEquatable
        public bool Equals(MapCell other)
        {
            return Location == other.Location
                && IsVisited == other.IsVisited
                && Sides.Equals(other.Sides);
        }

        public override int GetHashCode()
        {
            var result = IsVisited.GetHashCode();
            result = (result * 397) ^ Location.GetHashCode();
            result = (result * 397) ^ Sides.GetHashCode();
            return result;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != this.GetType())
                return false;
            return this.Equals((MapCell)obj);
        }
        #endregion
    }
}
