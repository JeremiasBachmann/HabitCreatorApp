using App1.Models;
using App1.ViewModels;
using Xamarin.Forms;

namespace App1.Views
{
    public partial class AboutPage : ContentPage
    {
        public IDataStore<Habit> DataStore => DependencyService.Get<IDataStore<Habit>>();
        public AboutPage()
        {
            InitializeComponent();
            this.BindingContext = new AboutViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            ProgressCollectionView.ItemsSource = await App.LocalDatabase.GetHabitAsync();
        }
    }
}