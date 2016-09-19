using System;
using System.Collections.Generic;
using CatSkald.Roguelike.Core.Objects;
using CatSkald.Roguelike.Core.Services;
using CatSkald.Roguelike.Core.Terrain;

namespace CatSkald.Roguelike.Drawing
{
    public sealed class ConsolePainter : IMapPainter
    {
        private const string GameInfoSpace = "      ";

        public void DrawMap(IDungeon map)
        {
            var gameInfoEnumerator = GetGameInfo().GetEnumerator();
            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    Console.ForegroundColor = SetColor(map[x, y].Type);
                    Console.Write(GetImage(map[x, y].Type));
                }
                if (gameInfoEnumerator.MoveNext())
                {
                    Console.Write(GameInfoSpace + gameInfoEnumerator.Current);
                }
                Console.WriteLine();
            }
        }

        public void DrawMessage(string message)
        {
            Console.WriteLine(message);
        }

        private static IEnumerable<string> GetGameInfo()
        {
            yield return "CHARACTER INFO";
            yield return "Level:       0";
            yield return "HP:          0";
            yield return "MP:          0";
            yield return "ATT:         0";
            yield return "DEF:         0";
            yield return "";
            yield return "DUNGEON INFO";
            yield return "Level:       -1";
        }

        private static char GetImage(XType type)
        {
            var image = ' ';

            switch (type)
            {
                case XType.Wall:
                    image = '#';
                    break;
                case XType.Empty:
                    image = '.';
                    break;
                case XType.DoorClosed:
                    image = '+';
                    break;
                case XType.DoorOpened:
                    image = '\'';
                    break;
                case XType.StairsUp:
                    image = '>';
                    break;
                case XType.StairsDown:
                    image = '<';
                    break;
                case XType.Character:
                    image = '@';
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        $"{type} is not mapped.");
            }

            return image;
        }

        private static ConsoleColor SetColor(XType type)
        {
            switch (type)
            {
                case XType.DoorClosed:
                case XType.DoorOpened:
                case XType.StairsUp:
                case XType.StairsDown:
                    return ConsoleColor.DarkYellow;
                case XType.Character:
                    return ConsoleColor.White;
                case XType.Wall:
                case XType.Empty:
                    return ConsoleColor.Gray;
                default:
                    throw new ArgumentOutOfRangeException(
                        $"{type} is not mapped.");
            }
        }
    }
}
