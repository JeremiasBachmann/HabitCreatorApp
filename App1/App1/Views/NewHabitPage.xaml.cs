using App1.Models;
using Xamarin.Forms;

namespace App1.Views
{
    public partial class NewHabitPage : ContentPage
    {
        public Habit habit { get; set; }

        public NewHabitPage()
        {
            InitializeComponent();
        }
    }
}