namespace CatSkald.Roguelike.Core.Parameters
{
    public sealed class MapParameters
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int TwistFactor { get; set; }
        public int CellSparseFactor { get; set; }
        public int DeadEndSparseFactor { get; set; }
        public RoomParameters Room { get; set; } = new RoomParameters();
    }
}