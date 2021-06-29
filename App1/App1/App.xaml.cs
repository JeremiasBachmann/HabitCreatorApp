using App1.Database;
using App1.Services;
using System;
using System.IO;
using Xamarin.Forms;

namespace App1
{
    public partial class App : Application
    {
        static LocalDatabase database;

        public static LocalDatabase LocalDatabase
        {
            get
            {
                if (database == null)
                {
                    database = new LocalDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Habits.db4"));
                }
                return database;
            }
        }

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
