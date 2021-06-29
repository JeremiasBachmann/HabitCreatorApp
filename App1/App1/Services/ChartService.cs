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
            var habitDays = await App.LocalDatabase.GetHabitDaysAsync();
            var habits = await App.LocalDatabase.GetHabitsAsync();
            var days = await App.LocalDatabase.GetDaysAsync();

            var sortedDays = days.OrderBy(x => x.Date.TimeOfDay).ToList();
            var lastDay = sortedDays.Skip(Math.Max(0, sortedDays.Count() - 1)).FirstOrDefault();

            var lastHabitDay = habitDays.FirstOrDefault(h => h.Day == lastDay);

            if (lastDay == null)
            {
                var lastMonth = DateTime.UtcNow.Date.AddDays(-30);
                do{
                    Day newDay = new Day()
                    {
                        Date = lastMonth
                    };
                    await App.LocalDatabase.InsertDayAsync(newDay);
                    lastMonth.AddDays(1);
                } while (lastMonth.Date != DateTime.UtcNow.Date) ;
            }
            else
            {
                while (lastDay.Date != DateTime.UtcNow.Date)
                {
                    lastDay.Date = lastDay.Date.AddDays(1);
                    Day newDay = new Day
                    {
                        Date = lastDay.Date
                    };
                    await App.LocalDatabase.InsertDayAsync(newDay);

                    foreach (Habit habit in habits)
                    {
                        var habitPerDay = new HabitPerDay()
                        {
                            HabitID = habit.ID,
                            Habit = habit,
                            DayID = lastDay.ID,
                            Day = lastDay,
                            RepetionsToDo = lastHabitDay.RepetionsToDo,
                            RepetionsDone = 0,
                            Round = 0
                        };

                        await App.LocalDatabase.InsertHabitPerDayAsync(habitPerDay);
                    }
                }
            }
        }
    }
}
