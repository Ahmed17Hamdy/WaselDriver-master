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
            if (AllFieldsFilled())
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
                    await DisplayAlert("Connection Error", "من فضلك تحقق من الإتصال بالإنترنت", "OK");
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
                            Settings.UserHash = JsonResponse.message.user_hash;
                            Settings.LastUsedDriverID = JsonResponse.message.id;
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
        private bool AllFieldsFilled()
        {

            if (EntryName.Text == null || EntryEmail.Text == null || EntryPassword.Text == null || EntryPhone.Text == null )
            {
                Activ.IsRunning = false;
                DisplayAlert("خطأ", "من فضلك أكمل البانات", "موافق");
            }
            else if (EntryName.Text.Length < 1 || EntryEmail.Text.Length < 1 || EntryPassword.Text.Length < 1 || EntryPhone.Text.Length < 1)
            {
                Activ.IsRunning = false;
                DisplayAlert("خطأ", "من فضلك أكمل البانات", "موافق");
            }
            else if (EntryPassword.Text != ConfirmPassword.Text)
            {
                Activ.IsRunning = false;
                DisplayAlert("خطأ", "عفواً كلمة السر غير متطابقة!!", "OK");
            }
       

            bool check = ((String.IsNullOrEmpty(EntryName.Text)) || (String.IsNullOrEmpty(EntryEmail.Text)) ||
                    (String.IsNullOrEmpty(EntryPassword.Text)) || (String.IsNullOrEmpty(ConfirmPassword.Text) ||
                    (String.IsNullOrEmpty(EntryPhone.Text))) )
                ? false : true;
            return check;

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