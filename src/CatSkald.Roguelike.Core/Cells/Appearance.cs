using System.Drawing;

namespace CatSkald.Roguelike.Core.Cells
{
    public struct Appearance
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
    }
}
