using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using ShoppingList.Models;

namespace ShoppingList.Data
{
    public class ShopDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public ShopDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Store>().Wait();
        }

        public Task<List<Store>> GetShopsAsync()
        {
            return _database.Table<Store>().ToListAsync();
        }

        public Task<Store> GetShopsAsync(int id)
        {
            return _database.Table<Store>()
                            .Where(i => i.ID == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveShopAsync(Store shop)
        {
            if (shop.ID != 0)
            {
                return _database.UpdateAsync(shop);
            }
            else
            {
                return _database.InsertAsync(shop);
            }
        }

        public Task<int> DeleteShopAsync(Store shop)
        {
            return _database.DeleteAsync(shop);
        }
    }
}
