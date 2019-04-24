using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaselDriver.Models;
using WaselDriver.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WaselDriver.Views.UserAuthentication
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewPassword : ContentPage
    {
        public NewPassword()
        {
            InitializeComponent();
            FlowDirection = (WaselDriver.Helper.Settings.LastUserGravity == "Arabic") ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            Activ.IsRunning = true;
            if (AllFieldsFilled())
            {
                UserServices ser = new UserServices();
                var response = await ser.ResetPassword(EntryPass.Text);
                var mess = JsonConvert.DeserializeObject<Response<string, string>>(response);
                if (mess.success)
                {
                    Activ.IsRunning = false;
                    await Navigation.PushModalAsync(new LoginPage());
                }
                else await DisplayAlert("تنبيه", "من فضلك تحقق من كلمة السر", "موافق");
                Activ.IsRunning = false;
            }
        }
        private bool AllFieldsFilled()
        {

            bool check = false;
            if (EntryPass.Text != ConfirmPass.Text)
            {
                Activ.IsRunning = false;
                DisplayAlert("تنبيه", "عفواً كلمة السر غير متطابقة!!", "موافق");
            }
            else if (EntryPass.Text == "" || ConfirmPass.Text == "")
            {
                DisplayAlert("تنبيه", "عفواً كلمة السر غير متطابقة!!", "موافق");
            }
            else return true;

            return check;

        }

    }
}