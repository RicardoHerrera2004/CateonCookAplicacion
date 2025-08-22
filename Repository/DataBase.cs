using CateonCook.Modelos;
using SQLite;

namespace CateonCook.Repositorios;

public interface IAppDatabase
{
    SQLiteAsyncConnection Connection { get; }
    Task InitializeAsync();
}

public class AppDatabase : IAppDatabase
{
    private readonly string _dbPath;
    private SQLiteAsyncConnection? _connection;

    public AppDatabase()
    {
        // Nombre del archivo de BD en el almacenamiento local de la app
        _dbPath = Path.Combine(FileSystem.AppDataDirectory, "cateoncook.db3");
    }

    public SQLiteAsyncConnection Connection
        => _connection ??= new SQLiteAsyncConnection(_dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);

    public async Task InitializeAsync()
    {
        // Habilitar FK
        await Connection.ExecuteAsync("PRAGMA foreign_keys = ON;");

        await Connection.CreateTableAsync<Producto>();
        await Connection.CreateTableAsync<Venta>();
        await Connection.CreateTableAsync<Usuario>();
        await Connection.CreateTableAsync<Financiamiento>();
        await Connection.CreateTableAsync<PlanPago>();

        // Índices útiles (si no existen)
        await Connection.ExecuteAsync("CREATE INDEX IF NOT EXISTS idx_venta_fecha ON Venta(Fecha);");
        await Connection.ExecuteAsync("CREATE INDEX IF NOT EXISTS idx_venta_producto ON Venta(ProductoId);");
        await Connection.ExecuteAsync("CREATE INDEX IF NOT EXISTS idx_planpago_vencimiento ON PlanPago(FechaVencimiento);");
    }
}
