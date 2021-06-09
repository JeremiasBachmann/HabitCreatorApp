using App1.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App1.Database
{
    public class LocalDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public LocalDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Habit>().Wait();
        }

        public Task<List<Habit>> GetHabitAsync()
        {
            return _database.Table<Habit>().ToListAsync();
        }

        public Task<int> SaveHabitAsync(Habit habit)
        {
            return _database.InsertAsync(habit);
        }

        public Task<int> UpdateHabitAsync(Habit habit)
        {
            return _database.UpdateAsync(habit);
        }

        public Task<int> DeleteHabitAsync(Habit habit)
        {
            return _database.DeleteAsync(habit);
        }
    }
}