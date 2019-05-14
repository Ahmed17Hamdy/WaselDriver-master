using Newtonsoft.Json;
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
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WaselDriver.Views.UserAuthentication
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Register : ContentPage
	{
		public Register ()
		{
			InitializeComponent ();
            
		}
        private async void Regbtn_Clicked(object sender, EventArgs e)
        {
            Activ.IsRunning = true;
            {
                User _user = new User
                {
                    name = EntryName.Text,
                    email = EntryEmail.Text,
                    password = EntryPassword.Text,
                    confirmpass = ConfirmPassword.Text,
                    phone = EntryPhone.Text,
                    country = Settings.LastCountry.ToString(),
                    role = Settings.LastUseeRole = 3,
                    device_id = "192.168.1.20",
                    firebase_token = Settings.UserFirebaseToken = "36666666",
                    

                };
                UserServices userService = new UserServices();
               var ResBack = await userService.InsertUser(_user);
                if (ResBack == "false")
                {
                    await DisplayAlert(AppResources.Error, AppResources.ErrorMessage, AppResources.Ok);
                }
                else
                {
                    bool checker = false;
                    try
                    {
                        Activ.IsRunning = false;
                        var JsonResponse = JsonConvert.DeserializeObject<Response<string, User>>(ResBack);
                        if (JsonResponse.success == true)
                        {
                            checker = true;

                            //  PopAlert(checker);
                            Settings.LastUserStatus = JsonResponse.message.status;
                            Settings.UserHash = JsonResponse.message.user_hash;
                            Settings.LastUsedEmail = JsonResponse.message.email;
                            Settings.LastRegister = JsonResponse.message.id.ToString();
                            Settings.UserFirebaseToken = JsonResponse.message.firebase_token;     
                            Device.BeginInvokeOnMainThread(() => App.Current.MainPage = new DriverRegestration());
                        }
                        else
                        {

                            Activ.IsRunning = false;
                          PopAlert(checker);
                            return;
                        }
                    }
                    catch (Exception)
                    {
                        Activ.IsRunning = false;
                        var JsonResponse = JsonConvert.DeserializeObject<Response<object, string>>(ResBack);
                     PopAlert(checker);
                        return;
                    }
                }

            }

        }     
        private void PopAlert(bool x)
        {
            PopupNavigation.Instance.PushAsync(new RequestPopUp(x, 1));
            return;
        }
        private async void LoginPageTapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new LoginPage());
        }
    }
}