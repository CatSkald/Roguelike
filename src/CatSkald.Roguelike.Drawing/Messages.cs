﻿using System;
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
                default:
                    throw new ArgumentOutOfRangeException(
                        $"Don't know how to write message: {type}.");
            }
        }

        public static IEnumerable<string> GetGameInfo()
        {
            yield return "CHARACTER INFO";
            yield return "Level:       0";
            yield return "HP:          0";
            yield return "MP:          0";
            yield return "ATT:         0";
            yield return "DEF:         0";
            yield return "";
            yield return "DUNGEON INFO";
            yield return "Level:      -1";
        }
    }
}