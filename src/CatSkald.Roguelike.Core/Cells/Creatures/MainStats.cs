namespace CatSkald.Roguelike.Core.Cells.Creatures
{
    public struct MainStats
    {
        public MainStats(int hp, int att, int def)
        {
            HP = hp;
            Att = att;
            Def = def;
        }

        public int HP { get; }
        public int Att { get; }
        public int Def { get; }
    }
}
