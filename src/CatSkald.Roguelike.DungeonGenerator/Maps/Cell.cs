using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace CatSkald.Roguelike.DungeonGenerator.Maps
{
    [DebuggerDisplay("{Location}(IsVisited:{IsVisited}, IsWall:{IsWall})")]
    public sealed class Cell : IEquatable<Cell>
    {
        public Cell()
        {
            Sides = new Sides();
        }
        public Cell(int x, int y) : this(new Point(x, y))
        {
        }
        public Cell(Point location) : this()
        {
            Location = location;
        }

        public Point Location { get; set; }
        public Sides Sides { get; }
        public bool IsVisited { get; set; }
        public bool IsWall =>
            Sides.Values.All(it => it == Side.Wall);
        public bool IsDeadEnd => 
            Sides.Values.Count(it => it == Side.Empty) == 1;

        #region IEquatable
        public bool Equals(Cell other)
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
            return this.Equals((Cell)obj);
        }
        #endregion
    }
}
