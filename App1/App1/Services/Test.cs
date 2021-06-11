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

             await App.LocalDatabase.SaveDayAsync(da);
        }

        public static async void Delete()
        {
            var days = await App.LocalDatabase.GetDayAsync();

            foreach (Day d in days)
            {
                await App.LocalDatabase.DeleteDaytAsync(d);
            }

            var habits = await App.LocalDatabase.GetHabitAsync();

            foreach (Habit h in habits)
            {
                await App.LocalDatabase.DeleteHabitAsync(h);
            }
        }
    }
}
