﻿using System;
using System.Text;
using CatSkald.Roguelike.Core;
using CatSkald.Roguelike.Core.Cells;
using CatSkald.Roguelike.Core.Services;
using CatSkald.Roguelike.Core.Terrain;

namespace CatSkald.Roguelike.Drawing
{
    public sealed class ConsolePainter : IMapPainter
    {
        public void DrawMap(MapImage map)
        {
            var gameInfoEnumerator = Messages.GetGameInfo().GetEnumerator();
            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    Console.ForegroundColor = SetColor(map[x, y].Type);
                    Console.Write(GetImage(map[x, y].Type));
                }
                if (gameInfoEnumerator.MoveNext())
                {
                    Console.Write(Messages.GameStatusSpace + gameInfoEnumerator.Current);
                }
                Console.WriteLine();
            }
        }

        public void DrawMessage(GameMessage message, params string[] args)
        {
            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendMessage(message.Type, message.Args);
            Console.Write(sb);
        }

        public void DrawEndGameScreen()
        {
            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendMessage(MessageType.EndGame);
            Console.Write(sb);
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
