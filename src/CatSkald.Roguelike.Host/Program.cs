using System;
using CatSkald.Roguelike.Core.Parameters;
using CatSkald.Roguelike.GameProcessor;
using CatSkald.Roguelike.GameProcessor.Procession;
using Microsoft.Extensions.Configuration;
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

                var provider = BuildServiceProvider();
                StartApplication(provider);
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Oops, you've just catched a fatal bug.");
                throw;
            }

            Console.ReadLine();
        }

        private static void StartApplication(IServiceProvider provider)
        {
            var processor = provider.GetService<IProcessor>();
            processor.Initialize(GatherParameters());

            RunGame(processor);
        }

        private static void RunGame(IProcessor processor)
        {
            var finish = false;
            var result = processor.Process(GameAction.StartGame);
            while (!finish)
            {
                var action = GetUserAction();
                Console.Clear();
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

        private static GameAction GetUserAction()
        {
            var key = Console.ReadKey();

            return ConsoleGameActionConverter.Convert(key.Key);
        }

        private static IServiceProvider BuildServiceProvider()
        {
            var services = new ServiceCollection();
            services.AddTransient(_ => GatherParameters())
                .AddLogging()
                .AddMapBuilding()
                .AddMapProcessing()
                .AddMapPainting();

            return services.BuildServiceProvider();
        }

        private static DungeonParameters GatherParameters()
        {
            var builder = new ConfigurationBuilder()
                    .AddJsonFile("AppSettings.json")
                    .AddEnvironmentVariables();

            var configuration = builder.Build();

            return new DungeonParameters
            {
                Width = configuration.GetValue<int>("Map:Width"),
                Height = configuration.GetValue<int>("Map:Height"),
                CellSparseFactor = configuration.GetValue<int>("Map:CellSparseFactor"),
                DeadEndSparseFactor = configuration.GetValue<int>("Map:DeadEndSparseFactor"),
                TwistFactor = configuration.GetValue<int>("Map:TwistFactor"),
                RoomParameters = new RoomParameters
                {
                    Count = configuration.GetValue<int>("Map:Room:Count"),
                    MinWidth = configuration.GetValue<int>("Map:Room:MinWidth"),
                    MaxWidth = configuration.GetValue<int>("Map:Room:MaxWidth"),
                    MinHeight = configuration.GetValue<int>("Map:Room:MinHeight"),
                    MaxHeight = configuration.GetValue<int>("Map:Room:MaxHeight")
                }
            };
        }
    }
}
