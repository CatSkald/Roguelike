namespace CatSkald.Roguelike.Core.Terrain
{
    public sealed class Cell
    {
        public Cell(int x, int y, string image)
        {
            X = x;
            Y = y;
            Image = image;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public string Image { get; set; }
    }
}