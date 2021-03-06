﻿using Newtonsoft.Json;
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
            FlowDirection = (WaselDriver.Helper.Settings.LastUserGravity == "Arabic") ?
                FlowDirection.RightToLeft : FlowDirection.LeftToRight;
            mail = Email;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            Activ.IsRunning = true;
            if (EntryCode.Text != "")
            {
                var resback = await UserServices.CodeVerfication(mail, EntryCode.Text);
                var mess = JsonConvert.DeserializeObject<Response<string, string>>(resback);
                if (mess.success)
                {
                    Activ.IsRunning = false;
                    await Navigation.PushModalAsync(new NewPassword());
                }
                else await DisplayAlert(AppResources.Error, AppResources.ErrorMessage, AppResources.Ok);
                Activ.IsRunning = false;
            }
            else
            {
                await DisplayAlert(AppResources.Alert, AppResources.EnterVerifyCode, AppResources.Ok);
            }
        }
    }
}