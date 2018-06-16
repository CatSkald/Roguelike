using System;
using CatSkald.Roguelike.Core.Cells.Creatures;

namespace CatSkald.Roguelike.Core.Information
{
    public sealed class CharacterInformation
    {
        public CharacterInformation(MainStats mainStats)
        {
            Stats = mainStats ?? throw new ArgumentNullException(nameof(mainStats));
        }

        public string FirstName { get; set; } = "John";
        public string LastName { get; set; } = "Doe";
        public string FullName => 
            string.IsNullOrEmpty(FirstName) ? LastName : $"{FirstName} {LastName}";

        public string Title { get; set; } = "Worm";
        public string Race { get; set; } = "Elf";
        public string Class { get; set; } = "Warrior";

        public int Age { get; set; } = 21;
        public double Height { get; set; } = 1.75;
        public double Weight { get; set; } = 75;

        public int Level { get; set; } = 1;
        public long XP { get; set; } = 0;
        public long XPForNextLevel => Level + 1;

        public double Money { get; set; } = 0;
        public double BagWeight => 0;
        public double MaxBagWeight => Weight/2;

        public MainStats Stats { get; set; }
    }
}
