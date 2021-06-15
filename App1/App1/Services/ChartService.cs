using App1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App1.Services
{
    class ChartService
    {
        public async void Initialise()
        {
            var habits = await App.LocalDatabase.GetHabitsAsync();
            var days = await App.LocalDatabase.GetDaysAsync();

            foreach (Habit habit in habits)
            {
                var habitDays = days.Where(d => d.HabitID == habit.ID).ToList();
                var sortedDays = habitDays.OrderBy(x => x.Date.TimeOfDay).ToList();
                var lastDay = sortedDays.Skip(Math.Max(0, sortedDays.Count() - 1)).ToList();
                var lastDate = lastDay.FirstOrDefault().Date;

                if (lastDate.Date != DateTime.UtcNow.Date)
                {
                    habit.Progress = 0;
                    await App.LocalDatabase.UpdateHabitAsync(habit);
                }

                while (lastDate.Date != DateTime.UtcNow.Date)
                {
                    lastDate = lastDate.Date.AddDays(1);
                    Day newDay = new Day
                    {
                        Date = lastDate.Date,
                        Value = 0,
                        HabitID = habit.ID
                    };
                    await App.LocalDatabase.InsertDayAsync(newDay);
                }
            }
        }
    }
}
