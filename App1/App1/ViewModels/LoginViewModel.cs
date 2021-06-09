using App1.ViewModels;
using App1.Views;
using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command SignInCommand { get; }
        public Command SignUpCommand { get; }
        public Command ForgotPasswordCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
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

        public LoginViewModel()
        {
            SignInCommand = new Command(OnSignInClicked);
            SignUpCommand = new Command(OnSignUpClicked);
            ForgotPasswordCommand = new Command(OnForgotPasswordCommand);
        }

        private async void OnSignInClicked(object obj)
        {
            Console.WriteLine(username + ", " + password);
            if (username == "jeremias" && password == "jeremias")
            {
                await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
            }
        }

        private async void OnSignUpClicked(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(SignUpPage)}");
        }

        private async void OnForgotPasswordCommand(object obj)
        {
            Console.WriteLine("Forgot Password Button clicked");
        }
    }
}
