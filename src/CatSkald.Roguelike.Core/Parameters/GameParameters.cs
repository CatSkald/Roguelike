namespace CatSkald.Roguelike.Core.Parameters
{
    public sealed class GameParameters
    {
        public MapParameters Map { get; set; } = new MapParameters();
        public DungeonParameters Dungeon { get; set; } = new DungeonParameters();
    }
}
