namespace CatSkald.Roguelike.Core.Parameters
{
    public sealed class DungeonParameters
    {
        public FillingParameters Population { get; set; } = new FillingParameters();
        public FillingParameters Objects { get; set; } = new FillingParameters();
        public FillingParameters Treasures { get; set; } = new FillingParameters();
    }
}
