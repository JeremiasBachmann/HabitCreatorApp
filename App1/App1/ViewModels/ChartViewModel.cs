using App1.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Xamarin.Forms;

namespace App1.ViewModels
{
    class ChartViewModel
    {
        public Command BackCommand { get; }
        public int id;

        public ChartViewModel()
        {
            BackCommand = new Command(OnBackCommand);
        }

        public async void OnBackCommand()
        {
            await Shell.Current.GoToAsync($"//{nameof(ProgressPage)}?Id={id}");
        }
    }
}
