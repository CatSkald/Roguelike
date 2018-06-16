using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using CatSkald.Roguelike.Core.Cells;
using CatSkald.Roguelike.Core.Information;
using CatSkald.Roguelike.Core.Messages;

namespace CatSkald.Roguelike.Drawing
{
    public static class Messages
    {
        private const int GameInfoTitlesWidth = 10;
        private const int GameInfoValuesWidth = 10;
        private static readonly string GameInfoSeparator = 
            new string('_', GameInfoTitlesWidth + GameInfoValuesWidth);

        public static readonly string GameStatusSpace = "      ";
        public static readonly string StartGame = "";
        public static readonly string EndGame = "Game over!";
        public static readonly string Bye = "Have a nice day :)";
        public static readonly string CannotMove = "Cannot move there.";
        public static readonly string OpenDoor = "You opened the door.";

        public static readonly string SeePattern = "You see {0}.";
        public static readonly string ObstacleDescriptionPattern = "You faced the {0}.";

        public static void AppendMessage(
            this StringBuilder sb, MessageType type, params object[] args)
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
                    var characterInfo = args.OfType<CharacterInformation>().Single();
                    var dungeonInfo = args.OfType<DungeonInformation>().Single();
                    foreach (var message in GetGameInfo(characterInfo, dungeonInfo))
                    {
                        sb.AppendLine(message);
                    }
                    sb.AppendLine();
                    break;
                case MessageType.CannotMoveThere:
                    sb.Append(Messages.CannotMove);
                    if (args != null && args[0] is Appearance obstacle)
                    {
                        sb.Append(" ");
                        sb.AppendFormat(ObstacleDescriptionPattern, obstacle.Name?.ToLower());
                    }
                    sb.AppendLine();
                    break;
                case MessageType.OpenDoor:
                    sb.AppendLine(Messages.OpenDoor);
                    break;
                case MessageType.StandOn:
                    if (args != null && args[0] is Appearance standOn)
                    {
                        sb.AppendFormat(Messages.SeePattern, standOn.Name?.ToLower());
                        sb.AppendLine();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        $"Don't know how to write message: {type}.");
            }
        }

        public static IEnumerable<string> GetGameInfo(
            CharacterInformation characterInfo, DungeonInformation dungeonInfo)
        {
            if (characterInfo == null) throw new ArgumentNullException(nameof(characterInfo));
            if (dungeonInfo == null) throw new ArgumentNullException(nameof(dungeonInfo));

            yield return "CHARACTER";
            yield return "Name:".PadRight(GameInfoValuesWidth) + $"{characterInfo.Details.FullName,GameInfoValuesWidth}";
            yield return "Title:".PadRight(GameInfoValuesWidth) + $"{characterInfo.Details.Title,GameInfoValuesWidth}";
            yield return "Age:".PadRight(GameInfoValuesWidth) + $"{characterInfo.Details.Age,GameInfoValuesWidth}";
            yield return "Race:".PadRight(GameInfoValuesWidth) + $"{characterInfo.Details.Race,GameInfoValuesWidth}";
            yield return "Class:".PadRight(GameInfoValuesWidth) + $"{characterInfo.Details.Class,GameInfoValuesWidth}";
            //yield return "Height:".PadRight(GameInfoValuesWidth) + $"{characterInfo.Details.Height,GameInfoWidth}";
            //yield return "Weight:".PadRight(GameInfoValuesWidth) + $"{characterInfo.Details.Weight,GameInfoWidth}";
            yield return GameInfoSeparator;
            yield return "Level:".PadRight(GameInfoValuesWidth) + $"{characterInfo.Details.Level,GameInfoValuesWidth}";
            yield return "HP:".PadRight(GameInfoValuesWidth) + $"{characterInfo.Stats.HP + "/" + characterInfo.Stats.MaxHP,GameInfoValuesWidth}";
            yield return "MP:".PadRight(GameInfoValuesWidth) + $"{characterInfo.Stats.MP + "/" + characterInfo.Stats.MaxMP,GameInfoValuesWidth}";
            yield return "ATT:".PadRight(GameInfoValuesWidth) + $"{characterInfo.Stats.Att,GameInfoValuesWidth}";
            yield return "DEF:".PadRight(GameInfoValuesWidth) + $"{characterInfo.Stats.Def,GameInfoValuesWidth}";
            yield return GameInfoSeparator;
            yield return "XP:".PadRight(GameInfoValuesWidth) + $"{characterInfo.Details.XP + "/" + characterInfo.Details.XPForNextLevel,GameInfoValuesWidth}";
            yield return "$:".PadRight(GameInfoValuesWidth) + $"{characterInfo.Details.Money,GameInfoValuesWidth}";
            yield return "Weight:".PadRight(GameInfoValuesWidth) + $"{characterInfo.Details.BagWeight + "/" + characterInfo.Details.MaxBagWeight.ToString(CultureInfo.InvariantCulture),GameInfoValuesWidth}";
            yield return GameInfoSeparator;
            yield return "L.Hand:";
            yield return "R.Hand:";
            yield return GameInfoSeparator;
            yield return "DUNGEON";
            yield return "Level:".PadRight(GameInfoValuesWidth) + $"{dungeonInfo.Level,GameInfoValuesWidth}";
        }
    }
}
