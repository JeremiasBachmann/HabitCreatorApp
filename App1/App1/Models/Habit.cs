using App1.Views;
using SQLite;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace App1.Models
{
    public class Habit
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public double MaxProgress { get; set; }
        public double Progress { get; set; }
        public double ProgressInPercent { get; set; }

        public string Color { get; set; }

        public Command<int> TappCommand { get; } = new Command<int>(OnTappCommand);

        private static async void OnTappCommand(int id)
        {
            Console.WriteLine("ID: " + id);
            await Shell.Current.GoToAsync($"//{nameof(ProgressPage)}?Id={id}");
        }
    }
}
