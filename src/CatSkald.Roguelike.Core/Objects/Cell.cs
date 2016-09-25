using System;
using System.Drawing;

namespace CatSkald.Roguelike.Core.Cells
{
    public class Cell : ICell, IEquatable<Cell>
    {
        public Point Location { get; set; }
        public XType Type { get; set; }

        #region IEquatable
        public bool Equals(Cell other)
        {
            return Location == other.Location
                && Type == other.Type;
        }

        public override int GetHashCode()
        {
            var result = Type.GetHashCode();
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