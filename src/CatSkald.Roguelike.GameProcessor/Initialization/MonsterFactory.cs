using System;
using System.Collections.Generic;
using System.Drawing;
using CatSkald.Roguelike.Core.Cells;
using CatSkald.Roguelike.Core.Cells.Creatures;
using CatSkald.Roguelike.Core.Parameters;
using CatSkald.Tools;

namespace CatSkald.Roguelike.GameProcessor.Initialization
{
    internal sealed class MonsterFactory
    {
        private const int DifferentMonstersExist = 5;
        private readonly FillingParameters _parameters;

        public MonsterFactory(FillingParameters parameters)
        {
            _parameters = parameters ?? throw new System.ArgumentNullException(nameof(parameters));

            Diversity = _parameters.Diversity;
        }

        public int CurrentNumber { get; set; } = StaticRandom.Next(DifferentMonstersExist);
        public int Diversity { get; set; }

        public Monster CreateIn(Point location)
        {
            CurrentNumber = Diversity > StaticRandom.Next(100)
                ? StaticRandom.NextNotEqualToOld(0, DifferentMonstersExist, CurrentNumber)
                : CurrentNumber;
            return Generate(CurrentNumber, location);
        }

        private Monster Generate(int number, Point location)
        {
            switch (number)
            {
                case 0:
                    return new Monster(
                        new MainStats(5, 0, 1, 0),
                        new Appearance("Goblin", "A small clot of pure green evil.",
                            'g', Color.Green, isVisible: true, isSolid: true, isObstacle: false));
                case 1:
                    return new Monster(
                        new MainStats(1000, 1000, 500, 500),
                        new Appearance("Red Dragon", "Beautiful creature.", 
                            'D', Color.Red, isVisible: true, isSolid: true, isObstacle: false))
                    {
                        Condition = Condition.Peaceful
                    };
                case 2:
                    return new Monster(
                        new MainStats(1000, 1000, 500, 500),
                        new Appearance("Blue Dragon", "Beautiful creature.",
                            'D', Color.Blue, isVisible: true, isSolid: true, isObstacle: false))
                    {
                        Condition = Condition.Peaceful
                    };
                case 3:
                    return new Monster(
                        new MainStats(1000, 1000, 500, 500),
                        new Appearance("Black Dragon", "Beautiful creature.",
                            'D', Color.Black, isVisible: true, isSolid: true, isObstacle: false))
                    {
                        Condition = Condition.Peaceful
                    };
                case 4:
                    return new Monster(
                        new MainStats(1000, 1000, 500, 500),
                        new Appearance("Gold Dragon", "Beautiful creature.", 
                            'D', Color.Gold, isVisible: true, isSolid: true, isObstacle: false))
                    {
                        Condition = Condition.Peaceful
                    };
                default:
                    return new Monster(
                        new MainStats(1, 0, 1, 0),
                        new Appearance("Rat", "...", 
                            'r', Color.DarkGray, isVisible: true, isSolid: true, isObstacle: false))
                    {
                        Condition = Condition.Sleeping
                    };
            }
        }
    }
}
