using System;
using CatSkald.Roguelike.GameProcessor.Procession;

namespace CatSkald.Roguelike.Host
{
    public class ConsoleGameActionConverter
    {
        public static GameAction Convert(ConsoleKey key)
        {
            GameAction result;
            switch (key)
            {
                case ConsoleKey.Escape:
                    result = GameAction.ShowMenu;
                    break;
                case ConsoleKey.LeftArrow:
                case ConsoleKey.NumPad4:
                    result = GameAction.MoveW;
                    break;
                case ConsoleKey.UpArrow:
                case ConsoleKey.NumPad8:
                    result = GameAction.MoveN;
                    break;
                case ConsoleKey.NumPad7:
                    result = GameAction.MoveNW;
                    break;
                case ConsoleKey.NumPad9:
                    result = GameAction.MoveNE;
                    break;
                case ConsoleKey.RightArrow:
                case ConsoleKey.NumPad6:
                    result = GameAction.MoveE;
                    break;
                case ConsoleKey.DownArrow:
                case ConsoleKey.NumPad2:
                    result = GameAction.MoveS;
                    break;
                case ConsoleKey.NumPad1:
                    result = GameAction.MoveSW;
                    break;
                case ConsoleKey.NumPad3:
                    result = GameAction.MoveSW;
                    break;
                case ConsoleKey.Help:
                    result = GameAction.ShowHelp;
                    break;
                case ConsoleKey.F1:
                    result = GameAction.ShowHelp;
                    break;
                case ConsoleKey.NumPad5:
                default:
                    result = GameAction.None;
                    break;
            }

            return result;
        }
    }
}
