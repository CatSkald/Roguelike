using System;
using CatSkald.Roguelike.DungeonGenerator.Maps;

namespace CatSkald.Roguelike.Drawing.Painters
{
    public sealed class TilesMapPainter : IMapPainter
    {
        public void PaintMap(IMap map)
        {
            var mapConverter = new MapConverter();
            var tiles = mapConverter.ConvertToTiles(map);

            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                for (int x = 0; x < tiles.GetLength(0); x++)
                {
                    var sides = (char)tiles[x, y];
                    Console.Write(sides);
                }
                Console.WriteLine();
            }
        }
    }
}
