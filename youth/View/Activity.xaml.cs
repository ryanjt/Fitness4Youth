using System;
using System.Collections.Generic;
using Microsoft.AppCenter.Auth;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Analytics;
using Xamarin.Forms.Maps;
using Plugin.Geolocator;
using Syncfusion.Buttons;
using youth.Users;
using youth.Activities;

using Xamarin.Forms;
using System.Timers;
using Xamarin.Essentials;
using System.Globalization;
using Microsoft.AppCenter.Data;
using Newtonsoft.Json;


namespace youth
{
  
    public partial class Activity : ContentPage
    {
        Timer timer;
        Location startLocation;
        int hour = 0, min = 0, sec = 0;
        public Activity()
        {
            InitializeComponent();
          
            
            FindMe();
            
            
            NavigationPage.SetHasNavigationBar(this, false);

        }
        async void NewLiveActivity(object sender, System.EventArgs e)
        {

            popUpLayout.StaysOpen = true;
            popUpLayout.PopupView.ShowCloseButton = true;
            popUpLayout.IsOpen = true;
            




        }
        async void NewRun(object sender, System.EventArgs e)
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Medium);

            startLocation = await Geolocation.GetLocationAsync(request);


            popUpLayout.IsOpen = false;
            this.FindByName<StackLayout>("ProgressBox").IsVisible = true;
            timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
            this.FindByName<Label>("labelAct").Text = "Run";

        }
        async void NewWalk(object sender, System.EventArgs e)
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Medium);

            startLocation = await Geolocation.GetLocationAsync(request);

            popUpLayout.IsOpen = false;
            this.FindByName<StackLayout>("ProgressBox").IsVisible = true;
            timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
            this.FindByName<Label>("labelAct").Text = "Walk";

        }
        async void NewCycle(object sender, System.EventArgs e)
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Medium);

            startLocation = await Geolocation.GetLocationAsync(request);

            popUpLayout.IsOpen = false;
            this.FindByName<StackLayout>("ProgressBox").IsVisible = true;
            timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
            this.FindByName<Label>("labelAct").Text = "Cycle";
        }

        private async void EndActivity(object sender, System.EventArgs e)
        {
            timer.Dispose();
            timer = null;
            

            var request = new GeolocationRequest(GeolocationAccuracy.Medium);

            var finalLocation = await Geolocation.GetLocationAsync(request);

            double km = Location.CalculateDistance(startLocation, finalLocation, DistanceUnits.Kilometers);
            this.FindByName<Label>("distance").Text = km.ToString("#.000") + "km";

            startLocation = null;
            string finaltime = this.FindByName<Label>("txtTimer").Text;
            string setColor = "Orange";
            int points = 0;
            DateTime totalTime = DateTime.ParseExact(finaltime, "H:m:s", CultureInfo.CurrentCulture);
            if (this.FindByName<Label>("labelAct").Text.ToLower().Equals("run"))
            {
                setColor = "#E02F2C";
                points += 10;
                
            }
            if (this.FindByName<Label>("labelAct").Text.ToLower().Equals("walk"))
            {
                setColor = "#2C68E0";
                points += 10;

            }
            if (this.FindByName<Label>("labelAct").Text.ToLower().Equals("cycle"))
            {
                setColor = "#2CE056";
                points += 10;
            }
            Act1 act = new Act1()
            {
                Name = this.FindByName<Label>("labelAct").Text,
                Duration = totalTime.ToString("HH:mm:ss"),
                Image = this.FindByName<Label>("labelAct").Text.ToLower() + ".png",
                Color = setColor,
                DateUtc = DateTime.UtcNow

            };
            var fetchedUser = await Data.ReadAsync<User>(Preferences.Get("UserID", null), DefaultPartitions.UserDocuments);

            var result = JsonConvert.DeserializeObject<User>(fetchedUser.JsonValue.ToString());
            result.Points += points;
            result.Acts.Add(act);
            Preferences.Set("AddedActivity", true);
            await Data.ReplaceAsync(Preferences.Get("UserID", null), result, DefaultPartitions.UserDocuments);


            this.FindByName<StackLayout>("ProgressBox").IsVisible = false;
            this.FindByName<Label>("txtTimer").Text = "0:0:0";
            hour = 0;
            min = 0;
            sec = 0;

        }

        private async void Timer_Elapsed(object sender, ElapsedEventArgs e)
    {

            sec++;
            if(sec == 60)
            {
                min++;
                sec = 0;
            }
            if(min == 60)
            {
                hour++;
                min = 0;
            }
            var request = new GeolocationRequest(GeolocationAccuracy.Medium);

            var finalLocation = await Geolocation.GetLocationAsync(request);

            double km = Location.CalculateDistance(startLocation, finalLocation, DistanceUnits.Kilometers);
            

            Device.BeginInvokeOnMainThread(() => this.FindByName<Label>("txtTimer").Text = hour + ":" + min + ":" + sec);
            Device.BeginInvokeOnMainThread(() => this.FindByName<Label>("distance").Text = km.ToString("#.000") + "km");
            //Console.WriteLine($"{hour}:{min}:{sec}");



        }
    private async void FindMe()
        {
            Xamarin.Forms.Maps.Map map = this.FindByName<Xamarin.Forms.Maps.Map>("map");
            var locator = CrossGeolocator.Current;
            Plugin.Geolocator.Abstractions.Position position = new Plugin.Geolocator.Abstractions.Position();
            position = await locator.GetPositionAsync();
            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromMiles(1)));
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            




        }

        
    }
}
