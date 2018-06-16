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

        public virtual Appearance GetAppearance()
        {
            return new Appearance(GetName(), GetDescription(), GetImage(), GetColor(), 
                isVisible: GetIsVisible(), isSolid: GetIsSolid(), isObstacle: GetIsObstacle());

            bool GetIsVisible()
            {
                return Type != XType.Empty;
            }

            string GetName()
            {
                string description = string.Empty;
                switch (Type)
                {
                    case XType.Wall:
                    case XType.Door:
                        description = Type.ToString();
                        break;
                    case XType.StairsUp:
                        description = "Stairs up";
                        break;
                    case XType.StairsDown:
                        description = "Stairs down";
                        break;
                    case XType.Empty:
                        description = string.Empty;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException($"{Type} is not mapped.");
                }

                return description;
            }

            string GetDescription()
            {
                string description = string.Empty;
                switch (Type)
                {
                    case XType.Wall:
                        description = "A wall.";
                        break;
                    case XType.StairsUp:
                        description = "Stairs leading to the upper level.";
                        break;
                    case XType.StairsDown:
                        description = "Stairs leading to the lover level.";
                        break;
                    case XType.Empty:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException($"{Type} is not mapped.");
                }

                return description;
            }

            bool GetIsObstacle()
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

            bool GetIsSolid()
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

            char GetImage()
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

            Color GetColor()
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