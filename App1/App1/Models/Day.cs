using App1.Views;
using SQLite;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace App1.Models
{
    public class Day
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public int Value { get; set; }
        public Habit Habit { get; set; }
    }
}
