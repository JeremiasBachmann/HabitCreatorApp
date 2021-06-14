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
            var days = new List<Day>();
            days = await App.LocalDatabase.GetDaysAsync();
            var habitDays = days.Where(d => d.HabitID == habitId).ToList();
            var sortedDays = habitDays.OrderBy(x => x.Date.TimeOfDay).ToList();
            var last7Days = sortedDays.Skip(Math.Max(0, sortedDays.Count() - 7)).ToList();
            var habit = await App.LocalDatabase.GetOneHabitAsync(habitId);

            while (last7Days.Count() < 7)
            {
                last7Days.Add(new Day());
            }

            foreach (Day d in last7Days)
            {
                ChartEntry chartEntry = new ChartEntry(Convert.ToSingle(d.Value))
                {
                    Label = d.Date.DayOfWeek.ToString(),
                    ValueLabel = d.Value.ToString(),
                    Color = SKColor.Parse("#3498db")
                };
                Entries.Add(chartEntry);
            }
            chartViewLine.Chart = new LineChart { Entries = Entries, LineMode = LineMode.Straight, IsAnimated = true, ValueLabelOrientation = Orientation.Horizontal};

            LoadLabels(sortedDays, habit);
        }

        private void LoadLabels(List<Day> sortedDays, Habit habit)
        {
            var last7Days = sortedDays.Skip(Math.Max(0, sortedDays.Count() - 7)).ToList();
            var lastMonth = sortedDays.Where(d => d.Date.Date.Month == DateTime.UtcNow.Date.Month);
            var lastDay = sortedDays.Skip(Math.Max(0, sortedDays.Count() - 1)).ToList();

            double perDayPushUps = 0;
            double dailyPushUps = lastDay.FirstOrDefault().Value;
            double weeklyPushUps = 0;
            double MonthlyPushUps = 0;

            foreach (Day d in last7Days)
            {
                weeklyPushUps += d.Value;
            }

            perDayPushUps = weeklyPushUps / 7;
            perDayPushUps = Math.Truncate(perDayPushUps);

            foreach (Day d in lastMonth)
            {
                MonthlyPushUps += d.Value;
            }


            PerDayLabelValue.Text = perDayPushUps.ToString();
            DaylyLabelValue.Text = dailyPushUps.ToString();
            WeeklyLabelValue.Text = weeklyPushUps.ToString();
            MonthlyLabelValue.Text = MonthlyPushUps.ToString();

            var habitName = habit.Name.ToString();

            PerDayLabel.Text = $"{habitName} per day:";
            DaylyLabel.Text = $"todays {habitName}:";
            WeeklyLabel.Text = $"weekly {habitName}:";
            MonthlyLabel.Text = $"monthly {habitName}:";

           Title = $"{habitName}";

        }
    }
}