using App1;
using App1.Models;
using App1.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
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
        private double progressValue;
        private int id;
        private string name;
        private string color;
        private int round;

        private Day day;

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
                round = h.Round;
            }

            ProgressValueForProgressBar = (progressValue / maxValue);
            ProgressValueAsString = progressValue.ToString();
        }

        private async void OnFinishCommand(object obj)
        {
            var habit = new Habit
            {
                ID = id,
                Name = name,
                Progress = progressValue,
                MaxProgress = maxValue,
                ProgressInPercent = (progressValue / maxValue),
                Color = color,
                Round = (round++)
            };
            await App.LocalDatabase.UpdateHabitAsync(habit);

            var date = DateTime.UtcNow.Date;
            var days = new List<Day>();
            days = await App.LocalDatabase.GetDayAsync();

            var day = days.FirstOrDefault(l => l.Date == date && l.HabitID == id);
            if (day == null)
            {
                day = new Day
                {
                    Date = date,
                    Value = progressValue,
                    HabitID = id
                };
                await App.LocalDatabase.SaveDayAsync(day);
            }
            else
            {
                day.Value = progressValue;
                await App.LocalDatabase.UpdateDayAsync(day);
            }
          

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
