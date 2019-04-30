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
            FlowDirection = (WaselDriver.Helper.Settings.LastUserGravity == "Arabic") ?
                FlowDirection.RightToLeft : FlowDirection.LeftToRight;
            userService = new UserServices();
        }


        private void PopAlert(bool x)
        {
            PopupNavigation.Instance.PushAsync(new RequestPopUp(x, 2));
            return;
        }

    
        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            Activ.IsRunning = true;
           
                string email = EntryEmail.Text;
                var ResBack = await userService.BackupEmail(email);
                if (ResBack == "False")
                {
                    Activ.IsRunning = false;
                    await DisplayAlert(AppResources.Error, AppResources.ErrorMessage, AppResources.Ok);
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
                            await DisplayAlert(AppResources.Alert, AppResources.ConfirmSentCode, AppResources.Ok);
                            Device.BeginInvokeOnMainThread(() => App.Current.MainPage = new ActiveCode(EntryEmail.Text));
                        }
                        else
                        {
                            Activ.IsRunning = false;
                            await DisplayAlert(AppResources.Alert, AppResources.WrongEmail, AppResources.Ok);
                            Device.BeginInvokeOnMainThread(() => App.Current.MainPage = new LoginPage());
                            return;
                        }

                    }
                    catch (Exception)
                    {
                        Activ.IsRunning = false;
                        var JsonResponse = JsonConvert.DeserializeObject<Response<string, string>>(ResBack);
                        Device.BeginInvokeOnMainThread(() => App.Current.MainPage = new LoginPage());
                        await DisplayAlert(AppResources.Error, AppResources.ErrorMessage, AppResources.Ok);
                        return;
                    }

                }
            
            App.Current.MainPage = new MainPage();
        }

       
    }
}