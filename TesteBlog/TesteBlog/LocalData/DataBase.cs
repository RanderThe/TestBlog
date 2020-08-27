using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TesteBlog.Model;

namespace TesteBlog.LocalData
{
  public class DataBase
  {
    private readonly SQLiteAsyncConnection _database;

    public DataBase(string databasePath)
    {
      _database = new SQLiteAsyncConnection(databasePath);
    }

    public void CreateTable<T>() where T : new()
    {
      _database.CreateTableAsync<T>().Wait();
    }

    public Task<List<T>> GetToListAsync<T>() where T : new()
    {
      return _database.Table<T>().ToListAsync();
    }

    public Task<T> GetFirstOrDefaultAsync<T>(int id) where T : IModelBase, new()
    {
      return _database.Table<T>()
        .Where(i => i.tableId == id)
        .FirstOrDefaultAsync();
    }

    public Task<int> UpdateInsertAsync<T>(T model) where T : IModelBase, new()
    {
      if (model.tableId != 0)
        return _database.UpdateAsync(model);
      return _database.InsertAsync(model);
    }

    public Task<int> DeleteAsync<T>(T model)
    {
      return _database.DeleteAsync(model);
    }

    public Task<int> InsertAsync<T>(T model) where T : IModelBase, new()
    {
      return _database.InsertAsync(model);
    }

    public Task<int> UpdateAsync<T>(T model) where T : IModelBase, new()
    {
      return _database.UpdateAsync(model);
    }

  }
}
