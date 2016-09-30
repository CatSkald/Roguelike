using CatSkald.Roguelike.Core.Parameters;
using CatSkald.Roguelike.Core.Services;
using CatSkald.Roguelike.Core.Terrain;
using NLog;

namespace CatSkald.Roguelike.GameProcessor.Initialization
{
    public class Processor : IProcessor
    {
        private readonly IMapPainter painter;
        private readonly IDungeonPopulator populator;
        private readonly IMapBuilder builder;
        private static Logger Log = LogManager.GetCurrentClassLogger();

        public Processor(
            IMapBuilder builder, 
            IDungeonPopulator populator,
            IMapPainter painter)
        {
            this.builder = builder;
            this.populator = populator;
            this.painter = painter;
        }

        public IGameDungeon Dungeon { get; private set; }
        public string Message { get; private set; }

        public void Initialize(DungeonParameters parameters)
        {
            var dungeonMap = builder.Build(parameters);
            var dungeon = Convert(dungeonMap);

            populator.Fill(dungeon);
            Dungeon = dungeon;
            Log.Debug("Dungeon initialized.");
        }

        private static Dungeon Convert(IDungeon dungeonMap)
        {
            return new Dungeon(dungeonMap);
        }

        public bool Process()
        {
            var mapPicture = GetMapPicture(Dungeon);

            painter.DrawMap(mapPicture);
            painter.DrawMessage("Welcome to our dungeon, brave Hero!");
            return true;
        }

        private static MapImage GetMapPicture(IGameDungeon dungeon)
        {
            var image = new MapImage(dungeon.Width, dungeon.Height);

            for (int x = 0; x < dungeon.Width; x++)
                for (int y = 0; y < dungeon.Height; y++)
                {
                    var cell = dungeon[x, y];
                    image.SetTile(cell.Location, cell.Type);
                }

            image.SetTile(dungeon.Character.Location, dungeon.Character.Type);

            return image;
        }
    }
}
