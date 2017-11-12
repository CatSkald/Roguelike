using CatSkald.Roguelike.GameProcessor.Initialization;
using Microsoft.Extensions.DependencyInjection;

namespace CatSkald.Roguelike.GameProcessor
{
    public sealed class GameProcessingModule
    {
        public void Register(IServiceCollection services)
        {
            services.AddScoped<IDungeonPopulator, DungeonPopulator>();
            services.AddScoped<IProcessor, Processor>();
        }
    }
}
