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
            _database.CreateTableAsync<Day>().Wait();
        }

        public Task<List<Habit>> GetHabitsAsync()
        {
            return _database.Table<Habit>().ToListAsync();
        }

        public Task<List<Day>> GetDaysAsync()
        {
            return _database.Table<Day>().ToListAsync();
        }

        public Task<Habit> GetOneHabitAsync(int id)
        {
            return _database.Table<Habit>().FirstOrDefaultAsync(h => h.ID == id);
        }

        public Task<Day> GetOneDaytAsync(int id)
        {
            return _database.Table<Day>().FirstOrDefaultAsync(h => h.ID == id);
        }

        public Task<int> InsertHabitAsync(Habit habit)
        {
            return _database.InsertAsync(habit);
        }

        public Task<int> InsertDayAsync(Day day)
        {
            return _database.InsertAsync(day);
        }

        public Task<int> UpdateHabitAsync(Habit habit)
        {
            return _database.UpdateAsync(habit);
        }

        public Task<int> UpdateDayAsync(Day day)
        {
            return _database.UpdateAsync(day);
        }

        public Task<int> DeleteHabitAsync(Habit habit)
        {
            return _database.DeleteAsync(habit);
        }

        public Task<int> DeleteDayAsync(Day day)
        {
            return _database.DeleteAsync(day);
        }
    }
}