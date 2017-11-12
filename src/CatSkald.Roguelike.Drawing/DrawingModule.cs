using CatSkald.Roguelike.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CatSkald.Roguelike.Drawing
{
    public sealed class DrawingModule
    {
        public void Register(IServiceCollection services)
        {
            services.AddTransient<IMapPainter, ConsolePainter>();
        }
    }
}
