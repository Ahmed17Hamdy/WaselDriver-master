using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaselDriver.Models;
using WaselDriver.Services;
using WaselDriver.Views.PopUps;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WaselDriver.Views.UserAuthentication
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgetPassword : ContentPage
    {
        UserServices userService;

        public ForgetPassword()
        {
            InitializeComponent();
            FlowDirection = (WaselDriver.Helper.Settings.LastUserGravity == "Arabic") ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
            userService = new UserServices();
        }


        private void PopAlert(bool x)
        {
            PopupNavigation.Instance.PushAsync(new RequestPopUp(x, 2));
            return;
        }

        private bool AllFieldsFilled()
        {
            bool check = ((String.IsNullOrEmpty(EntryEmail.Text))) ? false : true;
            if (EntryEmail.Text == null || EntryEmail.Text.Length < 1)
            {
                Activ.IsRunning = false;
                DisplayAlert("خطأ", "من فضلك أكمل الحقول الفارغة", "OK");
            }
            return check;

        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            Activ.IsRunning = true;
            if (AllFieldsFilled())
            {
                string email = EntryEmail.Text;
                var ResBack = await userService.BackupEmail(email);
                if (ResBack == "False")
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
                        var JsonResponse = JsonConvert.DeserializeObject<Response<string, string>>(ResBack);
                        if (JsonResponse.success == true)
                        {
                            checker = true;
                            await DisplayAlert("تنبيه", "لقد تم إرسال رقم التأكد من الهوية إلي البريد الإلكتروني الذى تم إدخاله", "موافق");
                            Device.BeginInvokeOnMainThread(() => App.Current.MainPage = new ActiveCode(EntryEmail.Text));
                        }
                        else
                        {
                            Activ.IsRunning = false;
                            await DisplayAlert("تنبيه", "عفوا لايوجد حساب لهذا البريد الإلكتروني من فضلك قم بالتسجيل ", "موافق");
                            Device.BeginInvokeOnMainThread(() => App.Current.MainPage = new LoginPage());
                            return;
                        }

                    }
                    catch (Exception)
                    {
                        Activ.IsRunning = false;
                        var JsonResponse = JsonConvert.DeserializeObject<Response<string, string>>(ResBack);
                        Device.BeginInvokeOnMainThread(() => App.Current.MainPage = new LoginPage());
                        await DisplayAlert("تنبيه", "لقد حدث خطأ بالإتصال بالإنترنت", "موافق");
                        return;
                    }

                }
            }
            App.Current.MainPage = new MainPage();
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}