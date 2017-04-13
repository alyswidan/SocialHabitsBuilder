using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SocialHabits.Abstractions;
using SocialHabits.Services;
using SocialHabitsBuilder.views;
using Xamarin.Forms;

namespace SocialHabits
{
    public partial class App : Application
    {
        public static ICloudService CloudService;
        public App()
        {

            InitializeComponent();
            CloudService = new AzureCloudService();
            MainPage = new NavigationPage(new LoginPage());
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
