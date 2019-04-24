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
    public partial class ActiveCode : ContentPage
    {
        string mail;
        public ActiveCode(string Email)
        {
            InitializeComponent();
            FlowDirection = (WaselDriver.Helper.Settings.LastUserGravity == "Arabic") ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
            mail = Email;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            Activ.IsRunning = true;
            if (EntryCode.Text != "")
            {
                UserServices ser = new UserServices();
                var resback = await ser.CodeVerfication(mail, EntryCode.Text);
                var mess = JsonConvert.DeserializeObject<Response<string, string>>(resback);
                if (mess.success)
                {
                    Activ.IsRunning = false;
                    await Navigation.PushModalAsync(new NewPassword());
                }
                else await DisplayAlert("تنبيه", "من فضلك أدخل الكود المرسل لك عبر البريد الإلكتروني الذي أدخلته مسبقاً", "موافق");
                Activ.IsRunning = false;
            }
        }
    }
}