using System;
using System.Drawing;

namespace CatSkald.Roguelike.Core.Cells
{
    public class Cell : ICell, IEquatable<Cell>
    {
        public Cell()
        {
        }

        public Cell(Point location, XType type)
        {
            Location = location;
            Type = type;
        }

        public Point Location { get; set; }
        public XType Type { get; set; }

        public virtual Appearance Appearance =>
            new Appearance(GetImage(), GetColor(), true, GetIsSolid(), GetIsObstacle());

        private bool GetIsObstacle()
        {
            var isObstacle = false;
            switch (Type)
            {
                case XType.Wall:
                    isObstacle = true;
                    break;
                case XType.Empty:
                case XType.StairsUp:
                case XType.StairsDown:
                    isObstacle = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"{Type} is not mapped.");
            }

            return isObstacle;
        }

        private bool GetIsSolid()
        {
            var isSolid = false;
            switch (Type)
            {
                case XType.Wall:
                case XType.StairsUp:
                case XType.StairsDown:
                    isSolid = true;
                    break;
                case XType.Empty:
                    isSolid = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"{Type} is not mapped.");
            }

            return isSolid;
        }

        private char GetImage()
        {
            var image = ' ';
            switch (Type)
            {
                case XType.Wall:
                    image = '#';
                    break;
                case XType.Empty:
                    image = '.';
                    break;
                case XType.StairsUp:
                    image = '>';
                    break;
                case XType.StairsDown:
                    image = '<';
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"{Type} is not mapped.");
            }

            return image;
        }

        private Color GetColor()
        {
            switch (Type)
            {
                case XType.StairsUp:
                case XType.StairsDown:
                    return Color.Blue;
                case XType.Wall:
                case XType.Empty:
                    return Color.Gray;
                default:
                    throw new ArgumentOutOfRangeException($"{Type} is not mapped.");
            }
        }

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