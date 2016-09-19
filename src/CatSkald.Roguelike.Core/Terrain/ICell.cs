using System.Drawing;

namespace CatSkald.Roguelike.Core.Terrain
{
    public interface ICell
    {
        Point Location { get; set; }
    }
}
