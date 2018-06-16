namespace CatSkald.Roguelike.Core.Cells.Creatures
{
    public class MainStats
    {
        public MainStats()
        {
        }

        public MainStats(int hp, int maxHP, int mp, int maxMP, int att, int def)
        {
            HP = hp;
            MaxHP = maxHP;
            MP = mp;
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
