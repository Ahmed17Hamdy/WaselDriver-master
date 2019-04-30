using Com.OneSignal;
using Com.OneSignal.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using TK.CustomMap;
using TK.CustomMap.Overlays;
using WaselDriver.Helper;
using WaselDriver.Models;
using WaselDriver.Views.OrderPage;
using WaselDriver.Views.PushNotificationPages;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WaselDriver
{
    public partial class MainPage : ContentPage
    {
   
        public MainPage()
        {
            InitializeComponent();
            GetLocation();
            CheckUserStatus();
        }
        private async void GetLocation()
        {
            var request = new GeolocationRequest(GeolocationAccuracy.High);
            var location = await Geolocation.GetLocationAsync(request);
            MainMap.MapRegion = MapSpan.FromCenterAndRadius(
                    new Position(location.Longitude, location.Longitude), Distance.FromMiles(1));
            OneSignal.Current.StartInit("1126a3d0-1d80-42ee-94db-d0449ac0a62c")
              .InFocusDisplaying(OSInFocusDisplayOption.None)
              .HandleNotificationReceived(OnNotificationRecevied)
              .HandleNotificationOpened(OnNotificationOpened)
              .EndInit();
        }
        private void OnNotificationOpened(OSNotificationOpenedResult result)
        {
            if (result.notification?.payload?.additionalData == null)
            {
                return;
            }

            if (result.notification.payload.additionalData.ContainsKey("body"))
            {
                var labelText = result.notification.payload.additionalData["body"].ToString();
                Settings.LastNotify = labelText;
                App.Current.MainPage = new NotificationSummaryPage();
            }

        }

        private void OnNotificationRecevied(OSNotification notification)
        {
            if (notification.payload?.additionalData == null)
            {
                return;
            }

            if (notification.payload.additionalData.ContainsKey("body"))
            {
                var labelText = notification.payload.additionalData["body"].ToString();
                Settings.LastNotify = labelText;
                App.Current.MainPage = new NotificationSummaryPage();
            }
        }
        private void CheckUserStatus()
        {
            if (Settings.LastUserStatus == "0")
            {
                UserStatuslbl.IsVisible = true;
            }
            else
            {
                UserStatuslbl.IsVisible = false;
            }

        }

        private async  void  UserLocationChanged(object sender, TK.CustomMap.TKGenericEventArgs<TK.CustomMap.Position> e)
        {
            var x = e.Value.Latitude;
            var y = e.Value.Longitude;
           if(Settings.LastLat != x.ToString() || Settings.LastLng != y.ToString())
            {
                if (Settings.LastUserStatus != "0")
                {
                    Settings.LastLat = x.ToString();
                    Settings.LastLng = y.ToString();
                    try
                    {
                        var CurrentLocation = new Position(x, y);
                        if (CurrentLocation != null)
                        {
                            Dictionary<string, string> values = new Dictionary<string, string>();
                            values.Add("driver_id",Settings.LastUsedDriverID.ToString());
                            values.Add("lat", Settings.LastLat);
                            values.Add("lng", Settings.LastLng);
                            string content = JsonConvert.SerializeObject(values);
                            var httpClient = new HttpClient();
                            try
                            {
                                var response = await httpClient.PostAsync("http://wassel.alsalil.net/api/updatelocal", 
                                    new StringContent(content, Encoding.UTF8, "text/json"));
                                var serverResponse = response.Content.ReadAsStringAsync().Result.ToString();
                                var json = JsonConvert.DeserializeObject<Response<string, string>>(serverResponse);
                              
                            }
                            catch (Exception)
                            {                             
                                await DisplayAlert(AppResources.Error, AppResources.ErrorMessage, AppResources.Ok);
                            }
                        }
                    }
                    catch
                    {
                        return;
                    }
                }
            
            }          
           
       
        }

    }
}
