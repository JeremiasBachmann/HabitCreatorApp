using App1.Models;
using App1.Views;
using System;
using System.Diagnostics;
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

        public double MaxProgress
        {
            get => maxProgress;
            set => SetProperty(ref maxProgress, value);
        }

        public double Progress
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

        public async void LoadHabitId(string habitId)
        {
            try
            {
                var habit = await DataStore.GetItemAsync(habitId);
                iD = habit.ID;
                Name = habit.Name;
                MaxProgress = habit.MaxProgress;
                Progress = habit.Progress;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Habit");
            }
        }
    }
}
