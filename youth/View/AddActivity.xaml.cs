using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using youth.Activities;
using SQLite;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Data;

using youth.Users;
using Xamarin.Essentials;
using Newtonsoft.Json;

using Xamarin.Forms;

namespace youth
{
    public partial class AddActivity: ContentPage
    {
        
        public AddActivity()
        {
            InitializeComponent();
           


        }
        public string classID;
        public string time; 
         void Activity_Clicked(object sender, System.EventArgs e)
        {
            var button = (ImageButton)sender;
           classID = button.ClassId;

            if (button.BackgroundColor.Equals(Color.Transparent))
                button.BackgroundColor = Color.FromHex("2EC4B6");
            else
                button.BackgroundColor = Color.Transparent;
            
           
        }
        async void ActivityButton_Clicked(object sender, System.EventArgs e)
        {
            var setColor = "";
            int points = 0;

            if (classID.Equals("run"))
            {
                setColor = "#E02F2C";
                points = 10;
            }
            if(classID.Equals("walk"))
            {
                setColor = "#2C68E0";
                points = 10;
            }
            if (classID.Equals("cycle"))
            {
                setColor = "#2CE056";
                points = 10;
            }
            if (classID.Equals("basketball"))
            {
                setColor = "#E09E2C";
                points = 10;
            }
            if (classID.Equals("soccer"))
            {
                setColor = "#12160F";
                points = 10;
            }
            if (classID.Equals("swim"))
            {
                setColor = "#2CCBE0";
                points = 10;
            }
            if (classID.Equals("tennis"))
            {
                setColor = "#E02C83";
                points = 10;
            }

           
            time = durationEntry.Time.ToString();
            Analytics.TrackEvent("[Tracker] Activity : " + char.ToUpper(classID[0]) + classID.Substring(1));



            Act1 act = new Act1 { Name = char.ToUpper(classID[0]) + classID.Substring(1), Duration = time, Image = classID + ".png", Color = setColor, DateUtc = DateTime.UtcNow};

            var fetchedUser = await Data.ReadAsync<User>(Preferences.Get("UserID", null), DefaultPartitions.UserDocuments);

            var result = JsonConvert.DeserializeObject<User>(fetchedUser.JsonValue.ToString());
            result.Points += points;
            result.Acts.Add(act);
            Preferences.Set("AddedActivity", true);


            await Data.ReplaceAsync(Preferences.Get("UserID", null), result, DefaultPartitions.UserDocuments);




            //Act1 user = new Act1()
            //{
            //    Name = char.ToUpper(classID[0]) + classID.Substring(1),
            //    Duration = time,
            //    Image = classID + ".png",
            //    Color = setColor,
            //    DateUtc = DateTime.UtcNow

            //};

            //using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            //{
            //    //Act1 - database name
            //    //conn.Execute("DELETE FROM user"); 
            //    conn.CreateTable<Act1>();
            //    int rowsAdded = conn.Insert(user);

            //}
            
            await Navigation.PopAsync();

        }
       
    }
}
