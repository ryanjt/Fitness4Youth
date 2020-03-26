using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using SQLite;
using youth.Users;
using System.Linq;

using Microsoft.AppCenter.Auth;
namespace youth
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);


        }

        

    }
}
