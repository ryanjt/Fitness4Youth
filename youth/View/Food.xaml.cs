using System;
using System.Collections.Generic;
using Microsoft.AppCenter.Auth;
using Microsoft.AppCenter.Data;
using Xamarin.Forms;
using youth.Users;
using Xamarin.Essentials;
using Newtonsoft.Json;
using youth.FooRetriever;
using Syncfusion.SfAutoComplete;
using youth.Foo;
using System.Linq;
using Syncfusion.XForms.PopupLayout;
using System.Threading.Tasks;


namespace youth
{
    public partial class Food : ContentPage
    {
        Label popupContent;
        DataTemplate templateView;

        SfPopupLayout popupLayout;

        public Food()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS)
            {
                LoadFood();
            }
            if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.Android)
            {
                LoadFoodAndroid();
            }
        }
            async void LoadFood()
            {
                var fetched = await Data.ReadAsync<FoodRetriever>("public-food", DefaultPartitions.AppDocuments);

                var result = JsonConvert.DeserializeObject<FoodRetriever>(fetched.JsonValue);

                //List<string> foodNames = new List<string>();

                //foreach (Food1 i in result.Foods)
                //{

                //    foodNames.Add(i.Name.ToString());

                //}

                var fetchedUser = await Data.ReadAsync<User>(Preferences.Get("UserID", null), DefaultPartitions.UserDocuments);

                var resultUser = JsonConvert.DeserializeObject<User>(fetchedUser.JsonValue.ToString());


                this.FindByName<Syncfusion.SfAutoComplete.XForms.SfAutoComplete>("foodLC").DataSource = result.Foods.ToList();
                this.FindByName<ListView>("foodLV").ItemsSource = resultUser.Foods.ToList();
                //await Data.ReplaceAsync(resultUser.Id.ToString(), resultUser, DefaultPartitions.UserDocuments);
            }

            async void LoadFoodAndroid()
            {
              
                //TO DO IN THE FUTURE
            }
        
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            var fetchedUser = await Data.ReadAsync<User>(Preferences.Get("UserID", null), DefaultPartitions.UserDocuments);
            var resultUser = JsonConvert.DeserializeObject<User>(fetchedUser.JsonValue.ToString());           
            this.FindByName<ListView>("foodLV").ItemsSource = resultUser.Foods.ToList();
        }

        async void foodLC_SelectionChanged(System.Object sender, Syncfusion.SfAutoComplete.XForms.SelectionChangedEventArgs e)
        {
            var fetchedUser = await Data.ReadAsync<User>(Preferences.Get("UserID", null), DefaultPartitions.UserDocuments);
            var resultUser = JsonConvert.DeserializeObject<User>(fetchedUser.JsonValue.ToString());
            var fetched = await Data.ReadAsync<FoodRetriever>("public-food", DefaultPartitions.AppDocuments);
            //var fetchedUser = await Data.ReadAsync<User>(Preferences.Get("UserID", null), DefaultPartitions.UserDocuments);
            var result = JsonConvert.DeserializeObject<FoodRetriever>(fetched.JsonValue);
            //var resultUser = JsonConvert.DeserializeObject<User>(fetchedUser.JsonValue.ToString());
            
            if(e.AddedItems as Food1 != null)
            {
                var selectedFood = e.AddedItems as Food1;
                Console.WriteLine(selectedFood.Name);
                resultUser.Foods.Add(selectedFood);
                resultUser.Points += selectedFood.Points;
                await Data.ReplaceAsync(resultUser.Id.ToString(), resultUser, DefaultPartitions.UserDocuments);
                popupLayout = new SfPopupLayout();

                popupLayout.PopupView.ShowHeader = false;
                popupLayout.PopupView.ShowFooter = false;
                popupLayout.PopupView.ShowCloseButton = false;
                templateView = new DataTemplate(() =>
                {
                    popupContent = new Label();
                    popupContent.Text = selectedFood.Name + " has been added. + " + selectedFood.Points + " points!";
                    popupContent.HorizontalTextAlignment = TextAlignment.Center;
                    popupContent.VerticalTextAlignment = TextAlignment.Center;
                    popupContent.FontAttributes = FontAttributes.Bold;
                    popupContent.FontSize = 15;


                    return popupContent;
                });

                // Adding ContentTemplate of the SfPopupLayout
                popupLayout.PopupView.HeightRequest = 120;
                popupLayout.PopupView.ContentTemplate = templateView;
                popupLayout.PopupView.AnimationMode = AnimationMode.SlideOnBottom;




                await Task.Delay(2000);

                popupLayout.Show();


                await Task.Delay(2000);
                popupLayout.PopupView.AnimationMode = AnimationMode.SlideOnTop;
                popupLayout.IsOpen = false;
                this.FindByName<ListView>("foodLV").ItemsSource = resultUser.Foods.ToList();
            }
           
          

        }
        public async void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var convert = ((Food1)mi.CommandParameter);
            var fetchedUser = await Data.ReadAsync<User>(Preferences.Get("UserID", null), DefaultPartitions.UserDocuments);
            var result = JsonConvert.DeserializeObject<User>(fetchedUser.JsonValue.ToString());
            var itemToRemove = result.Foods.Single(r => r.FID == convert.FID);

            if (itemToRemove != null)
            {
                result.Foods.Remove(itemToRemove);
            }




            await Data.ReplaceAsync(Preferences.Get("UserID", null), result, DefaultPartitions.UserDocuments);
            popupLayout = new SfPopupLayout();

            popupLayout.PopupView.ShowHeader = false;
            popupLayout.PopupView.ShowFooter = false;
            popupLayout.PopupView.ShowCloseButton = false;
            templateView = new DataTemplate(() =>
            {
                popupContent = new Label();
                popupContent.Text = "Deleted " + convert.Name + "!";
                popupContent.HorizontalTextAlignment = TextAlignment.Center;
                popupContent.VerticalTextAlignment = TextAlignment.Center;
                popupContent.FontAttributes = FontAttributes.Bold;
                popupContent.FontSize = 15;


                return popupContent;
            });

            // Adding ContentTemplate of the SfPopupLayout
            popupLayout.PopupView.HeightRequest = 120;
            popupLayout.PopupView.ContentTemplate = templateView;
            popupLayout.PopupView.AnimationMode = AnimationMode.SlideOnBottom;




            await Task.Delay(1000);

            popupLayout.Show();


            await Task.Delay(2000);
            popupLayout.PopupView.AnimationMode = AnimationMode.SlideOnTop;
            popupLayout.IsOpen = false;

            var fetchedUser1 = await Data.ReadAsync<User>(Preferences.Get("UserID", null), DefaultPartitions.UserDocuments);

            var result1 = JsonConvert.DeserializeObject<User>(fetchedUser1.JsonValue.ToString());



            this.FindByName<ListView>("foodLV").ItemsSource = result1.Foods.ToList();

        }
    }

}



