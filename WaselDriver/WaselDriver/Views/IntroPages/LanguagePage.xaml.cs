using Plugin.Connectivity;
using Plugin.Multilingual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaselDriver.Helper;
using WaselDriver.Models;
using WaselDriver.ViewModels;
using WaselDriver.Views.UserAuthentication;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WaselDriver.Views.IntroPages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LanguagePage : ContentPage
	{
        Country Countryitem;
        public LanguagePage()
        {
            InitializeComponent();
            FlowDirection = (Settings.LastUserGravity == "Arabic") ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
            Settings.LastCountry = 0;
            Settings.LastUserGravity = "";
        }
        private async void Arabic_Clicked()
        {
            Activ.IsRunning = true;
            if (CrossConnectivity.Current.IsConnected)
            {
                CrossMultilingual.Current.CurrentCultureInfo = CrossMultilingual.Current.NeutralCultureInfoList.ToList().First(element => element.EnglishName.Contains("Arabic"));
                AppResources.Culture = CrossMultilingual.Current.CurrentCultureInfo;
                //GravityClass.Grav();
                if (Settings.LastUsedID == 0)
                {
                    await Navigation.PushAsync(new LoginPage());
                }
                else
                {
                    App.Current.MainPage = new MainPage();
                }
                //App.Current.MainPage = new MenuPage();
            }
            else
            {
                await DisplayAlert("Message", AppResources.ErrorMessage, "Ok");
            }
            Activ.IsRunning = false;
        }
        private async void English_Clicked()
        {
            Activ.IsRunning = true;
            if (CrossConnectivity.Current.IsConnected)
            {
                CrossMultilingual.Current.CurrentCultureInfo = CrossMultilingual.Current.NeutralCultureInfoList.ToList().First(element => element.EnglishName.Contains("English"));
                AppResources.Culture = CrossMultilingual.Current.CurrentCultureInfo;
                if (Settings.LastUsedID == 0)
                {
                    Settings.LastUseeRole = 0;
                    await Navigation.PushAsync(new LoginPage());
                }
                else
                {
                    App.Current.MainPage = new MainPage();
                }
            }
            else await DisplayAlert("Message", AppResources.ErrorMessage, "Ok");
            Activ.IsRunning = false;
        }
        private bool Checker()
        {
            if (Settings.LastCountry == 0)
            {
                DisplayAlert("", "من فضلك أختر الدولة", "Ok");
                return false;
            }
            if (Settings.LastUserGravity == "")
            {
                DisplayAlert("", "من فضلك أختر اللغة", "Ok");
                return false;
            }
            return true;
        }

        private void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
        {
            ArLangImg.IsVisible = false;
            EnLangImg.IsVisible = true;
            CountryList.IsVisible = true;
            Settings.LastUserGravity = "English";
        }

        private void TapGestureRecognizer_Tapped_3(object sender, EventArgs e)
        {
            ArLangImg.IsVisible = true;
            EnLangImg.IsVisible = false;
            CountryList.IsVisible = true;
            Settings.LastUserGravity = "Arabic";
        }
        private void Button_Clicked(object sender, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                MainViewModel vm = new MainViewModel();
                this.BindingContext = vm;
            }
            else DisplayAlert("", AppResources.ErrorMessage, "Ok");
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Activ.IsRunning = true;
            Countryitem = e.Item as Country;
            Countryitem.TrueImage = "checked.png";
            Settings.LastCountry = Countryitem.id;
            if (Checker())
            {
                Settings.LastCountry = Countryitem.id;
                if (Settings.LastUserGravity == "Arabic")
                    Arabic_Clicked();
                else English_Clicked();
            }
            Activ.IsRunning = false;
        }
    }
}