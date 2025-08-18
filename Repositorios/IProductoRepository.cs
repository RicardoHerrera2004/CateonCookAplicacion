using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CateonCook.Modelos;

namespace CateonCook.Repositorios;

public interface IProductoRepository
{
    Task InicializarAsync();
    Task<int> InsertarAsync(Producto producto);
    Task<int> ActualizarAsync(Producto producto);
    Task<int> EliminarAsync(Producto producto);
    Task<Producto?> ObtenerPorIdAsync(string id);
    Task<IReadOnlyList<Producto>> ObtenerTodosAsync();
    Task<IReadOnlyList<Producto>> BuscarAsync(string query);
}
