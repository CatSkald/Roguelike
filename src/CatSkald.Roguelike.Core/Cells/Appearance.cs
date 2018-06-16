using System;
using System.Drawing;

namespace CatSkald.Roguelike.Core.Cells
{
    public struct Appearance : IEquatable<Appearance>
    {
        public Appearance(
            char image, Color colour, bool isVisible, bool isSolid, bool isObstacle)
        {
            Image = image;
            Colour = colour;
            IsVisible = isVisible;
            IsSolid = isSolid;
            IsObstacle = isObstacle;
        }

        public char Image { get; }
        public Color Colour { get; }
        public bool IsVisible { get; }
        public bool IsSolid { get; }
        public bool IsObstacle { get; }

        public bool Equals(Appearance other)
        {
            return Image == other.Image 
                && Colour == other.Colour
                && IsVisible == other.IsVisible
                && IsSolid == other.IsSolid
                && IsObstacle == other.IsObstacle;
        }

        public override bool Equals(object obj)
        {
            return (obj is Appearance other) && Equals(other);
        }

        public override int GetHashCode()
        {
            var result = Image.GetHashCode();
            result = (result * 397) ^ Colour.GetHashCode();
            result = (result * 397) ^ IsVisible.GetHashCode();
            result = (result * 397) ^ IsSolid.GetHashCode();
            result = (result * 397) ^ IsObstacle.GetHashCode();
            return result;
        }

        public override string ToString()
        {
            return $"'{Image}'";
        }

        public static bool operator ==(Appearance obj1, Appearance obj2)
        {
            return Equals(obj1, obj2);
        }

        public static bool operator !=(Appearance obj1, Appearance obj2)
        {
            return !Equals(obj1, obj2);
        }
    }
}
