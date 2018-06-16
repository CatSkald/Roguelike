using System;
using CatSkald.Roguelike.Core.Cells.Creatures;

namespace CatSkald.Roguelike.Core.Information
{
    public sealed class CharacterInformation
    {
        public CharacterInformation(CharacterSheet details, MainStats stats)
        {
            Details = details ?? throw new ArgumentNullException(nameof(details));
            Stats = stats ?? throw new ArgumentNullException(nameof(stats));
        }

        public CharacterSheet Details { get; set; }
        public MainStats Stats { get; set; }
    }
}
