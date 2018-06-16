using System.Drawing;
using CatSkald.Roguelike.Core.Information;

namespace CatSkald.Roguelike.Core.Cells.Creatures
{
    public sealed class Character : Creature
    {
        public Character(MainStats stats, Point location) 
            : base(stats, location, XType.Character)
        {
        }

        public CharacterSheet Details { get; set; } = new CharacterSheet();

        public override Appearance RealAppearance =>
            new Appearance(Details.FullName, Details.Story, '@', Color.White, isVisible: IsVisible);

        public CharacterInformation GetInfo()
        {
            return new CharacterInformation(Details, Stats);
        }
    }
}
