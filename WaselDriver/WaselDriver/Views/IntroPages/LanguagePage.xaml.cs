using Plugin.Connectivity;
using Plugin.Multilingual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaselDriver.Helper;
using WaselDriver.Models;
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
                CrossMultilingual.Current.CurrentCultureInfo = 
                    CrossMultilingual.Current.NeutralCultureInfoList.ToList().First(element => element.EnglishName.Contains("Arabic"));
                AppResources.Culture = CrossMultilingual.Current.CurrentCultureInfo;
                Settings.LastUserGravity = "Arabic";
                GravityClass.Grav();
                if (Settings.LastUsedID == 0 || Settings.LastUserStatus == "0")
                {
                    await Navigation.PushModalAsync(new LoginPage());
                }
                else
                {
                    await Navigation.PushModalAsync(new MainPage());
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
                Settings.LastUserGravity = "English";
                if (Settings.LastUsedID == 0 || Settings.LastUserStatus == "0")
                {
                    
                    await Navigation.PushModalAsync(new LoginPage());
                }
                else
                {
                    await Navigation.PushModalAsync(new MainPage());
                }
            }
            else await DisplayAlert("Message", AppResources.ErrorMessage, "Ok");
            Activ.IsRunning = false;
        }
        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Countryitem = e.SelectedItem as Country;
            Countryitem.TrueImage = "checked.png";
            Settings.LastCountry = Countryitem.id;
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

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
            Settings.LastUserGravity = "English";
        }

        private void TapGestureRecognizer_Tapped_3(object sender, EventArgs e)
        {
            ArLangImg.IsVisible = true;
            EnLangImg.IsVisible = false;
            Settings.LastUserGravity = "Arabic";
        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            if (Checker())
            {
                Settings.LastCountry = Countryitem.id;
                if (Settings.LastUserGravity == "Arabic")
                    Arabic_Clicked();
                else English_Clicked();
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new LanguagePage();
        }
    }
}