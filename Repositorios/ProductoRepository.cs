using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using CateonCook.Modelos;

namespace CateonCook.Repositorios;

public sealed class ProductoRepository : IProductoRepository
{
    private readonly SQLiteAsyncConnection _db;

    public ProductoRepository(string databasePath)
    {
        _db = new SQLiteAsyncConnection(databasePath);
    }

    public async Task InicializarAsync()
    {
        await _db.CreateTableAsync<Producto>();
    }

    public Task<int> InsertarAsync(Producto producto)
    {
        if (string.IsNullOrWhiteSpace(producto.Id))
            producto.Id = Guid.NewGuid().ToString();
        return _db.InsertAsync(producto);
    }

    public Task<int> ActualizarAsync(Producto producto) => _db.UpdateAsync(producto);

    public Task<int> EliminarAsync(Producto producto) => _db.DeleteAsync(producto);

    public Task<Producto?> ObtenerPorIdAsync(string id) => _db.Table<Producto>().Where(p => p.Id == id).FirstOrDefaultAsync();

    public Task<IReadOnlyList<Producto>> ObtenerTodosAsync() => _db.Table<Producto>().ToListAsync().ContinueWith(t => (IReadOnlyList<Producto>)t.Result);

    public Task<IReadOnlyList<Producto>> BuscarAsync(string query)
    {
        query = query?.Trim().ToLowerInvariant() ?? string.Empty;
        return _db.Table<Producto>()
                  .Where(p => p.Nombre.ToLower().Contains(query) || (p.Descripcion != null && p.Descripcion.ToLower().Contains(query)))
                  .ToListAsync()
                  .ContinueWith(t => (IReadOnlyList<Producto>)t.Result);
    }
}
