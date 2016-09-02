using CatSkald.Roguelike.DungeonGenerator.Maps;

namespace CatSkald.Roguelike.Drawing.Converters
{
    public interface ITilesConverter<T>
    {
        Tile[,] ConvertToTiles(T dungeon);
    }
}
