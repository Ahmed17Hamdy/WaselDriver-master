using Com.OneSignal;
using Com.OneSignal.Abstractions;
using System;
using WaselDriver.Views;
using WaselDriver.Views.IntroPages;
using WaselDriver.Views.PushNotificationPages;
using WaselDriver.Views.UserAuthentication;
using WaselDriver.Helper;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WaselDriver.Views.OrderPage;
using System.Collections.ObjectModel;
using TK.CustomMap.Overlays;
using TK.CustomMap;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace WaselDriver
{
    public partial class App : Application
    {
       

        public App()
        {
            InitializeComponent();
            OneSignal.Current.StartInit("1126a3d0-1d80-42ee-94db-d0449ac0a62c")
              .InFocusDisplaying(OSInFocusDisplayOption.None)
              .HandleNotificationReceived(OnNotificationRecevied)
              .HandleNotificationOpened(OnNotificationOpened)
              .EndInit();
            MainPage = new SplashPage();
          
        }

        private void OnNotificationOpened(OSNotificationOpenedResult result)
        {
            if (result.notification?.payload?.additionalData == null)
            {
                return;
            }

            if (result.notification.payload.additionalData.ContainsKey("body"))
            {
                var labelText = result.notification.payload.additionalData["body"].ToString();
                Settings.LastNotify = labelText;
                App.Current.MainPage = new NotificationSummaryPage();
            }

        }

        private void OnNotificationRecevied(OSNotification notification)
        {
            if (notification.payload?.additionalData == null)
            {
                return;
            }

            if (notification.payload.additionalData.ContainsKey("body"))
            {
                var labelText = notification.payload.additionalData["body"].ToString();
                Settings.LastNotify = labelText;
                App.Current.MainPage = new NotificationSummaryPage();
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
