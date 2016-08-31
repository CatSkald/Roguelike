using CatSkald.Roguelike.DungeonGenerator.Maps;

namespace CatSkald.Roguelike.Drawing.Painters
{
    public interface IMapPainter
    {
        void PaintMap(IMap map);
    }
}
