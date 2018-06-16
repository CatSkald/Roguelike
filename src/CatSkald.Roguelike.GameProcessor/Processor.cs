using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CatSkald.Roguelike.Core.Cells;
using CatSkald.Roguelike.Core.Cells.Creatures;
using CatSkald.Roguelike.Core.Messages;
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
        public IList<GameMessage> Messages { get; } = new List<GameMessage>();

        public void Initialize(MapParameters parameters)
        {
            var dungeonMap = builder.Build(parameters);
            var dungeon = Convert();

            populator.Fill(dungeon);
            Dungeon = dungeon;
            Log.Debug("Dungeon initialized.");

            Dungeon Convert()
            {
                return new Dungeon(dungeonMap);
            }
        }

        public ProcessResult Process(GameAction action)
        {
            var actionResult = ProcessAction(action);

            var mapPicture = GetMapPicture(Dungeon);

            painter.DrawMap(mapPicture);
            DrawMessages();

            return actionResult;
        }

        private void DrawMessages()
        {
            foreach (var message in Messages)
            {
                painter.DrawMessage(message);
            }
        }

        public ProcessResult ProcessSubAction(GameAction action)
        {
            var actionResult = ProcessAction(action);

            DrawMessages();

            return actionResult;
        }

        private ProcessResult ProcessAction(GameAction action)
        {
            var result = ProcessResult.None;
            Messages.Clear();

            switch (action)
            {
                case GameAction.None:
                    break;
                case GameAction.StartGame:
                    Messages.Add(new GameMessage(MessageType.StartGame));
                    result = ProcessResult.RequestAction;
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
                case GameAction.ShowHelp:
                    Messages.Add(new GameMessage(MessageType.ShowHelp));
                    break;
                case GameAction.ShowMenu:
                    Messages.Add(new GameMessage(MessageType.ShowMenu));
                    break;
                case GameAction.EndGame:
                    painter.DrawEndGameScreen();
                    Messages.Add(new GameMessage(MessageType.EndGame));
                    break;
                default:
                    throw new NotSupportedException(
                        "Action is not supported: " + action);
            }

            LookAround();

            return result;

            void LookAround()
            {
                var cellContent = Dungeon.GetCellContent(Dungeon.Character.Location);
                if (cellContent.Any())
                {
                    foreach (var item in cellContent)
                    {
                        //TODO extract object descriptor
                        Messages.Add(new GameMessage(MessageType.StandOn, item.ToString()));
                    }
                }
            }
        }

        //TODO extract class
        private ProcessResult MoveCharacter(GameAction action)
        {
            var character = Dungeon.Character;
            var newLocation = GetNewLocation(action, character);
            if (Dungeon.CanMove(newLocation))
            {
                //TODO extract methods
                var destination = Dungeon[newLocation];
                if (destination is Door door)
                {
                    if (!door.IsOpened)
                    {
                        door.Open();
                        Messages.Add(new GameMessage(MessageType.OpenDoor));
                    }
                }

                character.Location = newLocation;
            }
            else
            {
                Messages.Add(new GameMessage(
                    MessageType.CannotMoveThere,
                    Dungeon[newLocation].Type.ToString()));
            }

            return ProcessResult.None;
        }

        //TODO extract
        private static Point GetNewLocation(GameAction action, Character character)
        {
            Point newLocation;
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

            return newLocation;
        }

        private static MapImage GetMapPicture(IGameDungeon dungeon)
        {
            var image = new MapImage(dungeon.Width, dungeon.Height);

            for (int x = 0; x < dungeon.Width; x++)
                for (int y = 0; y < dungeon.Height; y++)
                {
                    var cell = dungeon[x, y];
                    image.SetTile(cell.Location, cell.Appearance);
                }

            image.SetTile(dungeon.Character.Location, dungeon.Character.Appearance);

            return image;
        }
    }
}
