using MyFirsApp1tMobileApp.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PractisePage : ContentPage
    {
        public PractisePage()
        {
            InitializeComponent();
            this.BindingContext = new PractiseViewModel();
        }
    }
}