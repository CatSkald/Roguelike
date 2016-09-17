using System;
using CatSkald.Roguelike.Core.Objects;
using CatSkald.Roguelike.Core.Services;
using CatSkald.Roguelike.Core.Terrain;

namespace CatSkald.Roguelike.Drawing
{
    public sealed class ConsolePainter : IMapPainter
    {
        public void DrawMap(IDungeon map)
        {
            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    Console.Write(GetImage(map[x, y].Type));
                }
                Console.WriteLine();
            }
        }

        public void DrawMessage(string message)
        {
            Console.WriteLine(message);
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
                    throw new ArgumentOutOfRangeException($"{type} is not mapped.");
            }

            return image;
        }
    }
}
