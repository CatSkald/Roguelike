using System.Drawing;
using CatSkald.Roguelike.Core.Cells;
using CatSkald.Roguelike.Core.Cells.Creatures;

namespace CatSkald.Roguelike.GameProcessor.Initialization
{
    internal sealed class MonsterFactory
    {
        public Monster Create(int number, Point location)
        {
            switch (number)
            {
                case 0:
                    return new Monster(
                        new MainStats(5, 5, 0, 0, 1, 0),
                        new Appearance("Goblin", "A small clot of pure green evil.",
                            'g', Color.Green, isVisible: true, isSolid: true, isObstacle: false));
                case 1:
                    return new Monster(
                        new MainStats(1000, 1000, 1000, 1000, 500, 500),
                        new Appearance("Red Dragon", "Beautiful creature.", 
                            'D', Color.Red, isVisible: true, isSolid: true, isObstacle: false))
                    {
                        Condition = Condition.Peaceful
                    };
                case 2:
                    return new Monster(
                        new MainStats(1000, 1000, 1000, 1000, 500, 500),
                        new Appearance("Blue Dragon", "Beautiful creature.",
                            'D', Color.Blue, isVisible: true, isSolid: true, isObstacle: false))
                    {
                        Condition = Condition.Peaceful
                    };
                case 3:
                    return new Monster(
                        new MainStats(1000, 1000, 1000, 1000, 500, 500),
                        new Appearance("Black Dragon", "Beautiful creature.",
                            'D', Color.Black, isVisible: true, isSolid: true, isObstacle: false))
                    {
                        Condition = Condition.Peaceful
                    };
                case 4:
                    return new Monster(
                        new MainStats( 1000, 1000, 1000, 1000, 500, 500),
                        new Appearance("Gold Dragon", "Beautiful creature.", 
                            'D', Color.Gold, isVisible: true, isSolid: true, isObstacle: false))
                    {
                        Condition = Condition.Peaceful
                    };
                default:
                    return new Monster(
                        new MainStats(1, 1, 0, 0, 1, 0),
                        new Appearance("Rat", "...", 
                            'r', Color.DarkGray, isVisible: true, isSolid: true, isObstacle: false))
                    {
                        Condition = Condition.Sleeping
                    };
            }
        }
    }
}
