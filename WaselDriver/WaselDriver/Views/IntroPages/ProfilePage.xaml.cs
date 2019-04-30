using Newtonsoft.Json;
using Plugin.Connectivity;
using Plugin.Multilingual;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaselDriver.Helper;
using WaselDriver.Models;
using WaselDriver.Services;
using WaselDriver.Views.PopUps;
using WaselDriver.Views.UserAuthentication;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WaselDriver.Views.IntroPages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfilePage : ContentPage
	{
        bool LangVis = false, vis = false;
        int rout1 = 180, rout2 = 180;
        UserServices ser;
        public ProfilePage ()
		{
			InitializeComponent ();
            ser = new UserServices();
            FlowDirection = (Settings.LastUserGravity == "Arabic") ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
            Namelbl.Text = Settings.ProfileName;
        }

        

        private void EnglishSelected(object sender, EventArgs e)
        {
            ArLangImg.IsVisible = false;
            EnLangImg.IsVisible = true;
            Settings.LastUserGravity = "English";
        }

        private void ArabicSelected(object sender, EventArgs e)
        {
            EnLangImg.IsVisible = false;
            ArLangImg.IsVisible = true;
            Settings.LastUserGravity = "Arabic";
        }

        private async void TapGestureRecognizer_Tapped_3(object sender, EventArgs e)
        {
          
            rout1 = (rout1 > 360) ? rout1 - 360 : rout1;
            StkPass.IsVisible = !vis;
            vis = !vis;
            await ImgDown.RotateTo(rout1);
            await ImgDown.RotateTo(rout1, 500, Easing.SpringOut);
            rout1 += 180;
            
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (EntryNewPass.Text == EntryConfirmPass.Text)
            {
                var response = await ser.ChangePassword(EntryOldPass.Text, EntryNewPass.Text);
                var json = JsonConvert.DeserializeObject<Response<string, string>>(response);
                if (json.success == true) await DisplayAlert("", json.message, "Ok");
                else await DisplayAlert(AppResources.Error, "من فضلك تحقق من كلمة السر القديمة", "Ok");
            }
            else if (EntryNewPass.Text.Length < 6)
            {
                await DisplayAlert("", "كلمة السر الجديدة يجب أن تكون أكثر من 6 أحرف", "Ok");
            }
            else
            {
                await DisplayAlert("", "كلمة السر الجديدة غير متطابقة", "Ok");
            }
        }
        private async void Arabic_Clicked()
        {
           
            if (CrossConnectivity.Current.IsConnected)
            {
                CrossMultilingual.Current.CurrentCultureInfo =
                    CrossMultilingual.Current.NeutralCultureInfoList.ToList().First(element => element.EnglishName.Contains("Arabic"));
                AppResources.Culture = CrossMultilingual.Current.CurrentCultureInfo;
                Settings.LastUserGravity = "Arabic";
                GravityClass.Grav();
                if (Settings.LastUsedDriverID == 0 || Settings.LastUserStatus == "0")
                {
                    await Navigation.PushModalAsync(new LoginPage());
                }
                else
                {
                    App.Current.MainPage = new MainTabbed();
                }
               
            }
            else
            {
                await DisplayAlert("Message", AppResources.ErrorMessage, "Ok");
            }
           
        }
        private async void English_Clicked()
        {
            
            if (CrossConnectivity.Current.IsConnected)
            {
                CrossMultilingual.Current.CurrentCultureInfo = CrossMultilingual.Current.NeutralCultureInfoList.ToList().First(element => element.EnglishName.Contains("English"));
                AppResources.Culture = CrossMultilingual.Current.CurrentCultureInfo;
                Settings.LastUserGravity = "English";
                if (Settings.LastUsedDriverID == 0 || Settings.LastUserStatus == "0")
                {

                    await Navigation.PushModalAsync(new LoginPage());
                }
                else
                {
                    App.Current.MainPage = new MainTabbed();
                }
            }
            else await DisplayAlert("Message", AppResources.ErrorMessage, "Ok");
           
        }

        private async void LogOutbtn_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new LogoutPopup());
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            rout2 = (rout2 > 360) ? rout2 - 360 : rout2;
            langStack.IsVisible = !LangVis;
            LangVis = !LangVis;
            await LangImgDown.RotateTo(rout2);
            await LangImgDown.RotateTo(rout2, 500, Easing.SpringOut);
            rout2 += 180;
        }

        private bool Checker()
        {
            
            if (Settings.LastUserGravity == "")
            {
                DisplayAlert("", "من فضلك أختر اللغة", "Ok");
                return false;
            }
            return true;
        }
        private void ConfirmLang(object sender, EventArgs e)
        {
            if (Checker())
            {
               
                if (Settings.LastUserGravity == "Arabic")
                    Arabic_Clicked();
                else English_Clicked();
            }
        }

    }
}