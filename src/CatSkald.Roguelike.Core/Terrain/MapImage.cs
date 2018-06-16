using System.Drawing;
using CatSkald.Roguelike.Core.Cells;

namespace CatSkald.Roguelike.Core.Terrain
{
    public class MapImage : BaseContainer<Tile>
    {
        public MapImage(int width, int height) : base(width, height)
        {
        }

        public void SetTile(Point location, Appearance appearance)
        {
            this[location.X, location.Y] = new Tile(appearance);
        }
    }
}
