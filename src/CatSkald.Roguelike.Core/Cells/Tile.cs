namespace CatSkald.Roguelike.Core.Cells
{
    public struct Tile
    {
        public Tile(XType type)
        {
            Type = type;
        }

        public XType Type { get; }
    }
}