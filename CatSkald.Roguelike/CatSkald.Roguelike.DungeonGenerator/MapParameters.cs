namespace CatSkald.Roguelike.DungeonGenerator
{
    public class MapParameters
    {
        public MapParameters()
        {
        }
        public MapParameters(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public int Width { get; set; }
        public int Height { get; set; }
    }
}