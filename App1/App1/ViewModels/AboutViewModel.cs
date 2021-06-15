using App1.Models;
using App1.Services;
using App1.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        private Habit _selectedHabit;
        public ObservableCollection<Habit> HabitList { get; } = new ObservableCollection<Habit>();
        public Command LoadHabitCommand { get; }
        public Command AddHabitCommand { get; }
        public Command<Habit> HabitTapped { get; }
        public Command AddComand {get; }
        public ICommand TappCommand { get; }


        public AboutViewModel()
        {
            Title = "About";
            AddComand = new Command(OnAddClicked);
            HabitTapped = new Command<Habit>(OnHabitSelected);
            AddHabitCommand = new Command(OnAddHabit);
            LoadHabitCommand = new Command(async () => await ExecuteLoadHabitsCommand());
            TappCommand = new Command<string>(OnTappCommand);
            var chartService = new ChartService();
            chartService.Initialise();
        }

        private async void OnAddClicked(object obj)
        {
            Console.WriteLine("AddHabitPage");
            await Shell.Current.GoToAsync($"//{nameof(AddHabitPage)}");
        }
        async Task ExecuteLoadHabitsCommand()
        {
            IsBusy = true;

            try
            {
                HabitList.Clear();
                var Habits = await DataStore.GetItemsAsync(true);
                foreach (var Habit in Habits)
                {
                    HabitList.Add(Habit);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        public void OnAppearing()
        {
            IsBusy = true;
            SelectedHabit = null;
        }

        public Habit SelectedHabit
        {
            get => _selectedHabit;
            set
            {
                SetProperty(ref _selectedHabit, value);
                OnHabitSelected(value);
            }
        }

        private async void OnAddHabit(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(NewHabitPage)}");
        }

        async void OnHabitSelected(Habit Habit)
        {
            if (Habit == null)
                return;

            // This will push the HabitDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(AddHabitPage)}?{nameof(AddHabitViewModel.HabitId)}={Habit.ID}");
        }

        private async void OnTappCommand(string value)
        {
            int.TryParse(value, out int id);
            Console.WriteLine("ID: " + id);
            await Shell.Current.GoToAsync($"//{nameof(ProgressPage)}?Id={id}");
        }
    }
}

