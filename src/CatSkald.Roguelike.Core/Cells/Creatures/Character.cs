using System.Drawing;

namespace CatSkald.Roguelike.Core.Cells.Creatures
{
    public sealed class Character : Creature
    {
        public Character(MainStats stats, Point location) 
            : base(stats, location, XType.Character)
        {
        }

        //TODO tests
        public override Appearance RealAppearance =>
            new Appearance('@', Color.White, isVisible: true, isSolid: true, isObstacle: false);
    }
}
