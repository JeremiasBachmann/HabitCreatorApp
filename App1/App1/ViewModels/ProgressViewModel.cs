using App1.Models;
using App1.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Input;
using Xamarin.Essentials;
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
        public ICommand ChartCommand { get; }

        public Image Image { get; set; } = new Image();

        private int id;
        public ProgressViewModel()
        {
            CamCommand = new Command(OnCamCommand);
            ShareCommand = new Command(OnShareCommand);
            BackCommand = new Command(OnBackCommand);
            PractiseCommand = new Command(OnPractiseCommand);
            DeleteCommand = new Command(OnDeleteCommand);
            ChartCommand = new Command(ONChartCommand);
        }

        private async void OnCamCommand(object obj)
        {
            Console.WriteLine("OnCamCommand");
        }

        private async void OnShareCommand(object obj)
        {
            Console.WriteLine("OnShareCommand");

            await CaptureScreenshot();

            var fn = "Attachment.txt";
            var file = Path.Combine(FileSystem.CacheDirectory, fn);
            File.WriteAllText(file, "Hello World");

            await Share.RequestAsync(new ShareFileRequest
            {
                Title = "Title",
                File = new ShareFile(file)
            });
        }

        private async void OnBackCommand(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        }

        private async void OnPractiseCommand()
        {
            await Shell.Current.GoToAsync($"//{nameof(PractisePage)}?Id={id}");
        }

        private async void ONChartCommand()
        {
            await Shell.Current.GoToAsync($"//{nameof(ChartsPage)}?Id={id}");
        }

        private async void OnDeleteCommand()
        {
            var habits = await App.LocalDatabase.GetHabitsAsync();
            foreach (Habit habit in habits.Where(h => h.ID == id))
            {
                await App.LocalDatabase.DeleteHabitAsync(habit);
            }

            var days = await App.LocalDatabase.GetDaysAsync();
            foreach (Day day in days.Where(d => d.HabitID == id))
            {
                await App.LocalDatabase.DeleteDayAsync(day);
            }
            await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        }

        async Task CaptureScreenshot()
        {
            var screenshot = await Screenshot.CaptureAsync();
            var stream = await screenshot.OpenReadAsync();

            var imageSource = ImageSource.FromStream(() => stream);
            Image.Source = imageSource;
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
