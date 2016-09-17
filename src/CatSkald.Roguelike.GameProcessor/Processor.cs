using CatSkald.Roguelike.Core.Terrain;
using CatSkald.Roguelike.DungeonGenerator;
using CatSkald.Roguelike.DungeonGenerator.Maps;
using CatSkald.Roguelike.DungeonGenerator.Parameters;
using NLog;

namespace CatSkald.Roguelike.GameProcessor
{
    public class Processor : IProcessor
    {
        private readonly IMapBuilder _mapBuilder;
        private static Logger Log = LogManager.GetCurrentClassLogger();

        public IMap Dungeon { get; private set; }
        public string Message { get; private set; }

        public Processor(IMapBuilder mapBuilder)
        {
            _mapBuilder = mapBuilder;
        }

        public void Initialize(IDungeonParameters parameters)
        {
            var map = _mapBuilder.Build(parameters);
            Log.Debug("Map created.");

            Dungeon = map;
        }

        public bool Process()
        {
            Message = "Done";
            return true;
        }
    }
}
