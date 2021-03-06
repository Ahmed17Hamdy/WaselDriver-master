﻿using Com.OneSignal;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Services;
using System;
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
        public LoginPage()
        {
            InitializeComponent();
            OneSignal.Current.IdsAvailable(IdsAvailable);
            FlowDirection = (WaselDriver.Helper.Settings.LastUserGravity == "Arabic") ? FlowDirection.RightToLeft :
                FlowDirection.LeftToRight;
        }
        private void IdsAvailable(string userID, string pushToken)
        {
            Settings.LastSignalID = pushToken;
            Settings.UserFirebaseToken = userID;
        }
        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            Activ.IsRunning = true;
            if (CrossConnectivity.Current.IsConnected)
            {
             
                    string email = EntryEmail.Text;
                    string password = EntryPassword.Text;
                    string device_id = "192.168.1.1";
                    string firebase_token = Settings.UserFirebaseToken;
                    string ss = Settings.LastSignalID;
                    var ResBack = await UserServices.login(email, password, device_id, firebase_token);
                    if (ResBack == null)
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
                            var JsonResponse = JsonConvert.DeserializeObject<Response<string, User>>(ResBack);
                        if (JsonResponse.success == true)
                        {
                            var _userID = JsonResponse.message.id;
                            checker = true;
                            Settings.LastUsedDriverID = _userID;
                            Settings.UserHash = JsonResponse.message.user_hash;
                            Settings.LastUseeRole = JsonResponse.message.role;
                            Settings.LastUserStatus = JsonResponse.message.status;
                            Settings.LastUsedEmail = JsonResponse.message.email;
                            Settings.ProfileName = JsonResponse.message.name;
                            Settings.UserFirebaseToken = JsonResponse.message.firebase_token;
                            PopAlert(checker);
                            if (JsonResponse.message.status=="0") 
                                Device.BeginInvokeOnMainThread(() => App.Current.MainPage = new DriverRegestration());
                            else
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
            else
            {
                await DisplayAlert(AppResources.Error, AppResources.ErrorMessage, AppResources.Ok);
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
            Navigation.PushAsync(new ForgetPassword());
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