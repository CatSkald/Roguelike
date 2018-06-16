using System.Drawing;

namespace CatSkald.Roguelike.Core.Cells.Creatures
{
    public abstract class Creature : Cell
    {
        public Creature(MainStats stats, Point location, XType type)
            : base(location, type)
        {
        }

        public MainStats Stats { get; set; }

        public abstract Appearance RealAppearance { get; }

        //TODO tests
        public override Appearance Appearance => 
            new Appearance(RealAppearance.Image, RealAppearance.Colour,
                isVisible: true, isSolid: true, isObstacle: false);
    }
}
