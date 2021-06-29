using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace App1.Models
{
    public class HabitPerDay
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        [ForeignKey(typeof(Day))]
        public int DayID { get; set; }
        [OneToOne]
        public Day Day { get; set; }
        [ForeignKey(typeof(Habit))]
        public int HabitID { get; set; }
        [OneToOne]
        public Habit Habit { get; set; }
        public int RepetionsToDo { get; set; }
        public int RepetionsDone { get; set; }
        public int Round { get; set; }
    }
}
