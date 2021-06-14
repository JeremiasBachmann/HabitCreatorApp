using App1.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App1.Services
{
    static class Test
    {
        public static async void Add()
        {
            var da = new Day()
            {
                Date = new DateTime(2021, 6, 1),
                Value = 0,
                HabitID = 8
            };

             await App.LocalDatabase.InsertDayAsync(da);
        }

        public static async void Delete()
        {
            var days = await App.LocalDatabase.GetDaysAsync();

            foreach (Day d in days)
            {
                await App.LocalDatabase.DeleteDayAsync(d);
            }

            var habits = await App.LocalDatabase.GetHabitsAsync();

            foreach (Habit h in habits)
            {
                await App.LocalDatabase.DeleteHabitAsync(h);
            }
        }
    }
}
