using System;
using System.Collections.Generic;
using Microsoft.AppCenter.Data;
using Newtonsoft.Json;
using youth.Users;
using youth.Ach;
using Xamarin.Essentials;
using youth.AchRetriever;
using Syncfusion.ListView;
using Microsoft.AppCenter.Auth;
using youth.Activities;
using youth.Foo;

using Xamarin.Forms;
using System.Linq;

namespace youth
{
    public partial class Profile : ContentPage
    {
        public Profile()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }
        async void SignoutButton_Clicked(object sender, System.EventArgs e)
        {


            Preferences.Clear();
            Auth.SignOut();
               

            await this.Navigation.PopToRootAsync();


            
           



        }
        async void ViewGraphsButton_Clicked(object sender, System.EventArgs e)
        {


            await this.Navigation.PushAsync(new MyGraphs());








        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            //Remove navigation bar
            NavigationPage.SetHasNavigationBar(this, false);
            //Fetch user from AppCenter
            var fetchedUser = await Data.ReadAsync<User>(Preferences.Get("UserID", null), DefaultPartitions.UserDocuments);

            //Convert fetched user from Deserialised JSON to string. 
            var result = JsonConvert.DeserializeObject<User>(fetchedUser.JsonValue.ToString());

            //Initalise int's
            int ActCount = 0;
            int FoodCount = 0;
            int AchCount = 0;

            //Loop Food to count.
            foreach (Food1 i in result.Foods)
            {
                FoodCount += 1;


            }
            // Loop Achievements to count. 
            foreach (Achievements i in result.Achs)
            {
                AchCount += 1;

               
            }
            // Loop Acts to count. 
            foreach (Act1 i in result.Acts)
            {
                ActCount += 1;


            }
            // Change value of each text accordingly. 
            this.FindByName<Label>("AchCount").Text = AchCount.ToString();
            this.FindByName<Label>("ActCount").Text = ActCount.ToString();
            this.FindByName<Label>("FoodCount").Text = FoodCount.ToString();
            this.FindByName<Label>("PointCount").Text = result.Points.ToString();

            // Add Achievements and Items to list view. 
            this.FindByName<Syncfusion.ListView.XForms.SfListView>("achLV").ItemsSource = result.Achs.ToList();
            this.FindByName<Syncfusion.ListView.XForms.SfListView>("itemsLV").ItemsSource = result.Items.ToList();

            //FOR SQLITE USAGE
            //using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            //{

            //    conn.CreateTable<Act1>();
            //    var users = conn.Table<Act1>().ToList();

            //    //this.FindByName<ListView>("testLV").ItemsSource = users; 

            //}
        }
        
    }
}
