using System.Text;
using CatSkald.Roguelike.Core.Information;
using CatSkald.Roguelike.Core.Messages;
using CatSkald.Roguelike.Core.Services;
using CatSkald.Roguelike.Core.Terrain;
using Colorful;
using Console = System.Console;

namespace CatSkald.Roguelike.Drawing
{
    //TODO fix message overlapping if next shorter than previous
    internal sealed class ConsolePainter : IMapPainter
    {
        public void DrawMap(
            MapImage map, CharacterInformation character, DungeonInformation dungeon)
        {
            var info = Messages.GetGameInfo(character, dungeon);
            var gameInfoEnumerator = info.GetEnumerator();
            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    var tile = map[x, y];
                    Console.ForegroundColor = tile.Appearance.Colour.ToNearestConsoleColor();
                    Console.Write(tile.Appearance.Image);
                }
                if (gameInfoEnumerator.MoveNext())
                {
                    Console.Write(Messages.GameStatusSpace + gameInfoEnumerator.Current);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public void DrawMessage(GameMessage message)
        {
            var sb = new StringBuilder();
            sb.AppendMessage(message.Type, message.Args);
            Console.Write(sb);
        }
    }
}
