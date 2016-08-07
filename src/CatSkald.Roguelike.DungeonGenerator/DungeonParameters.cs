namespace CatSkald.Roguelike.DungeonGenerator
{
    public sealed class DungeonParameters
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int TwistFactor { get; set; }
        public int CellSparseFactor { get; set; }
        public int DeadEndSparseFactor { get; set; }
        public RoomParameters RoomParameters { get; set; }
    }
}