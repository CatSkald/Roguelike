using System;
using System.Diagnostics;
using System.Drawing;

namespace CatSkald.Roguelike.DungeonGenerator.Maps
{
    [DebuggerDisplay("{Location}(IsVisited:{IsVisited})")]
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
        public bool IsVisited { get; set; }
        public Sides Sides { get; }

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
