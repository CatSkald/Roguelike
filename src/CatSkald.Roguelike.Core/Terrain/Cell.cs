using System;
using System.Drawing;
using CatSkald.Roguelike.Core.Objects;

namespace CatSkald.Roguelike.Core.Terrain
{
    public sealed class Cell : ICell, IEquatable<Cell>
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