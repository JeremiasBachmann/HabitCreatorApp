using App1;
using App1.Models;
using App1.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using Xamarin.Forms;

namespace MyFirsApp1tMobileApp.ViewModels
{
    class PractiseViewModel : INotifyPropertyChanged, IQueryAttributable
    {
        public Command FinishCommand { get; }
        public Command ClickedCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;
        private double maxValue = 100;
        private double progressValue = 0;
        private int id = 0;
        private string name;

        private double progressValueForProgressBar = 0;
        public double ProgressValueForProgressBar
        {
            get { return progressValueForProgressBar; }
            set
            {
                progressValueForProgressBar = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ProgressValueForProgressBar"));
            }
        }
        private string progressValueAsString = "lets Start";
        public string ProgressValueAsString
        {
            get { return progressValueAsString; }
            set
            {
                progressValueAsString = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ProgressValueAsString"));
            }
        }

        public PractiseViewModel()
        {
            FinishCommand = new Command(OnFinishCommand);
            ClickedCommand = new Command(OnClickedCommand);
          
        }

        private async void GetDataFromHabitAsync()
        {
            var habits = new List<Habit>();
            habits = await App.LocalDatabase.GetHabitAsync();

            foreach (Habit h in habits.Where(l => l.ID == id))
            {
                maxValue = h.MaxProgress;
                progressValue = h.Progress;
                name = h.Name;
            }
        }

        private async void SaveHabit()
        {
            var habits = new Habit
            {
                ID = id,
                Name = name,
                Progress = progressValue,
                MaxProgress = maxValue,
                ProgressInPercent = (progressValue / maxValue)
            };
            await App.LocalDatabase.UpdateHabitAsync(habits);
        }
      

        private async void OnFinishCommand(object obj)
        {
            SaveHabit();
            await Shell.Current.GoToAsync($"//{nameof(ProgressPage)}?Id={id}");
        }

        private async void OnClickedCommand(object obj)
        {
            progressValue++;
            ProgressValueForProgressBar = (progressValue / maxValue);
            ProgressValueAsString = progressValue.ToString();
            Console.WriteLine(ProgressValueForProgressBar);
            Console.WriteLine(ProgressValueAsString);
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            // The query parameter requires URL decoding.
            string idString = HttpUtility.UrlDecode(query["Id"]);
            int.TryParse(idString, out int Id);
            id = Id;
            GetDataFromHabitAsync();
        }
    }
}
