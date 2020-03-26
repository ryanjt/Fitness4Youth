using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Data;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Auth;

namespace youth
{
    public partial class App : Application
    {
        public static string FilePath;
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new Login());


        }
        public App(string filePath)
        {
            InitializeComponent();
            MainPage = new NavigationPage(new Login());
            FilePath = filePath;
        }

        protected override void OnStart()
        {

            AppCenter.Start("ios=aa6903ad-9ec2-4ed5-b067-bde25d4aa602;" + "android=57c9b3f5-30f0-42aa-92cd-76394490bb70", typeof(Analytics), typeof(Crashes), typeof(Auth), typeof(Data));
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
