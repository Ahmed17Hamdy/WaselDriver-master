using Com.OneSignal;
using Com.OneSignal.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WaselDriver.Helper;
using WaselDriver.Views.PushNotificationPages;

namespace WaselDriver.Views.IntroPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainTabbed : TabbedPage
    {
        public MainTabbed()
        {
            InitializeComponent();
            OneSignal.Current.StartInit("1126a3d0-1d80-42ee-94db-d0449ac0a62c")
              .InFocusDisplaying(OSInFocusDisplayOption.None)
              .HandleNotificationReceived(OnNotificationRecevied)
              .HandleNotificationOpened(OnNotificationOpened)
              .EndInit();
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
    }
}