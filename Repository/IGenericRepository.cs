using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CateonCook.Repositorios;

public interface IGenericRepository<T> where T : new()
{
    Task<int> InsertAsync(T entity);
    Task<int> UpdateAsync(T entity);
    Task<int> DeleteAsync(T entity);
    Task<T?> GetByIdAsync(int id);
    Task<List<T>> GetAllAsync();

    // Extras comunes
    Task<int> InsertAllAsync(IEnumerable<T> entities);
    Task<int> DeleteAllAsync();
}

