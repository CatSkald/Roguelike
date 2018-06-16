using System.Drawing;

namespace CatSkald.Roguelike.Core.Cells.Creatures
{
    public sealed class Monster : Creature
    {
        public Monster(Point location, MainStats stats, Appearance appearance)
            : base(stats, location, XType.Enemy)
        {
            RealAppearance = appearance;
        }

        public override Appearance RealAppearance { get; }
    }
}
