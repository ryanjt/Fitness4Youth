using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using youth.Users;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Data;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using SQLite;


using youth.Activities;

using Xamarin.Essentials;
using Xamarin.Forms;
using Microsoft.AppCenter.Auth;
using youth.Ach;
using youth.AchRetriever;
using Newtonsoft.Json;

using youth.Foo;
using youth.ShopClass;


namespace youth
{
    public partial class Login : ContentPage
    {
        

        public Login()
        {
            InitializeComponent();
            
           
            var span = new TimeSpan(0, 0, 4);
            Xamarin.Forms.Device.StartTimer(span, UpdateLogin);


        }

        private bool UpdateLogin()
        {
            
            
            List<string> Messages = new List<string> { "Get Active", "Track Food and Activities", "Earn Points" };
            List<string> Images = new List<string> { "run.png", "swim.png", "Food/apple.png", "Food/ban.png", "Food/water.png", "walk.png", "soccer.png" };

            Random rnd = new Random();
        
            int r2 = rnd.Next(Messages.Count);
            int r3 = rnd.Next(Images.Count);

           
            this.FindByName<Label>("LoginMessage").Text = Messages[r2];
            this.FindByName<Image>("LoginImage").Source = Images[r3];
            this.FindByName<Image>("LoginImage").Opacity = 0;
            this.FindByName<Label>("LoginMessage").Opacity = 0;
            this.FindByName<Image>("LoginImage").FadeTo(1, 2000);
            this.FindByName<Label>("LoginMessage").FadeTo(1, 2000);
            return true; 
            
        }

        void LoginButton_Clicked(object sender, System.EventArgs e)
        {

            if(Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS)
            {
                try
                {
                                     
                    SignInIOS();
                    //SignInAndroid();

                                                         
                }
                catch (Exception exception)
                {


                    Crashes.TrackError(exception);

                }
            }
            if(Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.Android)
            {
                try
                {
                    
                    SignInAndroid();
                                   

                }
                catch (Exception exception)
                {


                    Crashes.TrackError(exception);

                }
            }
            


        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
           
     
        }

       

        async void SignInAndroid()
        {
            try
            {
                

                if (Preferences.Get("UserID", null) == null)
                {
                    

                    User user = new User { Points = 100 };
                    Act1 act = new Act1 { Name = "Test Activity", Color = "Blue", Image = "Walk.png", Duration = "00:00:00", DateUtc = DateTime.UtcNow };
                    Food1 food = new Food1 { Name = "Broc", Calories = 100, Points = 10, FID = "1", Color = "Green", Image="test.png" };
                    Achievements ach = new Achievements { Name = "Test", AID=1, Description="Test", Image="Test.png", Points= 100};
                    Shop1 item = new Shop1 { Name = "Test", Description = "test", Color = "Red", Coupon = "2002", Image = "test.png", Points = 100, SId = 1 };


                    user.Items.Add(item);
                    user.Foods.Add(food);
                    user.Achs.Add(ach);
                    user.Acts.Add(act);


                    Preferences.Set("UserID", user.Id.ToString());

                   
                   

                    
                    Console.WriteLine("~Android~ : Successfully added user to firebase!");
                }

                

                await this.Navigation.PushAsync(new MainPage());



               
            }
            catch (Exception e)
            {

                Console.WriteLine("Debug 1: Unable to sign in");
                // Do something with sign-in failure.
            }
        }
        async void SignInIOS()
        {
            try
            {
                // Authenticate
                UserInformation userInfo = await Auth.SignInAsync();
                string accountId = userInfo.AccountId;
                

                // Testing
                //Act1 act = new Act1 { Name = "Run", Color = "Blue", Duration = "10:10:10" };
                //user.Acts.Add(act);
                //Preferences.Clear();

                //Check if new user
                if (Preferences.Get("UserID", null) == null)
                {
                    User user = new User { Points = 100 };
                    Act1 act = new Act1 { Name = "Test Activity", Color = "Blue", Image = "Walk.png", Duration = "00:00:00", DateUtc = DateTime.UtcNow };
                    user.Acts.Add(act);
                    Preferences.Set("UserID", user.Id.ToString());
                    // Create new user
                    await Data.CreateAsync(user.Id.ToString(), user, DefaultPartitions.UserDocuments);
                }


                // For achievements
                var fetchedUser = await Data.ReadAsync<User>(Preferences.Get("UserID", null), DefaultPartitions.UserDocuments);

                var fetched = await Data.ReadAsync<AchievementRetriever>("public-achievements", DefaultPartitions.AppDocuments);

                var result = JsonConvert.DeserializeObject<AchievementRetriever>(fetched.JsonValue.ToString());
                var resultUser = JsonConvert.DeserializeObject<User>(fetchedUser.JsonValue.ToString());


                foreach (Achievements i in result.Achs)
                {
                    //resultUser.Achs.Add(i);
                    Console.WriteLine(i.AID + "/" + i.Name + "/" + i.Description + "/" + i.Points);
                }

                //await Data.ReplaceAsync(resultUser.Id.ToString(), resultUser, DefaultPartitions.UserDocuments);



                await this.Navigation.PushAsync(new MainPage());



                Console.WriteLine("Debug 1: " + accountId);
            }
            catch (Exception e)
            {
               
                Console.WriteLine("Debug 1: Unable to sign in");
                // Do something with sign-in failure.
            }
        }
       

    }
    
}