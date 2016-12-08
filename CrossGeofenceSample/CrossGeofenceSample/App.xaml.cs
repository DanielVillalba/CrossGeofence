using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace CrossGeofenceSample
{
    public partial class App : Application
    {
        public static ObservableCollection<Models.Event> Events = Helpers.Settings.GetEvents();
        public App()
        {
            InitializeComponent();

            MainPage = new CrossGeofenceSample.MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
