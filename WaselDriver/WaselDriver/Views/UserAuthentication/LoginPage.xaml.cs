using Com.OneSignal;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaselDriver.Helper;
using WaselDriver.Models;
using WaselDriver.Services;
using WaselDriver.Views.IntroPages;
using WaselDriver.Views.PopUps;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WaselDriver.Views.UserAuthentication
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private UserServices userService;
        public LoginPage()
        {
            InitializeComponent();
            OneSignal.Current.IdsAvailable(IdsAvailable);
            FlowDirection = (WaselDriver.Helper.Settings.LastUserGravity == "Arabic") ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
            userService = new UserServices();
            EntryPhone.Completed += (Object sender, EventArgs e) =>
            {
                EntryEmail.Focus();
            };
            EntryEmail.Completed += (Object sender, EventArgs e) =>
            {
                EntryPassword.Focus();
            };
            Settings.LastUsedEmail = EntryEmail.Text;
            EntryPassword.Completed += (Object sender, EventArgs e) =>
            {
                Loginbtn.Focus();
            };
            Settings.LastUsedEmail = EntryEmail.Text;

        }

        private void IdsAvailable(string userID, string pushToken)
        {
            WaselDriver.Helper.Settings.LastSignalID = pushToken;
            WaselDriver.Helper.Settings.UserFirebaseToken = userID;

        }

        private bool AllFieldsFilled()
        {
            bool check = ((String.IsNullOrEmpty(EntryEmail.Text)) || (String.IsNullOrEmpty(EntryPassword.Text))) ? false : true;
            if (EntryEmail.Text == null || EntryEmail.Text == null || EntryPassword.Text == null)
            {
                Activ.IsRunning = false;
                DisplayAlert("خطأ", "من فضلك أكمل الحقول الفارغة", "OK");
            }
            else if (Settings.CarModelID == null)
            {
                Activ.IsRunning = false;
                DisplayAlert("خطأ", "من فضلك إختر نوع السيارة", "OK");
            }
            else if (EntryEmail.Text.Length < 1 || EntryEmail.Text.Length < 1 || EntryPassword.Text.Length < 1)
            {
                Activ.IsRunning = false;
                DisplayAlert("خطأ", "من فضلك أكمل الحقول الفارغة", "OK");
            }
            return check;

        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            Activ.IsRunning = true;
            if (CrossConnectivity.Current.IsConnected)
            {
                if (AllFieldsFilled())
                {
                    string email = EntryEmail.Text;
                    string password = EntryPassword.Text;
                    string device_id = "192.168.1.1";
                    string firebase_token = Settings.UserFirebaseToken;
                    var ResBack = await userService.login(email, password, device_id, firebase_token);
                    email = Settings.LastUsedEmail;
                    if (ResBack == null)
                    {
                        Activ.IsRunning = false;
                        await DisplayAlert("Communication Error", "من فضلك تحقق من الإتصال بالإنترنت", "OK");
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
                                var _userID = JsonResponse.message.id;
                                checker = true;
                                Settings.LastUsedDriverID = _userID;
                                Settings.LastUsedEmail = EntryEmail.Text;
                                Settings.UserHash = JsonResponse.message.user_hash;
                                Settings.LastUseeRole = JsonResponse.message.role;
                                Settings.LastUserStatus = JsonResponse.message.status;
                                Settings.ProfileName = JsonResponse.message.name;
                                PopAlert(checker);
                                Device.BeginInvokeOnMainThread(() => App.Current.MainPage = new MainTabbed());
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
            else
            {
                await DisplayAlert("Connection Error", "", "ok");
                Activ.IsRunning = false;
            }

        }

        private void PopAlert(bool x)
        {
            PopupNavigation.Instance.PushAsync(new RequestPopUp(x, 0));
            return;
        }

        private void EntryPhone_Focused(object sender, FocusEventArgs e)
        {
            EntryPhone.Text = "+";
        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new ForgetPassword());
        }

        private void EntryPhone_Unfocused(object sender, FocusEventArgs e)
        {
            if (EntryPhone.Text.Length < 3)
            {
                EntryPhone.Text = "";
            }
        }

        private async void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
        {
            Activ.IsRunning = true;
            if (CrossConnectivity.Current.IsConnected)
            {
               await Navigation.PushModalAsync(new Register());
                Activ.IsRunning = false;
            }
            else
            {
                await DisplayAlert("Connection Error", "", "ok");
                Activ.IsRunning = false;
            }
        }
    }
}