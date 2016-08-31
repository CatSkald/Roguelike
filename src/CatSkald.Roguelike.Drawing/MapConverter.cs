using System.Drawing;
using System.Linq;
using CatSkald.Roguelike.Core.Terrain;
using CatSkald.Roguelike.DungeonGenerator.Maps;

namespace CatSkald.Roguelike.Drawing
{
    public class MapConverter
    {
        public Tile[,] ConvertToTiles(IMap dungeon)
        {
            var tiles = new Tile[
                dungeon.Width * 2 + 1, 
                dungeon.Height * 2 + 1];

            for (var x = 0; x < dungeon.Width * 2 + 1; x++)
            {
                for (var y = 0; y < dungeon.Height * 2 + 1; y++)
                {
                    tiles[x, y] = Tile.Wall;
                }
            }

            foreach (var room in dungeon.Rooms)
            {
                var roomMinPoint = new Point(
                    room.Bounds.Location.X * 2 + 1, 
                    room.Bounds.Location.Y * 2 + 1);
                var roomMaxPoint = new Point(
                    room.Bounds.Right * 2, 
                    room.Bounds.Bottom * 2);

                // Fill tiles with corridor values for each room in dungeon
                for (var i = roomMinPoint.X; i < roomMaxPoint.X; i++)
                    for (var j = roomMinPoint.Y; j < roomMaxPoint.Y; j++)
                        tiles[i, j] = Tile.Empty;
            }

            // Expand it each corridor cell
            foreach (var cell in dungeon.Where(c => c.IsCorridor))
            {
                var tileLocation = new Point(
                    cell.Location.X * 2 + 1, 
                    cell.Location.Y * 2 + 1);

                tiles[tileLocation.X, tileLocation.Y] = Tile.Empty;

                switch (dungeon[cell.Location].Sides[Dir.N])
                {
                    case Side.Empty:
                        tiles[tileLocation.X, tileLocation.Y - 1] = Tile.Empty;
                        break;
                    case Side.Door:
                        tiles[tileLocation.X, tileLocation.Y - 1] = Tile.Door;
                        break;
                }

                switch (dungeon[cell.Location].Sides[Dir.S])
                {
                    case Side.Empty:
                        tiles[tileLocation.X, tileLocation.Y + 1] = Tile.Empty;
                        break;
                    case Side.Door:
                        tiles[tileLocation.X, tileLocation.Y + 1] = Tile.Door;
                        break;
                }

                switch (dungeon[cell.Location].Sides[Dir.W])
                {
                    case Side.Empty:
                        tiles[tileLocation.X - 1, tileLocation.Y] = Tile.Empty;
                        break;
                    case Side.Door:
                        tiles[tileLocation.X - 1, tileLocation.Y] = Tile.Door;
                        break;
                }

                switch (dungeon[cell.Location].Sides[Dir.E])
                {
                    case Side.Empty:
                        tiles[tileLocation.X + 1, tileLocation.Y] = Tile.Empty;
                        break;
                    case Side.Door:
                        tiles[tileLocation.X + 1, tileLocation.Y] = Tile.Door;
                        break;
                }
            }

            return tiles;
        }
    }
}
