using App1;
using App1.Models;
using App1.ViewModels;
using App1.Views;
using System;
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
                    MaxProgress = double.Parse(nummberEntry.Text),
                    Progress = 0
                });

                var h = await App.LocalDatabase.GetHabitsAsync();
                int id = h[h.Count -1].ID;

                Day newDay = new Day
                {
                    Date = DateTime.UtcNow.Date.AddDays(-7),
                    Value = 0,
                    HabitID = id
                };
                await App.LocalDatabase.InsertDayAsync(newDay);
                var lastDate = newDay.Date.Date;

                while (lastDate.Date != DateTime.UtcNow.Date)
                {
                    lastDate = lastDate.Date.AddDays(1);
                    Day day = new Day
                    {
                        Date = lastDate.Date,
                        Value = 0,
                        HabitID = id
                    };
                    await App.LocalDatabase.InsertDayAsync(day);
                }

                await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
                nameEntry.Text = nummberEntry.Text = string.Empty;
            }
        }
    }
}