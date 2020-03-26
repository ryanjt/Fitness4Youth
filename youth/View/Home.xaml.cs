using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using SQLite;
using youth.Users;
using youth.Activities;
using System.Linq;
using Newtonsoft.Json;

using Microsoft.AppCenter.Data;
using Microsoft.AppCenter.Auth;
using Xamarin.Essentials;
using Syncfusion.XForms.PopupLayout;
using System.Threading.Tasks;
using Syncfusion.XForms.Graphics;


namespace youth
{

    public partial class Home : ContentPage
    {
        
        DataTemplate templateView;
        Label popupContent;
        Label mesageContent;

        SfPopupLayout popupLayout;
        SfPopupLayout messageLayout;

        public Home()
        {
            InitializeComponent();





        }

        async void ShowMessage()
        {

            messageLayout = new SfPopupLayout();

            messageLayout.PopupView.ShowHeader = false;
            messageLayout.PopupView.ShowFooter = false;
            messageLayout.PopupView.ShowCloseButton = false;


            templateView = new DataTemplate(() =>
            {
                Grid messageGrid = new Grid();
                SfGradientView gradientView = new SfGradientView();
                SfLinearGradientBrush linearGradientBrush = new SfLinearGradientBrush();

                linearGradientBrush.GradientStops = new GradientStopCollection()
                {
                    new SfGradientStop(){Color = Color.FromHex("#0048b9"), Offset=0.0},
                    new SfGradientStop(){Color = Color.FromHex("#00a8e8"), Offset=1.0},
                };
                gradientView.BackgroundBrush = linearGradientBrush;
                messageGrid.Children.Add(gradientView);

                StackLayout messageStack = new StackLayout();


                Label mesageContent = new Label();


                mesageContent.Text = "New Activity Added! +10 points!";

                mesageContent.HorizontalTextAlignment = TextAlignment.Center;
                mesageContent.VerticalTextAlignment = TextAlignment.Center;
                mesageContent.FontAttributes = FontAttributes.Bold;
                mesageContent.FontSize = 15;
                mesageContent.Padding = 10;
                mesageContent.TextColor = Color.White;
                messageStack.Children.Add(mesageContent);
                messageGrid.Children.Add(messageStack);

                return messageGrid;

            });

            // Adding ContentTemplate of the SfPopupLayout
            messageLayout.PopupView.HeightRequest = 120;
            messageLayout.PopupView.ContentTemplate = templateView;
            messageLayout.PopupView.AnimationMode = AnimationMode.SlideOnBottom;


            await Task.Delay(2000);

            messageLayout.Show();


            await Task.Delay(2000);
            messageLayout.PopupView.AnimationMode = AnimationMode.SlideOnTop;
            messageLayout.IsOpen = false;
        }

        void NewActivity(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new AddActivity());
        }

        public async void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var convert = ((Act1)mi.CommandParameter);
            var fetchedUser = await Data.ReadAsync<User>(Preferences.Get("UserID", null), DefaultPartitions.UserDocuments);
            var result = JsonConvert.DeserializeObject<User>(fetchedUser.JsonValue.ToString());
            var itemToRemove = result.Acts.Single(r => r.Id == convert.Id);

            if (itemToRemove != null)
            {
                result.Acts.Remove(itemToRemove);
            }




            await Data.ReplaceAsync(Preferences.Get("UserID", null), result, DefaultPartitions.UserDocuments);
            popupLayout = new SfPopupLayout();

            popupLayout.PopupView.ShowHeader = false;
            popupLayout.PopupView.ShowFooter = false;
            popupLayout.PopupView.ShowCloseButton = false;
            templateView = new DataTemplate(() =>
            {
                popupContent = new Label();
                popupContent.Text = "Deleted Activity!";
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



            this.FindByName<ListView>("testLV").ItemsSource = result1.Acts.ToList();

        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            radialMenu.Point = new Point(140, 260);

            // Call from AppCenter if IOS
            if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS)
            {
                // Fetch User
                var fetchedUser = await Data.ReadAsync<User>(Preferences.Get("UserID", null), DefaultPartitions.UserDocuments);

                //Deserialize JSON
                var result = JsonConvert.DeserializeObject<User>(fetchedUser.JsonValue.ToString());


                //Used to check if new activity added. 
                if (Preferences.Get("AddedActivity", false) == true)
                {
                    ShowMessage();
                    Preferences.Set("AddedActivity", false);
                }




                // Conver Deserialised JSON to list for Home Page. 
                this.FindByName<ListView>("testLV").ItemsSource = result.Acts.ToList();
            }
            if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.Android)
            {
                

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
    }

