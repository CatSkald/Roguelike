using System.Diagnostics;
using System.Drawing;
using CatSkald.Roguelike.Core.Terrain;

namespace CatSkald.Roguelike.DungeonGenerator.Terrain
{
    [DebuggerDisplay("[{Width},{Height}]")]
    public sealed class Room : CellContainer<MapCell>
    {
        public Room(int width, int height)
            : base(width, height, cell => CellInitializer(cell, width, height))
        {
        }

        public void Offset(Point position)
        {
            var newBounds = Bounds;
            newBounds.Offset(position);
            Bounds = newBounds;
        }

        private static void CellInitializer(MapCell cell, int width, int height)
        {
            cell.Sides[Dir.W] = cell.Location.X == 0 ? Side.Wall : Side.Empty;
            cell.Sides[Dir.E] = cell.Location.X == width - 1 ? Side.Wall : Side.Empty;
            cell.Sides[Dir.N] = cell.Location.Y == 0 ? Side.Wall : Side.Empty;
            cell.Sides[Dir.S] = cell.Location.Y == height - 1 ? Side.Wall : Side.Empty;
        }
    }
}
