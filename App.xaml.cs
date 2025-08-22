using CateonCook.Repositorios;

namespace CateonCook
{
    public partial class App : Application
    {
        public App(AppShell shell, AppDatabase db)
        {
            InitializeComponent();
            MainPage = shell;

            // Inicializa BD en background
            _ = Task.Run(async () =>
            {
                try { await db.InitializeAsync(); }
                catch (Exception ex)
                {
                    var log = shell.Handler?.MauiContext?.Services.GetService<ILogService>();
                    if (log != null) await log.LogErrorAsync(nameof(App), "Init DB failed", ex);
                }
            });
        }
    }

}
