namespace CatSkald.Roguelike.Core.Cells
{
    public struct Tile
    {
        private readonly XType type;

        public Tile(XType type)
        {
            this.type = type;
        }

        public XType Type => type;
    }
}