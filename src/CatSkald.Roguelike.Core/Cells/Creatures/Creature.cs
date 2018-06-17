using System.Drawing;

namespace CatSkald.Roguelike.Core.Cells.Creatures
{
    public abstract class Creature : Cell
    {
        public Creature(MainStats stats, Point location, XType type)
            : base(location, type)
        {
            Stats = stats ?? throw new System.ArgumentNullException(nameof(stats));
        }

        public MainStats Stats { get; set; }

        public abstract Appearance RealAppearance { get; }

        public Condition Condition { get; set; }
        public bool IsVisible { get; protected set; } = true;
        public virtual bool IsAlive => Stats.HP > 0;

        public override Appearance GetAppearance() => 
            new Appearance(RealAppearance.Name, RealAppearance.Description,
                RealAppearance.Image, RealAppearance.Colour, isVisible: IsVisible, RealAppearance.IsSolid, RealAppearance.IsObstacle);
    }
}
