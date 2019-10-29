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
            
        }
        
    }
}