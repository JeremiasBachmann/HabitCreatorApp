using App1.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App1.Services
{
    static class Test
    {

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

            var habitsperDay = await App.LocalDatabase.GetHabitDaysAsync();

            foreach (HabitPerDay hb in habitsperDay)
            {
                await App.LocalDatabase.DeleteHabitPerDayAsync(hb);
            }
        }
    }
}
