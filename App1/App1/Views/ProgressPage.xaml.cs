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
            var habits = new List<Habit>();
            habits = await App.LocalDatabase.GetHabitsAsync();

            foreach (Habit h in habits.Where(l => l.ID == id))
            {
                progressRing.Progress = (h.Progress / h.MaxProgress);
                GoalLabel.Text = ($"Your goal for today: {h.MaxProgress} {h.Name}");
                ProgressLabel.Text = $"{h.Progress}/{h.MaxProgress}";
                RoundsLabel.Text = $"{h.Round} Rounds";
            }
        }
    }
}