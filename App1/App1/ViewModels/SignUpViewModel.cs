using App1.ViewModels;
using App1.Views;
using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace App1.ViewModels
{
    class SignUpViewModel : BaseViewModel
    {
        public Command SignInCommand { get; }
        public Command SignUpCommand { get; }
        public Command ForgotPasswordCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Email"));
            }
        }

        private string username;
        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Username"));
            }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Password"));
            }
        }

        public SignUpViewModel()
        {
            SignInCommand = new Command(OnSignInClicked);
            SignUpCommand = new Command(OnSignUpClicked);
            ForgotPasswordCommand = new Command(OnForgotPasswordCommand);
        }

        private async void OnSignInClicked(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }

        private async void OnSignUpClicked(object obj)
        {
            if (email != "jeremias@cvts.ch" && username != "jeremias")
            {
                await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
            }
        }

        private async void OnForgotPasswordCommand(object obj)
        {
            Console.WriteLine("Forgot Password Button clicked");
        }
    }
}