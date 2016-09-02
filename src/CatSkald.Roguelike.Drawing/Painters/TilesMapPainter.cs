using System;
using CatSkald.Roguelike.Drawing.Converters;
using CatSkald.Roguelike.DungeonGenerator.Maps;

namespace CatSkald.Roguelike.Drawing.Painters
{
    public sealed class TilesMapPainter : IMapPainter
    {
        private readonly ITilesConverter<IMap> _converter;

        public TilesMapPainter(ITilesConverter<IMap> converter)
        {
            _converter = converter;
        }

        public void PaintMap(IMap map)
        {
            var tiles = _converter.ConvertToTiles(map);

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
