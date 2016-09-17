using CatSkald.Roguelike.Core.Services;
using CatSkald.Roguelike.Core.Terrain;
using CatSkald.Roguelike.DungeonGenerator;
using CatSkald.Roguelike.DungeonGenerator.Parameters;
using NLog;

namespace CatSkald.Roguelike.GameProcessor.Initialization
{
    public class Processor : IProcessor
    {
        private readonly IMapPainter painter;
        private readonly IDungeonPopulator populator;
        private readonly IMapConverter converter;
        private readonly IMapBuilder builder;
        private static Logger Log = LogManager.GetCurrentClassLogger();

        public Processor(
            IMapBuilder builder, 
            IMapConverter converter, 
            IDungeonPopulator populator,
            IMapPainter painter)
        {
            this.builder = builder;
            this.converter = converter;
            this.populator = populator;
            this.painter = painter;
        }

        public IDungeon Dungeon { get; private set; }
        public string Message { get; private set; }

        public void Initialize(IDungeonParameters parameters)
        {
            var map = builder.Build(parameters);
            Dungeon = converter.ConvertToDungeon(map);
            populator.Fill(Dungeon);
            Log.Debug("Dungeon initialized.");
        }

        public bool Process()
        {
            painter.DrawMap(Dungeon);
            painter.DrawMessage("Welcome to our dungeon, brave Hero!");
            return true;
        }
    }
}
