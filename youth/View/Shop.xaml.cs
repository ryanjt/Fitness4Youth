using System;
using System.Collections.Generic;
using Microsoft.AppCenter.Data;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using youth.Users;
using youth.ShopRetriever;
using youth.ShopClass;
using System.Windows.Input;

namespace youth
{
    public partial class Shop : ContentPage
    {
        
        public Shop()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            //Fetch user
            var fetchedUser = await Data.ReadAsync<User>(Preferences.Get("UserID", null), DefaultPartitions.UserDocuments);

            //Fetch shop items document
            var fetched = await Data.ReadAsync<ShopRet>("public-shop", DefaultPartitions.AppDocuments);

            // Convert items document to string
            var result = JsonConvert.DeserializeObject<ShopRet>(fetched.JsonValue.ToString());

            //Convert user to string
            var resultUser = JsonConvert.DeserializeObject<User>(fetchedUser.JsonValue.ToString());




            //Add source to listviews.
            this.FindByName<Syncfusion.ListView.XForms.SfListView>("shopLV").ItemsSource = result.Shops;
            this.FindByName<Syncfusion.ListView.XForms.SfListView>("featureLV").ItemsSource = result.Featured;

            //Change points to user points
            this.FindByName<Label>("Pointslbl").Text = "Points: " + resultUser.Points.ToString();
            Console.WriteLine(resultUser.Points.ToString()); // debug
            //this.FindByName<ListView>("shopLV").ItemsSource = result.Shops;


        }

        void shopLV_ItemDoubleTapped(System.Object sender, Syncfusion.ListView.XForms.ItemDoubleTappedEventArgs e)
        {

            //On double tap - show Popup Modal
            Shop1 selectedShop = (Shop1)this.FindByName<Syncfusion.ListView.XForms.SfListView>("shopLV").SelectedItem;
            popUpLayout.BindingContext = selectedShop;
            popUpLayout.StaysOpen = true;
            popUpLayout.PopupView.ShowCloseButton = true;
            popUpLayout.IsOpen = true;
            

            //this.FindByName<Image>("PImage").Source = selectedShop.Image.ToString();
            //this.FindByName<Label>("PTitle").Text = selectedShop.Name;
            //this.FindByName<Label>("PDesc").Text = selectedShop.Description;




        }

        async void  SfButton_Clicked(System.Object sender, System.EventArgs e)
        {
            // Get selected item
            Shop1 selectedShop = (Shop1)this.FindByName<Syncfusion.ListView.XForms.SfListView>("shopLV").SelectedItem;

            // Fetch user
            var fetchedUser = await Data.ReadAsync<User>(Preferences.Get("UserID", null), DefaultPartitions.UserDocuments);

            // Convert fetched user to string. 
            var resultUser = JsonConvert.DeserializeObject<User>(fetchedUser.JsonValue.ToString());

            // Remove points.
            resultUser.Points -= selectedShop.Points;

            // Add item to users items.
            resultUser.Items.Add(selectedShop);

            //Send user back to database
            await Data.ReplaceAsync(Preferences.Get("UserID", null), resultUser, DefaultPartitions.UserDocuments);

            //Hide popup layout
            popUpLayout.StaysOpen = false;
            popUpLayout.PopupView.ShowCloseButton = false;
            popUpLayout.IsOpen = false;

        }

        void SfButton_Clicked_1(System.Object sender, System.EventArgs e)
        {
            popUpLayout.StaysOpen = false;
            popUpLayout.PopupView.ShowCloseButton = false;
            popUpLayout.IsOpen = false;
        }
    }
   
}
