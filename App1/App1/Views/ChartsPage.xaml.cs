using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using App1.Models;
using App1.Services;
using App1.ViewModels;
using Microcharts;
using SkiaSharp;
using Xamarin.Forms;

namespace App1.Views
{
    public partial class ChartsPage : ContentPage, IQueryAttributable, INotifyPropertyChanged
    {
        public List<ChartEntry> Entries { get; set; } = new List<ChartEntry>();
        ChartViewModel ChartviewModel { get; set; } = new ChartViewModel();

        public ChartsPage()
        {
            InitializeComponent();
            BindingContext = ChartviewModel;
            var chartService = new ChartService();
            chartService.Initialise();
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            // The query parameter requires URL decoding.
            string idString = HttpUtility.UrlDecode(query["Id"]);
            int.TryParse(idString, out int Id);
            LoadLineChart(Id);
            ChartviewModel.id = Id;
        }

        private async void LoadLineChart(int habitId)
        {
            Entries.Clear();
            var days = await App.LocalDatabase.GetDaysAsync();
            var sortedDays = days.OrderBy(x => x.Date.TimeOfDay).ToList();
            var last7Days = sortedDays.Skip(Math.Max(0, sortedDays.Count() - 7)).ToList();

            var habitDays = await App.LocalDatabase.GetHabitDaysAsync();
            List<HabitPerDay> lastSevenHabitDays = new List<HabitPerDay>();

            foreach (Day d in last7Days)
            {
                lastSevenHabitDays.Add(habitDays.First(h => h.Day == d && h.Habit.ID == habitId));
            }

            foreach (HabitPerDay habitPerDay in lastSevenHabitDays)
            {
                ChartEntry chartEntry = new ChartEntry(Convert.ToSingle(habitPerDay.RepetionsDone))
                {
                    Label = habitPerDay.Day.Date.DayOfWeek.ToString(),
                    ValueLabel = habitPerDay.RepetionsDone.ToString(),
                    Color = SKColor.Parse("#3498db")
                };
                Entries.Add(chartEntry);
            }
            chartViewLine.Chart = new LineChart { Entries = Entries, LineMode = LineMode.Straight, IsAnimated = true, ValueLabelOrientation = Orientation.Horizontal};

            LoadLabels(lastSevenHabitDays, habitDays.FirstOrDefault().Habit.Name);
        }

        private void LoadLabels(List<HabitPerDay> lastSevenHabitDays, string habitName)
        {
            double perDayPushUps = 0;
            double dailyPushUps = lastSevenHabitDays.FirstOrDefault().RepetionsDone;
            double weeklyPushUps = 0;
            double MonthlyPushUps = 0;

            foreach (var d in lastSevenHabitDays)
            {
                weeklyPushUps += d.RepetionsDone;
            }

            perDayPushUps = weeklyPushUps / 7;
            perDayPushUps = Math.Truncate(perDayPushUps);

            PerDayLabelValue.Text = perDayPushUps.ToString();
            DaylyLabelValue.Text = dailyPushUps.ToString();
            WeeklyLabelValue.Text = weeklyPushUps.ToString();
            MonthlyLabelValue.Text = MonthlyPushUps.ToString();

            PerDayLabel.Text = $"{habitName} per day:";
            DaylyLabel.Text = $"todays {habitName}:";
            WeeklyLabel.Text = $"weekly {habitName}:";
            MonthlyLabel.Text = $"monthly {habitName}:";

           Title = $"{habitName}";
        }
    }
}