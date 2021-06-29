using App1.Models;
using App1.Views;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace App1.ViewModels
{
    [QueryProperty(nameof(HabitId), nameof(HabitId))]
    public class AddHabitViewModel : BaseViewModel
    {
        private string habitId;
        public int iD;
        public string name;
        public double maxProgress;
        public double progress;

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public double RepetitionsToDo
        {
            get => maxProgress;
            set => SetProperty(ref maxProgress, value);
        }

        public double RepetitionsDone
        {
            get => progress;
            set => SetProperty(ref progress, value);
        }

        public string HabitId
        {
            get
            {
                return habitId;
            }
            set
            {
                habitId = value;
                LoadHabitId(value);
            }
        }

        public async void LoadHabitId(string habitIdAsString)
        {
            int.TryParse(habitIdAsString, out int habitId);

            var habitDays = await App.LocalDatabase.GetHabitDaysAsync();
            var habitDay = habitDays.FirstOrDefault(h => h.Habit.ID == habitId && h.Day.Date == DateTime.UtcNow.Date);
            iD = habitDay.Habit.ID;
            Name = habitDay.Habit.Name;
            RepetitionsToDo = habitDay.RepetionsToDo;
            RepetitionsDone = habitDay.RepetionsDone;
        }
    }
}
