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
                await App.LocalDatabase.SaveHabitAsync(new Habit
                {
                    Name = nameEntry.Text,
                    MaxProgress = double.Parse(nummberEntry.Text),
                    Progress = 0
                });
                await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
                nameEntry.Text = nummberEntry.Text = string.Empty;
            }
        }
    }
}