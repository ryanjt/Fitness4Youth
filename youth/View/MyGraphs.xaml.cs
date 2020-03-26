using System;
using System.Collections.Generic;

using Xamarin.Forms;
using youth.Model;
using Syncfusion.SfChart.XForms;
using System.Threading.Tasks;
using Microsoft.AppCenter.Data;
using Xamarin.Essentials;
using Newtonsoft.Json;
using youth.Activities;
using youth.Users;
using System.Collections.ObjectModel;
using System.Globalization;

namespace youth
{
    public partial class MyGraphs : ContentPage
    {
        public ObservableCollection<TotalAct> Total { get; set; } = new ObservableCollection<TotalAct>();
        public ObservableCollection<TotalMonth> Totalm { get; set; } = new ObservableCollection<TotalMonth>();
        public ObservableCollection<TotalMonth> Totalm2 { get; set; } = new ObservableCollection<TotalMonth>();
        public MyGraphs()
        {

            InitializeComponent();
           


        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            

           
            // Fetch user from AppCenter
            var fetchedUser = await Data.ReadAsync<User>(Preferences.Get("UserID", null), DefaultPartitions.UserDocuments);

            //Convert user to Derserialised String
            var result = JsonConvert.DeserializeObject<User>(fetchedUser.JsonValue.ToString());
            
           // Loop through Activities
            foreach(Act1 act in result.Acts){

              
                   
                    
                    Totalm.Add(new TotalMonth(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(act.DateUtc.Month), GetTotal(result, act.Name), act.Name));

                    
                
                
            }
            

           
            // Add each individual activity to ObservableCollection
            Total.Add(new TotalAct("Run", GetTotal(result, "Run")));
            Total.Add(new TotalAct("Walk", GetTotal(result, "Walk")));
            Total.Add(new TotalAct("Cycle", GetTotal(result, "Cycle")));
            Total.Add(new TotalAct("Basketball", GetTotal(result, "Basketball")));
            Total.Add(new TotalAct("Soccer", GetTotal(result, "Soccer")));
            Total.Add(new TotalAct("Tennis", GetTotal(result, "Tennis")));
            Total.Add(new TotalAct("Swim", GetTotal(result, "Swim")));

            // Add source to Doughnut Chart
            this.FindByName<DoughnutSeries>("Chart").ItemsSource = Total;
            //this.FindByName<DoughnutSeries>("Chart1").ItemsSource = Total;

            //Add source to Pyrmaid Chart
            this.FindByName<PyramidSeries>("Test").ItemsSource = Total;
            

        }

        

        public int GetTotal(User result, string act)
        {
            //Initalise var
            int total = 0;

            //Loop through activity
            foreach (Act1 i in result.Acts)
            {
                if (i.Name.Equals(act))
                {
                    //Increment
                    total += 1;
                }



            }
            //Return total
            return total;
        }

       
        
    }
}
