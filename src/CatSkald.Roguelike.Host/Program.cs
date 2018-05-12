using System;
using System.Linq;
using CatSkald.Roguelike.Core.Parameters;
using CatSkald.Roguelike.GameProcessor;
using CatSkald.Roguelike.GameProcessor.Procession;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace CatSkald.Roguelike.Host
{
    public static class Program
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();

        public static void Main()
        {
            try
            {
                Log.Debug("Roguelike started.");

                ConfigureConsole();

                var provider = new ServiceCollection()
                    .SetUp()
                    .BuildServiceProvider();

                StartApplication(provider);
            }
            catch (Exception e)
            {
                const string error = "Oops, you've just catched a fatal bug :(";
                Log.Fatal(e, error);

                Console.Clear();
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(error);
                Console.WriteLine();
                Console.WriteLine("Please send the below error to our support service.");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(e.ToString());
            }

            Console.ReadLine();

            void ConfigureConsole()
            {
                Console.CursorVisible = false;
                Console.Title = "CatSkald Roguelike";
            }
        }

        private static void StartApplication(IServiceProvider provider)
        {
            var processor = provider.GetService<IProcessor>();
            processor.Initialize(provider.GetService<DungeonParameters>());

            RunGame(processor);
        }

        private static void RunGame(IProcessor processor)
        {
            var finish = false;
            var result = processor.Process(GameAction.StartGame);
            while (!finish)
            {
                var action = GetUserAction();

                ClearConsole();
                ProcessUserAction(action);

                FillConsoleWithEmptyLines();
            }

            GameAction GetUserAction()
            {
                var key = Console.ReadKey();

                return ConsoleGameActionConverter.Convert(key.Key);
            }

            void ClearConsole()
            {
                Console.SetCursorPosition(0, 0);
            }

            void FillConsoleWithEmptyLines()
            {
                var emptyLinesTillWindowBottom = Console.WindowHeight - Console.CursorTop - 1;
                var emptyLineLength = Console.WindowWidth - 1;
                var emptyLine = new string(' ', emptyLineLength);

                Console.Write(
                    string.Join(Environment.NewLine,
                    Enumerable.Repeat(emptyLine, emptyLinesTillWindowBottom)));
            }

            void ProcessUserAction(GameAction action)
            {
                result = processor.Process(action);
                switch (result)
                {
                    case ProcessResult.None:
                    case ProcessResult.RequestAction:
                        break;
                    case ProcessResult.End:
                        finish = true;
                        break;
                    case ProcessResult.RequestSubAction:
                        processor.ProcessSubAction(GetUserAction());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(
                            "Unknown ProcessResult: " + result);
                }
            }
        }
    }
}
