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

            await TakeScreenshot();
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

            var HabitDays = await App.LocalDatabase.GetHabitDaysAsync();
            foreach (var habitPerDay in HabitDays.Where(d => d.Habit.ID == id))
            {
                await App.LocalDatabase.DeleteHabitPerDayAsync(habitPerDay);
            }
            await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        }

        private async Task TakeScreenshot()
        {
            var screenshot = await Screenshot.CaptureAsync();
            var stream = await screenshot.OpenReadAsync();
            var buffer = new byte[16 * 1024];
            byte[] imageBytes;

            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                imageBytes = ms.ToArray();
            }
            ShareImage(imageBytes);
        }

        private async void ShareImage(byte[] imageBytes)
        {
            try
            {
                string filename = "img.jpg";
                string tempFileName = Path.Combine(FileSystem.CacheDirectory, filename);
                if (File.Exists(tempFileName))
                {
                    File.Delete(tempFileName);
                }
                File.WriteAllBytes(tempFileName, imageBytes);
                await Share.RequestAsync(new ShareFileRequest()
                {
                    File = new ShareFile(tempFileName),
                    Title = "Sharing image",
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
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
