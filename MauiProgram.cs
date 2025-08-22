using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting; // Microcharts/Skia
using CateonCook.Repositorios;               // IAppDatabase, AppDatabase, ILogService, LogService, IGenericRepository<>, GenericRepository<>
using CateonCook.ViewModels;                 // DashboardViewModel, CatalogoViewModel, FinanciamientoViewModel, RendimientoViewModel, HistorialViewModel
using CateonCook.Views;                      // DashboardPage, CatalogoPage, FinanciamientoPage, RendimientoPage, HistorialPage

namespace CateonCook
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .UseSkiaSharp() // necesario para Microcharts/Skia
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    // Opcional: fuente de íconos (si la agregas a Resources/Fonts)
                    // fonts.AddFont("MaterialSymbolsRounded.ttf", "MaterialRounded");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // ============ Infraestructura ============
            builder.Services.AddSingleton<IAppDatabase, AppDatabase>();
            builder.Services.AddSingleton<ILogService, LogService>();

            // Repos genéricos (opcional, si los usas)
            builder.Services.AddSingleton(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // ============ Repos específicos ============
            // Asegúrate de tener estas clases en tu proyecto
            builder.Services.AddSingleton<IProductoRepository, ProductoRepository>();
            builder.Services.AddSingleton<IVentaRepository, VentaRepository>();
            builder.Services.AddSingleton<IFinanciamientoRepository, FinanciamientoRepository>();
            builder.Services.AddSingleton<IPlanPagoRepository, PlanPagoRepository>();
            builder.Services.AddSingleton<IUsuarioRepository, UsuarioRepository>();

            // ============ ViewModels ============
            builder.Services.AddSingleton<DashboardViewModel>();
            builder.Services.AddTransient<CatalogoViewModel>();
            builder.Services.AddTransient<FinanciamientoViewModel>();
            builder.Services.AddTransient<RendimientoViewModel>();
            builder.Services.AddTransient<HistorialViewModel>();

            // ============ Pages ============
            // Usa inyección de VM por constructor en cada Page
            builder.Services.AddSingleton<DashboardPage>();
            builder.Services.AddTransient<CatalogoPage>();
            builder.Services.AddTransient<FinanciamientoPage>();
            builder.Services.AddTransient<RendimientoPage>();
            builder.Services.AddTransient<HistorialPage>();

            // Si usas AppShell, puedes registrarlo también
            builder.Services.AddSingleton<AppShell>();

            return builder.Build();
        }
    }
}
