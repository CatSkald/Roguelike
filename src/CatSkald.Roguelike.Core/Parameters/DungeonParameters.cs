namespace CatSkald.Roguelike.Core.Parameters
{
    public sealed class DungeonParameters
    {
        public DungeonParameters()
        {
            RoomParameters = new RoomParameters();
        }

        public int Width { get; set; }
        public int Height { get; set; }
        public int TwistFactor { get; set; }
        public int CellSparseFactor { get; set; }
        public int DeadEndSparseFactor { get; set; }
        public RoomParameters RoomParameters { get; set; }
    }
}