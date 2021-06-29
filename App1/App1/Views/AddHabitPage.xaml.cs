using App1;
using App1.Models;
using App1.ViewModels;
using App1.Views;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddHabitPage : ContentPage
    {
        public AddHabitPage()
        {
            InitializeComponent();
            this.BindingContext = new AddHabitViewModel();
        }

        async void OnButtonClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(nameEntry.Text) && !string.IsNullOrWhiteSpace(nummberEntry.Text))
            {
                await App.LocalDatabase.InsertHabitAsync(new Habit
                {
                    Name = nameEntry.Text,
                });

                var habits = await App.LocalDatabase.GetHabitsAsync();
                var habit = habits[habits.Count - 1];
                var days = await App.LocalDatabase.GetDaysAsync();
                var sortedDays = days.OrderBy(x => x.Date).ToList();
                var lastMonth = DateTime.UtcNow.Date.AddDays(-30);

                for (int i = 0; lastMonth.Date < DateTime.UtcNow.Date; i++)
                {
                    await App.LocalDatabase.InsertHabitPerDayAsync(new HabitPerDay
                    {
                        HabitID = habit.ID,
                        Habit = habit,
                        DayID = sortedDays[i].ID,
                        Day = sortedDays[i],
                        RepetionsToDo = int.Parse(nummberEntry.Text),
                        RepetionsDone = 0,
                        Round = 0
                    });
                    lastMonth = lastMonth.Date.AddDays(1);
                }
                await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
                nameEntry.Text = nummberEntry.Text = string.Empty;
            }
        }
    }
}