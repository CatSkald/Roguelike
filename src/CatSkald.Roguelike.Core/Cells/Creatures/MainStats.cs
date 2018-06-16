namespace CatSkald.Roguelike.Core.Cells.Creatures
{
    public class MainStats
    {
        public MainStats()
        {
        }

        public MainStats(int maxHP, int maxMP, int att, int def)
        {
            HP = maxHP;
            MaxHP = maxHP;
            MP = maxMP;
            MaxMP = maxMP;
            Att = att;
            Def = def;
        }

        public int HP { get; }
        public int MaxHP { get; }
        public int MP { get; }
        public int MaxMP { get; }
        public int Att { get; }
        public int Def { get; }
    }
}
