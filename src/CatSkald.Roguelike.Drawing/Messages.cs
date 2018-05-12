using System;
using System.Collections.Generic;
using System.Text;
using CatSkald.Roguelike.Core;

namespace CatSkald.Roguelike.Drawing
{
    public static class Messages
    {
        public static readonly string GameStatusSpace = "      ";
        public static readonly string StartGame = "";
        public static readonly string EndGame = "Game over!";
        public static readonly string Bye = "Have a nice day :)";
        public static readonly string CannotMove = "Cannot move there.";
        public static readonly string OpenDoor = "You opened the door.";
        public static readonly string SeePattern = "You see {0}.";
        public static readonly string ObstacleDescriptionPattern = "You faced the {0}.";

        public static void AppendMessage(
            this StringBuilder sb, MessageType type, params string[] args)
        {
            switch (type)
            {
                case MessageType.None:
                    break;
                case MessageType.StartGame:
                    sb.AppendLine(Messages.StartGame);
                    break;
                case MessageType.EndGame:
                    sb.AppendLine(Messages.EndGame);
                    sb.AppendLine();
                    foreach (var message in GetGameInfo())
                    {
                        sb.AppendLine(message);
                    }
                    sb.AppendLine();
                    break;
                case MessageType.CannotMoveThere:
                    sb.AppendLine(Messages.CannotMove);
                    if (args != null)
                    {
                        sb.AppendFormat(ObstacleDescriptionPattern, args[0]);
                        sb.AppendLine();
                    }
                    break;
                case MessageType.OpenDoor:
                    sb.AppendLine(Messages.OpenDoor);
                    break;
                case MessageType.StandOn:
                    if (args != null)
                    {
                        sb.AppendFormat(Messages.SeePattern, args[0]);
                        sb.AppendLine();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        $"Don't know how to write message: {type}.");
            }
        }

        public static IEnumerable<string> GetGameInfo()
        {
            yield return "CHARACTER";
            yield return "Name:  John Doe";
            yield return "Title:     Worm";
            yield return "Age:         21";
            yield return "Race:       Elf";
            yield return "Class:  Warrior";
            //yield return "Height:        ";
            //yield return "Weight:        ";
            yield return "_______________";
            yield return "Level:        1";
            yield return "HP:         1/1";
            yield return "MP:         0/0";
            yield return "ATT:          0";
            yield return "DEF:          0";
            yield return "_______________";
            yield return "XP:         0/1";
            yield return "$:            0";
            yield return "Weight:     0/0";
            yield return "_______________";
            yield return "L.Hand:       -";
            yield return "R.Hand:       -";
            yield return string.Empty;
            yield return "_______________";
            yield return "DUNGEON";
            yield return "Level:       -1";
        }
    }
}
