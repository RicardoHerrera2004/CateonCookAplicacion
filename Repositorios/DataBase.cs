using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace CateonCook.Repositorios;

public sealed class SqliteConnectionProvider : ISQLiteConnection
{
    private readonly Lazy<SQLiteAsyncConnection> _lazy;

    public SqliteConnectionProvider()
    {
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "catalogo_CateonCook.db3");
        _lazy = new Lazy<SQLiteAsyncConnection>(() =>
        {
            var conn = new SQLiteAsyncConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);
            return conn;
        });
    }

    public SQLiteAsyncConnection GetConnection() => _lazy.Value;
}
