using App1.Models;
using App1.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Windows.Input;
using Xamarin.Forms;

namespace App1.ViewModels
{
    class ProgressViewModel : IQueryAttributable
    {
        public ICommand CamCommand { get; }
        public ICommand ShareCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand PractiseCommand { get; }
        public ICommand DeleteCommand { get; }

        private int id;
        public ProgressViewModel()
        {
            CamCommand = new Command(OnCamCommand);
            ShareCommand = new Command(OnShareCommand);
            BackCommand = new Command(OnBackCommand);
            PractiseCommand = new Command(OnPractiseCommand);
            DeleteCommand = new Command(OnDeleteCommand);
        }

        private async void OnCamCommand(object obj)
        {
            Console.WriteLine("OnCamCommand");
        }

        private async void OnShareCommand(object obj)
        {
            Console.WriteLine("OnShareCommand");
        }

      

        private async void OnBackCommand(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        }

        private async void OnPractiseCommand()
        {
            await Shell.Current.GoToAsync($"//{nameof(PractisePage)}?Id={id}");
        }

        private async void OnDeleteCommand()
        {
            var habits = new List<Habit>();
            habits = await App.LocalDatabase.GetHabitAsync();

            foreach (Habit h in habits.Where(l => l.ID == id))
            {
                await App.LocalDatabase.DeleteHabitAsync(h);
            }
            await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            // The query parameter requires URL decoding.
            string idString = HttpUtility.UrlDecode(query["Id"]);
            int.TryParse(idString, out int Id);
            id = Id;
        }
    }
}
