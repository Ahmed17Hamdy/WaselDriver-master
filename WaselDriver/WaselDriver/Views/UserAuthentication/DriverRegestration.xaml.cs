﻿using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WaselDriver.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using WaselDriver.Helper;
using Xamarin.Forms.Xaml;
using WaselDriver.Views.PopUps;
using Rg.Plugins.Popup.Services;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Plugin.Connectivity;

namespace WaselDriver.Views.UserAuthentication
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DriverRegestration : ContentPage
	{
        private MediaFile ProfilePic, NationalImg1, NationalImg2, DriverLicImg, CarLicImg, CarImg;
       
        public DriverRegestration ()
		{
			InitializeComponent ();
            GetUserLocationAsync();
		}
      private  bool AllNeeded()
        {
            if (Settings.LastLat == "" || Settings.LastLng == "")
            {
                DisplayAlert(AppResources.Error, AppResources.Location, AppResources.Ok);
                CrossPermissions.Current.OpenAppSettings();
                return false;
            }
            else if (ProfilePic == null || NationalImg1 ==null|| NationalImg2 == null||
                DriverLicImg == null || CarLicImg.Path==null || CarImg.Path ==null )
            {
                 DisplayAlert(AppResources.Error, AppResources.AddImages, AppResources.Ok);
                return false;
            }
            else if (Settings.CarModelID=="")
            {
                DisplayAlert(AppResources.Error, AppResources.SelectCarType, AppResources.Ok);
                 Navigation.PushModalAsync(new CarTypePage());
                return false;
            }
            return true;
        }
        private async Task GetUserLocationAsync()
        {
                var locationStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (locationStatus != PermissionStatus.Granted)
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Location });
                    locationStatus = results[Permission.Location];
                }
                if (locationStatus == PermissionStatus.Granted)
                {
                    var location = await Geolocation.GetLastKnownLocationAsync();
                    if (location != null)
                        Settings.LastLat = location.Latitude.ToString();
                        Settings.LastLng = location.Longitude.ToString();
                }
            else
            {
                await DisplayAlert(AppResources.PermissionsDenied, AppResources.PermissionLocationDetails, AppResources.Ok);
                //On iOS you may want to send your user to the settings screen.
                CrossPermissions.Current.OpenAppSettings();
            }
        }

        private async void Regbtn_Clicked(object sender, EventArgs e)
        {
           if(AllNeeded()==true)
            {
                Active.IsRunning = true;
                StringContent user_id = new StringContent(Settings.LastRegister);
                StringContent car_model_id = new StringContent(Settings.CarModelID);
                StringContent lat = new StringContent(Settings.LastLat);
                StringContent lng = new StringContent(Settings.LastLng);
                var content = new MultipartFormDataContent();
                content.Add(user_id, "user_id");
                content.Add(lat, "lat");
                content.Add(lng, "lng");
                content.Add(car_model_id, "car_model_id");
                content.Add(new StreamContent(ProfilePic.GetStream()), "personal_image", $"{ProfilePic.Path}");
                content.Add(new StreamContent(NationalImg1.GetStream()), "national_id_front", $"{NationalImg1.Path}");
                content.Add(new StreamContent(NationalImg2.GetStream()), "national_id_behind", $"{NationalImg2.Path}");
                content.Add(new StreamContent(DriverLicImg.GetStream()), "driving_license", $"{DriverLicImg.Path}");
                content.Add(new StreamContent(CarLicImg.GetStream()), "car_license", $"{CarLicImg.Path}");
                content.Add(new StreamContent(CarImg.GetStream()), "car_img", $"{CarImg.Path}");
                var httpClient = new HttpClient();

                if (CrossConnectivity.Current.IsConnected)
                {
                    try
                    {
                        var httpResponseMessage = await httpClient.PostAsync("http://wassel.alsalil.net/api/driverRegister", content);
                        var serverResponse = httpResponseMessage.Content.ReadAsStringAsync().Result.ToString();
                        var json = JsonConvert.DeserializeObject<Response<string, string>>(serverResponse);
                        if (json.success == false)
                        {
                            Active.IsRunning = false;
                            await DisplayAlert(AppResources.Error, json.message, AppResources.Ok);
                        }
                        else
                        {
                            Active.IsRunning = false;
                            await DisplayAlert(json.message, AppResources.RegisterSuccess, AppResources.Ok);
                            Settings.LastUsedDriverID = int.Parse(Settings.LastRegister);
                            
                            Device.BeginInvokeOnMainThread(() => App.Current.MainPage = new LoginPage());
                        }
                    }
                    catch (Exception)
                    {
                        Active.IsRunning = false;
                        await DisplayAlert(AppResources.ErrorMessage, AppResources.ErrorMessage, AppResources.Ok);
                    }
                }
                else
                {
                    Active.IsRunning = false;
                    await DisplayAlert(AppResources.ErrorMessage, AppResources.ErrorMessage, AppResources.Ok);
                }
            }
            
         
        }
       

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new CarTypePage());
        }
        private async void ProfileImg_Clicked(object sender, EventArgs e)
        {
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            if (storageStatus != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Storage });
                storageStatus = results[Permission.Storage];
            }
            if (storageStatus == PermissionStatus.Granted)
            {
                ProfilePic = await CrossMedia.Current.PickPhotoAsync();
                if (ProfilePic == null)
                    return;
                ProfImgSource.Source = ImageSource.FromStream(() =>
                {
                    return ProfilePic.GetStream();
                });
            }
            else
            {
                await DisplayAlert(AppResources.PermissionsDenied, AppResources.PermissionDetails, AppResources.Ok);
                //On iOS you may want to send your user to the settings screen.
                CrossPermissions.Current.OpenAppSettings();
            }
        }
        private async void NationalFront_Clicked(object sender, EventArgs e)
        {
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            if (storageStatus != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Storage });
                storageStatus = results[Permission.Storage];
            }
            if (storageStatus == PermissionStatus.Granted)
            {
                NationalImg1 = await CrossMedia.Current.PickPhotoAsync();
                if (NationalImg1 == null)
                    return;
                NatFrontImgSource.Source = ImageSource.FromStream(() =>
                {
                    return NationalImg1.GetStream();
                });
            }
            else
            {
                await DisplayAlert(AppResources.PermissionsDenied, AppResources.PermissionDetails, AppResources.Ok);
                //On iOS you may want to send your user to the settings screen.
                CrossPermissions.Current.OpenAppSettings();
            }
        }
        private async void NationalBack_Clicked(object sender, EventArgs e)
        {
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            if (storageStatus != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Storage });
                storageStatus = results[Permission.Storage];             
            }
            if (storageStatus == PermissionStatus.Granted)
            {
                NationalImg2 = await CrossMedia.Current.PickPhotoAsync();
                if (NationalImg2 == null)
                    return;
                NatBackImgSource.Source = ImageSource.FromStream(() =>
                {
                    return NationalImg2.GetStream();
                });
            }
            else
            {
                await DisplayAlert(AppResources.PermissionsDenied, AppResources.PermissionDetails, AppResources.Ok);
                //On iOS you may want to send your user to the settings screen.
                CrossPermissions.Current.OpenAppSettings();
            }
        }
        private async void CarLicImg_Clicked(object sender, EventArgs e)
        {
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            if (storageStatus != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Storage });
                storageStatus = results[Permission.Storage];
            }
            if (storageStatus == PermissionStatus.Granted)
            {
                CarLicImg = await CrossMedia.Current.PickPhotoAsync();
                if (CarLicImg == null)
                    return;
                CarLicImgSource.Source = ImageSource.FromStream(() =>
                {
                    return CarLicImg.GetStream();
                });
            }
            else
            {
                await DisplayAlert(AppResources.PermissionsDenied, AppResources.PermissionDetails, AppResources.Ok);
                //On iOS you may want to send your user to the settings screen.
                CrossPermissions.Current.OpenAppSettings();
            }
        }
        private async void DriverLicImg_Clicked(object sender, EventArgs e)
        {
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            if (storageStatus != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Storage });
                storageStatus = results[Permission.Storage];
            }
            if (storageStatus == PermissionStatus.Granted)
            {
                DriverLicImg = await CrossMedia.Current.PickPhotoAsync();
                if (DriverLicImg == null)
                    return;
                DriverLicImgSource.Source = ImageSource.FromStream(() =>
                {
                    return DriverLicImg.GetStream();
                });
            }
            else
            {
                await DisplayAlert(AppResources.PermissionsDenied, AppResources.PermissionDetails, AppResources.Ok);
                //On iOS you may want to send your user to the settings screen.
                CrossPermissions.Current.OpenAppSettings();
            }
        }
        private async void CarImg_Clicked(object sender, EventArgs e)
        {
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            if (storageStatus != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Storage });
                storageStatus = results[Permission.Storage];
            }
            if (storageStatus == PermissionStatus.Granted)
            {
                CarImg = await CrossMedia.Current.PickPhotoAsync();
                if (CarImg == null)
                    return;
                CarImgSource.Source = ImageSource.FromStream(() =>
                {
                    return CarImg.GetStream();
                });
            }
            else
            {
                await DisplayAlert(AppResources.PermissionsDenied, AppResources.PermissionDetails, AppResources.Ok);
                //On iOS you may want to send your user to the settings screen.
                CrossPermissions.Current.OpenAppSettings();
            }
        }
    }
}