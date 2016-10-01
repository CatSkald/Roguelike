using System;
using CatSkald.Roguelike.Core.Cells;
using CatSkald.Roguelike.Core.Parameters;
using CatSkald.Roguelike.Core.Services;
using CatSkald.Roguelike.Core.Terrain;
using CatSkald.Roguelike.GameProcessor.Procession;
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

        public ProcessResult Process(GameAction action)
        {
            var actionResult = ProcessAction(action);

            var mapPicture = GetMapPicture(Dungeon);

            painter.DrawMap(mapPicture);
            painter.DrawMessage(Message);

            return actionResult;
        }
        
        public ProcessResult ProcessSubAction(GameAction action)
        {
            var actionResult = ProcessAction(action);

            painter.DrawMessage(Message);

            return actionResult;
        }

        private ProcessResult ProcessAction(GameAction action)
        {
            var result = ProcessResult.None;

            switch (action)
            {
                case GameAction.StartGame:
                    Message = "Welcome to our dungeon, brave Hero!";
                    result = ProcessResult.RequestAction;
                    break;
                case GameAction.None:
                    Message = string.Empty;
                    break;
                case GameAction.MoveN:
                case GameAction.MoveNE:
                case GameAction.MoveNW:
                case GameAction.MoveE:
                case GameAction.MoveS:
                case GameAction.MoveSE:
                case GameAction.MoveSW:
                case GameAction.MoveW:
                    result = MoveCharacter(action);
                    break;
                case GameAction.PickUp:
                    Message = string.Empty;
                    break;
                case GameAction.Equip:
                    Message = string.Empty;
                    break;
                case GameAction.ShowHelp:
                    Message = "Help";
                    break;
                case GameAction.ShowMenu:
                    Message = "Menu";
                    break;
                case GameAction.EndGame:
                    painter.DrawEndGameScreen();
                    Message = "Game over!";
                    break;
                default:
                    throw new NotSupportedException(
                        "Action is not supported: " + action);
            }

            return result;
        }

        private ProcessResult MoveCharacter(GameAction action)
        {
            var character = Dungeon.Character;
            var newLocation = character.Location;
            Message = string.Empty;

            switch (action)
            {
                case GameAction.MoveN:
                    newLocation = DirHelper.MoveInDir(
                        character.Location, Dir.N);
                    break;
                case GameAction.MoveNE:
                    newLocation = DirHelper.MoveInDir(
                        character.Location, Dir.NE);
                    break;
                case GameAction.MoveNW:
                    newLocation = DirHelper.MoveInDir(
                        character.Location, Dir.NW);
                    break;
                case GameAction.MoveE:
                    newLocation = DirHelper.MoveInDir(
                        character.Location, Dir.E);
                    break;
                case GameAction.MoveS:
                    newLocation = DirHelper.MoveInDir(
                        character.Location, Dir.S);
                    break;
                case GameAction.MoveSE:
                    newLocation = DirHelper.MoveInDir(
                        character.Location, Dir.SE);
                    break;
                case GameAction.MoveSW:
                    newLocation = DirHelper.MoveInDir(
                        character.Location, Dir.SW);
                    break;
                case GameAction.MoveW:
                    newLocation = DirHelper.MoveInDir(
                        character.Location, Dir.W);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        "Action is not a move: " + action);
            }

            if (Dungeon[newLocation].Type == XType.Empty)
            {
                character.Location = newLocation;
            }
            else
            {
                Message = "Cannot go there. There is: " 
                    + Dungeon[newLocation].Type;
            }

            return ProcessResult.None;
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
