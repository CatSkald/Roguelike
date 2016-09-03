namespace CatSkald.Roguelike.DungeonGenerator.Parameters
{
    public interface IDungeonParameters
    {
        int Width { get; }
        int Height { get; }
        int TwistFactor { get; }
        int CellSparseFactor { get; }
        int DeadEndSparseFactor { get; }
        RoomParameters RoomParameters { get; }
    }
}
