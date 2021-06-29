using App1.Models;
using App1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProgressPage : ContentPage, IQueryAttributable
    {
        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            // The query parameter requires URL decoding.
            string Id = HttpUtility.UrlDecode(query["Id"]);
            int.TryParse(Id, out int id);
            LoadRingProgressBar(id);
        }

        public ProgressPage()
        {
            InitializeComponent();
            this.BindingContext = new ProgressViewModel();
        }

        private async void LoadRingProgressBar(int id)
        {
            var habitpDays = await App.LocalDatabase.GetHabitDaysAsync();

            foreach (HabitPerDay h in habitpDays.Where(l => l.HabitID == id && l.Day.Date == DateTime.UtcNow.Date))
            {
                double.TryParse(h.RepetionsDone.ToString(), out double repetitionsDoneForProgressBar);
                double.TryParse(h.RepetionsToDo.ToString(), out double repetisionsToDoForProgressBar);
                progressRing.Progress = (repetitionsDoneForProgressBar / repetisionsToDoForProgressBar);
                GoalLabel.Text = ($"Your goal for today: {h.RepetionsToDo} {h.Habit.Name}");
                ProgressLabel.Text = $"{h.RepetionsDone}/{h.RepetionsToDo}";
                RoundsLabel.Text = $"{h.Round} Rounds";
            }
        }
    }
}