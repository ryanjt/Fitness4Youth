using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Data;
using Microsoft.AppCenter.Auth;
using Xamarin.Forms.Maps;
using Syncfusion.XForms.iOS.EffectsView;
using Syncfusion.ListView.XForms.iOS;
using Syncfusion.XForms.iOS.Graphics;
using Syncfusion.XForms.iOS.Buttons;
using Syncfusion.SfAutoComplete.iOS;
using Foundation;
using UIKit;
using Syncfusion.SfRadialMenu.XForms.iOS;
using Syncfusion.SfChart.XForms.iOS;

namespace youth.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your applicatiosn.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            new Syncfusion.SfAutoComplete.XForms.iOS.SfAutoCompleteRenderer();
            global::Xamarin.Forms.Forms.Init();
            Xamarin.FormsMaps.Init();
            AppCenter.Start("ios=aa6903ad-9ec2-4ed5-b067-bde25d4aa602;" + "android=57c9b3f5-30f0-42aa-92cd-76394490bb70", typeof(Analytics), typeof(Crashes), typeof(Auth), typeof(Data));
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTk4MTMzQDMxMzcyZTM0MmUzMEtnZWV6UEplVUIrUms0ZmFpWGRUdXVMcERjNEpxQU16bkNrV1hQL2FqMXM9");
            SfListViewRenderer.Init();
            SfEffectsViewRenderer.Init();
            SfRadialMenuRenderer.Init();
            Syncfusion.SfChart.XForms.iOS.Renderers.SfChartRenderer.Init();
            Syncfusion.XForms.iOS.PopupLayout.SfPopupLayoutRenderer.Init();
            Syncfusion.XForms.iOS.Graphics.SfGradientViewRenderer.Init();
            Syncfusion.XForms.iOS.Border.SfBorderRenderer.Init();
            Syncfusion.XForms.iOS.Buttons.SfButtonRenderer.Init();
            

            string fileName = "user.db3";
            string folderPath = Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "..", "Library");
            string completePath = Path.Combine(folderPath, fileName);

            LoadApplication(new App(completePath));

            return base.FinishedLaunching(app, options);
            
        }
    }
}
