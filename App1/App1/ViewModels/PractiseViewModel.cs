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

        private int id;
        private Habit habit;
        private Day day;
        private int repetisionsToDo = 100;
        private int repetitionsDone;
        private int round;


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
            var Days = await App.LocalDatabase.GetDaysAsync();
            var habitDays = await App.LocalDatabase.GetHabitDaysAsync();

            foreach (HabitPerDay h in habitDays.Where(h => h.Habit.ID == id && h.Day.Date == DateTime.UtcNow.Date))
            {
                day = h.Day;
                habit = h.Habit;
                repetisionsToDo = h.RepetionsToDo;
                repetitionsDone = h.RepetionsDone;
                round = h.Round;
            }

            ProgressValueForProgressBar = (repetitionsDone / repetisionsToDo);
            ProgressValueAsString = repetitionsDone.ToString();
        }

        private async void OnFinishCommand(object obj)
        {
            var HabitPerDay = new HabitPerDay
            {
                ID = id,
                Habit = habit,
                Day = day,
                RepetionsToDo = repetisionsToDo,
                RepetionsDone = repetitionsDone,
                Round = ++round
            };
            await App.LocalDatabase.UpdateHabitPerDayAsync(HabitPerDay);
            await Shell.Current.GoToAsync($"//{nameof(ProgressPage)}?Id={habit.ID}");
        }

        private async void OnClickedCommand(object obj)
        {
            repetitionsDone++;
            double.TryParse(repetitionsDone.ToString(), out double repetitionsDoneForProgressBar);
            double.TryParse(repetisionsToDo.ToString(), out double repetisionsToDoForProgressBar);
            ProgressValueForProgressBar = (repetitionsDoneForProgressBar / repetisionsToDoForProgressBar);
            ProgressValueAsString = repetitionsDone.ToString();
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
