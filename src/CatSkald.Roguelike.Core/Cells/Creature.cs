namespace CatSkald.Roguelike.Core.Cells
{
    public class Creature : Cell
    {
        public int HP { get; protected set; }
        public int Att { get; protected set; }
        public int Def { get; protected set; }
    }
}
