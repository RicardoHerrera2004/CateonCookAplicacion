using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace CateonCook.Repositorios;

public class GenericRepository<T> : IGenericRepository<T> where T : new()
{
    protected readonly SQLiteAsyncConnection Db;

    public GenericRepository(IAppDatabase appDb)
    {
        Db = appDb.Connection;
    }

    public Task<int> InsertAsync(T entity) => Db.InsertAsync(entity);
    public Task<int> UpdateAsync(T entity) => Db.UpdateAsync(entity);
    public Task<int> DeleteAsync(T entity) => Db.DeleteAsync(entity);
    public Task<T?> GetByIdAsync(int id) => Db.FindAsync<T>(id);
    public Task<List<T>> GetAllAsync() => Db.Table<T>().ToListAsync();

    public async Task<int> InsertAllAsync(IEnumerable<T> entities)
    {
        var list = entities.ToList();
        await Db.RunInTransactionAsync(tran => tran.InsertAll(list));
        return list.Count;
    }

    public Task<int> DeleteAllAsync() => Db.DeleteAllAsync<T>();
}

