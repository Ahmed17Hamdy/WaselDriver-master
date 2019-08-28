using Com.OneSignal;
using Com.OneSignal.Abstractions;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaselDriver.Helper;
using WaselDriver.Views.IntroPages;
using WaselDriver.Views.PushNotificationPages;
using WaselDriver.Views.UserAuthentication;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WaselDriver.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplashPage : ContentPage
    {
        private string labelText;

        public SplashPage()
        {
            InitializeComponent();
        }
       

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await Animate.BallAnimate(this.logoImage, 50, 10, 5);
            Activ.IsRunning = false;
            OneSignal.Current.IdsAvailable((playerID, pushToken) =>
            {
                Settings.LastSignalID = playerID;
            });
            SetMainPage();
        }

        private void SetMainPage()
        {
            if (Settings.LastUsedDriverID == 0)
            {
                App.Current.MainPage = new NavigationPage(new LanguagePage());
            }
            else if(Settings.LastUsedDriverID==0 && Settings.LastRegister!= "")
            {
                App.Current.MainPage = new NavigationPage(new DriverRegestration());
            }
            else if ( Settings.LastUsedDriverID!= 0 && Settings.LastUserStatus != "0")
            {
                App.Current.MainPage = new NavigationPage(new MainTabbed());                                                                               
            }
            else if( Settings.LastUsedDriverID != 0 && Settings.LastUserStatus == "0")
            {
                App.Current.MainPage = new NavigationPage(new LoginPage());
            }
            
        }
        private void Button_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new SplashPage();
        }
    }
}