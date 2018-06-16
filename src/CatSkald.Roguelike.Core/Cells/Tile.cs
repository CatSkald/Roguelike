namespace CatSkald.Roguelike.Core.Cells
{
    public struct Tile
    {
        public Tile(Appearance appearance)
        {
            Appearance = appearance;
        }

        public Appearance Appearance { get; }
    }
}