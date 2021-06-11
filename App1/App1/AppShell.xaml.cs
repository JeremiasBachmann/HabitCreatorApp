using System;
using System.Collections.Generic;
using App1.ViewModels;
using App1.Views;
using Xamarin.Forms;

namespace App1
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        private void OnChartsClicked(object sender, EventArgs e)
        {
             Shell.Current.GoToAsync($"//{nameof(ChartPage)}");
        }

        private void OnLogoutClicked(object sender, EventArgs e)
        {
             Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}
