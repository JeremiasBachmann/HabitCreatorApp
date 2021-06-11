using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App1.Models;
using Microcharts;
using SkiaSharp;
using Xamarin.Forms;

namespace App1.Views
{
    public partial class ChartsPage : ContentPage
    {
        public List<ChartEntry> Entries { get; set; } = new List<ChartEntry>();




        public ChartsPage()
        {
            InitializeComponent();
            LoadLineChart(1);
        }

        private async void LoadLineChart(int id)
        {
            var days = new List<Day>();
            days = await App.LocalDatabase.GetDayAsync();

            foreach (Day d in days.Where(l => l.ID == id))
            {
                ChartEntry chartEntry = new ChartEntry(Convert.ToSingle(d.Value))
                {
                    Label = d.Habit.Name,
                    ValueLabel = d.Value.ToString(),
                    Color = SKColor.Parse("#3498db")
                };
                Entries.Add(chartEntry);
            }
            chartViewLine.Chart = new LineChart { Entries = Entries, LineMode = LineMode.Straight };
        }
    }
}